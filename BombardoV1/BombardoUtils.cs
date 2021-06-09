using System;
using System.Collections.Generic;

namespace Bombardo.V1
{
    public class BombardoUtils
    {
        public static List<Atom> AtomToList(Atom atom)
        {
            if(atom.type==AtomType.Pair)
            {
                List<Atom> list = new List<Atom>();
                for (; atom != null; atom = atom.next)
                    list.Add((Atom)atom.value);
                return list;
            }
            return null;
        }

        public static Atom ListToAtom(List<Atom>list)
        {
            if (list == null || list.Count==0)
                return new Atom();
            bool rest = false;
            Atom tail = new Atom();
            Atom head = tail;
            for (int i = 0; i < list.Count; i++)
            {
                if (rest) tail = tail.next = new Atom(); rest = true;
                tail.value = list[i];
            }
            return head;
        }

        public static Atom ToAtom<T>(T t)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return new Atom(AtomType.Bool, t);
            if (type == typeof(string))
                return new Atom(AtomType.String, t);
            if (UNumber.IsNumber(t))
                return new Atom(AtomType.Number, t);
            if (type == typeof(Atom))
                return t as Atom;
            if (type == typeof(List<Atom>))
                return ListToAtom(t as List<Atom>);
            return new Atom(AtomType.Native,t);
        }

        public static Atom ToList<T1, T2>(T1 t1, T2 t2)
        {
            Atom tail = new Atom();
            Atom head = tail;
            tail.value = ToAtom(t1);
            tail = tail.next = new Atom();
            tail.value = ToAtom(t2);
            return head;
        }

        public static Atom ToList<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
        {
            Atom tail = new Atom();
            Atom head = tail;
            tail.value = ToAtom(t1);
            tail = tail.next = new Atom();
            tail.value = ToAtom(t2);
            tail = tail.next = new Atom();
            tail.value = ToAtom(t3);
            return head;
        }

        public static Atom ToList<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            Atom tail = new Atom();
            Atom head = tail;
            tail.value = ToAtom(t1);
            tail = tail.next = new Atom();
            tail.value = ToAtom(t2);
            tail = tail.next = new Atom();
            tail.value = ToAtom(t3);
            tail = tail.next = new Atom();
            tail.value = ToAtom(t4);
            return head;
        }
    }
}
