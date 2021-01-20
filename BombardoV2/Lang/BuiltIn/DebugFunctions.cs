using System;

namespace Bombardo.V2
{
	public class DebugFunctions
	{
		public static void Define(Context ctx)
		{
			ctx.DefineFunction(Names.LISP_MARKER, Marker, false);
		}

		private static void Marker(Evaluator eval, StackFrame frame)
		{
			Console.WriteLine("<Marker reached>");
			eval.SetReturn(null);
		}
	}
}