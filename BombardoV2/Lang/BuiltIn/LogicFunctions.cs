using System;

namespace Bombardo.V2
{
	public static partial class Names
	{
		public static readonly string LOGIC_EQ = "eq?";
		public static readonly string LOGIC_NEQ = "neq?";

		public static readonly string LOGIC_AND = "and";
		public static readonly string LOGIC_OR = "or";
		public static readonly string LOGIC_XOR = "xor";
		public static readonly string LOGIC_IMP = "imp";
		public static readonly string LOGIC_NOT = "not";
	}

	public class LogicFunctions
	{
		public static void Define(Context ctx)
		{
			ctx.DefineFunction(Names.LOGIC_EQ, Eq);
			ctx.DefineFunction(Names.LOGIC_NEQ, Neq);

			ctx.DefineFunction(Names.LOGIC_AND, And);
			ctx.DefineFunction(Names.LOGIC_OR, Or);
			ctx.DefineFunction(Names.LOGIC_XOR, Xor);
			ctx.DefineFunction(Names.LOGIC_IMP, Imp);
			ctx.DefineFunction(Names.LOGIC_NOT, Not);
		}

#region Equation

		private static void Nope(Evaluator eval, StackFrame frame)
		{
			eval.Return(null);
		}

		public static void Eq(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			//  Особый механизм - сравнение всего
			for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
				if (!StructureUtils.Compare((Atom) iter.value, (Atom) iter.next.value))
				{
					eval.Return(Atoms.FALSE);
					return;
				}

			eval.Return(Atoms.TRUE);
		}

		public static void Neq(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			//  Особый механизм - сравнение всего
			for (Atom iter = args; iter != null && iter.next != null; iter = iter.next)
				if (StructureUtils.Compare((Atom) iter.value, (Atom) iter.next.value))
				{
					eval.Return(Atoms.FALSE);
					return;
				}

			eval.Return(Atoms.TRUE);
		}

#endregion Equation

#region Boolean operations

		private static bool AllBoolean(Atom args)
		{
			for (Atom iter = args; iter != null; iter = iter.next)
				if (((Atom) iter.value).type != AtomType.Bool)
					return false;
			return true;
		}

		private static void CheckAllBoolean(Atom args)
		{
			if (!AllBoolean(args))
				throw new ArgumentException("Not all arguments are booleans!");
		}

		public static void And(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			CheckAllBoolean(args);
			bool res = (bool) ((Atom) args.value).value;
			for (Atom iter = args.next; iter != null; iter = iter.next)
				res &= (bool) ((Atom) iter.value).value;
			eval.Return(new Atom(AtomType.Bool, res));
		}

		public static void Or(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			CheckAllBoolean(args);
			bool res = (bool) ((Atom) args.value).value;
			for (Atom iter = args.next; iter != null; iter = iter.next)
				res |= (bool) ((Atom) iter.value).value;
			eval.Return(new Atom(AtomType.Bool, res));
		}

		public static void Xor(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			CheckAllBoolean(args);
			bool res = (bool) ((Atom) args.value).value;
			for (Atom iter = args.next; iter != null; iter = iter.next)
				res ^= (bool) ((Atom) iter.value).value;
			eval.Return(new Atom(AtomType.Bool, res));
		}

		public static void Imp(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			CheckAllBoolean(args);
			bool res = (bool) ((Atom) args.value).value;
			for (Atom iter = args.next; iter != null; iter = iter.next)
				res = !res | (bool) ((Atom) iter.value).value;
			eval.Return(new Atom(AtomType.Bool, res));
		}

		public static void Not(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			CheckAllBoolean(args);
			if (args.next != null)
				throw new ArgumentException("Too many arguments!");
			eval.Return(new Atom(AtomType.Bool, !(bool) ((Atom) args.value).value));
		}

#endregion Boolean operations
	}
}