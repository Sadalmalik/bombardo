using System;

namespace Bombardo.Core
{
    public static partial class Names
    {
        public static readonly string LISP_CAR      = "car";
        public static readonly string LISP_CDR      = "cdr";
        public static readonly string LISP_CONS     = "cons";
        public static readonly string LISP_GET      = "get";
        public static readonly string LISP_LAST     = "last";
        public static readonly string LISP_END      = "end";
        public static readonly string LISP_APPEND   = "append";
        public static readonly string LISP_LIST     = "list";
        public static readonly string LISP_REVERSE  = "reverse";
        public static readonly string LISP_SET_CAR  = "set-car!";
        public static readonly string LISP_SET_CDR  = "set-cdr!";
        public static readonly string LISP_EACH     = "each";
        public static readonly string LISP_MAP      = "map";
        public static readonly string LISP_FILTER   = "filter";
        public static readonly string LISP_CONTAINS = "contains?";
    }

    public static class ListFunctions
    {
        public static void Define(Context ctx)
        {
            ctx.DefineFunction(Names.LISP_CONS, Cons);
            ctx.DefineFunction(Names.LISP_CAR, Car);
            ctx.DefineFunction(Names.LISP_CDR, Cdr);
            ctx.DefineFunction(Names.LISP_GET, GetElement);
            ctx.DefineFunction(Names.LISP_LAST, Last);
            ctx.DefineFunction(Names.LISP_END, ListEnd);
            ctx.DefineFunction(Names.LISP_APPEND, Append);

            ctx.DefineFunction(Names.LISP_LIST, List);
            ctx.DefineFunction(Names.LISP_REVERSE, Reverse);

            ctx.DefineFunction(Names.LISP_SET_CAR, SetCar);
            ctx.DefineFunction(Names.LISP_SET_CDR, SetCdr);

            ctx.DefineFunction(Names.LISP_CONTAINS, Contains);

            ctx.DefineFunction(Names.LISP_EACH, Each);
            ctx.DefineFunction(Names.LISP_MAP, Map);
            ctx.DefineFunction(Names.LISP_FILTER, Filter);
        }


        private static void Cons(Evaluator eval, StackFrame frame)
        {
            eval.Return(Atom.CreatePair(frame.args?.Head, frame.args?.Next?.Head));
        }

        private static void Car(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            if (args == null)
            {
                eval.Return(null);
                return;
            }

            var list = args.Head;
            if (!list.IsPair) throw new ArgumentException("Argument must be Pair!");
            eval.Return(list.Head);
        }

        private static void Cdr(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            if (args == null)
            {
                eval.Return(null);
                return;
            }

            var list = args.Head;
            if (!list.IsPair) throw new ArgumentException("Argument must be Pair!");
            eval.Return(list.Next);
        }

        private static void GetElement(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            var arg0 = args?.Head;
            var arg1 = args?.Next?.Head;

            if (!arg0.IsNumber || arg0.number.type > AtomNumberType.SINT32)
                throw new ArgumentException("First argument must be integer number!");

            if (!arg1.IsPair)
                throw new ArgumentException("Second argument must be list!");

            var res = arg1.ListSkip(arg0.number.ToSInt());

            if (res == null)
                throw new ArgumentException($"List too short for argument {arg0.Head}!");

            eval.Return(res);
        }

        private static void Last(Evaluator eval, StackFrame frame)
        {
            //  (define last (lambda [list]
            //      (if [null? (cdr list)]
            //          list
            //          (list (cdr list)))
            //  ))
            var args = frame.args;

            if (args == null)
            {
                eval.Return(null);
                return;
            }

            var list = args?.Head;
            if (!list.IsPair) throw new ArgumentException("Argument must be list!");
            while (
                list.IsPair &&
                list.Next != null)
                list = list.Next;
            eval.Return(list.IsPair ? list.Head : list);
        }

