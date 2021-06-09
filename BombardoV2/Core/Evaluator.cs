using System;
using System.Threading;

namespace Bombardo.V2
{
	public class Evaluator
	{
		private Atom _retValue;
		private bool _haveReturn;
		
		public StackHandler Stack { get; }
		public string ErrorMessage { get; set; } = null;

		public Evaluator()
		{
			Stack = new StackHandler(null);
		}

		public void SetReturn(Atom value)
		{
			_retValue = value;
			_haveReturn = true;
		}

		public bool HaveReturn()
		{
			return _haveReturn;
		}

		public Atom TakeReturn()
		{
			var temp = _retValue;
			_retValue = null;
			_haveReturn = false;
			return temp;
		}
		
		public void CloseFrame()
		{
			Stack.RemoveFrame();
		}
		
		public Atom Evaluate(Atom atom, Context current_context)
		{
			_retValue = null;
			_haveReturn = false;
			Stack.CreateFrame("-eval-", atom, current_context);

			Console.WriteLine("Start evaluation");
			while (Stack.stack.Count > 0)
			{
				if (ErrorMessage != null)
				{
					Console.WriteLine($"[ERROR] {ErrorMessage}");
					ErrorMessage = null;
					return null;
				}
				
				Console.WriteLine($"RetValue: {_retValue}");
				Stack.Dump();
				Thread.Sleep(50);
				
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
							frame.temp1 = StructureUtils.BuildListContainer(frame.temp1, TakeReturn());
						if (frame.expression != null)
						{
							var subExpression = frame.expression.atom;
							frame.expression = frame.expression.next;
							Stack.CreateFrame("-eval-", subExpression, frame.context);
							continue;
						}
						SetReturn(frame.temp1.atom);
						// frame.temp1 = null;
						CloseFrame();
						continue;
						
					case "-eval-block-":
						if (HaveReturn())
							frame.temp1 = TakeReturn();
						if (frame.expression != null)
						{
							var subExpression = frame.expression.atom;
							frame.expression = frame.expression.next;
							Stack.CreateFrame("-eval-", subExpression, frame.context);
							continue;
						}
						SetReturn(frame.temp1);
						// frame.temp1 = null;
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
						if (func.EvalResult)
						{
							frame.state = new Atom("-eval-sexp-result-");
							frame.temp1 = TakeReturn();
							continue;
						}
						CloseFrame();
						continue;

					case "-eval-sexp-result-":
						if (!HaveReturn())
						{
							Stack.CreateFrame("-eval-", frame.temp1, frame.context);
							continue;
						}
						SetReturn(frame.temp1);
						frame.temp1 = null;
						CloseFrame();
						continue;
				}

				//	  Ни один из указанных стейтов, значит:
				//  Либо это стейты функции и тогда её надо вызывать
				if (frame.function != null)
				{
					func = (Function) frame.function.value;
					func.Apply(this, frame);
					continue;
				}

				//	Либо если нет функции - то это ошибка интерпретации
				ErrorMessage = $"Wrong evaluation state: {frame.state.value}";
			}

			if (HaveReturn())
				return TakeReturn();
			
			return null;
		}
	}
}