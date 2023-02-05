using System;
using System.Text.RegularExpressions;

namespace Bombardo.V2
{
	public class NumberParser
	{
		private static Regex _regBin;
		private static Regex _regHex;
		private static Regex _regDec;
		private static Regex _regDouble;

		public static void Init()
		{
			if (_regBin != null)
				return;
			
			_regBin    = new Regex(@"^(?:0b[01]{1,64})$");
			_regHex    = new Regex(@"^(?:0x[\da-fA-F]{1,16})$");
			_regDec    = new Regex(@"^(?:-?\d{1,19})$");
			_regDouble = new Regex(@"^(?:\d*(?:\.|\,)?\d+(?:e[+-]?\d+)?)$");
		}

		public static bool TryParseValue(string symbol, ref int type, ref object value)
		{
			Init();
			
			if (_regBin.Match(symbol).Success)
			{
				type  = AtomType.Number;
				value = Convert.ToInt64(symbol.Substring(2), 2);
				return true;
			}

			if (_regHex.Match(symbol).Success)
			{
				type  = AtomType.Number;
				value = Convert.ToInt64(symbol.Substring(2), 16);
				return true;
			}

			if (_regDec.Match(symbol).Success)
			{
				type  = AtomType.Number;
				value = long.Parse(symbol);
				return true;
			}

			if (_regDouble.Match(symbol).Success)
			{
				type  = AtomType.Number;
				value = double.Parse(symbol.Replace('.', ','));
				return true;
			}

			return false;
		}
	}
}