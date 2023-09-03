// ReSharper disable IdentifierTypo

namespace Bombardo.Core
{
	public static partial class Names
	{
		public static readonly string LISP_CAAR = "caar";
		public static readonly string LISP_CADR = "cadr";
		public static readonly string LISP_CDAR = "cdar";
		public static readonly string LISP_CDDR = "cddr";

		public static readonly string LISP_CAAAR = "caaar";
		public static readonly string LISP_CAADR = "caadr";
		public static readonly string LISP_CADAR = "cadar";
		public static readonly string LISP_CADDR = "caddr";
		public static readonly string LISP_CDAAR = "cdaar";
		public static readonly string LISP_CDADR = "cdadr";
		public static readonly string LISP_CDDAR = "cddar";
		public static readonly string LISP_CDDDR = "cdddr";

		public static readonly string LISP_CAAAAR = "caaaar";
		public static readonly string LISP_CAAADR = "caaadr";
		public static readonly string LISP_CAADAR = "caadar";
		public static readonly string LISP_CAADDR = "caaddr";
		public static readonly string LISP_CADAAR = "cadaar";
		public static readonly string LISP_CADADR = "cadadr";
		public static readonly string LISP_CADDAR = "caddar";
		public static readonly string LISP_CADDDR = "cadddr";
		public static readonly string LISP_CDAAAR = "cdaaar";
		public static readonly string LISP_CDAADR = "cdaadr";
		public static readonly string LISP_CDADAR = "cdadar";
		public static readonly string LISP_CDADDR = "cdaddr";
		public static readonly string LISP_CDDAAR = "cddaar";
		public static readonly string LISP_CDDADR = "cddadr";
		public static readonly string LISP_CDDDAR = "cdddar";
		public static readonly string LISP_CDDDDR = "cddddr";
	}
	
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
		
		private static void Caar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.atom ); }
		private static void Cadr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.atom ); }
		private static void Cdar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.next ); }
		private static void Cddr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.next ); }

		private static void Caaar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.atom?.atom ); }
		private static void Caadr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.atom?.atom ); }
		private static void Cadar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.next?.atom ); }
		private static void Caddr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.next?.atom ); }
		private static void Cdaar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.atom?.next ); }
		private static void Cdadr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.atom?.next ); }
		private static void Cddar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.next?.next ); }
		private static void Cdddr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.next?.next ); }

		private static void Caaaar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.atom?.atom?.atom ); }
		private static void Caaadr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.atom?.atom?.atom ); }
		private static void Caadar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.next?.atom?.atom ); }
		private static void Caaddr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.next?.atom?.atom ); }
		private static void Cadaar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.atom?.next?.atom ); }
		private static void Cadadr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.atom?.next?.atom ); }
		private static void Caddar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.next?.next?.atom ); }
		private static void Cadddr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.next?.next?.atom ); }
		private static void Cdaaar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.atom?.atom?.next ); }
		private static void Cdaadr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.atom?.atom?.next ); }
		private static void Cdadar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.next?.atom?.next ); }
		private static void Cdaddr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.next?.atom?.next ); }
		private static void Cddaar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.atom?.next?.next ); }
		private static void Cddadr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.atom?.next?.next ); }
		private static void Cdddar(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.atom?.next?.next?.next ); }
		private static void Cddddr(Evaluator eval, StackFrame frame) { eval.Return( frame.args?.atom?.next?.next?.next?.next ); }
	}
}