        private static void ListEnd(Evaluator eval, StackFrame frame)
        {
            //  (define last (lambda [list]
            //      (if [null? (cdr list)]
            //          list
            //          (list (cdr list)))
            //  ))
            var args = frame.args;

            if (args == null)
            {
                eval.Return(null);
                return;
            }

            var list = args?.Head;
            if (!list.IsPair) throw new ArgumentException("Argument must be list!");
            while (
                list.IsPair &&
                list.Next != null)
                list = list.Next;
            eval.Return(list);
        }

        private static void Append(Evaluator eval, StackFrame frame)
        {
            //  (define append (lambda [list item]
            //      (set-cdr! (last list) item) list
            //  ))
            var args = frame.args;

            var list1 = StructureUtils.CloneList(args.Head);
            var list2 = StructureUtils.CloneList(args.Next.Head);

            if (list2 == null || list2.IsEmpty)
            {
                eval.Return(list1);
                return;
            }

            if (list1 == null || list1.IsEmpty)
            {
                eval.Return(list2);
                return;
            }

            if (!list1.IsPair) throw new ArgumentException("Argument must be list!");

            var pair = list1;
            while (
                pair.IsPair &&
                pair.Next != null)
                pair = pair.Next;
            pair.pair.next = list2;

            eval.Return(list1);
        }

        private static void List(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;

            if (args == null)
            {
                eval.SetReturn(null);
                return;
            }

            if (args.IsEmpty)
            {
                eval.SetReturn(Atoms.EMPTY);
                return;
            }

            Atom head, tail;
            head           = tail = Atom.CreatePair(null, null);
            head.pair.atom = args.Head;
            for (var iter = args.Next; iter != null; iter = iter.Next)
            {
                tail.pair.next = Atom.CreatePair(null, null);

                tail           = tail.Next;
                tail.pair.atom = iter.Head;
            }

            eval.Return(head);
        }

        private static void Reverse(Evaluator eval, StackFrame frame)
        {
            var args         = frame.args;
            var list         = StructureUtils.CloneList(args?.Head);
            var reversedList = StructureUtils.Reverse(list);
            eval.Return(reversedList);
        }


        private static void SetCar(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            var list = args?.Head;

            if (list.type != AtomType.Pair)
                throw new ArgumentException("Argument must be List!");

            if (StructureUtils.Compare(list, Atoms.EMPTY))
                throw new ArgumentException("Can't modyficate base EMPTY list!");

            list.pair.atom = args.Next.Head;

            eval.Return(list.Head);
        }

        private static void SetCdr(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            var list = args?.Head;

            if (list.type != AtomType.Pair)
                throw new ArgumentException("Argument must be List!");

            if (StructureUtils.Compare(list, Atoms.EMPTY))
                throw new ArgumentException("Can't modyficate base EMPTY list!");

            list.pair.next = args.Next.Head;

            eval.Return(list.Next);
        }

