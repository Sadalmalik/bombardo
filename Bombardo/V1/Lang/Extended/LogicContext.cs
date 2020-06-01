using System;

namespace Bombardo.V1
{
    internal class LogicContext
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, AllNames.LOGIC_EQ, Eq, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LOGIC_NEQ, Neq, 2);

            BombardoLangClass.SetProcedure(context, AllNames.LOGIC_AND, And, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LOGIC_OR, Or, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LOGIC_XOR, Xor, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LOGIC_IMP, Imp, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LOGIC_NOT, Not, 1);
        }

        #region Equation

        public static Atom Eq(Atom args, Context context)
        {
            //  Особый механизм - сравнение всего
            for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
                if (!Atom.Compare((Atom)iter.value, (Atom)iter.next.value))
                    return Atom.FALSE;
            return Atom.TRUE;
        }

        public static Atom Neq(Atom args, Context context)
        {
            //  Особый механизм - сравнение всего
            for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
                if (Atom.Compare((Atom)iter.value, (Atom)iter.next.value))
                    return Atom.FALSE;
            return Atom.TRUE;
        }

        #endregion Equation

        #region Boolean operations

        private static bool AllBooleand(Atom args)
        {
            for (Atom iter = args; iter != null; iter = iter.next)
                if (((Atom)iter.value).type != AtomType.Bool)
                    return false;
            return true;
        }

        private static void CheckAllBooleand(Atom args)
        {
            if (!AllBooleand(args))
                throw new ArgumentException("Not all arguments are booleans!");
        }

        public static Atom And(Atom args, Context context)
        {
            CheckAllBooleand(args);
            bool res = (bool)((Atom)args.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res &= (bool)((Atom)iter.value).value;
            return new Atom(AtomType.Bool, res);
        }

        public static Atom Or(Atom args, Context context)
        {
            CheckAllBooleand(args);
            bool res = (bool)((Atom)args.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res |= (bool)((Atom)iter.value).value;
            return new Atom(AtomType.Bool, res);
        }

        public static Atom Xor(Atom args, Context context)
        {
            CheckAllBooleand(args);
            bool res = (bool)((Atom)args.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res ^= (bool)((Atom)iter.value).value;
            return new Atom(AtomType.Bool, res);
        }

        public static Atom Imp(Atom args, Context context)
        {
            CheckAllBooleand(args);
            bool res = (bool)((Atom)args.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = !res | (bool)((Atom)iter.value).value;
            return new Atom(AtomType.Bool, res);
        }

        public static Atom Not(Atom args, Context context)
        {
            CheckAllBooleand(args);
            if (args.next != null)
                throw new ArgumentException("Too many arguments!");
            return new Atom(AtomType.Bool, !(bool)((Atom)args.value).value);
        }

        #endregion Boolean operations
    }
}