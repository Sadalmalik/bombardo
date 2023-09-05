using System;

namespace Bombardo.Core
{
	public static partial class Names
	{
		public static readonly string LISP_NOPE         = "nope";
		public static readonly string LISP_QUOTE        = "quote";
		public static readonly string LISP_PARSE        = "parse";
		public static readonly string LISP_EVAL         = "eval";
		public static readonly string LISP_EVAL_EACH    = "evalEach";
		public static readonly string LISP_EVAL_BLOCK   = "block";
		public static readonly string LISP_IF           = "if";
		public static readonly string LISP_COND         = "cond";
		public static readonly string LISP_WHILE        = "while";
		public static readonly string LISP_UNTIL        = "until";
		public static readonly string LISP_LAMBDA       = "lambda";
		public static readonly string LISP_MACROS       = "macros";
		public static readonly string LISP_SYNTAX       = "syntax";
		public static readonly string LISP_APPLY        = "apply";
		public static readonly string LISP_MACRO_EXPAND = "macro-expand";
		public static readonly string LISP_ERROR        = "error";
	}

	public static class ControlFunctions
	{
		public static void Define(Context ctx)
		{
			//  (nope anything) -> null - fully ignoring whole tree
			ctx.DefineFunction(Names.LISP_NOPE, Nope, false);

			//  `anything -> (quote anything) -> anything
			ctx.DefineFunction(Names.LISP_QUOTE, Quote, false);


			//  (parse "expression") -> expression
			ctx.DefineFunction(Names.LISP_PARSE, Parse);

			//  (eval expression) -> result of expression
			//  (eval context expression) -> result of expression in context
			ctx.DefineFunction(Names.LISP_EVAL, Eval);

			//  (evalEach (expression expression expression ...)) -> result of last expression
			//  (evalEach context (expression expression expression ...)) -> result of last expression in context
			ctx.DefineFunction(Names.LISP_EVAL_EACH, EvalEach);

			//  (block expression expression expression ...) -> result of last expression
			//  (block context expression expression expression ...) -> result of last expression in context
			ctx.DefineFunction(Names.LISP_EVAL_BLOCK, EvalBlock, false);


			//  (if condition expression [alternative-expression]) -> result of expression or alternative-expression if condition was false
			ctx.DefineFunction(Names.LISP_IF, If, false);

			//  (cond ((condition) expression) ((condition) expression) ...) -> result of success statement, or null
			ctx.DefineFunction(Names.LISP_COND, Cond, false);

			//  (while condition (expression expression expression ...)) -> evaluates expressions while condition is true, return null
			ctx.DefineFunction(Names.LISP_WHILE, While, false);

			//  (until condition (expression expression expression ...)) -> same as (while (not condition) ...)
			ctx.DefineFunction(Names.LISP_UNTIL, Until, false);


			//  (lambda (...) expression ...) -> closure
			//     = (syntax (lambda #T #F) (...) expression ...)
			ctx.DefineFunction(Names.LISP_LAMBDA, Lambda, false);

			//  (macros (...) expression ...) -> closure
			//     = (syntax (macros #F #T) (...) expression ...)
			ctx.DefineFunction(Names.LISP_MACROS, Macros, false);

			//  (preprocessor (...) expression ...) -> closure
			//     = (syntax (preprocessor #F #F) (...) expression ...)

			//
			//  (syntax (tag #F #F) (...) expression ...) -> closure
			//  (syntax (tag #T #F) (...) expression ...) -> closure
			//  (syntax (tag #F #T) (...) expression ...) -> closure
			//  (syntax (tag #T #T) (...) expression ...) -> closure
			ctx.DefineFunction(Names.LISP_SYNTAX, Syntax, false);

			//  (macro-expand (macros arguments)) -> result of (macros arguments)
			//ctx.DefineFunction(Names.LISP_MACROEXPAND, MacroExpand, 1, false);

			//  (apply function [arguments]) -> result of (function [arguments])
			ctx.DefineFunction(Names.LISP_APPLY, Apply);

			//  (error anythin) -> INTERRUPTION
			ctx.DefineFunction(Names.LISP_ERROR, Error);
		}

