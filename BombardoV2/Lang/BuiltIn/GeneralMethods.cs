using System;
using System.Collections.Generic;

namespace Bombardo.V2
{
	public class GeneralMethods
	{
		public static void Define(Context ctx)
        {
            ctx.DefineFunction(Names.LISP_PRINT, Print);
            ctx.DefineFunction(Names.LISP_READ, ReadLine);

            ctx.DefineFunction(Names.LISP_DEFINE, Define, false);
            ctx.DefineFunction(Names.LISP_SETFIRST, SetFirst, false);

            ctx.DefineFunction(Names.LISP_TO_STRING, ToString);
            ctx.DefineFunction(Names.LISP_FROM_STRING, FromString);

            ctx.DefineFunction(Names.LISP_SYMBOL_NAME, SymbolName);
            ctx.DefineFunction(Names.LISP_MAKE_SYMBOL, MakeSymbol);

            ctx.DefineFunction(Names.LISP_GET_CONTEXT, GetContext);
            ctx.DefineFunction(Names.LISP_GET_CONTEXT_PARENT, GetContextParent);

            ctx.Define(Names.NULL_SYMBOL, null); 
            ctx.Define(Names.EMPTY_SYMBOL, Atoms.EMPTY);
        }

        private static void Print(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            for (Atom iter = args; iter != null; iter = iter.next)
            {
                Atom value = iter.value as Atom;
                if (value == null)
                    Console.Write("null");
                else if (value.type == AtomType.String ||
                    value.type == AtomType.Symbol)
                    Console.Write(value.value);
                else
                    Console.Write(value.Stringify(true));
            }
            Console.WriteLine();
            eval.SetReturn(null);
        }

        private static void ReadLine(Evaluator eval, StackFrame frame)
        {
            string line = Console.ReadLine();
            eval.SetReturn(new Atom(AtomType.String, line));
        }
        
        private static void Define(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            Atom sym = (Atom)args?.value;
            if (sym.type != AtomType.Symbol)
                throw new ArgumentException("Definition name must be symbol!");
            
            if(eval.HaveReturn())
            {
                var result = eval.TakeReturn();
                var ctx = frame.context.value as Context;
                ContextUtils.Define(ctx, result, (string)sym.value);
                eval.SetReturn(result);
                return;
            }
            
            eval.Stack.CreateFrame("-eval-", args.next?.atom, frame.context);
        }

        private static void SetFirst(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            Atom sym = (Atom)args?.value;
            if (sym.type != AtomType.Symbol)
                throw new ArgumentException("Variable name must be symbol!");
            
            if(eval.HaveReturn())
            {
                var result = eval.TakeReturn();
                var ctx = frame.context.value as Context;
                ContextUtils.Set(ctx, result, (string)sym.value);
                eval.SetReturn(result);
                return;
            }
            eval.Stack.CreateFrame("-eval-", args.next?.atom, frame.context);
        }

        private static void ToString(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            Atom atom = (Atom)args.value;
            eval.SetReturn(new Atom(AtomType.String, atom.ToString()));
        }

        private static void FromString(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            Atom str = (Atom)args.value;
            if (!str.IsString) throw new ArgumentException("Argument must be string!");
            List<Atom> atoms = BombardoLangClass.Parse((string)str.value);
            eval.SetReturn(StructureUtils.List(atoms.ToArray()));
        }
        
        private static void SymbolName(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            Atom symbol = (Atom)args.value;
            if (!symbol.IsSymbol) throw new ArgumentException("Argument must be symbol!");
            eval.SetReturn(new Atom(AtomType.String, (string)symbol.value));
        }

        private static void MakeSymbol(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            Atom symbol = (Atom)args.value;
            if (!symbol.IsString) throw new ArgumentException("Argument must be string!");
            eval.SetReturn(new Atom(AtomType.Symbol, (string)symbol.value));
        }

        private static void GetContext(Evaluator eval, StackFrame frame)
        {
            eval.SetReturn(frame.context);
        }

        private static void GetContextParent(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            var ctx = args?.atom;
            var context = frame.context.value as Context;
            if (ctx != null && ctx.IsNative)
                if (ctx.value is Context other)
                    context = other;
            eval.SetReturn(context?.parent?.self);
        }
	}
}