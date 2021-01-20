using System;

namespace Bombardo.V2
{
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
			Atom pair = new Atom();
			pair.value = frame.args?.value;
			pair.next  = (Atom) frame.args?.next?.value;
			eval.SetReturn(pair);
		}

		private static void Car(Evaluator eval, StackFrame frame)
		{
			if (frame.args == null)
			{
				eval.SetReturn(null);
				return;
			}

			Atom list = (Atom) frame.args?.value;
			if (!list.IsPair) throw new ArgumentException("Argument must be Pair!");
			eval.SetReturn(list.atom);
		}

		private static void Cdr(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			if (args == null)
			{
				eval.SetReturn(null);
				return;
			}

			Atom list = (Atom) args?.value;
			if (!list.IsPair) throw new ArgumentException("Argument must be Pair!");
			eval.SetReturn(list.next.atom);
		}

		private static void GetElement(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom arg0 = (Atom) args?.value;
			Atom arg1 = (Atom) args?.next?.value;

			if (arg0.type != AtomType.Number ||
			    UNumber.NumberType(arg0.value) > UNumber.SINT)
				throw new ArgumentException("First argument must be integer number!");

			if (arg1.type != AtomType.Pair)
				throw new ArgumentException("Second argument must be list!");

			Atom res = arg1.ListSkip((int) arg0.value);

			if (res == null)
				throw new ArgumentException(string.Format("List too short for argument {0}!", arg0.value));

			eval.SetReturn(res);
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
				eval.SetReturn(null);
				return;
			}

			Atom list = (Atom) args?.value;
			if (!list.IsPair) throw new ArgumentException("Argument must be list!");
			while (
				list.IsPair &&
				list.next != null)
				list = list.next;
			eval.SetReturn(list.IsPair ? list.atom : list);
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
				eval.SetReturn(null);
				return;
			}

			Atom list = (Atom) args?.value;
			if (!list.IsPair) throw new ArgumentException("Argument must be list!");
			while (
				list.IsPair &&
				list.next != null)
				list = list.next;
			eval.SetReturn(list);
		}

		private static void Append(Evaluator eval, StackFrame frame)
		{
			//  (define append (lambda [list item]
			//      (set-cdr! (last list) item) list
			//  ))
			var args = frame.args;

			Atom list1 = StructureUtils.CloneList(args.atom);
			Atom list2 = StructureUtils.CloneList(args.next.atom);

			if (list2 == null || list2.IsEmpty)
			{
				eval.SetReturn(list1);
				return;
			}

			if (list1 == null || list1.IsEmpty)
			{
				eval.SetReturn(list2);
				return;
			}

			if (!list1.IsPair) throw new ArgumentException("Argument must be list!");

			Atom pair = list1;
			while (
				pair.IsPair &&
				pair.next != null)
				pair = pair.next;
			pair.next = list2;

			eval.SetReturn(list1);
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
			for (Atom iter = args.next; iter != null; iter = iter.next)
			{
				tail       = tail.next = new Atom();
				tail.value = iter.value;
			}

			eval.SetReturn(head);
		}

		private static void Reverse(Evaluator eval, StackFrame frame)
		{
			var args         = frame.args;
			var list         = StructureUtils.CloneList((Atom) args?.value);
			var reversedList = StructureUtils.Reverse(list);
			eval.SetReturn(reversedList);
		}


		private static void SetCar(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom list = (Atom) args?.value;

			if (list.type != AtomType.Pair)
				throw new ArgumentException("Argument must be List!");

			if (StructureUtils.Compare(list, Atoms.EMPTY))
				throw new ArgumentException("Can't modyficate base EMPTY list!");

			list.value = args.next.value;

			eval.SetReturn(list.atom);
		}

		private static void SetCdr(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom list = (Atom) args?.value;

			if (list.type != AtomType.Pair)
				throw new ArgumentException("Argument must be List!");

			if (StructureUtils.Compare(list, Atoms.EMPTY))
				throw new ArgumentException("Can't modyficate base EMPTY list!");

			list.next = (Atom) args.next.value;

			eval.SetReturn(list.next);
		}

		private static void Contains(Evaluator eval, StackFrame frame)
		{
			var  args  = frame.args;
			Atom list  = args?.atom;
			Atom value = args?.next?.atom;

			if (list == null || !list.IsPair)
				throw new ArgumentException("First argument must be list!");

			bool contains = false;
			for (Atom iter = list; iter != null; iter = iter.next)
			{
				if (StructureUtils.Compare(value, iter.atom))
				{
					contains = true;
					break;
				}
			}

			eval.SetReturn(contains ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void Each(Evaluator eval, StackFrame frame)
		{
			(var list, var proc, var skip) = StructureUtils.Split3(frame.args);
			
			bool skipNull = (bool)skip.value;
			
			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					frame.temp1 = list;
					frame.state = new Atom("-built-in-each-");
					break;
				case "-built-in-each-":
					if (frame.temp1 != null)
					{
						while (skipNull && frame.temp1.atom==null)
							frame.temp1 = frame.temp1.next;
						var subExpression = new Atom(frame.temp1.atom, null);
						frame.temp1 = frame.temp1.next;
						var newFrame = eval.Stack.CreateFrame(
							"-eval-sexp-args-",
							new Atom(proc, subExpression),
							frame.context);
						newFrame.function = proc;
						newFrame.args     = subExpression;
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
			(var list, var proc, var skip) = StructureUtils.Split3(frame.args);
			
			bool skipNull = (bool?) skip?.value ?? false;
			
			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					frame.temp1 = list;
					frame.state = new Atom("-built-in-map-");
					break;
				case "-built-in-map-":
					if (eval.HaveReturn())
					{
						frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, eval.TakeReturn());
					}
					if (frame.temp1 != null)
					{
						while (skipNull && frame.temp1.atom==null)
							frame.temp1 = frame.temp1.next;
						var subExpression = new Atom(frame.temp1.atom, null);
						frame.temp1 = frame.temp1.next;
						var newFrame = eval.Stack.CreateFrame(
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
			(var list, var proc, var skip) = StructureUtils.Split3(frame.args);
			
			bool skipNull = (bool?) skip?.value ?? false;
			
			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					frame.temp1 = list;
					frame.state = new Atom("-built-in-map-");
					break;
				case "-built-in-map-":
					if (eval.HaveReturn())
					{
						var  pred = eval.TakeReturn();
						bool add  = (bool?) pred?.value ?? false;
						if (add)
							frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, frame.temp3);
					}
					if (frame.temp1 != null)
					{
						while (skipNull && frame.temp1.atom==null)
							frame.temp1 = frame.temp1.next;
						var subExpression = frame.temp1.atom;
						frame.temp3 = subExpression;
						frame.temp1 = frame.temp1.next;
						subExpression = new Atom(subExpression, null);
						frame.temp1 = frame.temp1.next;
						var newFrame = eval.Stack.CreateFrame(
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