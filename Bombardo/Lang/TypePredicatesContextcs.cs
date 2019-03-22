using System;
using System.Collections.Generic;
using System.Text;

namespace Bombardo
{
    class TypePredicatesContextcs
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, "null?", PredNull, 1);
            BombardoLangClass.SetProcedure(context, "empty?", PredEmpty, 1);
            BombardoLangClass.SetProcedure(context, "symbol?", PredSym, 1);
            BombardoLangClass.SetProcedure(context, "pair?", PredList, 1);
            BombardoLangClass.SetProcedure(context, "list?", PredList, 1);
            BombardoLangClass.SetProcedure(context, "string?", PredString, 1);
            BombardoLangClass.SetProcedure(context, "bool?", PredBool, 1);
            BombardoLangClass.SetProcedure(context, "number?", PredNumber, 1);
            BombardoLangClass.SetProcedure(context, "proc?", PredProcedure, 1);
        }

        public static Atom PredNull(Atom args, Context context)
        {
            return args.value == null ? Atom.TRUE : Atom.FALSE;
        }

        public static Atom PredEmpty(Atom args, Context context)
        {
            if(args.value==null) return Atom.FALSE;
            return ((Atom)args.value).IsEmpty() ? Atom.TRUE : Atom.FALSE;
        }

        public static Atom PredSym(Atom args, Context context)
        {
            if (args.value == null)
                return Atom.FALSE;
            Atom atom = (Atom)args.value;
            if (atom.type == AtomType.Symbol)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredList(Atom args, Context context)
        {
            if (args.value == null)
                return Atom.FALSE;
            Atom atom = (Atom)args.value;
            if (atom.type == AtomType.Pair)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredString(Atom args, Context context)
        {
            if (args.value == null)
                return Atom.FALSE;
            Atom atom = (Atom)args.value;
            if (atom.type == AtomType.String)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredBool(Atom args, Context context)
        {
            if (args.value == null)
                return Atom.FALSE;
            Atom atom = (Atom)args.value;
            if (atom.type == AtomType.Bool)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredNumber(Atom args, Context context)
        {
            if (args.value == null)
                return Atom.FALSE;
            Atom atom = (Atom)args.value;
            if (atom.type == AtomType.Number)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredProcedure(Atom args, Context context)
        {
            if (args.value == null)
                return Atom.FALSE;
            Atom atom = (Atom)args.value;
            if (atom.type == AtomType.Procedure)
                return Atom.TRUE;
            return Atom.FALSE;
        }

    }
}
