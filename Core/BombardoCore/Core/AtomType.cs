namespace Bombardo.Core
{
    public static class AtomType
    {
        public const int Undefined = -1;
        public const int Pair      = 0;
        public const int Symbol    = 1;
        public const int String    = 2;
        public const int Bool      = 3;
        public const int Number    = 4;
        public const int Function  = 5;
        public const int Native    = 6;

        public static string ToString(int type)
        {
            switch (type)
            {
                case Pair:     return "List";
                case Symbol:   return "Symbol";
                case String:   return "String";
                case Bool:     return "Bool";
                case Number:   return "Number";
                case Function: return "Function";
                case Native:   return "Native";
                default:       return "Undefined";
            }
        }
    }
}