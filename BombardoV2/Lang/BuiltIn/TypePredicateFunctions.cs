using System;

namespace Bombardo.V2
{
	public static class TypePredicateFunctions
	{
		public static void Define(Context ctx)
		{
			ctx.DefineFunction(Names.LISP_PRED_NULL, PredNull);
			ctx.DefineFunction(Names.LISP_PRED_EMPTY, PredEmpty);
			ctx.DefineFunction(Names.LISP_PRED_SYM, PredSym);
			ctx.DefineFunction(Names.LISP_PRED_PAIR, PredList);
			ctx.DefineFunction(Names.LISP_PRED_LIST, PredList);
			ctx.DefineFunction(Names.LISP_PRED_STRING, PredString);
			ctx.DefineFunction(Names.LISP_PRED_BOOL, PredBool);
			ctx.DefineFunction(Names.LISP_PRED_NUMBER, PredNumber);
			ctx.DefineFunction(Names.LISP_PRED_PROCEDURE, PredProcedure);
		}

		private static void PredNull(Evaluator eval, StackFrame frame)
		{
			eval.SetReturn(frame.args.value == null ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredEmpty(Evaluator eval, StackFrame frame)
		{
			var atom = frame.args.atom;
			var result = atom != null && atom.type == AtomType.Symbol;
			eval.SetReturn(result && atom.atom.IsEmpty ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredSym(Evaluator eval, StackFrame frame)
		{
			var atom = frame.args.atom;
			var result = atom != null && atom.type == AtomType.Symbol;
			eval.SetReturn(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredList(Evaluator eval, StackFrame frame)
		{
			var atom = frame.args.atom;
			var result = atom != null && atom.type == AtomType.Pair;
			eval.SetReturn(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredString(Evaluator eval, StackFrame frame)
		{
			var atom = frame.args.atom;
			var result = atom != null && atom.type == AtomType.String;
			eval.SetReturn(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredBool(Evaluator eval, StackFrame frame)
		{
			var atom = frame.args.atom;
			var result = atom != null && atom.type == AtomType.Bool;
			eval.SetReturn(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredNumber(Evaluator eval, StackFrame frame)
		{
			var atom = frame.args.atom;
			var result = atom != null && atom.type == AtomType.Number;
			eval.SetReturn(result ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void PredProcedure(Evaluator eval, StackFrame frame)
		{
			var atom = frame.args.atom;
			var result = atom != null && atom.type == AtomType.Function;
			eval.SetReturn(result ? Atoms.TRUE : Atoms.FALSE);
		}
	}
}