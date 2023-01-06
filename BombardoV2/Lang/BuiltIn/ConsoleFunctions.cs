using System;

namespace Bombardo.V2
{
	public static partial class Names
	{
		public static readonly string LISP_PRINT = "print";
		public static readonly string LISP_READ  = "read";
	}

	public static class ConsoleFunctions
	{
		public static void Define(Context ctx)
		{
			// (print "Hello, World") -> null
			//   Hello, World
			// (print "This is a" `symbol 23) -> null
			//   This is a symbol 23
			// (print `(some lisp structure)) -> null
			//   (some lisp structure)
			ctx.DefineFunction(Names.LISP_PRINT, Print);
			
			// (read) -> console user input
			ctx.DefineFunction(Names.LISP_READ, ReadLine);
		}

		private static void Print(Evaluator eval, StackFrame frame)
		{
			var args  = frame.args;
			var first = true;
			for (var iter = args; iter != null; iter = iter.next)
			{
				if (!first)
					Console.Write(" ");
				first = false;
				var value = iter.value as Atom;
				if (value == null)
					Console.Write("null");
				else if (value.type == AtomType.String ||
				         value.type == AtomType.Symbol)
					Console.Write(value.value);
				else
					Console.Write(value.Stringify());
			}

			Console.WriteLine();
			eval.Return(null);
		}

		private static void ReadLine(Evaluator eval, StackFrame frame)
		{
			var line = Console.ReadLine();
			eval.Return(new Atom(AtomType.String, line));
		}
	}
}