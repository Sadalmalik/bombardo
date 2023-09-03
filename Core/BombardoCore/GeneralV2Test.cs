using System;

namespace Bombardo.Core
{
	public static class GeneralV2Test
	{
		public static void DoTests()
		{
			Console.WriteLine("DoTests");
			
			Console.WriteLine(">----------------------------------------------------------<");
			var content = "(require lang)\n(map 1 2 3)\n(print \"test\")";
			var atom = BombardoLang.Parse(content);
			Console.WriteLine($"Parse:\n\n{content}\n\n");
			Console.WriteLine($"Results:\n\n{atom}\n\n");
			Console.WriteLine(">----------------------------------------------------------<");
		}
		
		public static void Eval(Evaluator ev, string rawExpression)
		{
			// Console.WriteLine($"Try evaluate {rawExpression}");
			// var expressions = BombardoLangClass.Parse(rawExpression);
			// var time1 = DateTime.Now;
			// Atom result = ev.Evaluate(expressions[0], BuildTestContext());
			// var time2 = DateTime.Now;
			// var res = (time2-time1).TotalMilliseconds;
			// Console.WriteLine($"Evaluation done in {ev.count} steps\nResult: {result}\nEvaluation time: {res} ms");
		}
		
		
		private static void SaveCC(Evaluator eval, StackFrame frame)
		{
			
			//eval.SetReturn();
		}
		private static void CallCC(Evaluator eval, StackFrame frame)
		{
			
			//eval.SetReturn();
		}
	}
}