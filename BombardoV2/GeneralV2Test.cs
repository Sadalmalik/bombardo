using System;
using Bombardo.V2.Lang;

namespace Bombardo.V2
{
	public static class GeneralV2Test
	{
		public static void DoTests()
		{
			Console.WriteLine("DoTests");
			
			Evaluator ev = new Evaluator();
			Console.WriteLine(">----------------------------------------------------------<");
			Eval(ev, "(map `[(a b) (c d) (e f)] car)");
			Console.WriteLine(">----------------------------------------------------------<");
			Eval(ev, "(map `[(a b) (c d) (e f)] cdr)");
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