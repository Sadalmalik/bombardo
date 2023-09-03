namespace Bombardo.Core
{
	public static class Atoms
	{
		public static readonly Atom EMPTY = new Atom();
		public static readonly Atom TRUE = new Atom(AtomType.Bool, true);
		public static readonly Atom FALSE = new Atom(AtomType.Bool, false);
		public static readonly Atom QUOTE = new Atom(AtomType.Symbol, "quote");
		
		public static readonly Atom BUILT_IN = new Atom(AtomType.Symbol, "built-in");
		public static readonly Atom LAMBDA = new Atom(AtomType.Symbol, "lambda");
		public static readonly Atom MACROS = new Atom(AtomType.Symbol, "macros");
		public static readonly Atom PREPROCESSOR = new Atom(AtomType.Symbol, "preprocessor");
		
		public static readonly Atom INTERNAL_STATE = new Atom(AtomType.Symbol, "-internal-state-");
	}
}