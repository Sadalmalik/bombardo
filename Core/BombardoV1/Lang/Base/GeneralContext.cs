using System;
using System.Collections.Generic;

namespace Bombardo.V1
{
    internal class GeneralContext
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, AllNames.LISP_PRINT, Print, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_READ, ReadLine, 0);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_DEFINE, Define, 1, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_SETFIRST, SetFirst, 2, false);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_TO_STRING, ToString, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_FROM_STRING, FromString, 1);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_SYMBOL_NAME, SymbolName, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_MAKE_SYMBOL, MakeSymbol, 1);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_GET_CONTEXT, GetContext, 0);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_GET_CONTEXT_PARENT, GetContextParent, 0);

            context.Define(AllNames.NULL_SYMBOL, null); 
            context.Define(AllNames.EMPTY_SYMBOL, Atom.EMPTY);
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

        public static Atom ReadLine(Atom args, Context context)
        {
            string line = Console.ReadLine();
            return new Atom(AtomType.String, line);
        }
        
        public static Atom Define(Atom args, Context context)
        {
            Atom sym = (Atom)args?.value;
            Atom val = (Atom)args?.next?.value;

            val = Evaluator.Evaluate(val, context);

            if (sym.type != AtomType.Symbol)
                throw new ArgumentException("Definition name must be symbol!");
            
            ContextUtils.Define(context, val, (string)sym.value);

            return val;
        }

        public static Atom SetFirst(Atom args, Context context)
        {
            Atom sym = (Atom)args?.value;
            Atom val = (Atom)args?.next?.value;

            val = Evaluator.Evaluate(val, context);

            if (!sym.IsSymbol())
                throw new ArgumentException("Variable name must be symbol!");
            
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
            if (!str.IsString()) throw new ArgumentException("Argument must be string!");
            List<Atom> atoms = BombardoLangClass.Parse((string)str.value);
            return Atom.List(atoms.ToArray());
        }
        
        public static Atom SymbolName(Atom args, Context context)
        {
            Atom symbol = (Atom)args.value;
            if (!symbol.IsSymbol()) throw new ArgumentException("Argument must be symbol!");
            return new Atom(AtomType.String, (string)symbol.value);
        }

        public static Atom MakeSymbol(Atom args, Context context)
        {
            Atom symbol = (Atom)args.value;
            if (!symbol.IsString()) throw new ArgumentException("Argument must be string!");
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