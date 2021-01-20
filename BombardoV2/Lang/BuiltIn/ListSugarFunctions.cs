namespace Bombardo.V2
{
	public static class ListSugarFunctions
	{
		public static void Define(Context ctx)
		{
			ctx.DefineFunction(Names.LISP_CAAR, Caar);
			ctx.DefineFunction(Names.LISP_CADR, Cadr);
			ctx.DefineFunction(Names.LISP_CDAR, Cdar);
			ctx.DefineFunction(Names.LISP_CDDR, Cddr);

			ctx.DefineFunction(Names.LISP_CAAAR, Caaar);
			ctx.DefineFunction(Names.LISP_CAADR, Caadr);
			ctx.DefineFunction(Names.LISP_CADAR, Cadar);
			ctx.DefineFunction(Names.LISP_CADDR, Caddr);
			ctx.DefineFunction(Names.LISP_CDAAR, Cdaar);
			ctx.DefineFunction(Names.LISP_CDADR, Cdadr);
			ctx.DefineFunction(Names.LISP_CDDAR, Cddar);
			ctx.DefineFunction(Names.LISP_CDDDR, Cdddr);

			ctx.DefineFunction(Names.LISP_CAAAAR, Caaaar);
			ctx.DefineFunction(Names.LISP_CAAADR, Caaadr);
			ctx.DefineFunction(Names.LISP_CAADAR, Caadar);
			ctx.DefineFunction(Names.LISP_CAADDR, Caaddr);
			ctx.DefineFunction(Names.LISP_CADAAR, Cadaar);
			ctx.DefineFunction(Names.LISP_CADADR, Cadadr);
			ctx.DefineFunction(Names.LISP_CADDAR, Caddar);
			ctx.DefineFunction(Names.LISP_CADDDR, Cadddr);
			ctx.DefineFunction(Names.LISP_CDAAAR, Cdaaar);
			ctx.DefineFunction(Names.LISP_CDAADR, Cdaadr);
			ctx.DefineFunction(Names.LISP_CDADAR, Cdadar);
			ctx.DefineFunction(Names.LISP_CDADDR, Cdaddr);
			ctx.DefineFunction(Names.LISP_CDDAAR, Cddaar);
			ctx.DefineFunction(Names.LISP_CDDADR, Cddadr);
			ctx.DefineFunction(Names.LISP_CDDDAR, Cdddar);
			ctx.DefineFunction(Names.LISP_CDDDDR, Cddddr);
		}
		
		private static void Caar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.atom); }
		private static void Cadr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.atom); }
		private static void Cdar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.next); }
		private static void Cddr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.next); }

		private static void Caaar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.atom?.atom); }
		private static void Caadr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.atom?.atom); }
		private static void Cadar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.next?.atom); }
		private static void Caddr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.next?.atom); }
		private static void Cdaar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.atom?.next); }
		private static void Cdadr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.atom?.next); }
		private static void Cddar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.next?.next); }
		private static void Cdddr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.next?.next); }

		private static void Caaaar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.atom?.atom?.atom); }
		private static void Caaadr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.atom?.atom?.atom); }
		private static void Caadar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.next?.atom?.atom); }
		private static void Caaddr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.next?.atom?.atom); }
		private static void Cadaar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.atom?.next?.atom); }
		private static void Cadadr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.atom?.next?.atom); }
		private static void Caddar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.next?.next?.atom); }
		private static void Cadddr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.next?.next?.atom); }
		private static void Cdaaar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.atom?.atom?.next); }
		private static void Cdaadr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.atom?.atom?.next); }
		private static void Cdadar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.next?.atom?.next); }
		private static void Cdaddr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.next?.atom?.next); }
		private static void Cddaar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.atom?.next?.next); }
		private static void Cddadr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.atom?.next?.next); }
		private static void Cdddar(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.atom?.next?.next?.next); }
		private static void Cddddr(Evaluator eval, StackFrame frame) { eval.SetReturn(((Atom)frame.args?.value)?.next?.next?.next?.next); }
	}
}