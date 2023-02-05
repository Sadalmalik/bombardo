using System;

namespace Bombardo.V2
{
	public static partial class Names
	{
		//  Constants

		public static readonly string MATH_PI = "PI";
		public static readonly string MATH_E = "E";

		//  Base math

		public static readonly string MATH_SUM = "+";
		public static readonly string MATH_DIS = "-";
		public static readonly string MATH_MUL = "*";
		public static readonly string MATH_DIV = "/";
		public static readonly string MATH_MOD = "%";

		public static readonly string MATH_MIN = "min";
		public static readonly string MATH_MAX = "max";
		public static readonly string MATH_ABS = "abs";
		public static readonly string MATH_SIGN = "sign";
		public static readonly string MATH_CEIL = "ceil";
		public static readonly string MATH_FLOOR = "floor";

		public static readonly string MATH_TRUNC = "trunc";
		public static readonly string MATH_SQRT = "sqrt";
		public static readonly string MATH_POW = "pow";
		public static readonly string MATH_EXP = "exp";
		public static readonly string MATH_LOGN = "ln";
		public static readonly string MATH_LOG = "log";
		public static readonly string MATH_LOG10 = "ld";

		public static readonly string MATH_SIN = "sin";
		public static readonly string MATH_COS = "cos";
		public static readonly string MATH_TAN = "tan";
		public static readonly string MATH_ASIN = "asin";
		public static readonly string MATH_ACOS = "acos";
		public static readonly string MATH_ATAN = "atan";
		public static readonly string MATH_ATAN2 = "atan2";
		public static readonly string MATH_SINH = "sinh";
		public static readonly string MATH_COSH = "cosh";
		public static readonly string MATH_TANH = "tanh";

		public static readonly string MATH_AND = "&";
		public static readonly string MATH_OR = "|";
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

		public static readonly string MATH_PRED_BYTE = "byte?";
		public static readonly string MATH_PRED_UBYTE = "ubyte?";
		public static readonly string MATH_PRED_SBYTE = "sbyte?";
		public static readonly string MATH_PRED_CHAR = "char?";

		public static readonly string MATH_PRED_SSHORT = "short?";
		public static readonly string MATH_PRED_USHORT = "ushort?";

		public static readonly string MATH_PRED_SINT = "int?";
		public static readonly string MATH_PRED_UINT = "uint?";

		public static readonly string MATH_PRED_SLONG = "long?";
		public static readonly string MATH_PRED_ULONG = "ulong?";

		public static readonly string MATH_PRED_FLOAT = "float?";
		public static readonly string MATH_PRED_DOUBLE = "double?";

		//  Type cast

		public static readonly string MATH_CAST__BYTE = "byte:";
		public static readonly string MATH_CAST_UBYTE = "ubyte:";
		public static readonly string MATH_CAST_SBYTE = "sbyte:";
		public static readonly string MATH_CAST_CHAR = "char:";

		public static readonly string MATH_CAST_SSHORT = "short:";
		public static readonly string MATH_CAST_USHORT = "ushort:";

		public static readonly string MATH_CAST_SINT = "int:";
		public static readonly string MATH_CAST_UINT = "uint:";

		public static readonly string MATH_CAST_SLONG = "long:";
		public static readonly string MATH_CAST_ULONG = "ulong:";

		public static readonly string MATH_CAST_FLOAT = "float:";
		public static readonly string MATH_CAST_DOUBLE = "double:";
		
		//  String parse
		
		public static readonly string MATH_PARSE = "tryParseNumber";
	}

	public static class MathFunctions
	{
		public static void Define(Context ctx)
		{
			//  Constants

			ctx.Define(Names.MATH_PI, new Atom(AtomType.Number, Math.PI));
			ctx.Define(Names.MATH_E, new Atom(AtomType.Number, Math.E));

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
			
			//  String parse
		
			ctx.DefineFunction(Names.MATH_PARSE, Parse);
		}


#region Inner stuff

		private static bool AllNumbers(Atom args)
		{
			for (Atom atom = args; atom != null; atom = atom.next)
				if (((Atom) atom.value).type != AtomType.Number)
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
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Sum(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

		private static void Dis(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Dis(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

		private static void Mul(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Mul(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

		private static void Div(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Div(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

		private static void Mod(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Mod(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

#endregion Math operators

#region Math functions

		private static void Min(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Min(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

		private static void Max(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Max(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

		private static void Abs(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Abs(atom.value)));
		}

		private static void Sign(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Sign(atom.value)));
		}

		private static void Ceil(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Ceil(atom.value)));
		}

		private static void Floor(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Floor(atom.value)));
		}

		private static void Trunc(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Trunc(atom.value)));
		}

		private static void Sqrt(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Sqrt(atom.value)));
		}

		private static void Pow(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Pow(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

		private static void Exp(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Exp(atom.value)));
		}

		private static void Logn(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Logn(atom.value)));
		}

		private static void Log(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Log(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}

		private static void Log10(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Log10(atom.value)));
		}

