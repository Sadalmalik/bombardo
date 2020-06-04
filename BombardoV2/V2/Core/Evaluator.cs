using System;
using System.Threading;

namespace Bombardo.V2
{
	public class Evaluator
	{
		private Atom _retValue;
		public StackHandler Stack { get; }

		public Evaluator()
		{
			Stack = new StackHandler(null);
		}

		public void SetReturn(Atom value)
		{
			_retValue = value;
		}

		public bool HaveReturn()
		{
			return _retValue != null;
		}

		public Atom TakeReturn()
		{
			var temp = _retValue;
			_retValue = null;
			return temp;
		}
		
		public void CloseFrame()
		{
			Stack.RemoveFrame();
		}
		
		public int count = 0;
		public Atom Evaluate(Atom atom, Context current_context)
		{
			_retValue = null;
			Stack.CreateFrame("-eval-", atom, current_context);

			Console.WriteLine("Start evaluation");
			while (Stack.stack.Count > 0)
			{
				count++;
				// Console.WriteLine($"RetValue: {_retValue}");
				// Stack.Dump();
				// Thread.Sleep(50);
				
				Function func;
				StackFrame frame = Stack.TopFrame;
				switch (frame.state.value)
				{
					case "-eval-":
						if (frame.expression.IsPair)
						{
							frame.state = new Atom("-eval-sexp-head-");
							continue;
						}
						if (frame.expression.IsSymbol)
						{
							var name = (string) frame.expression.value;
							try
							{
								SetReturn(ContextUtils.Get((Context) frame.context.value, name));
							}
							catch (BombardoException e)
							{
								Console.WriteLine($"[ERROR] {e}");
								return null;
							}
							CloseFrame();
							continue;
						}
						SetReturn(frame.expression);
						CloseFrame();
						continue;
						
					case "-eval-each-":
						if (HaveReturn())
							frame.meta = StructureUtils.BuildListContainer(frame.meta, TakeReturn());
						if (frame.expression != null)
						{
							var subExpression = frame.expression.atom;
							frame.expression = frame.expression.next;
							Stack.CreateFrame("-eval-", subExpression, frame.context);
							continue;
						}
						SetReturn(frame.meta.atom);
						CloseFrame();
						continue;
						
					case "-eval-block-":
						if (HaveReturn())
							frame.meta = TakeReturn();
						if (frame.expression != null)
						{
							var subExpression = frame.expression.atom;
							frame.expression = frame.expression.next;
							Stack.CreateFrame("-eval-", subExpression, frame.context);
							continue;
						}
						SetReturn(frame.meta);
						CloseFrame();
						continue;

					case "-eval-sexp-head-":
						if (!HaveReturn())
						{
							var head = frame.expression.atom;
							Stack.CreateFrame("-eval-", head, frame.context);
							continue;
						}

						frame.function = TakeReturn();
						if (!frame.function.IsFunction)
						{
							Console.WriteLine($"[ERROR] Head is not function: {frame.function}");
							return null;
						}
						
						frame.state = new Atom("-eval-sexp-args-");
						continue;

					case "-eval-sexp-args-":
						if (!HaveReturn())
						{
							func = (Function) frame.function.value;
							if (func.EvalArgs)
							{
								Stack.CreateFrame("-eval-each-", frame.expression.next, frame.context);
								continue;
							}
							frame.args  = frame.expression.next;
							frame.state = new Atom("-eval-sexp-body-");
							continue;
						}
						
						frame.args  = TakeReturn();
						frame.state = new Atom("-eval-sexp-body-");
						continue;

					case "-eval-sexp-body-":
						func = (Function) frame.function.value;
						if (!HaveReturn())
						{
							func.Apply(this, frame);
							continue;
						}
						frame.state = new Atom("-eval-sexp-result-");
						frame.meta = TakeReturn();
						continue;

					case "-eval-sexp-result-":
						func = (Function) frame.function.value;
						if (!HaveReturn())
						{
							if (func.EvalResult)
							{
								Stack.CreateFrame("-eval-", frame.meta, frame.context);
								continue;
							}
							SetReturn(frame.meta);
							CloseFrame();
							continue;
						}
						CloseFrame();
						continue;
				}

				//	Ни один из указанных стейтов - либо стейты функции и тогда её надо вызывать
				if (frame.function != null)
				{
					func = (Function) frame.function.value;
					func.Apply(this, frame);
					continue;
				}

				//	Либо если нет функции - то это ошибка интерпретации
				Console.WriteLine($"[ERROR] Wrong evaluation state: {frame.state.value}");
				return null;
			}

			if (HaveReturn())
				return TakeReturn();
			
			return null;
		}
	}
}