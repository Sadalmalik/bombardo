using System;

namespace Bombardo.V1
{
    internal class MathContext
    {
        public static void Setup(Context context)
        {
            //  Base math

            BombardoLangClass.SetProcedure(context, AllNames.MATH_SUM, Sum, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_DIS, Dis, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_MUL, Mul, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_DIV, Div, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_MOD, Mod, 2);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_MIN, Min, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_MAX, Max, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_ABS, Abs, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_SIGN, Sign, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_CEIL, Ceil, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_FLOOR, Floor, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_TRUNC, Trunc, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_SQRT, Sqrt, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_POW, Pow, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_EXP, Exp, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_LOGN, Logn, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_LOG, Log, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_LOG10, Log10, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_SIN, Sin, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_COS, Cos, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_TAN, Tan, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_ASIN, Asin, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_ACOS, Acos, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_ATAN, Atan, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_ATAN2, Atan2, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_SINH, Sinh, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_COSH, Cosh, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_TANH, Tanh, 1);

            context.Define(AllNames.MATH_PI, new Atom(AtomType.Number, Math.PI));
            context.Define(AllNames.MATH_E, new Atom(AtomType.Number, Math.E));

            BombardoLangClass.SetProcedure(context, AllNames.MATH_AND, And, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_OR, Or, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_XOR, Xor, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_LSH, Lsh, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_RSH, Rsh, 2);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_LT, Lt, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_GT, Gt, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_LE, Le, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_GE, Ge, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_NE, Ne, 2);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_EQ, Eq, 2);

            //  Type predicates

            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDBYTE, PredSByte, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDUBYTE, PredUByte, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDSBYTE, PredSByte, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDCHAR, PredChar, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDSSHORT, PredSShort, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDUSHORT, PredUShort, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDSINT, PredSInt, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDUINT, PredUInt, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDSLONG, PredSLong, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDULONG, PredULong, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDFLOAT, PredFloat, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_PREDDOUBLE, PredDouble, 1);

            //  Type cast

            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTBYTE, CastSByte, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTUBYTE, CastUByte, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTSBYTE, CastSByte, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTCHAR, CastChar, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTSSHORT, CastSShort, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTUSHORT, CastUShort, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTSINT, CastSInt, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTUINT, CastUInt, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTSLONG, CastSLong, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTULONG, CastULong, 1);

            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTFLOAT, CastFloat, 1);
            BombardoLangClass.SetProcedure(context, AllNames.MATH_CASTDOUBLE, CastDouble, 1);
        }

        #region Inner stuff

        private static bool AllNumbers(Atom args)
        {
            for (Atom atom = args; atom != null; atom = atom.next)
                if (((Atom)atom.value).type != AtomType.Number)
                    return false;
            return true;
        }

        private static void CheckAllNumbers(Atom args)
        {
            if (!AllNumbers(args))
                throw new ArgumentException("Not all arguments are numbers!");
        }

        #endregion Inner stuff

        #region Math operators

        public static Atom Sum(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Sum(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Dis(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Dis(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Mul(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Mul(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Div(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Div(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Mod(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Mod(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        #endregion Math operators

        #region Math functions

        public static Atom Min(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Min(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Max(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Max(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Abs(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Abs(atom.value));
        }

        public static Atom Sign(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Sign(atom.value));
        }

        public static Atom Ceil(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Ceil(atom.value));
        }

        public static Atom Floor(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Floor(atom.value));
        }
        
        public static Atom Trunc(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Trunc(atom.value));
        }

        public static Atom Sqrt(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Sqrt(atom.value));
        }

        public static Atom Pow(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Pow(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Exp(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Exp(atom.value));
        }

        public static Atom Logn(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Logn(atom.value));
        }

        public static Atom Log(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Log(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Log10(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Log10(atom.value));
        }
        
        public static Atom Sin(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Sin(atom.value));
        }

        public static Atom Cos(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Cos(atom.value));
        }

        public static Atom Tan(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Tan(atom.value));
        }
        
        public static Atom Asin(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Asin(atom.value));
        }

        public static Atom Acos(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Acos(atom.value));
        }

        public static Atom Atan(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Atan(atom.value));
        }

        public static Atom Atan2(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Atan2(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }


        public static Atom Sinh(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Sinh(atom.value));
        }

        public static Atom Cosh(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Cosh(atom.value));
        }

        public static Atom Tanh(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Tanh(atom.value));
        }
        
        #endregion

        #region Binary operators

        public static Atom And(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.And(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Or(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Or(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Xor(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Xor(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Lsh(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Lsh(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        public static Atom Rsh(Atom args, Context context)
        {
            CheckAllNumbers(args);
            object res = ((Atom)args?.value).value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
                res = UNumber.Rsh(res, ((Atom)iter.value).value);
            return new Atom(AtomType.Number, res);
        }

        #endregion Binary operators

        #region Comparsion

        public static Atom Lt(Atom args, Context context)
        {
            CheckAllNumbers(args);
            for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Lt(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    return Atom.FALSE;
            return Atom.TRUE;
        }

        public static Atom Gt(Atom args, Context context)
        {
            CheckAllNumbers(args);
            for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Gt(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    return Atom.FALSE;
            return Atom.TRUE;
        }

        public static Atom Le(Atom args, Context context)
        {
            CheckAllNumbers(args);
            for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Le(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    return Atom.FALSE;
            return Atom.TRUE;
        }

        public static Atom Ge(Atom args, Context context)
        {
            CheckAllNumbers(args);
            for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Ge(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    return Atom.FALSE;
            return Atom.TRUE;
        }

        public static Atom Ne(Atom args, Context context)
        {
            CheckAllNumbers(args);
            for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Ne(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    return Atom.FALSE;
            return Atom.TRUE;
        }

        public static Atom Eq(Atom args, Context context)
        {
            CheckAllNumbers(args);
            for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Eq(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    return Atom.FALSE;
            return Atom.TRUE;
        }

        #endregion Comparsion

        #region Types Predicates

        public static Atom PredUByte(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.UBYTE)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredSByte(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.SBYTE)
                return Atom.TRUE;
            return Atom.FALSE;
        }
        
        public static Atom PredChar(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.CHAR)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredUShort(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.USHORT)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredSShort(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.SSHORT)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredUInt(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.UINT)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredSInt(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.SINT)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredULong(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.ULONG)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredSLong(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.SLONG)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredFloat(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.FLOAT)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        public static Atom PredDouble(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.DOUBLE)
                return Atom.TRUE;
            return Atom.FALSE;
        }

        #endregion

        #region Type Cast

        public static Atom CastUByte(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");
            
            return new Atom(AtomType.Number, (byte)Convert.ToInt64(atom.value));
        }

        public static Atom CastSByte(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, (sbyte)Convert.ToInt64(atom.value));
        }

        public static Atom CastChar(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, (char)Convert.ToChar(atom.value));
        }
        
        public static Atom CastUShort(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, (ushort)Convert.ToInt64(atom.value));
        }

        public static Atom CastSShort(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, (short)Convert.ToInt64(atom.value));
        }

        public static Atom CastUInt(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, (uint)Convert.ToInt64(atom.value));
        }

        public static Atom CastSInt(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, (int)Convert.ToInt64(atom.value));
        }

        public static Atom CastULong(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, Convert.ToUInt64(atom.value));
        }

        public static Atom CastSLong(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, Convert.ToInt64(atom.value));
        }

        public static Atom CastFloat(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, Convert.ToSingle(atom.value));
        }

        public static Atom CastDouble(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            return new Atom(AtomType.Number, Convert.ToDouble(atom.value));
        }

        #endregion
    }
}