using System;

namespace Bombardo.V2
{
	public static class ArgUtils
	{
		public static string GetNumber(int i)
		{
			switch (i)
			{
				case 1:  return "First";
				case 2:  return "Second";
				case 3:  return "Third";
				case 4:  return "Fourth";
				case 5:  return "Fith";
				default: return i + "th";
			}
		}

		public static T GetEnum<T>(Atom argument, int idx, T tenum = default) where T : struct, Enum
		{
			if (argument != null)
			{
				if (argument.type != AtomType.Symbol &&
				    !Enum.TryParse((string) argument.value, out tenum))
					throw new ArgumentException(string.Format(
							                            "{1} argument must be one of symbols: {2}!",
							                            GetNumber(idx), string.Join(", ", Enum.GetValues(typeof(T)))
						                            ));
			}

			return tenum;
		}
	}
}