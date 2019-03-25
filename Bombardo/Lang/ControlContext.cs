using System;

namespace Bombardo
{
    internal class ControlContext
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, "nope", Nope, 0, false);
            BombardoLangClass.SetProcedure(context, "marker", Marker, 0, false);
            BombardoLangClass.SetProcedure(context, "quote", Quote, 1, false);

            BombardoLangClass.SetProcedure(context, "parse", Parse, 1, true);
            BombardoLangClass.SetProcedure(context, "eval", Eval, 1, true);
            BombardoLangClass.SetProcedure(context, "block", EvalEach, 1, false);
            
            BombardoLangClass.SetProcedure(context, "cond", Cond, 2, false);
            BombardoLangClass.SetProcedure(context, "if", If, 2, false);
            BombardoLangClass.SetProcedure(context, "while", While, 1, false);
            BombardoLangClass.SetProcedure(context, "until", Until, 1, false);

            BombardoLangClass.SetProcedure(context, "lambda", Lambda, 2, false);
            BombardoLangClass.SetProcedure(context, "macros", Macros, 2, false);
            BombardoLangClass.SetProcedure(context, "syntax", Syntax, 3, false);

            BombardoLangClass.SetProcedure(context, "apply", Apply, 1, true);
            BombardoLangClass.SetProcedure(context, "macro-expand", MacroExpand, 1, false);

            //  (lambda (...) expression)
            //  (macros (...) (list expression))
            //  (syntax (#F #F) (...) expression)
            //  (syntax (#T #F) (...) expression)
            //  (syntax (#F #T) (...) (list expression))
            //  (syntax (#T #T) (...) (list expression))

            BombardoLangClass.SetProcedure(context, "error", Error, 1, true);
        }

        public static Atom Nope(Atom args, Context context)
        {
            return null;
        }

        public static Atom Marker(Atom args, Context context)
        {
            Console.WriteLine("<Marker reached>");
            return null;
        }

        public static Atom Quote(Atom args, Context context)
        {
            return (Atom)args.value;
        }
        
        public static Atom Parse(Atom args, Context context)
        {
            Atom text = args?.atom;

            if (text.type != AtomType.String)
                throw new BombardoException(string.Format("<PARSE> Argument must be string!"));

            try
            {
                var atoms = BombardoLangClass.Parse((string)text?.value);
                return Atom.List(atoms.ToArray());
            }
            catch { }

            return null;
        }
        public static Atom Eval(Atom args, Context context)
        {
            Atom expression = args?.atom;
            Atom ctxAtom = args?.next?.atom;
            Context ctx = ctxAtom?.value as Context;
            if (ctx != null) context = ctx;
            return Evaluator.Evaluate(expression, context);
        }

        public static Atom EvalEach(Atom args, Context context)
        {
            return Evaluator.EvaluateEach(args, context);
        }

        public static Atom Cond(Atom args, Context context)
        {
            for (Atom iter = args; iter != null; iter = iter.next)
            {
                Atom element = (Atom)iter.value;
                if (element.type != AtomType.Pair)
                    throw new BombardoException(string.Format("<COND> element '{0}' must be list!", element.ToString()));

                Atom condition = Evaluator.Evaluate((Atom)element.value, context);
                Atom block = element.next;

                if (condition.type != AtomType.Bool)
                {
                    throw new BombardoException(string.Format("<COND> Condition must return boolean atom, but have {0}!", condition.ToString()));
                }
                if ((bool)condition.value)
                {
                    return Evaluator.EvaluateEach(block, context);
                }
            }
            return null;
        }

        public static Atom If(Atom args, Context context)
        {
            Atom condition = Evaluator.Evaluate((Atom)args.value, context);
            Atom blockA = (Atom)args.next.value;
            Atom blockB = null;
            if (args.next.next != null)
                blockB = (Atom)args.next.next.value;

            if (condition.type != AtomType.Bool)
            {
                throw new BombardoException(string.Format("<IF> Condition must return boolean atom, but have {0}!", condition.ToString()));
            }
            if ((bool)condition.value)
            {
                return Evaluator.Evaluate(blockA, context);
            }
            else if (blockB != null)
            {
                return Evaluator.Evaluate(blockB, context);
            }
            return null;
        }

        public static Atom While(Atom args, Context context)
        {
            Atom condition = (Atom)args.value;
            Atom body = args.next;

            Atom flag;
            while (
                null != (flag = Evaluator.Evaluate(condition, context)) &&
                flag.type == AtomType.Bool &&
                (bool)flag.value)
            {
                Evaluator.EvaluateEach(body, context);
            }

            return null;
        }

        public static Atom Until(Atom args, Context context)
        {
            Atom condition = (Atom)args.value;
            Atom body = args.next;

            Atom flag;
            while (
                null != (flag = Evaluator.Evaluate(condition, context)) &&
                flag.type == AtomType.Bool &&
                !(bool)flag.value)
            {
                Evaluator.EvaluateEach(body, context);
            }

            return null;
        }

        public static Atom Lambda(Atom args, Context context)
        {
            Atom vars = Atom.CloneTree((Atom)args?.value);
            Atom body = Atom.CloneTree(args?.next);

            if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
                throw new BombardoException("<LAMBDA> Args must be list or symbol!");

            var closure = new Closure(context, vars, body);
            closure.EvalArgs = true;
            closure.EvalResult = false;

            return new Atom(AtomType.Procedure, closure);
        }

        public static Atom Macros(Atom args, Context context)
        {
            Atom vars = Atom.CloneTree((Atom)args?.value);
            Atom body = Atom.CloneTree(args?.next);

            if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
                throw new BombardoException("<MACROS> Args must be list or symbol!");

            var closure = new Closure(context, vars, body);
            closure.EvalArgs = false;
            closure.EvalResult = true;

            return new Atom(AtomType.Procedure, closure);
        }

        public static Atom Syntax(Atom args, Context context)
        {
            Atom flgs = args?.atom;

            if (flgs.type != AtomType.Pair ||
                flgs.atom.type != AtomType.Bool ||
                flgs.next.type != AtomType.Bool)
                throw new BombardoException("<SYNTAX> First argument must be pair of flags!");

            Atom vars = Atom.CloneTree(args?.next?.atom);
            Atom body = Atom.CloneTree(args?.next?.next);

            if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
                throw new BombardoException("<SYNTAX> Args must be list or symbol!");

            var closure = new Closure(context, vars, body);
            closure.EvalArgs = (bool)flgs.atom.value;
            closure.EvalResult = (bool)flgs.next.value;

            return new Atom(AtomType.Procedure, closure);
        }
        
        public static Atom Apply(Atom args, Context context)
        {
            Atom func = args.atom;
            Atom rest = Atom.CloneList(args.next.atom);

            if (func == null) throw new BombardoException("<APPLY> First argument must be procedure!");

            Procedure proc = func.value as Procedure;
            if (proc == null) throw new BombardoException("<APPLY> First argument must be procedure!");

            Atom result = proc.Apply(rest, context);

            return proc.EvalResult ? Evaluator.Evaluate(result, context) : result;
        }

        public static Atom MacroExpand(Atom args, Context context)
        {
            Atom expression = (Atom)args?.value;
            Atom head = Evaluator.Evaluate((Atom)expression?.value, context);
            Closure macros = (Closure)head?.value;
            Atom macroArgs = Atom.CloneTree(expression?.next);

            if (macros == null)
                throw new BombardoException("<MACRO-EXPAND> Macros is null!");
            
            Atom result = macros.Apply(macroArgs, context);

            return result;
        }

        public static Atom Error(Atom args, Context context)
        {
            Atom tag = (Atom)args?.value;
            if (args.next == null)
            {
                if (tag.type != AtomType.String &&
                    tag.type != AtomType.Symbol)
                    throw new BombardoException("<ERROR> Tag must be string or symbol!");
                throw new BombardoException((string)tag.value);
            }

            Atom text = (Atom)args?.next?.value;

            if (tag.type != AtomType.String &&
                tag.type != AtomType.Symbol)
                throw new BombardoException("<ERROR> Tag must be string or symbol!");

            if (text.type != AtomType.String)
                throw new BombardoException("<ERROR> Text must be string!");

            throw new BombardoException(string.Format("<{0}> {1}", tag.value, text.value));
        }
    }
}