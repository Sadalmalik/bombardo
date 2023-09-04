using System;
using System.Text.RegularExpressions;

namespace Bombardo.Core
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

        public static AtomNumber TryParseValue(string symbol)
        {
            Init();

            AtomNumber number = new AtomNumber {type = AtomNumberType.NaN};

            if (_regBin.Match(symbol).Success)
            {
                number.type       = AtomNumberType.UINT64;
                number.val_sint64 = Convert.ToInt64(symbol.Substring(2), 2);
                return number;
            }

            if (_regHex.Match(symbol).Success)
            {
                number.type       = AtomNumberType.UINT64;
                number.val_sint64 = Convert.ToInt64(symbol.Substring(2), 16);
                return number;
            }

            if (_regDec.Match(symbol).Success)
            {
                number.type       = AtomNumberType.UINT64;
                number.val_sint64 = long.Parse(symbol);
                return number;
            }

            if (_regDouble.Match(symbol).Success)
            {
                number.type       = AtomNumberType.UINT64;
                number.val_double = double.Parse(symbol.Replace('.', ','));
                return number;
            }

            return number;
        }
    }
}