namespace Bombardo.Core
{
    public static partial class Names
    {
        public static readonly string LISP_PRED_NULL      = "null?";
        public static readonly string LISP_PRED_NOT_NULL  = "not-null?";
        public static readonly string LISP_PRED_EMPTY     = "empty?";
        public static readonly string LISP_PRED_SYM       = "symbol?";
        public static readonly string LISP_PRED_PAIR      = "pair?";
        public static readonly string LISP_PRED_LIST      = "list?";
        public static readonly string LISP_PRED_STRING    = "string?";
        public static readonly string LISP_PRED_BOOL      = "bool?";
        public static readonly string LISP_PRED_NUMBER    = "number?";
        public static readonly string LISP_PRED_PROCEDURE = "proc?";
    }

    public static class TypePredicateFunctions
    {
        public static void Define(Context ctx)
        {
            ctx.DefineFunction(Names.LISP_PRED_NULL, PredNull);
            ctx.DefineFunction(Names.LISP_PRED_NOT_NULL, PredNotNull);
            ctx.DefineFunction(Names.LISP_PRED_EMPTY, PredEmpty);
            ctx.DefineFunction(Names.LISP_PRED_SYM, PredSym);
            ctx.DefineFunction(Names.LISP_PRED_PAIR, PredList);
            ctx.DefineFunction(Names.LISP_PRED_LIST, PredList);
            ctx.DefineFunction(Names.LISP_PRED_STRING, PredString);
            ctx.DefineFunction(Names.LISP_PRED_BOOL, PredBool);
            ctx.DefineFunction(Names.LISP_PRED_NUMBER, PredNumber);
            ctx.DefineFunction(Names.LISP_PRED_PROCEDURE, PredProcedure);
        }

        private static void PredNull(Evaluator eval, StackFrame frame)
        {
            eval.Return(frame.args.Head == null ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredNotNull(Evaluator eval, StackFrame frame)
        {
            eval.Return(frame.args.Head == null ? Atoms.FALSE : Atoms.TRUE);
        }

        private static void PredEmpty(Evaluator eval, StackFrame frame)
        {
            var atom   = frame.args.Head;
            var result = atom.IsPair && atom.Head == null && atom.Next == null;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredSym(Evaluator eval, StackFrame frame)
        {
            var atom   = frame.args.Head;
            var result = atom != null && atom.type == AtomType.Symbol;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredList(Evaluator eval, StackFrame frame)
        {
            var atom   = frame.args.Head;
            var result = atom != null && atom.type == AtomType.Pair;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredString(Evaluator eval, StackFrame frame)
        {
            var atom   = frame.args.Head;
            var result = atom != null && atom.type == AtomType.String;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredBool(Evaluator eval, StackFrame frame)
        {
            var atom   = frame.args.Head;
            var result = atom != null && atom.type == AtomType.Bool;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredNumber(Evaluator eval, StackFrame frame)
        {
            var atom   = frame.args.Head;
            var result = atom != null && atom.type == AtomType.Number;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void PredProcedure(Evaluator eval, StackFrame frame)
        {
            var atom   = frame.args.Head;
            var result = atom != null && atom.type == AtomType.Function;
            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }
    }
}