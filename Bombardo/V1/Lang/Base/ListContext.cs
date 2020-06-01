using System;

namespace Bombardo.V1
{
    class ListContext
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CONS, Cons, 0);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CAR, Car, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDR, Cdr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_GET, GetElement, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_LAST, Last, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_APPEND, Append, 1);
            
            BombardoLangClass.SetProcedure(context, AllNames.LISP_LIST, List, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_REVERSE, ReverseAtom, 1);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_SET_CAR, SetCar, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_SET_CDR, SetCdr, 2);
            
            BombardoLangClass.SetProcedure(context, AllNames.LISP_EACH, Each, 2, true);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_MAP, Map, 2, true);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_FILTER, Filter, 2, true);
            
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CONTAINS, Contains, 2, true);
        }

        public static Atom Cons(Atom args, Context context)
        {
            Atom pair = new Atom();
            pair.value = args?.value;
            pair.next = (Atom)args?.next?.value;
            return pair;
        }

        public static Atom Car(Atom args, Context context)
        {
            if (args == null) return null;
            Atom list = (Atom)args?.value;
            if (!list.IsPair()) throw new ArgumentException("Argument must be Pair!");
            return (Atom)list.value;
        }

        public static Atom Cdr(Atom args, Context context)
        {
            if (args == null) return null;
            Atom list = (Atom)args?.value;
            if (!list.IsPair()) throw new ArgumentException("Argument must be Pair!");
            return (Atom)list.next;
        }
        
        public static Atom Last(Atom args, Context context)
        {
            //  (define last (lambda [list]
            //      (if [null? (cdr list)]
            //          list
            //          (list (cdr list)))
            //  ))
            if (args == null) return null;
            Atom list = (Atom)args?.value;
            if (!list.IsPair()) throw new ArgumentException("Argument must be list!");
            while (
                list.IsPair() &&
                list.next != null)
                list = list.next;
            return list;
        }

        public static Atom Append(Atom args, Context context)
        {
            //  (define append (lambda [list item]
            //      (set-cdr! (last list) item) list
            //  ))
            Atom list1 = Atom.CloneList(args.atom);
            Atom list2 = Atom.CloneList(args.next.atom);

            if (list2 == null || list2.IsEmpty()) return list1;
            if (list1 == null || list1.IsEmpty()) return list2;

            if (!list1.IsPair()) throw new ArgumentException("Argument must be list!");

            Atom pair = list1;
            while (
                pair.IsPair() &&
                pair.next != null)
                pair = pair.next;
            pair.next = list2;

            return list1;
        }

        public static Atom GetElement(Atom args, Context context)
        {
            Atom arg0 = (Atom)args?.value;
            Atom arg1 = (Atom)args?.next?.value;

            if (arg0.type != AtomType.Number ||
                UNumber.NumberType(arg0.value) > UNumber.SINT)
                throw new ArgumentException("First argument must be integer number!");

            if (arg1.type != AtomType.Pair)
                throw new ArgumentException("Second argument must be list!");

            Atom res = arg1.ListSkip((int)arg0.value);

            if (res == null)
                throw new ArgumentException(string.Format("List too short for argument {0}!", arg0.value));

            return res;
        }

        public static Atom List(Atom args, Context context)
        {
            if (args == null) return null;
            if (args.IsEmpty()) return new Atom();

            Atom head, tail;
            head = tail = new Atom();
            head.value = args.value;
            for (Atom iter = args.next; iter != null; iter = iter.next)
            {
                tail = tail.next = new Atom();
                tail.value = iter.value;
            }
            return head;
        }

        public static Atom ReverseAtom(Atom args, Context context)
        {
            Atom prev = null;
            Atom current = Atom.CloneList((Atom)args?.value);
            Atom next = null;
            while (current != null)
            {
                next = current.next;
                current.next = prev;
                prev = current;
                current = next;
            }
            return prev;
        }

        public static Atom SetCar(Atom args, Context context)
        {
            Atom list = (Atom)args?.value;

            if (list.type != AtomType.Pair)
                throw new ArgumentException("Argument must be List!");

            if (Object.ReferenceEquals(list, Atom.EMPTY))
                throw new ArgumentException("Can't modyficate base EMPTY list!");

            list.value = args.next.value;

            return (Atom)list.value;
        }

        public static Atom SetCdr(Atom args, Context context)
        {
            Atom list = (Atom)args?.value;

            if (list.type != AtomType.Pair)
                throw new ArgumentException("Argument must be List!");

            if (Object.ReferenceEquals(list, Atom.EMPTY))
                throw new ArgumentException("Can't modyficate base EMPTY list!");

            list.next = (Atom)args.next.value;

            return list.next;
        }

        private static void UnpackLoopArguments(Atom args, out Atom list, out Procedure proc, out bool skipNull)
        {
            skipNull = false;
            list = (Atom)args.value;
            Atom procedure = (Atom)args.next?.value;

            if (args.next?.next != null)
            {
                Atom skip = (Atom)args.next.next.value;
                if (!skip.IsBool())
                    throw new ArgumentException("third argument must be bool!");
                skipNull = (bool)skip.value;
            }

            if (list != null && !list.IsPair())
                throw new ArgumentException("first argument must be list!");

            if (!procedure.IsProcedure())
                throw new ArgumentException("second argument must be procedure!");

            proc = (Procedure)procedure.value;
            if (proc == null)
                throw new ArgumentException("procedure can't be null!");
        }

        public static Atom Each(Atom args, Context context)
        {
            Atom list;
            Procedure proc;
            bool skipNull;
            UnpackLoopArguments(args, out list, out proc, out skipNull);

            for (Atom iter = list; iter != null; iter = iter.next)
            {
                Atom arguments = Atom.List((Atom)iter.value);
                proc.Apply(arguments, context);
            }

            return null;
        }

        public static Atom Map(Atom args, Context context)
        {
            Atom list;
            Procedure proc;
            bool skipNull;
            UnpackLoopArguments(args, out list, out proc, out skipNull);

            Atom head = null;
            Atom tail = null;
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                if (skipNull && iter.atom == null) continue;
                Atom arguments = Atom.List(iter.atom);
                Atom result = proc.Apply(arguments, context);
                if (!skipNull || result != null)
                {
                    if (head == null) head = tail = new Atom();
                    else tail = tail.next = new Atom();
                    tail.value = result;
                }
            }

            return head;
        }

        public static Atom Filter(Atom args, Context context)
        {
            Atom list;
            Procedure proc;
            bool skipNull;
            UnpackLoopArguments(args, out list, out proc, out skipNull);

            Atom head = null;
            Atom tail = null;
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                if (skipNull && iter.atom == null) continue;
                Atom arguments = Atom.List(iter.atom);
                Atom predicate = proc.Apply(arguments, context);
                if (predicate.type != AtomType.Bool)
                    throw new ArgumentException("predicate must return bool!");
                if ((bool)predicate.value)
                {
                    if (head == null) head = tail = new Atom();
                    else tail = tail.next = new Atom();
                    tail.value = iter.atom;
                }
            }

            return head;
        }
        
        public static Atom Contains(Atom args, Context context)
        {
            Atom list = args?.atom;
            Atom value = args?.next?.atom;

            if (list == null || !list.IsPair())
                throw new ArgumentException("First argument must be list!");

            bool contains = false;
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                if (Atom.Compare(value, iter.atom))
                {
                    contains = true;
                    break;
                }
            }

            return contains ? Atom.TRUE : Atom.FALSE;
        }
    }
}
