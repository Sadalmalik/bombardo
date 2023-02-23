using System;
using System.Linq;
using System.Threading;

namespace Bombardo.V2
{
	public class Evaluator
	{
		private Atom _retValue;
		private bool _haveReturn;
		
		public StackHandler Stack { get; }
		public string ErrorMessage { get; private set; } = null;

		public Evaluator()
		{
			Stack = new StackHandler(null);
		}

		public void SetError(string error)
		{
			ErrorMessage = error;
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
		
		public StackFrame CreateFrame(string state, Atom expression, Context context)
		{
			return Stack.CreateFrame(state, expression, context);
		}
		
		public StackFrame CreateFrame(string state, Atom expression, Atom context)
		{
			return Stack.CreateFrame(state, expression, context);
		}
		
		public void CloseFrame()
		{
			Stack.RemoveFrame();
		}
		
		public void Call(string state, Atom expression, Context context)
		{
			Stack.CreateFrame(state, expression, context);
		}
		
		public void Call(string state, Atom expression, Atom context)
		{
			Stack.CreateFrame(state, expression, context);
		}
		
		public void Return(Atom value)
		{
			_retValue = value;
			_haveReturn = true;
			Stack.RemoveFrame();
		}
		
		private static string GetType(Atom atom)
		{
			if (atom==null) return "<-:->";
			var strType = AtomType.ToString(atom.type);
			return $"<{atom.type}:{strType}>";
		}
		
		public Atom Evaluate(Atom atom, Context current_context, string startState = null)
		{
			_retValue = null;
			_haveReturn = false;
			Stack.CreateFrame(startState ?? "-eval-", atom, current_context);

			while (Stack.stack.Count > 0)
			{
				if (ErrorMessage != null)
				{
					Console.WriteLine($"[ERROR] {ErrorMessage}");
					ErrorMessage = null;
					return null;
				}
				
				Function func;
				StackFrame frame = Stack.TopFrame;
				switch (frame.state.value)
				{
					case "-eval-":	            State_Eval(frame);           continue;
					case "-eval-each-":         State_EvalEach(frame);       continue;
					case "-eval-block-":        State_EvalBlock(frame);      continue;
					
					case "-eval-sexp-head-":    State_EvalSExpHead(frame);   continue;
					case "-eval-sexp-args-":    State_EvalSExpArgs(frame);   continue;
					case "-eval-sexp-body-":    State_EvalSExpBody(frame);   continue;
					case "-eval-sexp-result-":  State_EvalSExpResult(frame); continue;
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
				SetError($"Wrong evaluation state: {frame.state.value}");
			}

			if (HaveReturn())
				return TakeReturn();
			
			return null;
		}

		private void State_Eval(StackFrame frame)
		{
			if (frame.expression == null)
			{
				Return(frame.expression);
				return;
			}
			
			if (frame.expression.IsPair)
			{
				frame.SetState("-eval-sexp-head-");
				return;
			}
			
			if (frame.expression.IsSymbol)
			{
				var name = (string)frame.expression.value;
				Atom value;
				try
				{
					value = ContextUtils.Get((Context)frame.context.value, name);
				}
				catch (BombardoException bex)
				{
					ErrorMessage = bex.Message;
					return;
				}
				Return(value);
				return;
			}
			
			Return(frame.expression);
		}
		
		private void State_EvalEach(StackFrame frame)
		{
			if (HaveReturn())
			{
				frame.temp1 = StructureUtils.BuildListContainer(frame.temp1, TakeReturn());
			}
			
			if (frame.expression != null)
			{
				var subExpression = frame.expression.atom;
				frame.expression = frame.expression.next;
				Call("-eval-", subExpression, frame.context);
				return;
			}
			
			Return(frame.temp1.atom);
		}

		private void State_EvalBlock(StackFrame frame)
		{
			if (HaveReturn())
			{
				frame.temp1 = TakeReturn();
			}
			
			if (frame.expression != null)
			{
				var subExpression = frame.expression.atom;
				frame.expression = frame.expression.next;
				Call("-eval-", subExpression, frame.context);
				return;
			}
			
			Return(frame.temp1);
		}

		private void State_EvalSExpHead(StackFrame frame)
		{
			if (!HaveReturn())
			{
				Call("-eval-", frame.expression.atom, frame.context);
				return;
			}
			
			frame.function = TakeReturn();
			
			if (!frame.function.IsFunction)
			{
				ErrorMessage = $"Head is not function: {frame.function} in {frame.expression}";
				return;
			}

			frame.SetState("-eval-sexp-args-");
		}

		private void State_EvalSExpArgs(StackFrame frame)
		{
			if (!HaveReturn())
			{
				var func = (Function)frame.function.value;
				if (func.EvalArgs && frame.expression.next != null)
				{
					Call("-eval-each-", frame.expression.next, frame.context);
					return;
				}
				
				frame.args = frame.expression.next;
				frame.SetState("-eval-sexp-body-");
				return;
			}

			frame.args = TakeReturn();
			frame.SetState("-eval-sexp-body-");
		}

		private void State_EvalSExpBody(StackFrame frame)
		{
			var func = (Function)frame.function.value;
			
			if (!HaveReturn())
			{
				func.Apply(this, frame);
				return;
			}
			
			if (func.EvalResult)
			{
				frame.SetState("-eval-sexp-result-");
				frame.temp1 = TakeReturn();
				return;
			}
			
			CloseFrame();
		}

		private void State_EvalSExpResult(StackFrame frame)
		{
			if (!HaveReturn())
			{
				Call("-eval-", frame.temp1, frame.context);
				return;
			}

			Return(TakeReturn());
		}
	}
}