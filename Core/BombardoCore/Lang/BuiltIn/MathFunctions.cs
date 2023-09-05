using System;

namespace Bombardo.Core
{
    public static partial class Names
    {
        //  Constants

        public static readonly string MATH_PI = "PI";
        public static readonly string MATH_E  = "E";

        //  Base math

        public static readonly string MATH_SUM = "+";
        public static readonly string MATH_DIS = "-";
        public static readonly string MATH_MUL = "*";
        public static readonly string MATH_DIV = "/";
        public static readonly string MATH_MOD = "%";

        public static readonly string MATH_MIN   = "min";
        public static readonly string MATH_MAX   = "max";
        public static readonly string MATH_ABS   = "abs";
        public static readonly string MATH_SIGN  = "sign";
        public static readonly string MATH_CEIL  = "ceil";
        public static readonly string MATH_FLOOR = "floor";

        public static readonly string MATH_TRUNC = "trunc";
        public static readonly string MATH_SQRT  = "sqrt";
        public static readonly string MATH_POW   = "pow";
        public static readonly string MATH_EXP   = "exp";
        public static readonly string MATH_LOGN  = "ln";
        public static readonly string MATH_LOG   = "log";
        public static readonly string MATH_LOG10 = "ld";

        public static readonly string MATH_SIN   = "sin";
        public static readonly string MATH_COS   = "cos";
        public static readonly string MATH_TAN   = "tan";
        public static readonly string MATH_ASIN  = "asin";
        public static readonly string MATH_ACOS  = "acos";
        public static readonly string MATH_ATAN  = "atan";
        public static readonly string MATH_ATAN2 = "atan2";
        public static readonly string MATH_SINH  = "sinh";
        public static readonly string MATH_COSH  = "cosh";
        public static readonly string MATH_TANH  = "tanh";

        public static readonly string MATH_AND = "&";
        public static readonly string MATH_OR  = "|";
        public static readonly string MATH_XOR = "^";
        public static readonly string MATH_LSH = "<<";
        public static readonly string MATH_RSH = ">>";

        public static readonly string MATH_LT = "<";
        public static readonly string MATH_GT = ">";
        public static readonly string MATH_LE = "<=";
        public static readonly string MATH_GE = ">=";
        public static readonly string MATH_NE = "!=";
        public static readonly string MATH_EQ = "==";

        //  Type predicates

        public static readonly string MATH_PRED_BYTE  = "byte?";
        public static readonly string MATH_PRED_UBYTE = "ubyte?";
        public static readonly string MATH_PRED_SBYTE = "sbyte?";
        public static readonly string MATH_PRED_CHAR  = "char?";

        public static readonly string MATH_PRED_SSHORT = "short?";
        public static readonly string MATH_PRED_USHORT = "ushort?";

        public static readonly string MATH_PRED_SINT = "int?";
        public static readonly string MATH_PRED_UINT = "uint?";

        public static readonly string MATH_PRED_SLONG = "long?";
        public static readonly string MATH_PRED_ULONG = "ulong?";

        public static readonly string MATH_PRED_FLOAT   = "float?";
        public static readonly string MATH_PRED_DOUBLE  = "double?";
        public static readonly string MATH_PRED_DECIMAL = "decimal?";

        //  Type cast

        public static readonly string MATH_CAST__BYTE = "byte:";
        public static readonly string MATH_CAST_UBYTE = "ubyte:";
        public static readonly string MATH_CAST_SBYTE = "sbyte:";
        public static readonly string MATH_CAST_CHAR  = "char:";

        public static readonly string MATH_CAST_SSHORT = "short:";
        public static readonly string MATH_CAST_USHORT = "ushort:";

        public static readonly string MATH_CAST_SINT = "int:";
        public static readonly string MATH_CAST_UINT = "uint:";

        public static readonly string MATH_CAST_SLONG = "long:";
        public static readonly string MATH_CAST_ULONG = "ulong:";

