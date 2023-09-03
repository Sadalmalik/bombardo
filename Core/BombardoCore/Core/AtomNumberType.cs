namespace Bombardo.Core
{
    public static class AtomNumberType
    {
        public const int NaN = int.MaxValue; //  if one of numbers is NaN - all of them is NaN

        public const int UINT_8  = 0;
        public const int SINT_8  = 1;
        public const int UINT16  = 2;
        public const int SINT16  = 3;
        public const int _CHAR_  = 4;
        public const int UINT32  = 5;
        public const int SINT32  = 6;
        public const int UINT64  = 7;
        public const int SINT64  = 8;
        public const int SINGLE  = 9;
        public const int DOUBLE  = 10;
        public const int DECIMAL = 11;

        public static string ToString(int type)
        {
            switch (type)
            {
                case UINT_8:  return "uint8";
                case SINT_8:  return "int8";
                case UINT16:  return "uint16";
                case SINT16:  return "int16";
                case _CHAR_:  return "char";
                case UINT32:  return "uint32";
                case SINT32:  return "int32";
                case UINT64:  return "uint64";
                case SINT64:  return "int64";
                case SINGLE:  return "single";
                case DOUBLE:  return "double";
                case DECIMAL: return "decimal";
            }

            return "NaN";
        }
    }
}