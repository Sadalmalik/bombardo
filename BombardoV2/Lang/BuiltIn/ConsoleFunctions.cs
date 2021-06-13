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
			for (Atom iter = args; iter != null; iter = iter.next)
			{
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
			eval.SetReturn(null);
		}

		private static void ReadLine(Evaluator eval, StackFrame frame)
		{
			string line = Console.ReadLine();
			eval.SetReturn(new Atom(AtomType.String, line));
		}
	}
}