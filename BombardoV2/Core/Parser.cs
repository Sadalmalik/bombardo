using System;
using System.Collections.Generic;
using System.Text;

namespace Bombardo.V2
{
    public class Parser
    {
        public static Atom Handle(List<Token> tokens)
        {
            Atom list = null;

            int offset = 0;
            int length = tokens.Count;
            while (offset < length)
            {
                if(tokens[offset].type == TokenType.Separator)
                {
                    offset++;
                    continue;
                }
                Atom node = ReadNode(tokens, ref offset, true);
                list = StructureUtils.BuildListContainer(list, node);
            }

            return list?.atom;
        }

        public static bool ValidateBrackets(List<Token> tokens)
        {
            int i = 0;
            foreach (var t in tokens)
            {
                if (t.value == "(")
                    i++;
                if (t.value == ")")
                    i--;
                if (i < 0)
                    return false;
            }
            if (i != 0)
                return false;
            return true;
        }

        private static bool IsSeparator(List<Token> tokens, ref int offset)
        {
            return tokens[offset].type == TokenType.Separator;
        }
        
        private static Atom ReadNode(List<Token> tokens, ref int offset, bool outer=false)
        {
            Atom atom = new Atom(AtomType.Undefined);
            Token token = tokens[offset];
            switch (token.type)
            {
                case TokenType.Quoting:
                    {
                        offset++;
                        Atom subnode = ReadNode(tokens, ref offset);
                        atom.type = AtomType.Pair;
                        atom.value = Atoms.QUOTE;
                        atom.next = new Atom(AtomType.Pair, subnode);
                    }
                    return atom;

                case TokenType.Operator:
                    {
                        if (token.value != "(" && token.value != "[" && token.value != "{")
                        {
                            StringBuilder lastTokens = new StringBuilder("Last tokens:\n");
                            for(int i=-20;i<0;i++) lastTokens.AppendFormat(" {0}", tokens[offset+i].value);
                            throw new ArgumentException("Incorrect ( ) [ ] { } order!\n"+ lastTokens.ToString());
                        }
                        offset++;
                        atom.type = AtomType.Pair;
                        Atom node = atom;
                        bool rest = false;
                        bool dotted = false;
                        
                        string ending = null;
                        switch (token.value)
                        {
                            case "(": ending = ")"; break;
                            case "[": ending = "]"; break;
                            case "{": ending = "}"; break;
                        }
                        
                        while (tokens[offset].value != ending)
                        {
                            if (IsSeparator(tokens, ref offset))
                            {
                                offset++;
                                continue;
                            }
                            if (dotted)
                                throw new ArgumentException("Token \".\" not allowed here! ");
                            //  Точка не может быть первой
                            bool isDot = (tokens[offset].value == ".");
                            if (!rest && isDot)
                                throw new ArgumentException("Token \".\" not allowed here!");
                            if (rest)
                            {
                                if(isDot)
                                {
                                    offset++;
                                    dotted = true;
                                    while(IsSeparator(tokens, ref offset))
                                        offset++;
                                }
                                else
                                {
                                    node.next = new Atom();
                                    node = node.next;
                                }
                            }
                            Atom subnode = ReadNode(tokens, ref offset);
                            if (isDot) node.next = subnode;
                            else node.value = subnode;
                            rest = true;
                        }
                    }
                    break;

                case TokenType.String:
                    {
                        atom.type = AtomType.String;
                        atom.value = token.value
                            .Substring(1, token.value.Length - 2)
                            .Replace("\\n", "\n")
                            .Replace("\\t", "\t")
                            .Replace("\\\"", "\"")
                            .Replace("\\\\", "\\");
                    }
                    break;

                case TokenType.Symbol:
                    {
                        if(outer && token.value==".")
                            throw new ArgumentException("Token \".\" not allowed here!");

                        if (token.value == Names.NULL_SYMBOL)
                        {
                            atom = null;
                            break;
                        }
                        atom.type = AtomType.Symbol;
                        atom.value = token.value;
                    }
                    break;

                default:
                    {
                        throw new ArgumentException(string.Format("Unexpected token {0} '{1}'", token.type, token.value));
                    }
            }
            offset++;

            return atom;
        }
    }
}