		private static void Nope(Evaluator eval, StackFrame frame)
		{
			eval.Return(null);
		}

		private static void Quote(Evaluator eval, StackFrame frame)
		{
			eval.Return(frame.args.Head);
		}

		private static void Parse(Evaluator eval, StackFrame frame)
		{
			var text = frame.args?.Head;

			if (text.type != AtomType.String)
				throw new ArgumentException("Argument must be string!");

			try
			{
				var list = BombardoLang.Parse(text.@string);

				eval.Return(list);
			}
			catch
			{
			}

			eval.Return(null);
		}

		private static void Eval(Evaluator eval, StackFrame frame)
		{
			if (!eval.HaveReturn())
			{
				var (expression, ctxAtom) = StructureUtils.Split2(frame.args);
				var ctx = ctxAtom?.@object as Context ?? frame.context.@object as Context;
				eval.CreateFrame(Atoms.STATE_EVAL, expression, ctx.self);
				return;
			}

			eval.CloseFrame();
		}

		private static void EvalEach(Evaluator eval, StackFrame frame)
		{
			if (!eval.HaveReturn())
			{
				var expression = frame.args?.Head;
				if (expression is null)
				{
					eval.SetReturn(null);
					eval.CloseFrame();
					return;
				}

				if (expression.@object is Context ctx)
					expression = frame.args?.Next?.Head;
				else
					ctx = frame.context.@object as Context;
						
				eval.CreateFrame(Atoms.STATE_EVAL_BLOCK, expression, ctx.self);
				return;
			}

			eval.CloseFrame();
		}

		private static void EvalBlock(Evaluator eval, StackFrame frame)
		{
			if (!eval.HaveReturn())
			{
				var expression = frame.args;
				if (expression is null)
				{
					eval.SetReturn(null);
					eval.CloseFrame();
					return;
				}

				if (expression.@object is Context ctx)
					expression = expression.Next;
				else
					ctx = frame.context.@object as Context;
				eval.CreateFrame(Atoms.STATE_EVAL_BLOCK, expression, ctx.self);
				return;
			}

			eval.CloseFrame();
		}
		

		private static void Cond(Evaluator eval, StackFrame frame)
		{
			switch (frame.state.@string)
			{
				case "-eval-sexp-body-":
					frame.temp1 = frame.args;
					frame.state = Atoms.STATE_COND_HEAD;
					break;
				case "-built-in-cond-head-":
					if (eval.HaveReturn())
					{
						var condition = eval.TakeReturn();
						if (!condition.IsBool)
							throw new ArgumentException($"Condition must return boolean atom, but found: {condition}!");
						if (condition.@bool)
						{
							frame.state = Atoms.STATE_COND_BODY;
							break;
						}
					}

					if (frame.temp1 != null)
					{
						frame.temp2 = frame.temp1.Head;
						frame.temp1 = frame.temp1.Next;

						if (!frame.temp2.IsPair)
							throw new ArgumentException($"Condition element must be list, but found: {frame.temp2}!");

						eval.CreateFrame(Atoms.STATE_EVAL, frame.temp2.Head, frame.context);
					}
					else
					{
						//  Ни одно из условий не выполнилось
						frame.state = Atoms.STATE_EVAL_SEXP_BODY;
						eval.Return(Atoms.FALSE);
					}

					break;
				case "-built-in-cond-body-":
					if (eval.HaveReturn())
						eval.CloseFrame();
					else
						eval.CreateFrame(Atoms.STATE_EVAL_BLOCK, frame.temp2.Next, frame.context);

					break;
			}
		}

