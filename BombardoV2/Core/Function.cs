using System;

namespace Bombardo.V2
{
	public class Function
	{
		public string Name;
		public Atom tag;
		public bool EvalArgs;
		public bool EvalResult;
		public Action<Evaluator, StackFrame> Perform;

		public Function(Action<Evaluator, StackFrame> function, bool evalArgs = true, bool evalResult = false)
		{
			Name       = "??";
			tag        = new Atom("built-in");
			Perform    = function;
			EvalArgs   = evalArgs;
			EvalResult = evalResult;
		}

		public Function(string name, Atom tag, Action<Evaluator, StackFrame> function, bool evalArgs = true,
		                bool evalResult = false)
		{
			Name       = name;
			this.tag   = tag;
			Perform    = function;
			EvalArgs   = evalArgs;
			EvalResult = evalResult;
		}

		public virtual void Apply(Evaluator evaluator, StackFrame frame)
		{
			if (Perform == null)
			{
				throw new InvalidProgramException(string.Format("Function {0} have no executable code!", Name));
			}

			try
			{
				Perform(evaluator, frame);
			}
			catch (BombardoException exc)
			{
				throw exc;
			}
			catch (Exception exc)
			{
				throw new BombardoException(Name, exc);
			}
		}
	}
}