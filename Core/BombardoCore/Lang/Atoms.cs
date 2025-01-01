namespace Bombardo.Core
{
    public static class Atoms
    {
        public static readonly Atom EMPTY = Atom.CreatePair(null, null);
        public static readonly Atom TRUE  = Atom.CreateBoolean(true);
        public static readonly Atom FALSE = Atom.CreateBoolean(false);
        public static readonly Atom QUOTE = Atom.CreateSymbol("quote");

        public static readonly Atom BUILT_IN     = Atom.CreateSymbol("built-in");
        public static readonly Atom LAMBDA       = Atom.CreateSymbol("lambda");
        public static readonly Atom MACROS       = Atom.CreateSymbol("macros");
        public static readonly Atom PREPROCESSOR = Atom.CreateSymbol("preprocessor");

        public static readonly Atom INTERNAL_STATE = Atom.CreateSymbol("-internal-state-");

        // Core
        public static readonly Atom STATE_EVAL             = Atom.CreateSymbol("-eval-");
        public static readonly Atom STATE_EVAL_EACH        = Atom.CreateSymbol("-eval-each-");
        public static readonly Atom STATE_EVAL_BLOCK       = Atom.CreateSymbol("-eval-block-");
        public static readonly Atom STATE_EVAL_SEXP_HEAD   = Atom.CreateSymbol("-eval-sexp-head-");
        public static readonly Atom STATE_EVAL_SEXP_ARGS   = Atom.CreateSymbol("-eval-sexp-args-");
        public static readonly Atom STATE_EVAL_SEXP_BODY   = Atom.CreateSymbol("-eval-sexp-body-");
        public static readonly Atom STATE_EVAL_SEXP_RESULT = Atom.CreateSymbol("-eval-sexp-result-");

        // Built-in
        public static readonly Atom STATE_COND_HEAD = Atom.CreateSymbol("-built-in-cond-head-");
        public static readonly Atom STATE_COND_BODY = Atom.CreateSymbol("-built-in-cond-body-");

        public static readonly Atom STATE_IF_COND = Atom.CreateSymbol("-built-in-if-cond-");
        public static readonly Atom STATE_IF_THEN = Atom.CreateSymbol("-built-in-if-then-");
        public static readonly Atom STATE_IF_ELSE = Atom.CreateSymbol("-built-in-if-else-");

        public static readonly Atom STATE_WHILE_COND = Atom.CreateSymbol("-built-in-while-cond-");
        public static readonly Atom STATE_WHILE_BODY = Atom.CreateSymbol("-built-in-while-body-");

        public static readonly Atom STATE_ITERATE_EACH = Atom.CreateSymbol("-built-in-each-");
        public static readonly Atom STATE_ITERATE_MAP  = Atom.CreateSymbol("-built-in-map-");

        public static readonly Atom STATE_TABLE_EACH = Atom.CreateSymbol("-built-in-table-each-");
        
        public static readonly Atom STATE_LOGIC_OPERATOR = Atom.CreateSymbol("-built-logic-operator-");
    }
}