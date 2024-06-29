using System.Collections.Generic;

namespace Bombardo.Core
{
    public static class BombardoLang
    {
        public static Atom Parse(string raw)
        {
            Lexer.Init();
            Parser.Init();
            var tokens = Lexer.Handle(raw);
            return Parser.Handle(tokens);
        }
    }
}