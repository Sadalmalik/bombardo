using System;

namespace Bombardo.V2
{
	public static partial class Names
	{
		public static readonly string LISP_PRINT = "print";
		public static readonly string LISP_READ = "read";
	}

	public static class ConsoleFunctions
	{
		public static void Define(Context ctx)
		{
			ctx.DefineFunction(Names.LISP_PRINT, Print);
			ctx.DefineFunction(Names.LISP_READ, ReadLine);
		}

		private static void Print(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var first = true;
			for (Atom iter = args; iter != null; iter = iter.next)
			{
				if (!first)
					Console.Write(" ");
				first = false;
				Atom value = iter.value as Atom;
				if (value == null)
					Console.Write("null");
				else if (value.type == AtomType.String ||
				         value.type == AtomType.Symbol)
					Console.Write(value.value);
				else
					Console.Write(value.Stringify(true));
			}

			Console.WriteLine();
			eval.Return( null );
		}

		private static void ReadLine(Evaluator eval, StackFrame frame)
		{
			string line = Console.ReadLine();
			eval.Return( new Atom(AtomType.String, line) );
		}
	}
}