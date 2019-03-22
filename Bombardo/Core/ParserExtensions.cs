using System;
using System.Text.RegularExpressions;

namespace Bombardo
{
    public class ParserBoolean : IParserExtension
    {
        private Regex regTrue_;
        private Regex regFalse_;

        public ParserBoolean()
        {
            regTrue_ = new Regex("^(?:true|#t|#T)$", RegexOptions.IgnoreCase);
            regFalse_ = new Regex("^(?:false|#f|#F)$", RegexOptions.IgnoreCase);
        }

        public bool TryParseValue(string symbol, ref int type, ref object value)
        {
            if (regTrue_.Match(symbol).Success)
            {
                type = AtomType.Bool;
                value = true;
                return true;
            }
            if (regFalse_.Match(symbol).Success)
            {
                type = AtomType.Bool;
                value = false;
                return true;
            }
            return false;
        }
    }

    internal delegate object ParseDelegate(string s);

    public class ParserNumber : IParserExtension
    {
        private Regex regBin_;
        private Regex regHex_;
        private Regex regDec_;
        private Regex regDouble_;
        
        public ParserNumber()
        {
            regBin_ = new Regex(@"^(?:0b[01]{1,64})$");
            regHex_ = new Regex(@"^(?:0x[\da-fA-F]{1,16})$");
            regDec_ = new Regex(@"^(?:-?\d{1,19})$");
            regDouble_ = new Regex(@"^(?:\d*(?:\.|\,)?\d+(?:e[+-]?\d+)?)$");
        }

        public bool TryParseValue(string symbol, ref int type, ref object value)
        {
            if (regBin_.Match(symbol).Success)
            {
                type = AtomType.Number;
                value = Convert.ToInt64(symbol.Substring(2), 2);
                return true;
            }
            if (regHex_.Match(symbol).Success)
            {
                type = AtomType.Number;
                value = Convert.ToInt64(symbol.Substring(2), 16);
                return true;
            }
            if (regDec_.Match(symbol).Success)
            {
                type = AtomType.Number;
                value = long.Parse(symbol);
                return true;
            }
            if (regDouble_.Match(symbol).Success)
            {
                type = AtomType.Number;
                value = double.Parse(symbol.Replace('.',','));
                return true;
            }
            return false;
        }
    }
}