using System;
using System.Collections.Generic;

namespace Bombardo.V2
{
	public static partial class Names
	{
		public static readonly string LISP_DEFINE = "define";
		public static readonly string LISP_UNDEFINE = "undef";
		public static readonly string LISP_SET_FIRST = "set!";

		public static readonly string LISP_TO_STRING = "toString";
		public static readonly string LISP_FROM_STRING = "fromString";

		public static readonly string LISP_SYMBOL_NAME = "symbolName";
		public static readonly string LISP_MAKE_SYMBOL = "symbolMake";

		public static readonly string LISP_GET_CONTEXT = "getContext";
		public static readonly string LISP_GET_CONTEXT_PARENT = "getContextParent";
	}

	public class ContextFunctions
	{
		public static void Define(Context ctx)
		{
			ctx.DefineFunction(Names.LISP_DEFINE, Define, false);
			ctx.DefineFunction(Names.LISP_UNDEFINE, Undefine, false);
			ctx.DefineFunction(Names.LISP_SET_FIRST, SetFirst, false);

			ctx.DefineFunction(Names.LISP_TO_STRING, ToString);
			ctx.DefineFunction(Names.LISP_FROM_STRING, FromString);

			ctx.DefineFunction(Names.LISP_SYMBOL_NAME, SymbolName);
			ctx.DefineFunction(Names.LISP_MAKE_SYMBOL, MakeSymbol);

			ctx.DefineFunction(Names.LISP_GET_CONTEXT, GetContext);
			ctx.DefineFunction(Names.LISP_GET_CONTEXT_PARENT, GetContextParent);

			ctx.Define(Names.NULL_SYMBOL, null);
			ctx.Define(Names.EMPTY_SYMBOL, Atoms.EMPTY);
		}

		private static void Define(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var sym  = args?.atom;

			if (sym.type != AtomType.Symbol)
				throw new ArgumentException("Definition name must be symbol!");

			if (eval.HaveReturn())
			{
				var name = (string) sym.value;
				var result = eval.TakeReturn();
				var ctx    = frame.context.value as Context;
				if (result?.value is Closure function)
					function.Name = name;
				//Console.WriteLine($"DEFINE: '{name}' = '{result}' at {ctx}");
				ContextUtils.Define(ctx, result, name);
				eval.Return(result);
				return;
			}

			eval.CreateFrame("-eval-", args.next?.atom, frame.context);
			frame.state = Atoms.INTERNAL_STATE;
		}

		private static void Undefine(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom sym  = (Atom) args?.value;
			if (sym.type != AtomType.Symbol)
				throw new ArgumentException("Undefining name must be symbol!");

			var ctx    = frame.context.value as Context;
			var result = ContextUtils.Undefine(ctx, (string) sym.value);
			eval.Return(result);
		}


		private static void SetFirst(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom sym  = (Atom) args?.value;
			if (sym.type != AtomType.Symbol)
				throw new ArgumentException("Variable name must be symbol!");

			if (eval.HaveReturn())
			{
				var result = eval.TakeReturn();
				var ctx    = frame.context.value as Context;
				ContextUtils.Set(ctx, result, (string) sym.value);
				eval.Return(result);
				return;
			}

			eval.CreateFrame("-eval-", args.next?.atom, frame.context);
			frame.state = Atoms.INTERNAL_STATE;
		}

		private static void ToString(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom atom = (Atom) args.value;
			eval.Return(new Atom(AtomType.String, atom.ToString()));
		}

		private static void FromString(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom str  = (Atom) args.value;
			if (!str.IsString) throw new ArgumentException("Argument must be string!");
			Atom list = BombardoLang.Parse((string) str.value);
			eval.Return(list);
		}

		private static void SymbolName(Evaluator eval, StackFrame frame)
		{
			var  args   = frame.args;
			Atom symbol = (Atom) args.value;
			if (!symbol.IsSymbol) throw new ArgumentException("Argument must be symbol!");
			eval.Return(new Atom(AtomType.String, (string) symbol.value));
		}

		private static void MakeSymbol(Evaluator eval, StackFrame frame)
		{
			var  args   = frame.args;
			Atom symbol = (Atom) args.value;
			if (!symbol.IsString) throw new ArgumentException("Argument must be string!");
			eval.Return(new Atom(AtomType.Symbol, (string) symbol.value));
		}

		private static void GetContext(Evaluator eval, StackFrame frame)
		{
			eval.Return(frame.context);
		}

		private static void GetContextParent(Evaluator eval, StackFrame frame)
		{
			var args    = frame.args;
			var ctx     = args?.atom;
			var context = frame.context.value as Context;
			if (ctx != null && ctx.IsNative)
				if (ctx.value is Context other)
					context = other;
			eval.Return(context?.parent?.self);
		}
	}
}