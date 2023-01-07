using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bombardo.V2
{
	public static class TokenType
	{
		public const int Separator = 0;
		public const int Brackets = 1;
		public const int Symbol = 2;
		public const int String = 3;
	}

	public class Token
	{
		public int type;
		public string value;

		public Token(int type, string value)
		{
			this.type  = type;
			this.value = value;
		}
	}

	public class LexerNode
	{
		public int type;
		public Regex reg;
		public Func<string, string> postprocess;

		public LexerNode(int type, string expression, Func<string, string> postprocess = null)
		{
			this.type        = type;
			reg              = new Regex(expression, RegexOptions.IgnoreCase);
			this.postprocess = postprocess;
		}
	}

	public static class Lexer
	{
		private static List<LexerNode> _nodes;

		public static void Init()
		{
			if (_nodes != null) return;

			_nodes = new List<LexerNode>();
			//  [\s\r\n\t]+ - separators
			//  ;[^\r\n]*\n - commentary
			_nodes.Add(new LexerNode(TokenType.Separator, @"[\s\r\n\t]+|#[^\n]*(?:\n|\z)"));
			_nodes.Add(new LexerNode(TokenType.Brackets, @"[\(\)\[\]\{\}]"));
			_nodes.Add(new LexerNode(TokenType.Symbol, @"`|[^""'\s\r\n\t()[\]]+"));
			_nodes.Add(new LexerNode(TokenType.String,
			                         @"(?:'(?:[^\\]\\'|[^'])*'|""(?:[^\\]\\""|[^""])*"")",
			                         literal => literal.Substring(1, literal.Length - 2)
			                                           .Replace("\\n", "\n")
			                                           .Replace("\\t", "\t")
			                                           .Replace("\\\"", "\"")
			                                           .Replace("\\\\", "\\")));
		}

		public static List<Token> Handle(string raw)
		{
			List<Token> tokens = new List<Token>();

			int offset = 0;

			int    type  = 0;
			string token = null;

			while (offset < raw.Length)
			{
				if (TryReadToken(raw, ref offset, ref type, ref token))
				{
					tokens.Add(new Token(type, token));
				}
				else
				{
					throw new ArgumentException($"Unknown char '{raw[offset]}' at {offset}! Check lexer setup!");
				}
			}

			return tokens;
		}

		private static bool TryReadToken(string source, ref int offset, ref int type, ref string token)
		{
			foreach (var node in _nodes)
			{
				Match match = node.reg.Match(source, offset);
				if (match.Success && match.Index == offset)
				{
					token  =  match.Value;
					type   =  node.type;
					offset += token.Length;
					if (node.postprocess != null)
						token = node.postprocess(token);
					return true;
				}
			}

			return false;
		}
	}
}