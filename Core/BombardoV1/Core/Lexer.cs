using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bombardo.V1
{
    public class TokenType
    {
        public const int Separator = 0;
        public const int Quoting = 1;
        public const int Operator = 2;
        public const int String = 3;
        public const int Symbol = 4;
    }

    public class Token
    {
        public int type;
        public string value;

        public Token(int type, string value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public class LexerNode
    {
        public int type;
        public Regex reg;

        public LexerNode(int type, string expression)
        {
            this.type = type;
            reg = new Regex(expression, RegexOptions.IgnoreCase);
        }
    }

    public class Lexer
    {
        private static List<LexerNode> nodes_;

        private static void Init()
        {
            if (nodes_ != null) return;
            nodes_ = new List<LexerNode>();
            nodes_.Add(new LexerNode(TokenType.Separator, @"[\s\r\n\t]+|;[^\n]*\n")); //  [\s\r\n\t]+ - separators, ;[^\r\n]*\n - commentary
            nodes_.Add(new LexerNode(TokenType.Quoting, @"`"));
            nodes_.Add(new LexerNode(TokenType.Operator, @"[()[\]]"));
            nodes_.Add(new LexerNode(TokenType.String, @"(?:'(?:[^\\]\\'|[^'])*'|""(?:[^\\]\\""|[^""])*"")"));
            nodes_.Add(new LexerNode(TokenType.Symbol, @"[^""'\s\r\n\t()[\]]+"));
        }

        public static List<Token> Handle(string raw)
        {
            Init();
            List<Token> tokens = new List<Token>();

            int offset = 0;

            int type = 0;
            string token = null;

            while (offset < raw.Length)
            {
                if (TryReadToken(raw, offset, ref type, ref token))
                {
                    tokens.Add(new Token(type, token));
                    offset += token.Length;
                }
                else
                {
                    throw new ArgumentException(string.Format("Unknown char '{0}'! Check lexer setup!", raw[offset]));
                }
            }

            return tokens;
        }

        private static bool TryReadToken(string source, int offset, ref int type, ref string token)
        {
            int length = nodes_.Count;
            for (int i = 0; i < length; i++)
            {
                LexerNode node = nodes_[i];
                Match match = node.reg.Match(source, offset);
                if (match.Success && match.Index == offset)
                {
                    token = match.Value;
                    type = node.type;
                    return true;
                }
            }
            return false;
        }
    }
}