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
			Console.WriteLine($"Try evaluate {rawExpression}");
			var expressions = BombardoLangClass.Parse(rawExpression);
			var time1 = DateTime.Now;
			Atom result = ev.Evaluate(expressions[0], BuildTestContext());
			var time2 = DateTime.Now;
			var res = (time2-time1).TotalMilliseconds;
			Console.WriteLine($"Evaluation done in {ev.count} steps\nResult: {result}\nEvaluation time: {res} ms");
		}
		
		
		public static Context BuildTestContext()
		{
			Context ctx = new Context();
			AddFunction(ctx, "+", Sum);
			AddFunction(ctx, "-", Sub);
			AddFunction(ctx, "*", Mul);
			AddFunction(ctx, "save/cc", SaveCC);
			AddFunction(ctx, "call/cc", CallCC);
			ListMethods.Define(ctx);
			return ctx;
		}
		
		private static void SaveCC(Evaluator eval, StackFrame frame)
		{
			
			//eval.SetReturn();
		}
		private static void CallCC(Evaluator eval, StackFrame frame)
		{
			
			//eval.SetReturn();
		}
		

		private static void AddFunction(
			Context ctx, string name,
			Action<Evaluator, StackFrame> rawFunction,
			bool evalArgs = true, bool evalResult = false)
		{
			ctx.Add(name, new Atom(AtomType.Function, new Function(name, Atoms.BUILT_IN, rawFunction, evalArgs, evalResult)));
		}

		public static void Sum(Evaluator eval, StackFrame frame)
		{
			Console.WriteLine($"Sum {frame.args}");
			
			var sum  = 0f;
			var iter = frame.args;
			while (iter != null)
			{
				sum  += (float) iter.atom.value;
				iter =  iter.next;
			}

			eval.SetReturn(new Atom(AtomType.Number, sum));
		}

		public static void Sub(Evaluator eval, StackFrame frame)
		{
			Console.WriteLine($"Sub {frame.args}");
			
			var iter = frame.args;
			var sub  = (float) iter.atom.value;
			iter = iter.next;
			while (iter != null)
			{
				sub  -= (float) iter.atom.value;
				iter =  iter.next;
			}

			eval.SetReturn(new Atom(AtomType.Number, sub));
		}

		public static void Mul(Evaluator eval, StackFrame frame)
		{
			Console.WriteLine($"Mul {frame.args}");
			var mul  = 1f;
			var iter = frame.args;
			while (iter != null)
			{
				mul  *= (float) iter.atom.value;
				iter =  iter.next;
			}

			eval.SetReturn(new Atom(AtomType.Number, mul));
		}
		
		
	}
}