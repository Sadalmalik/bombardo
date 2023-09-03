namespace Bombardo.Core
{
    public struct AtomPair
    {
        public Atom atom;
        public Atom next;

        public bool IsEmpty => atom == null && next == null;
    }
}