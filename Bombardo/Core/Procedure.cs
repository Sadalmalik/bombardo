using System;

namespace Bombardo
{
    public class Procedure
    {
        public string Name;
        public int RequiresArgs;
        public bool EvalArgs;
        public bool EvalResult;
        public Func<Atom, Context, Atom> Perform;

        public Procedure(Func<Atom, Context, Atom> funct, int args = 0, bool evalArgs = true, bool evalResult = false)
        {
            Name = "??";
            Perform = funct;
            RequiresArgs = args;
            EvalArgs = evalArgs;
            EvalResult = evalResult;
        }

        public Procedure(string name, Func<Atom, Context, Atom> funct, int args = 0, bool evalArgs = true, bool evalResult = false)
        {
            Name = name;
            Perform = funct;
            RequiresArgs = args;
            EvalArgs = evalArgs;
            EvalResult = evalResult;
        }

        public virtual Atom Apply(Atom args, Context context)
        {
            if (RequiresArgs != 0 && (args==null || args.ListLength() < RequiresArgs))
            {
                throw new ArgumentException(string.Format("Function {0} requires {1} arguments!", Name, RequiresArgs));
            }
            if (Perform == null)
            {
                throw new InvalidProgramException(string.Format("Function {0} have no executable code!", Name));
            }

            try
            {
                return Perform(args, context);
            }
            catch (BombardoException exc) { throw exc; }
            catch (Exception exc) { throw new BombardoException(Name, exc); }
        }
    }
}