using System;

namespace Bombardo.Core
{
    public class Function
    {
        public string Name;
        public Atom   tag;
        public Atom   self;
        public bool   EvalArgs;
        public bool   EvalResult;

        public Action<Evaluator, StackFrame> Perform;

        public Function(Action<Evaluator, StackFrame> function, bool evalArgs = true, bool evalResult = false)
        {
            Name       = "??";
            tag        = Atoms.BUILT_IN;
            self       = Atom.CreateFunction(this);
            Perform    = function;
            EvalArgs   = evalArgs;
            EvalResult = evalResult;
        }

        public Function(string name, Atom tag, Action<Evaluator, StackFrame> function, bool evalArgs = true,
            bool               evalResult = false)
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
                throw new InvalidProgramException($"Function {Name} have no executable code!");
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