        public static readonly string MATH_CAST_FLOAT   = "float:";
        public static readonly string MATH_CAST_DOUBLE  = "double:";
        public static readonly string MATH_CAST_DECIMAL = "decimal:";

        //  String parse

        public static readonly string MATH_PARSE = "tryParseNumber";
    }

    public static class MathFunctions
    {
        public static void Define(Context ctx)
        {
            //  Constants

            ctx.Define(Names.MATH_PI, Atom.CreateNumber(Math.PI));
            ctx.Define(Names.MATH_E, Atom.CreateNumber(Math.E));

            //  Base math

            ctx.DefineFunction(Names.MATH_SUM, Sum);
            ctx.DefineFunction(Names.MATH_DIS, Dis);
            ctx.DefineFunction(Names.MATH_MUL, Mul);
            ctx.DefineFunction(Names.MATH_DIV, Div);
            ctx.DefineFunction(Names.MATH_MOD, Mod);

            ctx.DefineFunction(Names.MATH_MIN, Min);
            ctx.DefineFunction(Names.MATH_MAX, Max);
            ctx.DefineFunction(Names.MATH_ABS, Abs);
            ctx.DefineFunction(Names.MATH_SIGN, Sign);
            ctx.DefineFunction(Names.MATH_CEIL, Ceil);
            ctx.DefineFunction(Names.MATH_FLOOR, Floor);

            ctx.DefineFunction(Names.MATH_TRUNC, Trunc);
            ctx.DefineFunction(Names.MATH_SQRT, Sqrt);
            ctx.DefineFunction(Names.MATH_POW, Pow);
            ctx.DefineFunction(Names.MATH_EXP, Exp);
            ctx.DefineFunction(Names.MATH_LOGN, Ln);
            ctx.DefineFunction(Names.MATH_LOG, Log);
            ctx.DefineFunction(Names.MATH_LOG10, Log10);

            ctx.DefineFunction(Names.MATH_SIN, Sin);
            ctx.DefineFunction(Names.MATH_COS, Cos);
            ctx.DefineFunction(Names.MATH_TAN, Tan);
            ctx.DefineFunction(Names.MATH_ASIN, Asin);
            ctx.DefineFunction(Names.MATH_ACOS, Acos);
            ctx.DefineFunction(Names.MATH_ATAN, Atan);
            ctx.DefineFunction(Names.MATH_ATAN2, Atan2);
            ctx.DefineFunction(Names.MATH_SINH, Sinh);
            ctx.DefineFunction(Names.MATH_COSH, Cosh);
            ctx.DefineFunction(Names.MATH_TANH, Tanh);

            ctx.DefineFunction(Names.MATH_AND, And);
            ctx.DefineFunction(Names.MATH_OR, Or);
            ctx.DefineFunction(Names.MATH_XOR, Xor);
            ctx.DefineFunction(Names.MATH_LSH, Lsh);
            ctx.DefineFunction(Names.MATH_RSH, Rsh);

            ctx.DefineFunction(Names.MATH_LT, Lt);
            ctx.DefineFunction(Names.MATH_GT, Gt);
            ctx.DefineFunction(Names.MATH_LE, Le);
            ctx.DefineFunction(Names.MATH_GE, Ge);
            ctx.DefineFunction(Names.MATH_NE, Ne);
            ctx.DefineFunction(Names.MATH_EQ, Eq);

            //  Type predicates

            ctx.DefineFunction(Names.MATH_PRED_BYTE, PredSByte);
            ctx.DefineFunction(Names.MATH_PRED_UBYTE, PredUByte);
            ctx.DefineFunction(Names.MATH_PRED_SBYTE, PredSByte);
            ctx.DefineFunction(Names.MATH_PRED_CHAR, PredChar);

            ctx.DefineFunction(Names.MATH_PRED_SSHORT, PredSShort);
            ctx.DefineFunction(Names.MATH_PRED_USHORT, PredUShort);

            ctx.DefineFunction(Names.MATH_PRED_SINT, PredSInt);
            ctx.DefineFunction(Names.MATH_PRED_UINT, PredUInt);

            ctx.DefineFunction(Names.MATH_PRED_SLONG, PredSLong);
            ctx.DefineFunction(Names.MATH_PRED_ULONG, PredULong);

            ctx.DefineFunction(Names.MATH_PRED_FLOAT, PredFloat);
            ctx.DefineFunction(Names.MATH_PRED_DOUBLE, PredDouble);
            ctx.DefineFunction(Names.MATH_PRED_DECIMAL, PredDecimal);


            //  Type cast

            ctx.DefineFunction(Names.MATH_CAST__BYTE, CastSByte);
            ctx.DefineFunction(Names.MATH_CAST_UBYTE, CastUByte);
            ctx.DefineFunction(Names.MATH_CAST_SBYTE, CastSByte);
            ctx.DefineFunction(Names.MATH_CAST_CHAR, CastChar);

            ctx.DefineFunction(Names.MATH_CAST_SSHORT, CastSShort);
            ctx.DefineFunction(Names.MATH_CAST_USHORT, CastUShort);

            ctx.DefineFunction(Names.MATH_CAST_SINT, CastSInt);
            ctx.DefineFunction(Names.MATH_CAST_UINT, CastUInt);

            ctx.DefineFunction(Names.MATH_CAST_SLONG, CastSLong);
            ctx.DefineFunction(Names.MATH_CAST_ULONG, CastULong);

            ctx.DefineFunction(Names.MATH_CAST_FLOAT, CastFloat);
            ctx.DefineFunction(Names.MATH_CAST_DECIMAL, CastDecimal);

            //  String parse

            ctx.DefineFunction(Names.MATH_PARSE, Parse);
        }


#region Internal stuff

