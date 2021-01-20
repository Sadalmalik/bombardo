using System.Collections.Generic;

namespace Bombardo.V2
{
	public class BombardoLang
	{
        public static Atom Parse(string raw)
        {
            var tokens = Lexer.Handle(raw);
            return Parser.Handle(tokens);
        }
	}
}