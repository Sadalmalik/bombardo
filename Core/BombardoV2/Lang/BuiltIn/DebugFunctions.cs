using System;

namespace Bombardo.V2
{
	public static partial class Names
	{
		public static readonly string LISP_MARKER = "marker";
	}

	public class DebugFunctions
	{
		public static void Define(Context ctx)
		{
			// Created for debugging in visual studio
			//  (marker anything) -> null

			ctx.DefineFunction(Names.LISP_MARKER, Marker, false);
		}

		private static void Marker(Evaluator eval, StackFrame frame)
		{
			var tag = frame.args?.atom?.value;
			Console.WriteLine(tag == null ? "<Marker reached>" : $"<Marker reached: {frame.args.atom}>");
			eval.Return(null);
		}
	}
}