        private static bool AllNumbers(Atom args)
        {
            for (Atom atom = args; atom != null; atom = atom.Next)
                if (atom.Head.type != AtomType.Number)
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

        private static void Sum(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Sum(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Dis(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Dis(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Mul(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Mul(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Div(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Div(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Mod(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Mod(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

#endregion Math operators


#region Math functions

        private static void Min(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Min(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Max(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Max(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Pow(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Pow(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Abs(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.IsNumber)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Abs(atom.number)));
        }

        private static void Sign(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Sign(atom.number)));
        }

        private static void Ceil(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Ceil(atom.number)));
        }

        private static void Floor(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Floor(atom.number)));
        }

        private static void Trunc(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Trunc(atom.number)));
        }

        private static void Sqrt(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Sqrt(atom.number)));
        }

        private static void Exp(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Exp(atom.number)));
        }

        private static void Ln(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Ln(atom.number)));
        }

        private static void Log(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Log(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Log10(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Log10(atom.number)));
        }

        private static void Sin(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Sin(atom.number)));
        }

        private static void Cos(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Cos(atom.number)));
        }

        private static void Tan(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Tan(atom.number)));
        }

        private static void Asin(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Asin(atom.number)));
        }

        private static void Acos(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Acos(atom.number)));
        }

        private static void Atan(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Atan(atom.number)));
        }

        private static void Atan2(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Atan2(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }


        private static void Sinh(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Sinh(atom.number)));
        }

        private static void Cosh(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Cosh(atom.number)));
        }

        private static void Tanh(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(AtomNumberOperations.Tanh(atom.number)));
        }

#endregion


