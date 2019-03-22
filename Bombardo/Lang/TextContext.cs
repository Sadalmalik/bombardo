﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombardo
{
    internal class TextContext
    {
        public static void Setup(Context context)
        {
            //  num is char
            //  (str-create string (num num num)) -> num
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

            BombardoLangClass.SetProcedure(context, "str-create", Create, 1);
            BombardoLangClass.SetProcedure(context, "str-length", Length, 1);
            BombardoLangClass.SetProcedure(context, "str-chars", GetChars, 1);
            BombardoLangClass.SetProcedure(context, "str-get", GetChar, 2);

            BombardoLangClass.SetProcedure(context, "str-concat", Concat, 0);
            BombardoLangClass.SetProcedure(context, "str-substr", Substr, 3);
            BombardoLangClass.SetProcedure(context, "str-split", Split, 2);

            BombardoLangClass.SetProcedure(context, "str-starts-with", StartsWith, 2);
            BombardoLangClass.SetProcedure(context, "str-ends-with", EndsWith, 2);
            BombardoLangClass.SetProcedure(context, "str-contains", Contains, 2);

            BombardoLangClass.SetProcedure(context, "str-replace", Replace, 3);
        }

        public static Atom Create(Atom args, Context context)
        {
            Atom list = (Atom)args?.value;

            if(!list.IsPair())
                throw new BombardoException("<STR-CREATE> Argument must be list!");

            StringBuilder sb = new StringBuilder();
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                Atom ch = list.value as Atom;

                if(ch==null || !ch.IsNumber())
                    throw new BombardoException("<STR-CREATE> List musr contains only numbers!");

                sb.Append((char)ch.value);
            }

            return new Atom(sb);
        }

        public static Atom GetChars(Atom args, Context context)
        {
            Atom str = (Atom)args?.value;

            if (str == null || str.type != AtomType.String)
                throw new BombardoException("<STR-GET> Argument must be string!");
            
            char[] chars = ((string)str.value).ToCharArray();

            Atom list = null, tail = null;
            for (int i = 0; i < chars.Length; i++)
            {
                if (tail == null)
                    tail = list = new Atom();
                else tail = tail.next = new Atom();
                tail.type = AtomType.Number;
                tail.value = chars[i];
            }
            return list;
        }

        public static Atom GetChar(Atom args, Context context)
        {
            Atom str = (Atom)args?.value;
            Atom num = (Atom)args?.next?.value;

            if (str == null || str.type != AtomType.String)
                throw new BombardoException("<STR-GET> Argument must be string!");

            if (num == null || num.type != AtomType.Number)
                throw new BombardoException("<STR-GET> Argument must be number!");

            int index = Convert.ToInt32(num.value);

            return new Atom(AtomType.Number, (int)((string)str.value)[index]);
        }

        public static Atom Length(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;

            if (atom.type != AtomType.String)
                throw new BombardoException("<STR-LENGTH> Argument must be string!");

            return new Atom(AtomType.Number,((string)atom.value).Length);
        }

        public static Atom Concat(Atom args, Context context)
        {
            StringBuilder sb = new StringBuilder();

            for(Atom iter=args; iter != null; iter = iter.next)
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

            return new Atom(sb);
        }

        public static Atom Substr(Atom args, Context context)
        {
            Atom strArg = (Atom)args?.value;
            Atom starts = (Atom)args?.next?.value;
            Atom length = (Atom)args?.next?.next?.value;

            if (!strArg.IsString()) throw new BombardoException("<STR-SUBSTR> first argument must be string!");
            if (!starts.IsNumber()) throw new BombardoException("<STR-SUBSTR> second argument must be string!");
            if (!length.IsNumber()) throw new BombardoException("<STR-SUBSTR> third argument must be string!");
            
            string str = strArg.value as string;

            return new Atom(AtomType.String, str.Substring((int)starts.value, (int)length.value));
        }

        public static Atom Split(Atom args, Context context)
        {
            Atom strArg = (Atom)args?.value;
            Atom splits = (Atom)args?.next?.value;

            if (!strArg.IsString()) throw new BombardoException("<STR-SPLIT> first argument must be string!");
            if (!splits.IsString()) throw new BombardoException("<STR-SPLIT> second argument must be string!");

            string str = strArg.value as string;
            string spl = splits.value as string;
            string[] list = str.Split(new string[] { spl }, StringSplitOptions.RemoveEmptyEntries);

            Atom head, tail;
            head = tail = new Atom();
            tail.value = new Atom(AtomType.String, list[0]);
            for (int i=1;i<list.Length;i++)
            {
                tail = tail.next = new Atom();
                tail.value = new Atom(AtomType.String, list[0]);
            }

            return head;
        }

        public static Atom StartsWith(Atom args, Context context)
        {
            Atom strArg = (Atom)args?.value;
            Atom subArg = (Atom)args?.next?.value;

            if (!strArg.IsString()) throw new BombardoException("<STR-STARTS-WITH> first argument must be string!");
            if (!subArg.IsString()) throw new BombardoException("<STR-STARTS-WITH> first argument must be string!");

            string str = strArg.value as string;
            string sub = subArg.value as string;

            return str.StartsWith(sub) ? Atom.TRUE : Atom.FALSE;
        }

        public static Atom EndsWith(Atom args, Context context)
        {
            Atom strArg = (Atom)args?.value;
            Atom subArg = (Atom)args?.next?.value;

            if (!strArg.IsString()) throw new BombardoException("<STR-ENDS-WITH> first argument must be string!");
            if (!subArg.IsString()) throw new BombardoException("<STR-ENDS-WITH> first argument must be string!");

            string str = strArg.value as string;
            string sub = subArg.value as string;

            return str.EndsWith(sub) ? Atom.TRUE : Atom.FALSE;
        }

        public static Atom Contains(Atom args, Context context)
        {
            Atom strArg = (Atom)args?.value;
            Atom subArg = (Atom)args?.next?.value;

            if (!strArg.IsString()) throw new BombardoException("<STR-ENDS-WITH> first argument must be string!");
            if (!subArg.IsString()) throw new BombardoException("<STR-ENDS-WITH> second argument must be string!");

            string str = strArg.value as string;
            string sub = subArg.value as string;

            return str.Contains(sub) ? Atom.TRUE : Atom.FALSE;
        }

        public static Atom Replace(Atom args, Context context)
        {
            Atom strArg = (Atom)args?.value;
            Atom subArg = (Atom)args?.next?.value;
            Atom newArg = (Atom)args?.next?.next?.value;

            if (!strArg.IsString()) throw new BombardoException("<STR-ENDS-WITH> first argument must be string!");
            if (!subArg.IsString()) throw new BombardoException("<STR-ENDS-WITH> second argument must be string!");
            if (!newArg.IsString()) throw new BombardoException("<STR-ENDS-WITH> third argument must be string!");

            string str = strArg.value as string;
            string sub = subArg.value as string;
            string n_w = newArg.value as string;

            return new Atom(AtomType.String, str.Replace(sub, n_w));
        }
    }
}
