using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bombardo.Core
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
            switch (a.type)
            {
                case AtomType.Pair:
                    return Compare(a.pair.atom, a.pair.atom) &&
                           Compare(a.pair.next, b.pair.next);
                case AtomType.Symbol:
                case AtomType.String:
                    return a.@string == b.@string;
                case AtomType.Bool:
                    return a.@bool == b.@bool;
                case AtomType.Number:
                    return a.number.Equals(b.@number);
                case AtomType.Function:
                    return a.@function.Equals(b.@function);
                case AtomType.Native:
                    return a.@object.Equals(b.@object);
            }

            return false;
        }

        public static Atom List(params Atom[] atoms)
        {
            Atom list = null, tail = null;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (tail == null)
                    tail = list = Atom.CreatePair(null, null);
                else
                    tail = tail.pair.next = Atom.CreatePair(null, null);
                tail.pair.atom = atoms[i];
            }

            return list;
        }

        public static Atom[] ToArray(Atom list)
        {
            List<Atom> array = new List<Atom>();
            while (list != null)
            {
                array.Add(list.Head);
                list = list.Next;
            }

            return array.ToArray();
        }

        public static string[] ListToStringArray(Atom names, string tag)
        {
            List<string> nameList = new List<string>();

            while (names != null)
            {
                Atom key = names.Head;
                if (key.type != AtomType.String && key.type != AtomType.Symbol)
                    throw new BombardoException(string.Format("<{0}> key must be string or symbol!", tag));
                nameList.Add(key.@string);
                names = names.Next;
            }

            return nameList.ToArray();
        }

        public static Atom CloneList(Atom atom)
        {
            if (atom == null) return null;
            switch (atom.type)
            {
                case AtomType.Pair:
                    return Atom.CreatePair(atom.Head, CloneList(atom.Next));

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
                    return Atom.CreatePair(CloneTree(atom.Head), CloneTree(atom.Next));

                default:
                    return atom;
            }
        }

        public static Atom BuildListContainer(Atom container, Atom value)
        {
            if (container == null)
            {
                var head = Atom.CreatePair(value, null);
                container = Atom.CreatePair(head, head);
            }
            else
            {
                var tail = container.Next;
                tail.pair.next      = Atom.CreatePair(value, null);
                container.pair.next = tail.pair.next;
            }

            return container;
        }

        public static Atom Reverse(Atom current)
        {
            Atom prev = null;
            while (current != null)
            {
                var next = current.Next;
                current.pair.next = prev;
                prev              = current;
                current           = next;
            }

            return prev;
        }

        public static void Each(Atom list, Action<Atom> callback)
        {
            if (list.type != AtomType.Pair)
                throw new ArgumentException("Atom must be list!");
            if (callback == null)
                throw new ArgumentException("Callback can't be null!");

            for (var iter = list; iter != null; iter = iter.Next)
                callback(iter.Head);
        }

        public static void Each2(Atom list, Action<Atom, Atom> callback)
        {
            if (list.type != AtomType.Pair)
                throw new ArgumentException("Atom must be list!");
            if (callback == null)
                throw new ArgumentException("Callback can't be null!");

            for (Atom iter = list; iter?.Next != null; iter = iter.Next)
                callback(iter.Head, iter.Next.Head);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Atom Split1(Atom value)
        {
            Atom iter = value;
            Atom a_1;
            (a_1, iter) = (iter?.Head, iter?.Next);
            return (a_1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Atom, Atom) Split1Next(Atom value)
        {
            Atom iter = value;
            Atom a_1;
            (a_1, iter) = (iter?.Head, iter?.Next);
            return (a_1, iter);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Atom, Atom) Split2(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2;
            (a_1, iter) = (iter?.Head, iter?.Next);
            (a_2, iter) = (iter?.Head, iter?.Next);
            return (a_1, a_2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Atom, Atom, Atom) Split2Next(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2;
            (a_1, iter) = (iter?.Head, iter?.Next);
            (a_2, iter) = (iter?.Head, iter?.Next);
            return (a_1, a_2, iter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Atom, Atom, Atom) Split3(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3;
            (a_1, iter) = (iter?.Head, iter?.Next);
            (a_2, iter) = (iter?.Head, iter?.Next);
            (a_3, iter) = (iter?.Head, iter?.Next);
            return (a_1, a_2, a_3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Atom, Atom, Atom, Atom) Split4(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3, a_4;
            (a_1, iter) = (iter?.Head, iter?.Next);
            (a_2, iter) = (iter?.Head, iter?.Next);
            (a_3, iter) = (iter?.Head, iter?.Next);
            (a_4, iter) = (iter?.Head, iter?.Next);
            return (a_1, a_2, a_3, a_4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Atom, Atom, Atom, Atom, Atom) Split5(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3, a_4, a_5;
            (a_1, iter) = (iter?.Head, iter?.Next);
            (a_2, iter) = (iter?.Head, iter?.Next);
            (a_3, iter) = (iter?.Head, iter?.Next);
            (a_4, iter) = (iter?.Head, iter?.Next);
            (a_5, iter) = (iter?.Head, iter?.Next);
            return (a_1, a_2, a_3, a_4, a_5);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Atom, Atom, Atom, Atom, Atom, Atom) Split6(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3, a_4, a_5, a_6;
            (a_1, iter) = (iter?.Head, iter?.Next);
            (a_2, iter) = (iter?.Head, iter?.Next);
            (a_3, iter) = (iter?.Head, iter?.Next);
            (a_4, iter) = (iter?.Head, iter?.Next);
            (a_5, iter) = (iter?.Head, iter?.Next);
            (a_6, iter) = (iter?.Head, iter?.Next);
            return (a_1, a_2, a_3, a_4, a_5, a_6);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Atom, Atom, Atom, Atom, Atom, Atom, Atom) Split7(Atom value)
        {
            Atom iter = value;
            Atom a_1, a_2, a_3, a_4, a_5, a_6, a_7;
            (a_1, iter) = (iter?.Head, iter?.Next);
            (a_2, iter) = (iter?.Head, iter?.Next);
            (a_3, iter) = (iter?.Head, iter?.Next);
            (a_4, iter) = (iter?.Head, iter?.Next);
            (a_5, iter) = (iter?.Head, iter?.Next);
            (a_6, iter) = (iter?.Head, iter?.Next);
            (a_7, iter) = (iter?.Head, iter?.Next);
            return (a_1, a_2, a_3, a_4, a_5, a_6, a_7);
        }
    }
}