using System;

namespace Bombardo.V2
{
	public static partial class Names
	{
		public static readonly string LISP_NOPE = "nope";
		public static readonly string LISP_QUOTE = "quote";

		public static readonly string LISP_PARSE = "parse";
		public static readonly string LISP_EVAL = "eval";
		public static readonly string LISP_EVAL_EACH = "block";

		public static readonly string LISP_COND = "cond";
		public static readonly string LISP_IF = "if";
		public static readonly string LISP_WHILE = "while";
		public static readonly string LISP_UNTIL = "until";

		public static readonly string LISP_LAMBDA = "lambda";
		public static readonly string LISP_MACROS = "macros";
		public static readonly string LISP_PREPROCESSOR = "preprocessor";
		public static readonly string LISP_SYNTAX = "syntax";

		public static readonly string LISP_APPLY = "apply";
		public static readonly string LISP_MACRO_EXPAND = "macro-expand";

		public static readonly string LISP_ERROR = "error";
	}
	
	public static class ControlFunctions
	{
		public static void Define(Context ctx)
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

			ctx.DefineFunction(Names.LISP_NOPE, Nope, false);
			ctx.DefineFunction(Names.LISP_QUOTE, Quote, false);

			ctx.DefineFunction(Names.LISP_PARSE, Parse);
			ctx.DefineFunction(Names.LISP_EVAL, Eval);
			ctx.DefineFunction(Names.LISP_EVAL_EACH, EvalEach, false);

			ctx.DefineFunction(Names.LISP_COND, Cond, false);
			ctx.DefineFunction(Names.LISP_IF, If, false);
			ctx.DefineFunction(Names.LISP_WHILE, While, false);
			ctx.DefineFunction(Names.LISP_UNTIL, Until, false);

			//  (lambda (...) expression ...)
			//     = (syntax (lambda #T #F) (...) expression ...)
			//  (macros (...) expression ...)
			//     = (syntax (macros #F #T) (...) expression ...)
			//  (preprocessor (...) expression ...)
			//     = (syntax (preprocessor #F #F) (...) expression ...)
			//
			//  (syntax (tag #F #F) (...) expression ...)
			//  (syntax (tag #T #F) (...) expression ...)
			//  (syntax (tag #F #T) (...) expression ...)
			//  (syntax (tag #T #T) (...) expression ...)

			ctx.DefineFunction(Names.LISP_LAMBDA, Lambda, false);
			ctx.DefineFunction(Names.LISP_MACROS, Macros, false);
			ctx.DefineFunction(Names.LISP_PREPROCESSOR, Preprocessor, false);
			ctx.DefineFunction(Names.LISP_SYNTAX, Syntax, false);

			//ctx.DefineFunction(Names.LISP_MACROEXPAND, MacroExpand, 1, false);

			//  (apply function [arguments]) -> result of (function [arguments])
			ctx.DefineFunction(Names.LISP_APPLY, Apply);

			ctx.DefineFunction(Names.LISP_ERROR, Error);
		}

		private static void Nope(Evaluator eval, StackFrame frame)
		{
			eval.SetReturn(null);
		}

		private static void Quote(Evaluator eval, StackFrame frame)
		{
			eval.SetReturn(frame.args.atom);
		}

		private static void Parse(Evaluator eval, StackFrame frame)
		{
			Atom text = frame.args?.atom;

			if (text.type != AtomType.String)
				throw new ArgumentException("Argument must be string!");

			try
			{
				var list = BombardoLang.Parse((string) text.value);

				eval.SetReturn(list);
			}
			catch
			{
			}

			eval.SetReturn(null);
		}

		private static void Eval(Evaluator eval, StackFrame frame)
		{
			if (!eval.HaveReturn())
			{
				Atom    expression = frame.args?.atom;
				Atom    ctxAtom    = frame.args?.next?.atom;
				Context ctx        = ctxAtom?.value as Context ?? frame.context.value as Context;
				eval.CreateFrame("-eval-", expression, ctx);
			}
		}

		private static void EvalEach(Evaluator eval, StackFrame frame)
		{
			if (!eval.HaveReturn())
			{
				Atom    expression = frame.args?.atom;
				Atom    ctxAtom    = frame.args?.next?.atom;
				Context ctx        = ctxAtom?.value as Context;
				eval.CreateFrame("-eval-each-", expression, ctx);
			}
		}

