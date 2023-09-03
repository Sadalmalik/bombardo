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
			var pair = new Atom();
			pair.value = frame.args?.atom;
			pair.next  = frame.args?.next?.atom;
			eval.Return(pair);
		}

		private static void Car(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			if (args == null)
			{
				eval.Return(null);
				return;
			}

			var list = args.atom;
			if (!list.IsPair) throw new ArgumentException("Argument must be Pair!");
			eval.Return(list.atom);
		}

		private static void Cdr(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			if (args == null)
			{
				eval.Return(null);
				return;
			}

			var list = args.atom;
			if (!list.IsPair) throw new ArgumentException("Argument must be Pair!");
			eval.Return(list.next);
		}

		private static void GetElement(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var arg0 = (Atom) args?.value;
			var arg1 = (Atom) args?.next?.value;

			if (arg0.type != AtomType.Number ||
			    AtomNumberOperations.NumberType(arg0.value) > AtomNumberOperations.SINT)
				throw new ArgumentException("First argument must be integer number!");

			if (arg1.type != AtomType.Pair)
				throw new ArgumentException("Second argument must be list!");

			var res = arg1.ListSkip((int) arg0.value);

			if (res == null)
				throw new ArgumentException(string.Format("List too short for argument {0}!", arg0.value));

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

			var list = (Atom) args?.value;
			if (!list.IsPair) throw new ArgumentException("Argument must be list!");
			while (
				list.IsPair &&
				list.next != null)
				list = list.next;
			eval.Return(list.IsPair ? list.atom : list);
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

			var list = (Atom) args?.value;
			if (!list.IsPair) throw new ArgumentException("Argument must be list!");
			while (
				list.IsPair &&
				list.next != null)
				list = list.next;
			eval.Return(list);
		}

		private static void Append(Evaluator eval, StackFrame frame)
		{
			//  (define append (lambda [list item]
			//      (set-cdr! (last list) item) list
			//  ))
			var args = frame.args;

			var list1 = StructureUtils.CloneList(args.atom);
			var list2 = StructureUtils.CloneList(args.next.atom);

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
				pair.next != null)
				pair = pair.next;
			pair.next = list2;

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
				eval.SetReturn(new Atom());
				return;
			}

			Atom head, tail;
			head       = tail = new Atom();
			head.value = args.value;
			for (var iter = args.next; iter != null; iter = iter.next)
			{
				tail       = tail.next = new Atom();
				tail.value = iter.value;
			}

			eval.Return(head);
		}

		private static void Reverse(Evaluator eval, StackFrame frame)
		{
			var args         = frame.args;
			var list         = StructureUtils.CloneList((Atom) args?.value);
			var reversedList = StructureUtils.Reverse(list);
			eval.Return(reversedList);
		}


		private static void SetCar(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var list = (Atom) args?.value;

			if (list.type != AtomType.Pair)
				throw new ArgumentException("Argument must be List!");

			if (StructureUtils.Compare(list, Atoms.EMPTY))
				throw new ArgumentException("Can't modyficate base EMPTY list!");

			list.value = args.next.value;

			eval.Return(list.atom);
		}

		private static void SetCdr(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var list = (Atom) args?.value;

			if (list.type != AtomType.Pair)
				throw new ArgumentException("Argument must be List!");

			if (StructureUtils.Compare(list, Atoms.EMPTY))
				throw new ArgumentException("Can't modyficate base EMPTY list!");

			list.next = (Atom) args.next.value;

			eval.Return(list.next);
		}

		private static void Contains(Evaluator eval, StackFrame frame)
		{
			var args  = frame.args;
			var list  = args?.atom;
			var value = args?.next?.atom;

			if (list == null || !list.IsPair)
				throw new ArgumentException("First argument must be list!");

			var contains = false;
			for (var iter = list; iter != null; iter = iter.next)
				if (StructureUtils.Compare(value, iter.atom))
				{
					contains = true;
					break;
				}

			eval.Return(contains ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void Each(Evaluator eval, StackFrame frame)
		{
			var (list, proc, skip) = StructureUtils.Split3(frame.args);

			var skipNull = (bool?) skip?.value ?? false;

			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					frame.temp1 = list;
					frame.state = new Atom("-built-in-each-");
					break;
				case "-built-in-each-":
					if (eval.HaveReturn())
						frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, eval.TakeReturn());
					if (frame.temp1 != null)
					{
						while (skipNull && frame.temp1.atom == null)
							frame.temp1 = frame.temp1.next;

						var subExpression = new Atom(frame.temp1.atom, null);

						var newFrame = eval.CreateFrame(
							"-eval-sexp-body-",
							StructureUtils.List(proc, frame.temp1.atom),
							frame.context);
						newFrame.function = proc;
						newFrame.args     = subExpression;

						frame.temp1 = frame.temp1.next;
					}
					else
					{
						eval.SetReturn(null);
						frame.state = new Atom("-eval-sexp-body-");
					}

					break;
			}
		}

		private static void Map(Evaluator eval, StackFrame frame)
		{
			var (list, proc, skip) = StructureUtils.Split3(frame.args);

			var skipNull = (bool?) skip?.value ?? false;

			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					frame.temp1 = list;
					frame.state = new Atom("-built-in-map-");
					break;
				case "-built-in-map-":
					if (eval.HaveReturn())
						frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, eval.TakeReturn());
					if (frame.temp1 != null)
					{
						while (skipNull && frame.temp1.atom == null)
							frame.temp1 = frame.temp1.next;
						var subExpression = new Atom(frame.temp1.atom, null);
						frame.temp1 = frame.temp1.next;
						var newFrame = eval.CreateFrame(
							"-eval-sexp-args-",
							new Atom(proc, subExpression),
							frame.context);
						newFrame.function = proc;
						newFrame.args     = subExpression;
					}
					else
					{
						eval.SetReturn(frame.temp2.atom);
						frame.state = new Atom("-eval-sexp-body-");
					}

					break;
			}
		}

		private static void Filter(Evaluator eval, StackFrame frame)
		{
			var (list, proc, skip) = StructureUtils.Split3(frame.args);

			var skipNull = (bool?) skip?.value ?? false;

			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					frame.temp1 = list;
					frame.state = new Atom("-built-in-map-");
					break;
				case "-built-in-map-":
					if (eval.HaveReturn())
					{
						var pred = eval.TakeReturn();
						var add  = (bool?) pred?.value ?? false;
						if (add)
							frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, frame.temp3);
					}

					if (frame.temp1 != null)
					{
						while (skipNull && frame.temp1.atom == null)
							frame.temp1 = frame.temp1.next;
						var subExpression = frame.temp1.atom;
						frame.temp3   = subExpression;
						frame.temp1   = frame.temp1.next;
						subExpression = new Atom(subExpression, null);
						frame.temp1   = frame.temp1.next;
						var newFrame = eval.CreateFrame(
							"-eval-sexp-args-",
							new Atom(proc, subExpression),
							frame.context);
						newFrame.function = proc;
						newFrame.args     = subExpression;
					}
					else
					{
						eval.SetReturn(frame.temp2.atom);
						frame.state = new Atom("-eval-sexp-body-");
					}

					break;
			}
		}
	}
}