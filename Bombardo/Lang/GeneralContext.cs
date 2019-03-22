using System;
using System.Collections.Generic;

namespace Bombardo
{
    internal class GeneralContext
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, "print", Print, 1);

            BombardoLangClass.SetProcedure(context, "define", Define, 1, false);
            BombardoLangClass.SetProcedure(context, "set!", SetFirst, 2, false);

            BombardoLangClass.SetProcedure(context, "to-string", ToString, 1);
            BombardoLangClass.SetProcedure(context, "from-string", FromString, 1);

            BombardoLangClass.SetProcedure(context, "symbol-name", SymbolName, 1);
            BombardoLangClass.SetProcedure(context, "symbol-make", MakeSymbol, 1);

            BombardoLangClass.SetProcedure(context, "get-context", GetContext, 0);
            BombardoLangClass.SetProcedure(context, "get-context-parent", GetContextParent, 0);

            context.Define(Atom.NULL_SYMBOL, null); 
            context.Define(Atom.EMPTY_SYMBOL, Atom.EMPTY);
        }

        public static Atom Print(Atom args, Context context)
        {
            for (Atom iter = args; iter != null; iter = iter.next)
            {
                Atom value = iter.value as Atom;
                if (value == null)
                    Console.Write("null");
                else if (value.type == AtomType.String ||
                    value.type == AtomType.Symbol)
                    Console.Write(value.value);
                else
                    Console.Write(value.Stringify(true));
            }
            Console.WriteLine();
            return null;
        }

        public static Atom Define(Atom args, Context context)
        {
            Atom sym = (Atom)args?.value;
            Atom val = (Atom)args?.next?.value;

            val = Evaluator.Evaluate(val, context);

            if (sym.type != AtomType.Symbol)
                throw new BombardoException("<DEFINE> Definition name must be symbol!");
            
            ContextUtils.Define(context, val, (string)sym.value);

            return val;
        }

        public static Atom SetFirst(Atom args, Context context)
        {
            Atom sym = (Atom)args?.value;
            Atom val = (Atom)args?.next?.value;

            val = Evaluator.Evaluate(val, context);

            if (!sym.IsSymbol())
                throw new BombardoException("<SET!> Variable name must be symbol!");
            
            ContextUtils.Set(context, val, (string)sym.value);

            return val;
        }

        public static Atom ToString(Atom args, Context context)
        {
            Atom atom = (Atom)args.value;
            return new Atom(AtomType.String, atom.ToString());
        }

        public static Atom FromString(Atom args, Context context)
        {
            Atom str = (Atom)args.value;
            if (!str.IsString()) throw new BombardoException("Argument must be string!");
            List<Atom> atoms = BombardoLangClass.Parse((string)str.value);
            return Atom.List(atoms.ToArray());
        }
        
        public static Atom SymbolName(Atom args, Context context)
        {
            Atom symbol = (Atom)args.value;
            if (!symbol.IsSymbol()) throw new BombardoException("Argument must be symbol!");
            return new Atom(AtomType.String, (string)symbol.value);
        }

        public static Atom MakeSymbol(Atom args, Context context)
        {
            Atom symbol = (Atom)args.value;
            if (!symbol.IsString()) throw new BombardoException("Argument must be string!");
            return new Atom(AtomType.Symbol, (string)symbol.value);
        }

        public static Atom GetContext(Atom args, Context context)
        {
            return context.self;
        }

        public static Atom GetContextParent(Atom args, Context context)
        {
            Atom ctx = args==null ? null : (Atom)args.value;
            if (ctx != null && ctx.IsNative())
            {
                Context other = ctx.value as Context;
                if (other != null) context = other;
            }
            return context.parent==null ? null : context.parent.self;
        }
    }
}