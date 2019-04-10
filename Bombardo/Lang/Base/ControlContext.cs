using System;

namespace Bombardo
{
    internal class ControlContext
    {
        public static void Setup(Context context)
        {
            //  (nope anything) -> null
            //  (marker anything) -> null // Created for debugging in visual studio
            //  `anything -> (quote anything) -> anything
            //  (parse "expression") -> expression
            //  (eval expression) -> result of expression
            //  (evalEach (expression expression expression ...)) -> result of last expression
            //  (cond ((condition) expression) ((condition) expression) ...) -> result of success statement, or null
            //  (if condition expression [alternative-expression]) -> result of expression or alternative-expression if condition was false
            //  (while condition (expression expression expression ...)) -> evaluates expressions while condition is true, return null
            //  (until condition (expression expression expression ...)) -> same as (while (not condition) ...)

            BombardoLangClass.SetProcedure(context, AllNames.LISP_NOPE,     Nope, 0, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_MARKER,   Marker, 0, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_QUOTE,    Quote, 1, false);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_PARSE,    Parse, 1, true);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_EVAL,     Eval, 1, true);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_EVALEACH, EvalEach, 1, false);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_COND,     Cond, 2, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_IF,       If, 2, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_WHILE,    While, 1, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_UNTIL,    Until, 1, false);

            //  (lambda (...) expression)
            //  (macros (...) (list expression))
            //  (syntax (#F #F) (...) expression)
            //  (syntax (#T #F) (...) expression)
            //  (syntax (#F #T) (...) (list expression))
            //  (syntax (#T #T) (...) (list expression))

            BombardoLangClass.SetProcedure(context, AllNames.LISP_LAMBDA,   Lambda, 2, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_MACROS,   Macros, 2, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_SYNTAX,   Syntax, 3, false);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_MACROEXPAND, MacroExpand, 1, false);

            //  (apply function [arguments]) -> result of (function [arguments])
            BombardoLangClass.SetProcedure(context, AllNames.LISP_APPLY,        Apply, 1, true);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_ERROR,        Error, 1, true);
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
                throw new ArgumentException("Argument must be string!");

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
                    throw new ArgumentException(string.Format("Element '{0}' must be list!", element.ToString()));

                Atom condition = Evaluator.Evaluate((Atom)element.value, context);
                Atom block = element.next;

                if (condition.type != AtomType.Bool)
                {
                    throw new ArgumentException(string.Format("Condition must return boolean atom, but have {0}!", condition.ToString()));
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
                throw new ArgumentException(string.Format("Condition must return boolean atom, but have {0}!", condition.ToString()));
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
                throw new ArgumentException("Args must be list or symbol!");

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
                throw new ArgumentException("Args must be list or symbol!");

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
                throw new ArgumentException("First argument must be pair of flags!");

            Atom vars = Atom.CloneTree(args?.next?.atom);
            Atom body = Atom.CloneTree(args?.next?.next);

            if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
                throw new ArgumentException("Args must be list or symbol!");

            var closure = new Closure(context, vars, body);
            closure.EvalArgs = (bool)flgs.atom.value;
            closure.EvalResult = (bool)flgs.next.value;

            return new Atom(AtomType.Procedure, closure);
        }
        
        public static Atom Apply(Atom args, Context context)
        {
            Atom func = args.atom;
            Atom rest = Atom.CloneList(args.next.atom);

            if (func == null) throw new ArgumentException("First argument must be procedure!");

            Procedure proc = func.value as Procedure;
            if (proc == null) throw new ArgumentException("First argument must be procedure!");

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
                throw new ArgumentException("Macros is null!");
            
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
                    throw new ArgumentException("Tag must be string or symbol!");
                throw new BombardoException((string)tag.value);
            }

            Atom text = (Atom)args?.next?.value;

            if (tag.type != AtomType.String &&
                tag.type != AtomType.Symbol)
                throw new ArgumentException("Tag must be string or symbol!");

            if (text.type != AtomType.String)
                throw new ArgumentException("Text must be string!");

            throw new BombardoException(string.Format("<{0}> {1}", tag.value, text.value));
        }
    }
}