#region Binary operators

        private static void And(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.And(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Or(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Or(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Xor(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Xor(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Lsh(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Lsh(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

        private static void Rsh(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            AtomNumber res = args.Head.number;
            for (Atom iter = args.Next; iter != null; iter = iter.Next)
                res = AtomNumberOperations.Rsh(res, iter.Head.number);
            eval.Return(Atom.CreateNumber(res));
        }

#endregion Binary operators


#region Comparsion

        private static void Lt(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            bool res = true;
            for (Atom iter = args; iter?.Next != null; iter = iter.Next)
                res &= AtomNumberOperations.Lt(iter.Head.number, iter.Next.Head.number);
            eval.Return(res ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void Gt(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            bool res = true;
            for (Atom iter = args; iter?.Next != null; iter = iter.Next)
                res &= AtomNumberOperations.Gt(iter.Head.number, iter.Next.Head.number);
            eval.Return(res ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void Le(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            bool res = true;
            for (Atom iter = args; iter?.Next != null; iter = iter.Next)
                res &= AtomNumberOperations.Le(iter.Head.number, iter.Next.Head.number);
            eval.Return(res ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void Ge(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            bool res = true;
            for (Atom iter = args; iter?.Next != null; iter = iter.Next)
                res &= AtomNumberOperations.Ge(iter.Head.number, iter.Next.Head.number);
            eval.Return(res ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void Ne(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            bool res = true;
            for (Atom iter = args; iter?.Next != null; iter = iter.Next)
                res &= AtomNumberOperations.Ne(iter.Head.number, iter.Next.Head.number);
            eval.Return(res ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void Eq(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            CheckAllNumbers(args);
            bool res = true;
            for (Atom iter = args; iter?.Next != null; iter = iter.Next)
                res &= AtomNumberOperations.Eq(iter.Head.number, iter.Next.Head.number);
            eval.Return(res ? Atoms.TRUE : Atoms.FALSE);
        }

#endregion Comparsion


#region Types Predicates

        private static void PredUByte(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.UINT_8;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredSByte(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.SINT_8;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredChar(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType._CHAR_;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredUShort(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.UINT16;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredSShort(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.SINT16;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredUInt(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.UINT32;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredSInt(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.SINT32;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredULong(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.UINT64;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredSLong(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.SINT64;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredFloat(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.SINGLE;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredDouble(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.DOUBLE;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredDecimal(Evaluator eval, StackFrame frame)
        {
            Atom atom   = frame.args.Head;
            bool result = atom.IsNumber && atom.type == AtomNumberType.DECIMAL;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

#endregion


#region Type Cast

        private static void CastUByte(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type      = AtomNumberType.UINT_8,
                val_uint8 = atom.number.ToUByte()
            }));
        }

        private static void CastSByte(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type      = AtomNumberType.SINT_8,
                val_sint8 = atom.number.ToSByte()
            }));
        }

        private static void CastChar(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type     = AtomNumberType._CHAR_,
                val_char = atom.number.ToChar()
            }));
        }

        private static void CastUShort(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.UINT16,
                val_uint16 = atom.number.ToUShort()
            }));
        }

        private static void CastSShort(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.SINT16,
                val_sint16 = atom.number.ToSShort()
            }));
        }

        private static void CastUInt(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.UINT32,
                val_uint32 = atom.number.ToUInt()
            }));
        }

        private static void CastSInt(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.SINT32,
                val_sint32 = atom.number.ToSInt()
            }));
        }

        private static void CastULong(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.UINT64,
                val_uint64 = atom.number.ToULong()
            }));
        }

        private static void CastSLong(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.SINT64,
                val_sint64 = atom.number.ToSLong()
            }));
        }

        private static void CastFloat(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.SINGLE,
                val_single = atom.number.ToSingle()
            }));
        }

        private static void CastDouble(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = atom.number.ToDouble()
            }));
        }

        private static void CastDecimal(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args.Head;

            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.Return(Atom.CreateNumber(new AtomNumber
            {
                type        = AtomNumberType.DECIMAL,
                val_decimal = atom.number.ToDecimal()
            }));
        }

#endregion


#region String parse

        private static void Parse(Evaluator eval, StackFrame frame)
        {
            Atom atom = frame.args?.Head;

            if (atom.type == AtomType.Symbol || atom.type == AtomType.String)
            {
                AtomNumber number = NumberParser.TryParseValue(atom.@string);
                if (number.type != AtomNumberType.NaN)
                {
                    eval.Return(Atom.CreateNumber(number));
                    return;
                }
            }

            eval.Return(null);
        }

#endregion
    }
}