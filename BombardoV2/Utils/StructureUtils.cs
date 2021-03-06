using System;
using System.Collections.Generic;

namespace Bombardo.V2
{
	public static class StructureUtils
	{
        public static bool Compare(Atom a, Atom b)
        {
            bool aNull = (a == null);
            bool bNull = (b == null);
            if (aNull && bNull)
                return true;
            if ((aNull && !bNull) || (!aNull && bNull))
                return false;
            if (a.type != b.type)
                return false;
            if (a.type == AtomType.Pair)
                return Compare((Atom)a.value, (Atom)b.value) && Compare(a.next, b.next);
            return a.value.Equals(b.value);
        }
        
        public static Atom List(params Atom[] atoms)
        {
            Atom list = null, tail = null;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (tail == null)
                    tail = list = new Atom();
                else tail = tail.next = new Atom();
                tail.value = atoms[i];
            }
            return list;
        }
        
        public static Atom[] ToArray(Atom list)
        {
            List<Atom> array = new List<Atom>();
            while (list!=null)
            {
                array.Add(list.atom);
                list = list.next;
            }
            return array.ToArray();
        }

        public static string[] ListToStringArray(Atom names, string tag)
        {
            List<string> nameList = new List<string>();

            while (names != null)
            {
                Atom key = names.atom;
                if (key.type != AtomType.String && key.type != AtomType.Symbol)
                    throw new BombardoException(string.Format("<{0}> key must be string or symbol!", tag));
                nameList.Add((string)key.value);
                names = names.next;
            }

            return nameList.ToArray();
        }
        
        public static Atom CloneList(Atom atom)
        {
            if (atom == null) return null;
            switch (atom.type)
            {
                case AtomType.Pair:
                    return new Atom(
                        atom.atom,
                        CloneList(atom.next));

                default:
                    return atom;
            }
        }

        public static Atom CloneTree(Atom atom)
        {
            if (atom == null) return null;
            switch (atom.type)
            {
                case AtomType.Pair:
                    return new Atom(
                        CloneTree((Atom)atom.value),
                        CloneTree(atom.next));

                default:
                    return atom;
            }
        }
        
		public static Atom BuildListContainer(Atom container, Atom value)
		{
			if(container==null)
			{
				var head = new Atom(value, null);
				container = new Atom(head, head);
			}
			else
			{
				var tail = container.next;
				tail.next = new Atom(value, null);
				container.next = tail.next;
			}
			return container;
		}
		
		public static Atom Reverse(Atom current)
        {
            Atom prev = null;
            while (current != null)
            {
                var next = current.next;
                current.next = prev;
                prev = current;
                current = next;
            }
            return prev;
        }
		
        public static void Each(Atom list, Action<Atom> callback)
        {
            if (list.type != AtomType.Pair)
                throw new ArgumentException("Atom must be list!");
            if (callback == null)
                throw new ArgumentException("Callback can't be null!");
            
            for (var iter = list; iter != null; iter = iter.next)
                callback(iter.atom);
        }
        
        public static void Each2(Atom list, Action<Atom, Atom> callback)
        {
            if (list.type != AtomType.Pair)
                throw new ArgumentException("Atom must be list!");
            if (callback == null)
                throw new ArgumentException("Callback can't be null!");
                
			for (Atom iter = list; iter?.next != null; iter = iter.next)
                callback(iter.atom, iter.next.atom);
        }
        
        public static (Atom, Atom) Split2(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2;
            (a_1, iter) = ((Atom)iter?.value, iter?.next);
            (a_2, iter) = ((Atom)iter?.value, iter?.next);
            return (a_1, a_2);
        }
        
        public static (Atom, Atom, Atom) Split2Next(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2;
            (a_1, iter) = ((Atom)iter?.value, iter?.next);
            (a_2, iter) = ((Atom)iter?.value, iter?.next);
            return (a_1, a_2, iter);
        }
        
        public static (Atom, Atom, Atom) Split3(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3;
            (a_1, iter) = ((Atom)iter?.value, iter?.next);
            (a_2, iter) = ((Atom)iter?.value, iter?.next);
            (a_3, iter) = ((Atom)iter?.value, iter?.next);
            return (a_1, a_2, a_3);
        }
        
        public static (Atom, Atom, Atom, Atom) Split4(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3, a_4;
            (a_1, iter) = ((Atom)iter?.value, iter?.next);
            (a_2, iter) = ((Atom)iter?.value, iter?.next);
            (a_3, iter) = ((Atom)iter?.value, iter?.next);
            (a_4, iter) = ((Atom)iter?.value, iter?.next);
            return (a_1, a_2, a_3, a_4);
        }
        
        public static (Atom, Atom, Atom, Atom, Atom) Split5(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3, a_4, a_5;
            (a_1, iter) = ((Atom)iter?.value, iter?.next);
            (a_2, iter) = ((Atom)iter?.value, iter?.next);
            (a_3, iter) = ((Atom)iter?.value, iter?.next);
            (a_4, iter) = ((Atom)iter?.value, iter?.next);
            (a_5, iter) = ((Atom)iter?.value, iter?.next);
            return (a_1, a_2, a_3, a_4, a_5);
        }

        public static (Atom, Atom, Atom, Atom, Atom, Atom) Split6(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3, a_4, a_5, a_6;
            (a_1, iter) = ((Atom)iter?.value, iter?.next);
            (a_2, iter) = ((Atom)iter?.value, iter?.next);
            (a_3, iter) = ((Atom)iter?.value, iter?.next);
            (a_4, iter) = ((Atom)iter?.value, iter?.next);
            (a_5, iter) = ((Atom)iter?.value, iter?.next);
            (a_6, iter) = ((Atom)iter?.value, iter?.next);
            return (a_1, a_2, a_3, a_4, a_5, a_6);
        }
        
        public static (Atom, Atom, Atom, Atom, Atom, Atom, Atom) Split7(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3, a_4, a_5, a_6, a_7;
            (a_1, iter) = ((Atom)iter?.value, iter?.next);
            (a_2, iter) = ((Atom)iter?.value, iter?.next);
            (a_3, iter) = ((Atom)iter?.value, iter?.next);
            (a_4, iter) = ((Atom)iter?.value, iter?.next);
            (a_5, iter) = ((Atom)iter?.value, iter?.next);
            (a_6, iter) = ((Atom)iter?.value, iter?.next);
            (a_7, iter) = ((Atom)iter?.value, iter?.next);
            return (a_1, a_2, a_3, a_4, a_5, a_6, a_7);
        }
	}
}