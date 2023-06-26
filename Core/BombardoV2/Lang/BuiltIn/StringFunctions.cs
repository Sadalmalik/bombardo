using System;
using System.Text;

namespace Bombardo.V2
{
	public static partial class Names
	{
        public static readonly string TEXT_CREATE = "create";
        public static readonly string TEXT_LENGTH = "len";
        public static readonly string TEXT_GETCHARS = "chars";
        public static readonly string TEXT_GETCHAR = "get";

        public static readonly string TEXT_CONCAT = "concat";
        public static readonly string TEXT_SUBSTR = "substr";
        public static readonly string TEXT_SPLIT = "split";
        public static readonly string TEXT_REPLACE = "replace";

        public static readonly string TEXT_STARTSWITH = "startsWith?";
        public static readonly string TEXT_ENDSWITH = "endsWith?";
        public static readonly string TEXT_CONTAINS = "contains?";
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

            ctx.DefineFunction(Names.TEXT_STARTSWITH, StartsWith);
            ctx.DefineFunction(Names.TEXT_ENDSWITH, EndsWith);
            ctx.DefineFunction(Names.TEXT_CONTAINS, Contains);
        }

        public static void Create(Evaluator eval, StackFrame frame)
        {
            Atom list = (Atom)frame.args?.value;

            if(!list.IsPair) throw new ArgumentException("Argument must be list!");

            StringBuilder sb = new StringBuilder();
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                Atom ch = iter.value as Atom;

                if(!ch.IsNumber) throw new ArgumentException("List must contains only numbers!");

                sb.Append(Convert.ToChar(ch.value));
            }

            eval.Return(new Atom(AtomType.String, sb.ToString()));
        }

        public static void GetChars(Evaluator eval, StackFrame frame)
        {
            Atom strArg = (Atom)frame.args?.value;

            // if (!str.IsString) throw new ArgumentException("Argument must be string!");
            
            string str = strArg.value as string;
            char[] chars = str.ToCharArray();

            Atom list = null, tail = null;
            for (int i = 0; i < chars.Length; i++)
            {
                if (tail == null)
                    tail = list = new Atom();
                else tail = tail.next = new Atom();
                tail.value = new Atom(AtomType.Number, chars[i]);
            }
            
            eval.Return(list);
        }

        public static void GetChar(Evaluator eval, StackFrame frame)
        {
            var (strArg, num) = StructureUtils.Split2(frame.args);

            // if (!str.IsString) throw new ArgumentException("Argument must be string!");
            // if (!num.IsNumber) throw new ArgumentException("Argument must be number!");

            string str = strArg.value as string;
            int index = Convert.ToInt32(num.value);
            char res = str[index];
            
            eval.Return(new Atom(AtomType.Number, res));
        }

        public static void Length(Evaluator eval, StackFrame frame)
        {
            Atom atom = (Atom)frame.args?.value;

            // if (!atom.IsString) throw new ArgumentException("Argument must be string!");
            string str = atom.value as string;
            int res = str.Length;

            eval.Return(new Atom(AtomType.Number, res));
        }

        public static void Concat(Evaluator eval, StackFrame frame)
        {
            StringBuilder sb = new StringBuilder();

            for(Atom iter=frame.args; iter != null; iter = iter.next)
            {
                Atom atom = (Atom)iter.value;
                if (atom == null)
                    sb.Append("null");
                else if (atom.type == AtomType.String ||
                    atom.type == AtomType.Symbol)
                    sb.Append(atom.value);
                else
                    sb.Append(atom.Stringify(true));
            }

            eval.Return(new Atom(AtomType.String, sb.ToString()));
        }

        public static void Substr(Evaluator eval, StackFrame frame)
        {
            var (strArg, starts, length) = StructureUtils.Split3(frame.args);

            // if (!strArg.IsString) throw new ArgumentException("first argument must be string!");
            // if (!starts.IsNumber) throw new ArgumentException("second argument must be string!");
            // if (!length.IsNumber) throw new ArgumentException("third argument must be string!");
            
            string str = strArg.value as string;
            string res = str.Substring((int)starts.value, (int)length.value);

            eval.Return(new Atom(AtomType.String, res));
        }

        public static void Split(Evaluator eval, StackFrame frame)
        {
            var (strArg, splits) = StructureUtils.Split2(frame.args);

            // if (!strArg.IsString) throw new ArgumentException("first argument must be string!");
            // if (!splits.IsString) throw new ArgumentException("second argument must be string!");

            string str = strArg.value as string;
            string spl = splits.value as string;
            string[] list = str.Split(new string[] { spl }, StringSplitOptions.RemoveEmptyEntries);

            Atom head, tail;
            head = tail = new Atom();
            tail.value = new Atom(AtomType.String, list[0]);
            for (int i = 1; i < list.Length; i++)
            {
                tail = tail.next = new Atom();
                tail.value = new Atom(AtomType.String, list[i]);
            }

            eval.Return(head);
        }

        public static void Replace(Evaluator eval, StackFrame frame)
        {
            var (strArg, subArg, newArg) = StructureUtils.Split3(frame.args);

            // if (!strArg.IsString) throw new ArgumentException("first argument must be string!");
            // if (!subArg.IsString) throw new ArgumentException("second argument must be string!");
            // if (!newArg.IsString) throw new ArgumentException("third argument must be string!");

            string str = strArg.value as string;
            string sub = subArg.value as string;
            string n_w = newArg.value as string;
            string res = str.Replace(sub, n_w);

            eval.Return(new Atom(AtomType.String, res));
        }

        public static void StartsWith(Evaluator eval, StackFrame frame)
        {
            var (strArg, subArg) = StructureUtils.Split2(frame.args);

            // if (!strArg.IsString) throw new ArgumentException("first argument must be string!");
            // if (!subArg.IsString) throw new ArgumentException("first argument must be string!");

            string str = strArg.value as string;
            string sub = subArg.value as string;

            eval.Return(str.StartsWith(sub) ? Atoms.TRUE : Atoms.FALSE);
        }

        public static void EndsWith(Evaluator eval, StackFrame frame)
        {
            var (strArg, subArg) = StructureUtils.Split2(frame.args);

            // if (!strArg.IsString) throw new ArgumentException("first argument must be string!");
            // if (!subArg.IsString) throw new ArgumentException("first argument must be string!");

            string str = strArg.value as string;
            string sub = subArg.value as string;

            eval.Return(str.EndsWith(sub) ? Atoms.TRUE : Atoms.FALSE);
        }

        public static void Contains(Evaluator eval, StackFrame frame)
        {
            var (strArg, subArg) = StructureUtils.Split2(frame.args);

            // if (!strArg.IsString) throw new ArgumentException("first argument must be string!");
            // if (!subArg.IsString) throw new ArgumentException("second argument must be string!");

            string str = strArg.value as string;
            string sub = subArg.value as string;

            eval.Return(str.Contains(sub) ? Atoms.TRUE : Atoms.FALSE);
        }
	}
}