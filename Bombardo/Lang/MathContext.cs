using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombardo
{
    internal class MathContext
    {
        public static void Setup(Context context)
        {
            //  Base math

            BombardoLangClass.SetProcedure(context, "+", Sum, 2);
            BombardoLangClass.SetProcedure(context, "-", Dis, 2);
            BombardoLangClass.SetProcedure(context, "*", Mul, 2);
            BombardoLangClass.SetProcedure(context, "/", Div, 2);
            BombardoLangClass.SetProcedure(context, "%", Mod, 2);

            BombardoLangClass.SetProcedure(context, "min", Min, 2);
            BombardoLangClass.SetProcedure(context, "max", Max, 2);
            BombardoLangClass.SetProcedure(context, "abs", Abs, 1);
            BombardoLangClass.SetProcedure(context, "sign", Sign, 1);
            BombardoLangClass.SetProcedure(context, "ceil", Ceil, 1);
            BombardoLangClass.SetProcedure(context, "floor", Floor, 1);
            //BombardoLang.SetProcedure(context, "clamp", Clamp, 3);
            //BombardoLang.SetProcedure(context, "clamp01", Clamp01, 3);

            BombardoLangClass.SetProcedure(context, "trunc", Trunc, 1);
            BombardoLangClass.SetProcedure(context, "sqrt", Sqrt, 1);
            BombardoLangClass.SetProcedure(context, "pow", Pow, 2);
            BombardoLangClass.SetProcedure(context, "exp", Exp, 1);
            BombardoLangClass.SetProcedure(context, "ln", Logn, 1);
            BombardoLangClass.SetProcedure(context, "log", Log, 2);
            BombardoLangClass.SetProcedure(context, "log10", Log10, 1);

            BombardoLangClass.SetProcedure(context, "sin", Sin, 1);
            BombardoLangClass.SetProcedure(context, "cos", Cos, 1);
            BombardoLangClass.SetProcedure(context, "tan", Tan, 1);
            BombardoLangClass.SetProcedure(context, "asin", Asin, 1);
            BombardoLangClass.SetProcedure(context, "acos", Acos, 1);
            BombardoLangClass.SetProcedure(context, "atan", Atan, 1);
            BombardoLangClass.SetProcedure(context, "atan2", Atan2, 2);
            BombardoLangClass.SetProcedure(context, "sinh", Sinh, 1);
            BombardoLangClass.SetProcedure(context, "cosh", Cosh, 1);
            BombardoLangClass.SetProcedure(context, "tanh", Tanh, 1);

            context.Define("#PI", new Atom(AtomType.Number, Math.PI));
            context.Define("#E", new Atom(AtomType.Number, Math.E));

            BombardoLangClass.SetProcedure(context, "&", And, 2);
            BombardoLangClass.SetProcedure(context, "|", Or, 2);
            BombardoLangClass.SetProcedure(context, "^", Xor, 2);
            BombardoLangClass.SetProcedure(context, "<<", Lsh, 2);
            BombardoLangClass.SetProcedure(context, ">>", Rsh, 2);
            
            BombardoLangClass.SetProcedure(context, "<", Lt, 2);
            BombardoLangClass.SetProcedure(context, ">", Gt, 2);
            BombardoLangClass.SetProcedure(context, "<=", Le, 2);
            BombardoLangClass.SetProcedure(context, ">=", Ge, 2);
            BombardoLangClass.SetProcedure(context, "!=", Ne, 2);
            BombardoLangClass.SetProcedure(context, "==", Eq, 2);

            //  Type predicates

            BombardoLangClass.SetProcedure(context, "byte?", PredUByte, 1);
            BombardoLangClass.SetProcedure(context, "ubyte?", PredUByte, 1);
            BombardoLangClass.SetProcedure(context, "sbyte?", PredSByte, 1);
            BombardoLangClass.SetProcedure(context, "char?", PredChar, 1);

            BombardoLangClass.SetProcedure(context, "short?", PredSShort, 1);
            BombardoLangClass.SetProcedure(context, "ushort?", PredUShort, 1);

            BombardoLangClass.SetProcedure(context, "int?", PredSInt, 1);
            BombardoLangClass.SetProcedure(context, "uint?", PredUInt, 1);

            BombardoLangClass.SetProcedure(context, "long?", PredSLong, 1);
            BombardoLangClass.SetProcedure(context, "ulong?", PredULong, 1);

            BombardoLangClass.SetProcedure(context, "float?", PredFloat, 1);
            BombardoLangClass.SetProcedure(context, "double?", PredDouble, 1);

            //  Type cast

            BombardoLangClass.SetProcedure(context, "byte:", CastUByte, 1);
            BombardoLangClass.SetProcedure(context, "ubyte:", CastUByte, 1);
            BombardoLangClass.SetProcedure(context, "sbyte:", CastSByte, 1);
            BombardoLangClass.SetProcedure(context, "char:", CastChar, 1);

            BombardoLangClass.SetProcedure(context, "short:", CastSShort, 1);
            BombardoLangClass.SetProcedure(context, "ushort:", CastUShort, 1);

            BombardoLangClass.SetProcedure(context, "int:", CastSInt, 1);
            BombardoLangClass.SetProcedure(context, "uint:", CastUInt, 1);

            BombardoLangClass.SetProcedure(context, "long:", CastSLong, 1);
            BombardoLangClass.SetProcedure(context, "ulong:", CastULong, 1);

            BombardoLangClass.SetProcedure(context, "float:", CastFloat, 1);
            BombardoLangClass.SetProcedure(context, "double:", CastDouble, 1);
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
                throw new BombardoException("<MATH> Not all arguments are numbers!");
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
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Abs(atom.value));
        }

        public static Atom Sign(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Sign(atom.value));
        }

        public static Atom Ceil(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Ceil(atom.value));
        }

        public static Atom Floor(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Floor(atom.value));
        }
        
        public static Atom Trunc(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Trunc(atom.value));
        }

        public static Atom Sqrt(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

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
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Exp(atom.value));
        }

        public static Atom Logn(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

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
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Log10(atom.value));
        }
        
        public static Atom Sin(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Sin(atom.value));
        }

        public static Atom Cos(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Cos(atom.value));
        }

        public static Atom Tan(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Tan(atom.value));
        }
        
        public static Atom Asin(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Asin(atom.value));
        }

        public static Atom Acos(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Acos(atom.value));
        }

        public static Atom Atan(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

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
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Sinh(atom.value));
        }

        public static Atom Cosh(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, UNumber.Cosh(atom.value));
        }

        public static Atom Tanh(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

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
                throw new BombardoException("<MATH> Argument must be number!");
            
            return new Atom(AtomType.Number, (byte)Convert.ToInt64(atom.value));
        }

        public static Atom CastSByte(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, (sbyte)Convert.ToInt64(atom.value));
        }

        public static Atom CastChar(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, (char)Convert.ToChar(atom.value));
        }
        
        public static Atom CastUShort(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, (ushort)Convert.ToInt64(atom.value));
        }

        public static Atom CastSShort(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, (short)Convert.ToInt64(atom.value));
        }

        public static Atom CastUInt(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, (uint)Convert.ToInt64(atom.value));
        }

        public static Atom CastSInt(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, (int)Convert.ToInt64(atom.value));
        }

        public static Atom CastULong(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, Convert.ToUInt64(atom.value));
        }

        public static Atom CastSLong(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, Convert.ToInt64(atom.value));
        }

        public static Atom CastFloat(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, Convert.ToSingle(atom.value));
        }

        public static Atom CastDouble(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;
            if (atom.type != AtomType.Number)
                throw new BombardoException("<MATH> Argument must be number!");

            return new Atom(AtomType.Number, Convert.ToDouble(atom.value));
        }

        #endregion
    }
}