        private static void Contains(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;
            var list = args?.Head;
            var targ = args?.Next?.Head;

            if (list == null || !list.IsPair)
                throw new ArgumentException("First argument must be list!");

            var contains = false;
            for (var iter = list; iter != null; iter = iter.Next)
                if (StructureUtils.Compare(targ, iter.Head))
                {
                    contains = true;
                    break;
                }

            eval.Return(contains ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void Each(Evaluator eval, StackFrame frame)
        {
            var (list, proc, skip) = StructureUtils.Split3(frame.args);

            var skipNull = true;
            if (skip != null)
            {
                if (!skip.IsBool)
                    throw new ArgumentException("Argument 'skip' must be boolean!");
                skipNull = skip.@bool;
            }

            switch (frame.state.@string)
            {
                case "-eval-sexp-body-":
                    frame.temp1 = list;
                    frame.state = Atoms.STATE_ITERATE_EACH;
                    break;
                case "-built-in-each-":
                    if (eval.HaveReturn())
                        frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, eval.TakeReturn());
                    if (frame.temp1 != null)
                    {
                        while (skipNull && frame.temp1.Head == null)
                            frame.temp1 = frame.temp1.Next;

                        var subExpression = Atom.CreatePair(frame.temp1.Head, null);
                        
                        var newFrame = eval.CreateFrame(
                            Atoms.STATE_EVAL_SEXP_BODY,
                            StructureUtils.List(proc, frame.temp1.Head),
                            frame.context);
                        newFrame.function = proc;
                        newFrame.args     = subExpression;

                        frame.temp1 = frame.temp1.Next;
                    }
                    else
                    {
                        eval.SetReturn(null);
                        frame.state = Atoms.STATE_EVAL_SEXP_BODY;
                    }

                    break;
            }
        }

        private static void Map(Evaluator eval, StackFrame frame)
        {
            var (list, proc, skip) = StructureUtils.Split3(frame.args);

            var skipNull = true;
            if (skip != null)
            {
                if (!skip.IsBool)
                    throw new ArgumentException("Argument 'skip' must be boolean!");
                skipNull = skip.@bool;
            }

            switch (frame.state.@string)
            {
                case "-eval-sexp-body-":
                    frame.temp1 = list;
                    frame.state = Atoms.STATE_ITERATE_MAP;
                    break;
                case "-built-in-map-":
                    if (eval.HaveReturn())
                        frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, eval.TakeReturn());
                    if (frame.temp1 != null)
                    {
                        while (skipNull && frame.temp1.Head == null)
                            frame.temp1 = frame.temp1.Next;
                        var subExpression = Atom.CreatePair(frame.temp1.Head, null);
                        frame.temp1 = frame.temp1.Next;
                        var newFrame = eval.CreateFrame(
                            Atoms.STATE_EVAL_SEXP_ARGS,
                            Atom.CreatePair(proc, subExpression),
                            frame.context);
                        newFrame.function = proc;
                        newFrame.args     = subExpression;
                    }
                    else
                    {
                        eval.SetReturn(frame.temp2.Head);
                        frame.state = Atoms.STATE_EVAL_SEXP_BODY;
                    }

                    break;
            }
        }

        private static void Filter(Evaluator eval, StackFrame frame)
        {
            var (list, proc, skip) = StructureUtils.Split3(frame.args);

            var skipNull = true;
            if (skip != null)
            {
                if (!skip.IsBool)
                    throw new ArgumentException("Argument 'skip' must be boolean!");
                skipNull = skip.@bool;
            }

            switch (frame.state.@string)
            {
                case "-eval-sexp-body-":
                    frame.temp1 = list;
                    frame.state = Atoms.STATE_ITERATE_MAP;
                    break;
                case "-built-in-map-":
                    if (eval.HaveReturn())
                    {
                        var pred = eval.TakeReturn();
                        if (pred == null || !pred.IsBool)
                            throw new ArgumentException("Predicate must return boolean!");
                        var add = pred.@bool;
                        if (add)
                            frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, frame.temp3);
                    }

                    if (frame.temp1 != null)
                    {
                        while (skipNull && frame.temp1.Head == null)
                            frame.temp1 = frame.temp1.Next;
                        var subExpression = frame.temp1.Head;
                        frame.temp3   = subExpression;
                        frame.temp1   = frame.temp1.Next;
                        subExpression = Atom.CreatePair(subExpression, null);
                        frame.temp1   = frame.temp1.Next;
                        var newFrame = eval.CreateFrame(
                            Atoms.STATE_EVAL_SEXP_ARGS,
                            Atom.CreatePair(proc, subExpression),
                            frame.context);
                        newFrame.function = proc;
                        newFrame.args     = subExpression;
                    }
                    else
                    {
                        eval.SetReturn(frame.temp2.Head);
                        frame.state = Atoms.STATE_EVAL_SEXP_BODY;
                    }

                    break;
            }
        }
    }
}