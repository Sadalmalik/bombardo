using System;

namespace Bombardo.V2
{
	public static class MathFunctions
	{
		public static void Define(Context ctx)
		{
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
            ctx.DefineFunction(Names.MATH_LOGN, Logn);
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

            ctx.Define(Names.MATH_PI, new Atom(AtomType.Number, Math.PI));
            ctx.Define(Names.MATH_E, new Atom(AtomType.Number, Math.E));

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
            ctx.DefineFunction(Names.MATH_CAST_DOUBLE, CastDouble);
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

        private static void Sum(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Sum(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Dis(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Dis(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Mul(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Mul(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Div(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Div(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Mod(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Mod(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        #endregion Math operators

        #region Math functions

        private static void Min(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Min(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Max(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Max(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Abs(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Abs(atom.value)) );
        }

        private static void Sign(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Sign(atom.value)) );
        }

        private static void Ceil(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Ceil(atom.value)) );
        }

        private static void Floor(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Floor(atom.value)) );
        }
        
        private static void Trunc(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Trunc(atom.value)) );
        }

        private static void Sqrt(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Sqrt(atom.value)) );
        }

        private static void Pow(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Pow(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Exp(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Exp(atom.value)) );
        }

        private static void Logn(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Logn(atom.value)) );
        }

        private static void Log(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Log(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Log10(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Log10(atom.value)) );
        }
        
        private static void Sin(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Sin(atom.value)) );
        }

        private static void Cos(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Cos(atom.value)) );
        }

        private static void Tan(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Tan(atom.value)) );
        }
        
        private static void Asin(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Asin(atom.value)) );
        }

        private static void Acos(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Acos(atom.value)) );
        }

        private static void Atan(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Atan(atom.value)) );
        }

        private static void Atan2(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Atan2(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }


        private static void Sinh(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Sinh(atom.value)) );
        }

        private static void Cosh(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Cosh(atom.value)) );
        }

        private static void Tanh(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, UNumber.Tanh(atom.value)) );
        }
        
        #endregion

        #region Binary operators

        private static void And(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.And(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Or(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Or(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Xor(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Xor(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Lsh(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Lsh(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        private static void Rsh(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            object res = ((Atom)frame.args?.value)?.value;
            for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
                res = UNumber.Rsh(res, ((Atom)iter.value).value);
            eval.SetReturn( new Atom(AtomType.Number, res) );
        }

        #endregion Binary operators

        #region Comparsion

        private static void Lt(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            for (Atom iter = frame.args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Lt(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    eval.SetReturn( Atoms.FALSE );
            eval.SetReturn( Atoms.TRUE );
        }

        private static void Gt(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            for (Atom iter = frame.args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Gt(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    eval.SetReturn( Atoms.FALSE );
            eval.SetReturn( Atoms.TRUE );
        }

        private static void Le(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            for (Atom iter = frame.args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Le(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    eval.SetReturn( Atoms.FALSE );
            eval.SetReturn( Atoms.TRUE );
        }

        private static void Ge(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            for (Atom iter = frame.args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Ge(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    eval.SetReturn( Atoms.FALSE );
            eval.SetReturn( Atoms.TRUE );
        }

        private static void Ne(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            for (Atom iter = frame.args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Ne(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    eval.SetReturn( Atoms.FALSE );
            eval.SetReturn( Atoms.TRUE );
        }

        private static void Eq(Evaluator eval, StackFrame frame)
        {
            CheckAllNumbers(frame.args);
            for (Atom iter = frame.args; iter != null && iter.next != null; iter = iter.next)
                if (!(bool)UNumber.Eq(
                    ((Atom)iter.value).value,
                    ((Atom)iter.next?.value).value))
                    eval.SetReturn( Atoms.FALSE );
            eval.SetReturn( Atoms.TRUE );
        }

        #endregion Comparsion

        #region Types Predicates

        private static void PredUByte(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.UBYTE)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredSByte(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.SBYTE)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }
        
        private static void PredChar(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.CHAR)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredUShort(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.USHORT)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredSShort(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.SSHORT)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredUInt(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.UINT)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredSInt(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.SINT)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredULong(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.ULONG)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredSLong(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.SLONG)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredFloat(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.FLOAT)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        private static void PredDouble(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type == AtomType.Number &&
                UNumber.NumberType(atom.value) == UNumber.DOUBLE)
                eval.SetReturn( Atoms.TRUE );
            eval.SetReturn( Atoms.FALSE );
        }

        #endregion

        #region Type Cast

        private static void CastUByte(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");
            
            eval.SetReturn( new Atom(AtomType.Number, (byte)Convert.ToInt64(atom.value)) );
        }

        private static void CastSByte(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, (sbyte)Convert.ToInt64(atom.value)) );
        }

        private static void CastChar(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, (char)Convert.ToChar(atom.value)) );
        }
        
        private static void CastUShort(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, (ushort)Convert.ToInt64(atom.value)) );
        }

        private static void CastSShort(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, (short)Convert.ToInt64(atom.value)) );
        }

        private static void CastUInt(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, (uint)Convert.ToInt64(atom.value)) );
        }

        private static void CastSInt(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, (int)Convert.ToInt64(atom.value)) );
        }

        private static void CastULong(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, Convert.ToUInt64(atom.value)) );
        }

        private static void CastSLong(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, Convert.ToInt64(atom.value)) );
        }

        private static void CastFloat(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, Convert.ToSingle(atom.value)) );
        }

        private static void CastDouble(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;
            if (atom.type != AtomType.Number)
                throw new ArgumentException("Argument must be number!");

            eval.SetReturn( new Atom(AtomType.Number, Convert.ToDouble(atom.value)) );
        }

        #endregion
    }
}