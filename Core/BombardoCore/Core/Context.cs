using System;
using System.Collections.Generic;
using System.Text;

namespace Bombardo.Core
{
    public class Context : Dictionary<string, Atom>
    {
        public Atom self;
        public Context parent;
        public bool @sealed;

        public Context(Context parent = null)
        {
            this.parent = parent;
            this.@sealed = false;
            self = new Atom(AtomType.Native, this);
        }
        
		public Atom DefineFunction(string name,
			Action<Evaluator, StackFrame> rawFunction,
			bool evalArgs = true, bool evalResult = false)
		{
		    var funct = new Function(name, Atoms.BUILT_IN, rawFunction, evalArgs, evalResult);
		    var atom = new Atom(AtomType.Function, funct);
			this.Add(name, atom);
			return atom;
		}

        public Atom Define(string symbol, Atom value)
        {
            if (@sealed)
                throw new ArgumentException(string.Format("Context is sealed! Symbol '{0}' can't be changed!", symbol));
            return this[symbol] = value;
        }
        
        public Atom Undefine(string symbol)
        {
            if (@sealed)
                throw new ArgumentException(string.Format("Context is sealed! Symbol '{0}' can't be changed!", symbol));
                
            if (Remove(symbol))
                return Atoms.TRUE;
            
            return Atoms.FALSE;
        }

        public Atom Set(string symbol, Atom value)
        {
            if (ContainsKey(symbol))
            {
                if (@sealed) throw new ArgumentException(string.Format("Context is sealed! Symbol '{0}' can't be changed!", symbol));
                return this[symbol] = value;
            }
            else if (parent != null)
                return parent.Set(symbol, value);
            throw new ArgumentException(string.Format("Symbol '{0}' not defined in current context!", symbol));
        }

        public Atom Get(string symbol, bool noException=false)
        {
            if (ContainsKey(symbol))
                return this[symbol];
            if (parent != null)
                return parent.Get(symbol, noException);
            if (noException)
                return null;
            throw new ArgumentException(string.Format("Symbol '{0}' not defined in current context!", symbol));
        }

        public void SetArgs(Atom args, Atom values)
        {
            if (args == null) return;
            if (args.IsEmpty) return;
            Atom keys = args;
            while (keys != null)
            {
                if (keys.IsSymbol)
                {
                    Define((string)keys.value, values);
                    break;
                }
                if (values!=null && !values.IsPair)
                {   //  Skip invalid stuff
                    values = null;
                }
                Atom key = (Atom)keys.value;
                if (key != null)
                {
                    if (!key.IsSymbol) throw new ArgumentException(string.Format("Argument name '{0}' must be symbol!", key.ToString()));
                    
                    Atom value = (Atom)values?.value;

                    Define((string)key.value, value);
                }
                keys = keys.next;
                if (values != null)
                    values = values.next;
            }
        }
        
        public void ImportSymbols(Context source, params string[] symbols)
        {
            if (source==null)
                return;
            foreach (var symbol in symbols)
                Define(symbol, source.Get(symbol));
        }
        
        public void ImportAllSymbols(Context source)
        {
            if (source==null)
                return;
            foreach (var pair in source)
                Define(pair.Key, pair.Value);
        }

        public override string ToString()
        {
            return parent != null ?
                $"{parent}->Table:{base.GetHashCode()}" :
                $"Table:{base.GetHashCode()}";
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            InternalDump(sb, 0, 2);
            return sb.ToString();
        }
        
        private void InternalDump(StringBuilder sb, int indentSize, int indentStep)
        {
            parent?.InternalDump(sb, indentSize, indentStep);
            var indent = new string(' ', indentSize);
            foreach (var pair in this)
            {
                if (pair.Value == null)
                {
                    sb.AppendLine($"{indent}{pair.Key}: {pair.Value}");
                    return;
                }
                if (pair.Value.IsNative && pair.Value.value is Context ctx)
                {
                    sb.AppendLine($"{indent}{pair.Key}:");
                    ctx.InternalDump(sb, indentSize + indentStep, indentStep);
                }
                else
                {
                    sb.AppendLine($"{indent}{pair.Key}: {pair.Value}");
                }
            }
        }
    }
}