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
    }
}