using System;

namespace Bombardo.V2
{
	public static class GeneralV2Test
	{
		public static void DoTests()
		{
			Console.WriteLine("DoTests");
			var expression = StructureUtils.List(new Atom("+"),
				                    BuildMathExpr("*", 5, 3, 7),
				                    BuildMathExpr("-", 63, 17, 2, 8, 5)
					);
			Console.WriteLine($"Try evaluate {expression}");
			Evaluator ev = new Evaluator();
			Console.WriteLine("Evaluator ready");
			var time1 = DateTime.Now;
			Atom result = ev.Evaluate(expression, BuildTestContext());
			var time2 = DateTime.Now;
			var res = (time2-time1).TotalMilliseconds;
			Console.WriteLine($"Evaluation done in {ev.count} steps\nResult: {result}\nEvaluation time: {res} ms");
		}

		private static Atom BuildMathExpr(string oper, params float[] args)
		{
			var iter = new Atom(new Atom(oper), null);
			var head = iter;
			foreach (var arg in args)
			{
				iter.next = new Atom(new Atom(AtomType.Number, arg), null);
				iter      = iter.next;
			}

			return head;
		}

		public static Context BuildTestContext()
		{
			Context ctx = new Context();
			AddFunction(ctx, "+", Sum);
			AddFunction(ctx, "-", Sub);
			AddFunction(ctx, "*", Mul);
			return ctx;
		}

		private static void AddFunction(
			Context ctx, string name,
			Action<Evaluator, StackFrame> rawFunction,
			bool evalArgs = true, bool evalResult = false)
		{
			ctx.Add(name, new Atom(AtomType.Function, new Function(name, rawFunction, evalArgs, evalResult)));
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