		private static void If(Evaluator eval, StackFrame frame)
		{
			switch (frame.state.@string)
			{
				case "-eval-sexp-body-":
				{
					//  (cond BlockA BlockB)
					(frame.temp1, frame.temp2, frame.temp3) = StructureUtils.Split3(frame.args);

					frame.state = Atoms.STATE_IF_COND;
				}
					break;
				case "-built-in-if-cond-":
				{
					if (eval.HaveReturn())
					{
						var condition = eval.TakeReturn();
						if (!condition.IsBool)
							throw new ArgumentException($"Condition must return boolean atom, but found: {condition}!");
						frame.state = condition.@bool ? Atoms.STATE_IF_THEN : Atoms.STATE_IF_ELSE;
					}
					else
					{
						eval.CreateFrame(Atoms.STATE_EVAL, frame.temp1, frame.context);
					}
				}
					break;
				case "-built-in-if-then-":
				{
					if (frame.temp2 == null)
						throw new ArgumentException("If statement must have at least one block of code!");

					if (eval.HaveReturn())
					{
						eval.CloseFrame();
						break;
					}

					eval.CreateFrame(Atoms.STATE_EVAL, frame.temp2, frame.context);
				}
					break;
				case "-built-in-if-else-":
					frame.state = Atoms.STATE_EVAL_SEXP_BODY;

					if (eval.HaveReturn())
					{
						eval.CloseFrame();
						break;
					}

					if (frame.temp3 == null)
						eval.Return(null);
					else
						eval.CreateFrame(Atoms.STATE_EVAL, frame.temp3, frame.context);

					break;
			}
		}

		private static void While(Evaluator eval, StackFrame frame)
		{
			switch (frame.state.@string)
			{
				case "-eval-sexp-body-":
					//  (Cond Body)
					(frame.temp1, frame.temp2) = (frame.args.Head, frame.args.Next);
					
					frame.state = Atoms.STATE_WHILE_COND;
					break;
				case "-built-in-while-cond-":
					if (eval.HaveReturn())
					{
						var condition = eval.TakeReturn();
						if (!condition.IsBool)
							throw new ArgumentException($"Condition must return boolean atom, but found: {condition}!");
						if (condition.@bool)
						{
							frame.state = Atoms.STATE_WHILE_BODY;
						}
						else
						{
							frame.state = Atoms.STATE_EVAL_SEXP_BODY;
							eval.Return(frame.temp3);
						}
					}
					else
					{
						eval.CreateFrame(Atoms.STATE_EVAL, frame.temp1, frame.context);
					}

					break;
				case "-built-in-while-body-":
					if (eval.HaveReturn())
					{
						frame.temp3 = eval.TakeReturn();
						frame.state = Atoms.STATE_WHILE_COND;
					}
					else
					{
						eval.CreateFrame(Atoms.STATE_EVAL_BLOCK, frame.temp2, frame.context);
					}

					break;
			}
		}

		private static void Until(Evaluator eval, StackFrame frame)
		{
			switch (frame.state.@string)
			{
				case "-eval-sexp-body-":
					//  (Cond Body)
					(frame.temp1, frame.temp2) = (frame.args.Head, frame.args.Next);
					
					frame.state = Atoms.STATE_WHILE_COND;
					break;
				case "-built-in-while-cond-":
					if (eval.HaveReturn())
					{
						var condition = eval.TakeReturn();
						if (!condition.IsBool)
							throw new ArgumentException($"Condition must return boolean atom, but found: {condition}!");
						if (!condition.@bool)
						{
							frame.state = Atoms.STATE_WHILE_BODY;
						}
						else
						{
							frame.state = Atoms.STATE_EVAL_SEXP_BODY;
							eval.Return(frame.temp3);
						}
					}
					else
					{
						eval.CreateFrame(Atoms.STATE_EVAL, frame.temp1, frame.context);
					}

					break;
				case "-built-in-while-body-":
					if (eval.HaveReturn())
					{
						frame.temp3 = eval.TakeReturn();
						frame.state = Atoms.STATE_WHILE_COND;
					}
					else
					{
						eval.CreateFrame(Atoms.STATE_EVAL_BLOCK, frame.temp2, frame.context);
					}

					break;
			}
		}

