using System;

namespace Bombardo
{
    class ListContext
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, "cons", Cons, 0);
            BombardoLangClass.SetProcedure(context, "car", Car, 1);
            BombardoLangClass.SetProcedure(context, "cdr", Cdr, 1);
            BombardoLangClass.SetProcedure(context, "last", Last, 1);
            BombardoLangClass.SetProcedure(context, "append", Append, 1);

            BombardoLangClass.SetProcedure(context, "get", GetElement, 2);

            BombardoLangClass.SetProcedure(context, "first", Car, 1);
            BombardoLangClass.SetProcedure(context, "second", Cadr, 1);
            BombardoLangClass.SetProcedure(context, "third", Caddr, 1);

            BombardoLangClass.SetProcedure(context, "caar", Caar, 1);
            BombardoLangClass.SetProcedure(context, "cadr", Cadr, 1);
            BombardoLangClass.SetProcedure(context, "cdar", Cdar, 1);
            BombardoLangClass.SetProcedure(context, "cddr", Cddr, 1);

            BombardoLangClass.SetProcedure(context, "caaar", Caaar, 1);
            BombardoLangClass.SetProcedure(context, "caadr", Caadr, 1);
            BombardoLangClass.SetProcedure(context, "cadar", Cadar, 1);
            BombardoLangClass.SetProcedure(context, "caddr", Caddr, 1);
            BombardoLangClass.SetProcedure(context, "cdaar", Cdaar, 1);
            BombardoLangClass.SetProcedure(context, "cdadr", Cdadr, 1);
            BombardoLangClass.SetProcedure(context, "cddar", Cddar, 1);
            BombardoLangClass.SetProcedure(context, "cdddr", Cdddr, 1);

            BombardoLangClass.SetProcedure(context, "caaaar", Caaaar, 1);
            BombardoLangClass.SetProcedure(context, "caaadr", Caaadr, 1);
            BombardoLangClass.SetProcedure(context, "caadar", Caadar, 1);
            BombardoLangClass.SetProcedure(context, "caaddr", Caaddr, 1);
            BombardoLangClass.SetProcedure(context, "cadaar", Cadaar, 1);
            BombardoLangClass.SetProcedure(context, "cadadr", Cadadr, 1);
            BombardoLangClass.SetProcedure(context, "caddar", Caddar, 1);
            BombardoLangClass.SetProcedure(context, "cadddr", Cadddr, 1);
            BombardoLangClass.SetProcedure(context, "cdaaar", Cdaaar, 1);
            BombardoLangClass.SetProcedure(context, "cdaadr", Cdaadr, 1);
            BombardoLangClass.SetProcedure(context, "cdadar", Cdadar, 1);
            BombardoLangClass.SetProcedure(context, "cdaddr", Cdaddr, 1);
            BombardoLangClass.SetProcedure(context, "cddaar", Cddaar, 1);
            BombardoLangClass.SetProcedure(context, "cddadr", Cddadr, 1);
            BombardoLangClass.SetProcedure(context, "cdddar", Cdddar, 1);
            BombardoLangClass.SetProcedure(context, "cddddr", Cddddr, 1);

            BombardoLangClass.SetProcedure(context, "list", List, 1);
            BombardoLangClass.SetProcedure(context, "reverse", ReverseAtom, 1);

            BombardoLangClass.SetProcedure(context, "set-car!", SetCar, 2);
            BombardoLangClass.SetProcedure(context, "set-cdr!", SetCdr, 2);
            
            BombardoLangClass.SetProcedure(context, "each", Each, 2, true);
            BombardoLangClass.SetProcedure(context, "map", Map, 2, true);
            BombardoLangClass.SetProcedure(context, "filter", Filter, 2, true);
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
            if (!list.IsPair()) throw new BombardoException("<CAR> Argument must be Pair!");
            return (Atom)list.value;
        }

        public static Atom Cdr(Atom args, Context context)
        {
            if (args == null) return null;
            Atom list = (Atom)args?.value;
            if (!list.IsPair()) throw new BombardoException("<CDR> Argument must be Pair!");
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
            if (!list.IsPair()) throw new BombardoException("<LAST> Argument must be list!");
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

            if (!list1.IsPair()) throw new BombardoException("<APPEND> Argument must be list!");

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
                throw new BombardoException("<GET[]> First argument must be integer number!");

            if (arg1.type != AtomType.Pair)
                throw new BombardoException("<GET[]> Second argument must be list!");

            Atom res = arg1.ListSkip((int)arg0.value);

            if (res == null)
                throw new BombardoException(string.Format("<GET[]> List too short for argument {0}!", arg0.value));

            return res;
        }

        public static Atom Caar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom; }
        public static Atom Cadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom; }
        public static Atom Cdar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next; }
        public static Atom Cddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next; }

        public static Atom Caaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.atom; }
        public static Atom Caadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.atom; }
        public static Atom Cadar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.atom; }
        public static Atom Caddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.atom; }
        public static Atom Cdaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.next; }
        public static Atom Cdadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.next; }
        public static Atom Cddar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.next; }
        public static Atom Cdddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.next; }

        public static Atom Caaaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.atom?.atom; }
        public static Atom Caaadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.atom?.atom; }
        public static Atom Caadar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.atom?.atom; }
        public static Atom Caaddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.atom?.atom; }
        public static Atom Cadaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.next?.atom; }
        public static Atom Cadadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.next?.atom; }
        public static Atom Caddar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.next?.atom; }
        public static Atom Cadddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.next?.atom; }
        public static Atom Cdaaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.atom?.next; }
        public static Atom Cdaadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.atom?.next; }
        public static Atom Cdadar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.atom?.next; }
        public static Atom Cdaddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.atom?.next; }
        public static Atom Cddaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.next?.next; }
        public static Atom Cddadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.next?.next; }
        public static Atom Cdddar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.next?.next; }
        public static Atom Cddddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.next?.next; }


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
                throw new BombardoException("<SET-CAR!> Argument must be List!");

            if (Object.ReferenceEquals(list, Atom.EMPTY))
                throw new BombardoException("<SET-CAR!> Can't modyficate base EMPTY list!");

            list.value = args.next.value;

            return (Atom)list.value;
        }

        public static Atom SetCdr(Atom args, Context context)
        {
            Atom list = (Atom)args?.value;

            if (list.type != AtomType.Pair)
                throw new BombardoException("<SET-CDR!> Argument must be List!");

            if (Object.ReferenceEquals(list, Atom.EMPTY))
                throw new BombardoException("<SET-CAR!> Can't modyficate base EMPTY list!");

            list.next = (Atom)args.next.value;

            return list.next;
        }

        private static void UnpackLoopArguments(string tag, Atom args, out Atom list, out Procedure proc, out bool skipNull)
        {
            skipNull = false;
            list = (Atom)args.value;
            Atom procedure = (Atom)args.next?.value;

            if (args.next?.next != null)
            {
                Atom skip = (Atom)args.next.next.value;
                if (!skip.IsBool())
                    throw new BombardoException(string.Format("<{0}> third argument must be bool!", tag));
                skipNull = (bool)skip.value;
            }

            if (list != null && !list.IsPair())
                throw new BombardoException(string.Format("<{0}> first argument must be list!", tag));

            if (!procedure.IsProcedure())
                throw new BombardoException(string.Format("<{0}> second argument must be procedure!", tag));

            proc = (Procedure)procedure.value;
            if (proc == null)
                throw new BombardoException(string.Format("<{0}> procedure can't be null!", tag));
        }

        public static Atom Each(Atom args, Context context)
        {
            Atom list;
            Procedure proc;
            bool skipNull;
            UnpackLoopArguments("EACH", args, out list, out proc, out skipNull);

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
            UnpackLoopArguments("MAP", args, out list, out proc, out skipNull);

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
            UnpackLoopArguments("FILTER", args, out list, out proc, out skipNull);

            Atom head = null;
            Atom tail = null;
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                if (skipNull && iter.atom == null) continue;
                Atom arguments = Atom.List(iter.atom);
                Atom predicate = proc.Apply(arguments, context);
                if (predicate.type != AtomType.Bool)
                    throw new BombardoException("<FILTER> predicate must return bool!");
                if ((bool)predicate.value)
                {
                    if (head == null) head = tail = new Atom();
                    else tail = tail.next = new Atom();
                    tail.value = iter.atom;
                }
            }

            return head;
        }
    }
}
