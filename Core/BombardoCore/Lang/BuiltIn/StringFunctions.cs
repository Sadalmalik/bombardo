using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Bombardo.Core
{
    public static partial class Names
    {
        public static readonly string TEXT_CREATE   = "create";
        public static readonly string TEXT_LENGTH   = "len";
        public static readonly string TEXT_GETCHARS = "chars";
        public static readonly string TEXT_GETCHAR  = "get";

        public static readonly string TEXT_CONCAT    = "concat";
        public static readonly string TEXT_SUBSTR    = "substr";
        public static readonly string TEXT_SPLIT     = "split";
        public static readonly string TEXT_REPLACE   = "replace";
        public static readonly string TEXT_REG_MATCH = "regex-match";

        public static readonly string TEXT_STARTSWITH = "startsWith?";
        public static readonly string TEXT_ENDSWITH   = "endsWith?";
        public static readonly string TEXT_CONTAINS   = "contains?";
    }

    public class StringFunctions
    {
        public static void Define(Context ctx)
        {
            //  num is char
            //  (str-create (num num num)) -> num
            //  (str-length string) -> length
            //  (str-get string element) -> num
            //  
            //  (str-concat string1 string2 string3) -> string1string2string3
            //  (str-substr string start length) -> substring
            //  (str-split string separator) -> ("substring1" "substring2" "substring3")
            //
            //  (str-starts-with string check) -> true|false
            //  (str-ends-with string check) -> true|false
            //  (str-contains string substring) -> true|false
            //
            //  (str-replace string subString newSubString) -> string

            ctx.DefineFunction(Names.TEXT_CREATE, Create);
            ctx.DefineFunction(Names.TEXT_LENGTH, Length);
            ctx.DefineFunction(Names.TEXT_GETCHARS, GetChars);
            ctx.DefineFunction(Names.TEXT_GETCHAR, GetChar);

            ctx.DefineFunction(Names.TEXT_CONCAT, Concat);
            ctx.DefineFunction(Names.TEXT_SUBSTR, Substr);
            ctx.DefineFunction(Names.TEXT_SPLIT, Split);
            ctx.DefineFunction(Names.TEXT_REPLACE, Replace);
            ctx.DefineFunction(Names.TEXT_REG_MATCH, Match);

            ctx.DefineFunction(Names.TEXT_STARTSWITH, StartsWith);
            ctx.DefineFunction(Names.TEXT_ENDSWITH, EndsWith);
            ctx.DefineFunction(Names.TEXT_CONTAINS, Contains);
        }

        private static void Create(Evaluator eval, StackFrame frame)
        {
            Atom list = frame.args?.Head;

            if (!list.IsPair)
                throw new ArgumentException("Argument must be list!");

            StringBuilder sb = new StringBuilder();
            for (Atom iter = list; iter != null; iter = iter.Next)
            {
                Atom ch = iter.Head;

                if (!ch.IsNumber)
                    throw new ArgumentException("List must contains only numbers!");

                sb.Append(ch.number.ToChar());
            }

            eval.Return(Atom.CreateString(sb.ToString()));
        }

        private static void GetChars(Evaluator eval, StackFrame frame)
        {
            Atom str = frame.args?.Head;

            if (!str.IsString)
                throw new ArgumentException("Argument must be string!");

            char[] chars = str.@string.ToCharArray();

            Atom list = null, tail = null;
            for (int i = 0; i < chars.Length; i++)
            {
                if (tail == null)
                    tail  = list           = Atom.CreatePair(null, null);
                else tail = tail.pair.next = Atom.CreatePair(null, null);
                tail.pair.atom = Atom.CreateNumber(chars[i]);
            }

            eval.Return(list);
        }

        private static void GetChar(Evaluator eval, StackFrame frame)
        {
            var (str, num) = StructureUtils.Split2(frame.args);

            if (!str.IsString)
                throw new ArgumentException("Argument must be string!");
            if (!num.IsNumber)
                throw new ArgumentException("Argument must be number!");

            int index = num.number.ToSInt();
            eval.Return(Atom.CreateNumber(str.@string[index]));
        }

        private static void Length(Evaluator eval, StackFrame frame)
        {
            Atom str = frame.args?.Head;

            if (!str.IsString)
                throw new ArgumentException("Argument must be string!");

            eval.Return(Atom.CreateNumber(str.@string.Length));
        }

        private static void Concat(Evaluator eval, StackFrame frame)
        {
            StringBuilder sb = new StringBuilder();

            for (Atom iter = frame.args; iter != null; iter = iter.Next)
            {
                Atom atom = iter.Head;
                if (atom == null)
                    continue;
                if (atom.type == AtomType.String ||
                    atom.type == AtomType.Symbol)
                    sb.Append(atom.@string);
                else
                    sb.Append(atom.Stringify(true));
            }

            eval.Return(Atom.CreateString(sb.ToString()));
        }

        private static void Substr(Evaluator eval, StackFrame frame)
        {
            var (strArg, starts, length) = StructureUtils.Split3(frame.args);

            if (!strArg.IsString)
                throw new ArgumentException("first argument must be string!");
            if (!starts.IsNumber)
                throw new ArgumentException("second argument must be string!");
            if (!length.IsNumber)
                throw new ArgumentException("third argument must be string!");

            string str = strArg.@string;
            string res = str.Substring(starts.number.ToSInt(), length.number.ToSInt());

            eval.Return(Atom.CreateString(res));
        }

        private static void Split(Evaluator eval, StackFrame frame)
        {
            var (strArg, splits) = StructureUtils.Split2(frame.args);

            if (!strArg.IsString)
                throw new ArgumentException("first argument must be string!");
            if (!splits.IsString)
                throw new ArgumentException("second argument must be string!");

            string   str  = strArg.@string;
            string   spl  = splits.@string;
            string[] list = str.Split(new[] {spl}, StringSplitOptions.RemoveEmptyEntries);

            Atom head, tail;
            head = tail = Atom.CreatePair(Atom.CreateString(list[0]), null);
            for (int i = 1; i < list.Length; i++)
                tail = tail.pair.next = Atom.CreatePair(Atom.CreateString(list[i]), null);

            eval.Return(head);
        }

        private static void Replace(Evaluator eval, StackFrame frame)
        {
            var (strArg, subArg, newArg) = StructureUtils.Split3(frame.args);

            if (!strArg.IsString)
                throw new ArgumentException("first argument must be string!");
            if (!subArg.IsString)
                throw new ArgumentException("second argument must be string!");
            if (!newArg.IsString)
                throw new ArgumentException("third argument must be string!");

            string res =
                strArg.@string.Replace(
                    subArg.@string,
                    newArg.@string);

            eval.Return(Atom.CreateString(res));
        }

        private static void Match(Evaluator eval, StackFrame frame)
        {
            var (pattern, text) = StructureUtils.Split2(frame.args);

            if (!pattern.IsString)
                throw new Exception("Pattern must be string!");
            if (!text.IsString)
                throw new Exception("Text must be string!");

            var result = Regex.Match(pattern.@string, text.@string);

            eval.Return(result.Success ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void StartsWith(Evaluator eval, StackFrame frame)
        {
            var (strArg, subArg) = StructureUtils.Split2(frame.args);

            if (!strArg.IsString)
                throw new ArgumentException("first argument must be string!");
            if (!subArg.IsString)
                throw new ArgumentException("first argument must be string!");

            bool result = strArg.@string.StartsWith(subArg.@string);

            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void EndsWith(Evaluator eval, StackFrame frame)
        {
            var (strArg, subArg) = StructureUtils.Split2(frame.args);

            if (!strArg.IsString)
                throw new ArgumentException("first argument must be string!");
            if (!subArg.IsString)
                throw new ArgumentException("first argument must be string!");

            bool result = strArg.@string.EndsWith(subArg.@string);

            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void Contains(Evaluator eval, StackFrame frame)
        {
            var (strArg, subArg) = StructureUtils.Split2(frame.args);

            if (!strArg.IsString)
                throw new ArgumentException("first argument must be string!");
            if (!subArg.IsString)
                throw new ArgumentException("second argument must be string!");

            bool result = strArg.@string.Contains(subArg.@string);

            eval.Return(result ? Atoms.TRUE : Atoms.FALSE);
        }
    }
}