		private static void Lambda(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var vars = StructureUtils.CloneTree(args?.Head);
			var body = StructureUtils.CloneTree(args?.Next);

			if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
				throw new ArgumentException("Args must be list or symbol!");

			var ctx     = frame.context.@object as Context;
			var closure = new Closure(ctx, vars, body, "lambda")
			{
				EvalArgs   = true,
				EvalResult = false
			};

			eval.Return(Atom.CreateFunction(closure));
		}

		private static void Macros(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var vars = StructureUtils.CloneTree(args?.Head);
			var body = StructureUtils.CloneTree(args?.Next);

			if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
				throw new ArgumentException("Args must be list or symbol!");

			var ctx     = frame.context.@object as Context;
			var closure = new Closure(ctx, vars, body, "macros")
			{
				EvalArgs   = false,
				EvalResult = true
			};

			eval.Return(Atom.CreateFunction(closure));
		}

		private static void Preprocessor(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var vars = StructureUtils.CloneTree(args?.Head);
			var body = StructureUtils.CloneTree(args?.Next);

			if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
				throw new ArgumentException("Args must be list or symbol!");

			var ctx     = frame.context.@object as Context;
			var closure = new Closure(ctx, vars, body, "preprocessor")
			{
				EvalArgs   = false,
				EvalResult = false
			};

			eval.Return(Atom.CreateFunction(closure));
		}

		private static void Syntax(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			//  [tag before after]
			var (tag, before, after) = StructureUtils.Split3(args.Head);

			if (!tag.IsSymbol && !tag.IsString)
				throw new ArgumentException("Tag must be string or symbol!");
			if (!before.IsBool)
				throw new ArgumentException("Eval args flag must be boolean!");
			if (!after.IsBool)
				throw new ArgumentException("Eval result flag must be boolean!");

			args = args?.Next;
			var vars = StructureUtils.CloneTree(args?.Head);
			var body = StructureUtils.CloneTree(args?.Next);

			if (vars.type != AtomType.Pair && vars.type != AtomType.Symbol)
				throw new ArgumentException("Args must be list or symbol!");

			var ctx     = frame.context.@object as Context;
			var closure = new Closure(ctx, vars, body, tag.@string)
			{
				EvalArgs   = before.@bool,
				EvalResult = after.@bool
			};

			eval.Return(Atom.CreateFunction(closure));
		}

		private static void Apply(Evaluator eval, StackFrame frame)
		{
			if (eval.HaveReturn())
			{
				var args = frame.args;
				var func = args.Head;
				var rest = StructureUtils.CloneList(args.Next.Head);

				if (!func.IsFunction)
					throw new ArgumentException("First argument must be procedure!");

				eval.CreateFrame(Atoms.STATE_EVAL, Atom.CreatePair(func, rest), frame.context);
				return;
			}

			eval.CloseFrame();
		}

		// private static Atom MacroExpand(Atom args, Context context)
		// {
		//     Atom expression = (Atom)args?.value;
		//     Atom head = Evaluator.Evaluate((Atom)expression?.value, context);
		//     Closure macros = (Closure)head?.value;
		//     Atom macroArgs = Atom.CloneTree(expression?.Next);
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
			var args = frame.args;
			var tag  =  args?.Head;
			if (args.Next == null)
			{
				if (tag.type != AtomType.String &&
				    tag.type != AtomType.Symbol)
					throw new ArgumentException("Tag must be string or symbol!");
				throw new BombardoException(tag.@string);
			}

			var text =  args?.Next?.Head;

			if (tag.type != AtomType.String &&
			    tag.type != AtomType.Symbol)
				throw new ArgumentException("Tag must be string or symbol!");

			if (text.type != AtomType.String)
				throw new ArgumentException("Text must be string!");

			throw new BombardoException($"<{tag.@string}> {text.@string}");
		}
	}
}