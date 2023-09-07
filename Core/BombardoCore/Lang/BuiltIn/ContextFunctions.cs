using System;

namespace Bombardo.Core
{
    public static partial class Names
    {
        public static readonly string LISP_DEFINE             = "define";
        public static readonly string LISP_UN_DEFINE          = "undef";
        public static readonly string LISP_SET_FIRST          = "set!";
        public static readonly string LISP_TO_STRING          = "toString";
        public static readonly string LISP_FROM_STRING        = "fromString";
        public static readonly string LISP_SYMBOL_NAME        = "symbolName";
        public static readonly string LISP_MAKE_SYMBOL        = "makeSymbol";
        public static readonly string LISP_GET_CONTEXT        = "getContext";
        public static readonly string LISP_GET_CONTEXT_PARENT = "getContextParent";
    }

    public static class ContextFunctions
    {
        public static void Define(Context ctx)
        {
            // (define X 15) -> 15
            // (define Y 25 context) -> 25, Y in context
            ctx.DefineFunction(Names.LISP_DEFINE, Define, false);


            ctx.DefineFunction(Names.LISP_UN_DEFINE, Undefine, false);
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
            var (sym, value, context) = StructureUtils.Split3(frame.args);

            if (sym.type != AtomType.Symbol)
                throw new ArgumentException("Definition name must be symbol!");

            if (eval.HaveReturn())
            {
                var name   = sym.@string;
                var result = eval.TakeReturn();
                var ctx    = context.IsContext ? context.context : frame.context.context;
                if (result?.function is Closure function
                    && (function.Name.Equals("??") || function.Name.Equals("λ")))
                    function.Name = name;
                //Console.WriteLine($"DEFINE: '{name}' = '{result}' at -internal-state-{ctx}");
                ContextUtils.Define(ctx, result, name);
                eval.Return(result);
                return;
            }

            eval.CreateFrame(Atoms.STATE_EVAL, value, frame.context);
            frame.state = Atoms.INTERNAL_STATE;
        }

        private static void Undefine(Evaluator eval, StackFrame frame)
        {
            var (sym, context) = StructureUtils.Split2(frame.args);

            if (sym == null || sym.type != AtomType.Symbol)
                throw new ArgumentException("Undefining name must be symbol!");
            var ctx = context.IsContext ? context.context : frame.context.context;

            var result = ContextUtils.Undefine(ctx, sym.@string);

            eval.Return(result);
        }


        private static void SetFirst(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            var sym  = args?.Head;
            if (sym == null || sym.type != AtomType.Symbol)
                throw new ArgumentException("Variable name must be symbol!");

            if (!eval.HaveReturn())
            {
                eval.CreateFrame(Atoms.STATE_EVAL, args.Next?.Head, frame.context);
                frame.state = Atoms.INTERNAL_STATE;
                return;
            }

            var result = eval.TakeReturn();
            var ctx    = frame.context.@context;
            ContextUtils.Set(ctx, result, sym.@string);
            eval.Return(result);
        }

        private static void ToString(Evaluator eval, StackFrame frame)
        {
            var atom = frame.args.Head;
            eval.Return(Atom.CreateString(atom.ToString()));
        }

        private static void FromString(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            var str  = args.Head;
            if (!str.IsString) throw new ArgumentException("Argument must be string!");
            var list = BombardoLang.Parse((string) str.@string);
            eval.Return(list);
        }

        private static void SymbolName(Evaluator eval, StackFrame frame)
        {
            var args   = frame.args;
            var symbol = args.Head;
            if (!symbol.IsSymbol) throw new ArgumentException("Argument must be symbol!");
            eval.Return(Atom.CreateString(symbol.@string));
        }

        private static void MakeSymbol(Evaluator eval, StackFrame frame)
        {
            var args   = frame.args;
            var symbol = args.Head;
            if (!symbol.IsString) throw new ArgumentException("Argument must be string!");
            eval.Return(Atom.CreateSymbol(symbol.@string));
        }

        private static void GetContext(Evaluator eval, StackFrame frame)
        {
            eval.Return(frame.context);
        }

        private static void GetContextParent(Evaluator eval, StackFrame frame)
        {
            var args    = frame.args;
            var ctx     = args?.Head;
            var context = frame.context.@context;
            if (ctx != null && ctx.IsNative)
                if (ctx.@object is Context other)
                    context = other;
            eval.Return(context?.parent?.self);
        }
    }
}