using System;
using System.Collections.Generic;
using System.Text;

namespace Bombardo.Core
{
	public static class Parser
	{
		private static Dictionary<string, string> _brackets;

		public static void Init()
		{
			if (_brackets != null)
				return;

			_brackets = new Dictionary<string, string>
			{
				["("] = ")",
				["["] = "]",
				["{"] = "}"
			};
		}

		public static Atom Handle(List<Token> tokens)
		{
			Atom list = null;

			int offset = 0;
			int length = tokens.Count;
			while (offset < length)
			{
				Atom node = ReadNode(tokens, ref offset, true);
				list = StructureUtils.BuildListContainer(list, node);
			}

			return list?.Atom;
		}

		private static void SkipSeparator(List<Token> tokens, ref int offset)
		{
			while (offset < tokens.Count && tokens[offset].type == TokenType.Separator)
				offset++;
		}

		private static Atom ReadNode(List<Token> tokens, ref int offset, bool outer = false)
		{
			SkipSeparator(tokens, ref offset);
			Atom  atom  = new Atom(AtomType.Undefined);
			Token token = tokens[offset];

			switch (token.type)
			{
				case TokenType.Brackets:
				{
					if (!_brackets.ContainsKey(token.value))
					{
						StringBuilder lastTokens = new StringBuilder("Last tokens:\n");
						for (int i = -20; i < 0; i++) lastTokens.AppendFormat(" {0}", tokens[offset + i].value);
						throw new ArgumentException("Incorrect ( ) [ ] { } order!\n" + lastTokens);
					}

					offset++;

					atom.type = AtomType.Pair;
					Atom node = atom;

					string ending = _brackets[token.value];

					bool first = true;
					while (tokens[offset].value != ending)
					{
						if (!first)
							node = node.pair.next = new Atom(AtomType.Pair);
						Atom subnode = ReadNode(tokens, ref offset);
						node.pair.atom = subnode;
						first     = false;
					}
				}
					break;

				case TokenType.Symbol:
				{
					atom.type  = AtomType.Symbol;
					atom.@string = token.value;
				}
					break;

				case TokenType.String:
				{
					atom.type    = AtomType.String;
					atom.@string = token.value;
				}
					break;

				default:
				{
					throw new ArgumentException(string.Format("Unexpected token {0} '{1}'", token.type, token.value));
				}
			}

			offset++;
			SkipSeparator(tokens, ref offset);

			return atom;
		}
	}
}