		private static void Cond(Evaluator eval, StackFrame frame)
		{
			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					frame.temp1 = frame.args;
					frame.state = new Atom("-built-in-cond-head-");
					break;
				case "-built-in-cond-head-":
					if (eval.HaveReturn())
					{
						var condition = eval.TakeReturn();
						if (!condition.IsBool)
							throw new ArgumentException($"Condition must return boolean atom, but found: {condition}!");
						if ((bool) condition.value)
						{
							frame.state = new Atom("-built-in-cond-body-");
							break;
						}
					}

					if (frame.temp1 != null)
					{
						frame.temp2 = frame.temp1.atom;
						frame.temp1 = frame.temp1.next;

						if (!frame.temp2.IsPair)
						{
							throw new ArgumentException($"Condition element must be list, but found: {frame.temp2}!");
						}

						eval.CreateFrame("-eval-", frame.temp2.atom, frame.context);
					}
					else
					{
						//  Ни одно из условий не выполнилось
						frame.state = new Atom("-eval-sexp-body-");
						eval.SetReturn(Atoms.FALSE);
					}

					break;
				case "-built-in-cond-body-":
					if (eval.HaveReturn())
					{
						frame.state = new Atom("-eval-sexp-body-");
					}
					else
					{
						eval.CreateFrame("-eval-block-", frame.temp2.next, frame.context);
					}

					break;
			}
		}

		private static void If(Evaluator eval, StackFrame frame)
		{
			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					//  (cond BlockA BlockB)
					(frame.temp1, frame.temp2, frame.temp3) = StructureUtils.Split3(frame.args);
					frame.state                             = new Atom("-built-in-if-cond-");
					break;
				case "-built-in-if-cond-":
					if (eval.HaveReturn())
					{
						var condition = eval.TakeReturn();
						if (!condition.IsBool)
							throw new ArgumentException($"Condition must return boolean atom, but found: {condition}!");
						frame.state = new Atom((bool) condition.value ? "-built-in-if-then-" : "-built-in-if-else-");
					}
					else
					{
						eval.CreateFrame("-eval-", frame.temp1, frame.context);
					}

					break;
				case "-built-in-if-then-":
					frame.state = new Atom("-eval-sexp-body-");
					if (frame.temp2 == null)
						throw new ArgumentException($"If statement must have at least one block of code!");
					else
						eval.CreateFrame("-eval-block-", frame.temp2, frame.context);
					break;
				case "-built-in-if-else-":
					frame.state = new Atom("-eval-sexp-body-");
					if (frame.temp3 == null)
						eval.SetReturn(null);
					else
						eval.CreateFrame("-eval-block-", frame.temp3, frame.context);
					break;
			}
		}

		private static void While(Evaluator eval, StackFrame frame)
		{
			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					//  (Cond Body)
					(frame.temp1, frame.temp2) = (frame.args.atom, frame.args.next);
					frame.state                = new Atom("-built-in-while-cond-");
					break;
				case "-built-in-while-cond-":
					if (eval.HaveReturn())
					{
						var condition = eval.TakeReturn();
						if (!condition.IsBool)
							throw new ArgumentException($"Condition must return boolean atom, but found: {condition}!");
						if ((bool) condition.value)
						{
							frame.state = new Atom("-built-in-while-body-");
						}
						else
						{
							frame.state = new Atom("-eval-sexp-body-");
							eval.SetReturn(frame.temp3);
						}
					}
					else
					{
						eval.CreateFrame("-eval-", frame.temp1, frame.context);
					}

					break;
				case "-built-in-while-body-":
					if (eval.HaveReturn())
					{
						frame.temp3 = eval.TakeReturn();
						frame.state = new Atom("-built-in-while-cond-");
					}
					else
					{
						eval.CreateFrame("-eval-block-", frame.temp2, frame.context);
					}

					break;
			}
		}

		private static void Until(Evaluator eval, StackFrame frame)
		{
			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					//  (Cond Body)
					(frame.temp1, frame.temp2) = (frame.args.atom, frame.args.next);
					frame.state                = new Atom("-built-in-while-cond-");
					break;
				case "-built-in-while-cond-":
					if (eval.HaveReturn())
					{
						var condition = eval.TakeReturn();
						if (!condition.IsBool)
							throw new ArgumentException($"Condition must return boolean atom, but found: {condition}!");
						if (!(bool) condition.value)
						{
							frame.state = new Atom("-built-in-while-body-");
						}
						else
						{
							frame.state = new Atom("-eval-sexp-body-");
							eval.SetReturn(frame.temp3);
						}
					}
					else
					{
						eval.CreateFrame("-eval-", frame.temp1, frame.context);
					}

					break;
				case "-built-in-while-body-":
					if (eval.HaveReturn())
					{
						frame.temp3 = eval.TakeReturn();
						frame.state = new Atom("-built-in-while-cond-");
					}
					else
					{
						eval.CreateFrame("-eval-block-", frame.temp2, frame.context);
					}

					break;
			}
		}

		private static void Lambda(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom vars = StructureUtils.CloneTree((Atom) args?.value);
			Atom body = StructureUtils.CloneTree(args?.next);

			if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
				throw new ArgumentException("Args must be list or symbol!");

			var ctx     = frame.content.value as Context;
			var closure = new Closure(ctx, vars, body, "lambda");
			closure.EvalArgs   = true;
			closure.EvalResult = false;

			eval.SetReturn(new Atom(AtomType.Function, closure));
		}

		private static void Macros(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom vars = StructureUtils.CloneTree((Atom) args?.value);
			Atom body = StructureUtils.CloneTree(args?.next);

			if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
				throw new ArgumentException("Args must be list or symbol!");

			var ctx     = frame.content.value as Context;
			var closure = new Closure(ctx, vars, body, "macros");
			closure.EvalArgs   = false;
			closure.EvalResult = true;

			eval.SetReturn(new Atom(AtomType.Function, closure));
		}

		private static void Preprocessor(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom vars = StructureUtils.CloneTree((Atom) args?.value);
			Atom body = StructureUtils.CloneTree(args?.next);

			if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
				throw new ArgumentException("Args must be list or symbol!");

			var ctx     = frame.content.value as Context;
			var closure = new Closure(ctx, vars, body, "preprocessor");
			closure.EvalArgs   = false;
			closure.EvalResult = false;

			eval.SetReturn(new Atom(AtomType.Function, closure));
		}

		private static void Syntax(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			//  [tag before after]
			var (tag, before, after) = StructureUtils.Split3(args.atom);

			if (!tag.IsSymbol && !tag.IsString)
				throw new ArgumentException("Tag must be string or symbol!");
			if (!before.IsBool)
				throw new ArgumentException("Eval args flag must be boolean!");
			if (!after.IsBool)
				throw new ArgumentException("Eval result flag must be boolean!");

			Atom vars = StructureUtils.CloneTree(args?.next?.atom);
			Atom body = StructureUtils.CloneTree(args?.next?.next);

			if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
				throw new ArgumentException("Args must be list or symbol!");

			var ctx     = frame.content.value as Context;
			var closure = new Closure(ctx, vars, body, (string) tag.value);
			closure.EvalArgs   = (bool) before.value;
			closure.EvalResult = (bool) after.value;

			eval.SetReturn(new Atom(AtomType.Function, closure));
		}

		private static void Apply(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom func = args.atom;
			Atom rest = StructureUtils.CloneList(args.next.atom);

			Function proc = func?.value as Function;
			if (proc == null) throw new ArgumentException("First argument must be procedure!");

			eval.CreateFrame("-eval-", new Atom(func, rest), frame.context);
		}

		// private static Atom MacroExpand(Atom args, Context context)
		// {
		//     Atom expression = (Atom)args?.value;
		//     Atom head = Evaluator.Evaluate((Atom)expression?.value, context);
		//     Closure macros = (Closure)head?.value;
		//     Atom macroArgs = Atom.CloneTree(expression?.next);
		//
		//     if (macros == null)
		//         throw new ArgumentException("Macros is null!");
		//     
		//     Atom result = macros.Apply(macroArgs, context);
		//
		//     return result;
		// }

		private static void Error(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom tag  = (Atom) args?.value;
			if (args.next == null)
			{
				if (tag.type != AtomType.String &&
				    tag.type != AtomType.Symbol)
					throw new ArgumentException("Tag must be string or symbol!");
				throw new BombardoException((string) tag.value);
			}

			Atom text = (Atom) args?.next?.value;

			if (tag.type != AtomType.String &&
			    tag.type != AtomType.Symbol)
				throw new ArgumentException("Tag must be string or symbol!");

			if (text.type != AtomType.String)
				throw new ArgumentException("Text must be string!");

			throw new BombardoException($"<{tag.value}> {text.value}");
		}
	}
}