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