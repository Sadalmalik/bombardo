using System;

namespace Bombardo.Core
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
			if (argument == null) return tenum;
			if (argument.type == AtomType.Symbol || Enum.TryParse(argument.@string, out tenum)) return tenum;
			var number = GetNumber(idx);
			var values = string.Join(", ", Enum.GetValues(typeof(T)));
			throw new ArgumentException($"{number} argument must be one of symbols: {values}!");
		}
	}
}