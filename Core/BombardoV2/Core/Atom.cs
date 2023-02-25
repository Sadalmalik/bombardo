using System;
using System.Text;

namespace Bombardo.V2
{
	public static class AtomType
	{
		public const int Undefined = -1;
		public const int Pair = 0;
		public const int Symbol = 1;
		public const int String = 2;
		public const int Bool = 3;
		public const int Number = 4;
		public const int Function = 5;
		public const int Native = 6;

		public static string ToString(int type)
		{
			switch (type)
			{
				case Pair:      return "List";
				case Symbol:    return "Symbol";
				case String:    return "String";
				case Bool:      return "Bool";
				case Number:    return "Number";
				case Function:  return "Function";
				case Native:    return "Native";
				default:        return "Undefined";
			}
		}
	}

	public class Atom
	{
		public static readonly string EMPTY_PAIR = "()";

		public int type;
		public object value;

		public Atom atom => value as Atom;

		public Atom next;

		public Atom()
		{
			type  = AtomType.Pair;
			value = null;
			next  = null;
		}

		public Atom(string Value)
		{
			type  = AtomType.Symbol;
			value = Value;
			next  = null;
		}

		public Atom(int Type, object Value = null)
		{
			type  = Type;
			value = Value;
			next  = null;
		}

		public Atom(Atom Value, Atom Next)
		{
			type  = AtomType.Pair;
			value = Value;
			next  = Next;
		}

		public Atom(StringBuilder sb)
		{
			type  = AtomType.String;
			value = sb.ToString();
			next  = null;
		}

		public bool IsEmpty => type == AtomType.Pair && value == null && next == null;
		public bool IsPair => type == AtomType.Pair;
		public bool IsSymbol => type == AtomType.Symbol;
		public bool IsString => type == AtomType.String;
		public bool IsBool => type == AtomType.Bool;
		public bool IsNumber => type == AtomType.Number;
		public bool IsFunction => type == AtomType.Function;
		public bool IsNative => type == AtomType.Native;

		public static Atom FromString(string value)
		{
			if (string.IsNullOrEmpty(value))
				return null;
			return new Atom(AtomType.String, value);
		}
		
		public int ListLength()
		{
			if (type != AtomType.Pair) return -1;
			int count = 0;
			for (Atom item = this; item != null; item = item.next) count++;
			return count;
		}

		public Atom ListSkip(int count)
		{
			Atom atom = this;
			while (count > 0 && atom != null)
			{
				atom = atom.next;
				count--;
			}

			return atom;
		}

		public string Stringify(bool recursive = true, bool wrapped = false)
		{
			switch (type)
			{
				case AtomType.Symbol: return (string) value;
				case AtomType.String: return string.Format("\"{0}\"", value);
				case AtomType.Bool:   return (bool) value ? "true" : "false";
				case AtomType.Number: return value.ToString();
				case AtomType.Pair:
				{
					if (this.value == null && this.next == null)
						return Atom.EMPTY_PAIR;
					StringBuilder sb = new StringBuilder();
					if (!wrapped) sb.Append("( ");

					Atom value = (Atom) this.value;
					if (value == null)
					{
						sb.Append(Names.NULL_SYMBOL);
					}
					else if (recursive || value.type != AtomType.Pair)
					{
						sb.Append(value.Stringify(recursive));
					}
					else
					{
						sb.Append("<list>");
					}

					if (next != null)
					{
						sb.Append(" ");
						if (next.type == AtomType.Pair)
						{
							sb.Append(next.Stringify(recursive, true));
						}
						else
						{
							sb.Append(". ");
							sb.Append(next.Stringify(recursive, true));
						}
					}

					if (!wrapped) sb.Append(" )");

					return sb.ToString();
				}

				case AtomType.Function:
					return string.Format("<procedure {0}>", ((Function) value).Name);

				case AtomType.Native:
					return string.Format("<object {0}>", value.ToString());

				default:
					return "undefined";
			}
		}

		public override string ToString()
		{
			return Stringify(true);
		}
	}
}