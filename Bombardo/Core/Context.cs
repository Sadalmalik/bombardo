using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombardo
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

        public Atom Define(string symbol, Atom value = null)
        {
            if (@sealed)
                throw new BombardoException(string.Format("Context is sealed! Symbol '{0}' can't be changed!", symbol));
            return this[symbol] = value;
        }

        public Atom Set(string symbol, Atom value = null)
        {
            if (ContainsKey(symbol))
            {
                if (@sealed) throw new BombardoException(string.Format("Context is sealed! Symbol '{0}' can't be changed!", symbol));
                return this[symbol] = value;
            }
            else if (parent != null)
                return parent.Set(symbol, value);
            throw new BombardoException(string.Format("<SET> Symbol '{0}' not defined in current context!", symbol));
        }

        public Atom Get(string symbol, bool noException=false)
        {
            if (ContainsKey(symbol))
                return this[symbol];
            if (parent != null)
                return parent.Get(symbol);
            if (noException)
                return null;
            throw new BombardoException(string.Format("<GET> Symbol '{0}' not defined in current context!", symbol));
        }

        public void SetArgs(Atom args, Atom values)
        {
            if (args == null) return;
            if (args.IsEmpty()) return;
            Atom keys = args;
            while (keys != null)
            {
                if (keys.IsSymbol())
                {
                    Define((string)keys.value, values);
                    break;
                }
                if (values!=null && !values.IsPair())
                {   //  Skip invalid stuff
                    values = null;
                }
                Atom key = (Atom)keys.value;
                if (key != null)
                {
                    if (!key.IsSymbol()) throw new BombardoException(string.Format("Argument name '{0}' must be symbol!", key.ToString()));
                    
                    Atom value = (Atom)values?.value;

                    Define((string)key.value, value);
                }
                keys = keys.next;
                if (values != null)
                    values = values.next;
            }
        }

        public override string ToString()
        {
            return "Table:"+base.GetHashCode();
        }
    }
}