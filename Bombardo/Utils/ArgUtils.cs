using System;

namespace Bombardo.Utils
{
    class ArgUtils
    {
        public static string GetNumber(int i)
        {
            switch(i)
            {
                case 1: return "First";
                case 2: return "First";
                case 3: return "First";
                case 4: return "First";
                case 5: return "First";
                default: return i.ToString()+"th";
            }
        }

        public static Atom GetArgument()
        {
            return null;
        }

        public static T GetEnum<T>(Atom argument, int idx, T tenum = default(T)) where T : struct, Enum
        {
            if (argument != null)
            {
                if (argument.type != AtomType.Symbol &&
                    !Enum.TryParse((string)argument.value, out tenum))
                    throw new ArgumentException(string.Format(
                        "{1} argument must be one of symbols: {2}!",
                        GetNumber(idx), string.Join(", ", Enum.GetValues(typeof(T)))
                        ));
            }

            return tenum;
        }

    }
}