		private static void Sin(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Sin(atom.value)));
		}

		private static void Cos(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Cos(atom.value)));
		}

		private static void Tan(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Tan(atom.value)));
		}

		private static void Asin(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Asin(atom.value)));
		}

		private static void Acos(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Acos(atom.value)));
		}

		private static void Atan(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Atan(atom.value)));
		}

		private static void Atan2(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object res = ((Atom) frame.args?.value)?.value;
			for (Atom iter = frame.args?.next; iter != null; iter = iter.next)
				res = UNumber.Atan2(res, ((Atom) iter.value).value);
			eval.Return(new Atom(AtomType.Number, res));
		}


		private static void Sinh(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Sinh(atom.value)));
		}

		private static void Cosh(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Cosh(atom.value)));
		}

		private static void Tanh(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, UNumber.Tanh(atom.value)));
		}

#endregion

#region Binary operators

		private static object BitOperation(Atom list, Func<object, object, object> handle)
		{
			object result = list.atom.value;
			StructureUtils.Each(list.next, atom => { result = handle(result, atom.value); });
			return result;
		}

		private static void And(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object result = BitOperation(frame.args, UNumber.And);
			eval.Return(new Atom(AtomType.Number, result));
		}

		private static void Or(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object result = BitOperation(frame.args, UNumber.Or);
			eval.Return(new Atom(AtomType.Number, result));
		}

		private static void Xor(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object result = BitOperation(frame.args, UNumber.Xor);
			eval.Return(new Atom(AtomType.Number, result));
		}

		private static void Lsh(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object result = BitOperation(frame.args, UNumber.Lsh);
			eval.Return(new Atom(AtomType.Number, result));
		}

		private static void Rsh(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			object result = BitOperation(frame.args, UNumber.Rsh);
			eval.Return(new Atom(AtomType.Number, result));
		}

#endregion Binary operators

#region Comparsion

		private static bool CheckLogic(Atom list, Func<object, object, bool> check)
		{
			bool result = true;
			StructureUtils.Each2(list, (a, b) => { result &= check(a.value, b.value); });
			return result;
		}

		private static void Lt(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			bool result = CheckLogic(frame.args, UNumber.Lt);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void Gt(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			bool result = CheckLogic(frame.args, UNumber.Gt);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void Le(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			bool result = CheckLogic(frame.args, UNumber.Le);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void Ge(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			bool result = CheckLogic(frame.args, UNumber.Ge);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void Ne(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			bool result = CheckLogic(frame.args, UNumber.Ne);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void Eq(Evaluator eval, StackFrame frame)
		{
			CheckAllNumbers(frame.args);
			bool result = CheckLogic(frame.args, UNumber.Eq);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

#endregion Comparsion

#region Types Predicates

		private static bool CheckNumberType(Atom atom, int type)
		{
			return atom.type == AtomType.Number && UNumber.NumberType(atom.value) == type;
		}

		private static void PredUByte(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.UBYTE);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredSByte(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.SBYTE);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredChar(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.CHAR);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredUShort(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.USHORT);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredSShort(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.SSHORT);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredUInt(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.UINT);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredSInt(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.SINT);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredULong(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.ULONG);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredSLong(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.SLONG);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredFloat(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.FLOAT);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredDouble(Evaluator eval, StackFrame frame)
		{
			Atom atom   = (Atom) frame.args?.value;
			bool result = CheckNumberType(atom, UNumber.DOUBLE);
			eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
		}

#endregion

#region Type Cast

		private static void CastUByte(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, (byte) Convert.ToInt64(atom.value)));
		}

		private static void CastSByte(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, (sbyte) Convert.ToInt64(atom.value)));
		}

		private static void CastChar(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, (char) Convert.ToChar(atom.value)));
		}

		private static void CastUShort(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, (ushort) Convert.ToInt64(atom.value)));
		}

		private static void CastSShort(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, (short) Convert.ToInt64(atom.value)));
		}

		private static void CastUInt(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, (uint) Convert.ToInt64(atom.value)));
		}

		private static void CastSInt(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, (int) Convert.ToInt64(atom.value)));
		}

		private static void CastULong(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, Convert.ToUInt64(atom.value)));
		}

		private static void CastSLong(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, Convert.ToInt64(atom.value)));
		}

		private static void CastFloat(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, Convert.ToSingle(atom.value)));
		}

		private static void CastDouble(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			if (atom.type != AtomType.Number)
				throw new ArgumentException("Argument must be number!");

			eval.Return(new Atom(AtomType.Number, Convert.ToDouble(atom.value)));
		}

#endregion

#region String parse

		private static void Parse(Evaluator eval, StackFrame frame)
		{
			Atom atom = (Atom) frame.args?.value;
			
			if (atom.type != AtomType.Symbol && atom.type != AtomType.String)
				throw new ArgumentException("Argument must be symbol or string!");

			int type = 0;
			object value = null;
			if (NumberParser.TryParseValue(atom.value as string, ref type, ref value))
			{
				eval.Return(new Atom(type, value));
			}
			else
			{
				eval.Return(null);
			}
		}

#endregion
	}
}