using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bombardo.Core
{
	public class StackFrame
	{
		public Atom content;

		private Atom _state;
		private Atom _expression;
		private Atom _context;
		private Atom _function;
		private Atom _args;
		private Atom _temp1;
		private Atom _temp2;
		private Atom _temp3;

		public Atom state      { get => _state.pair.atom;      set => _state.pair.atom = value; }
		public Atom expression { get => _expression.pair.atom; set => _expression.pair.atom = value; }
		public Atom context    { get => _context.pair.atom;    set => _context.pair.atom = value; }
		public Atom function   { get => _function.pair.atom;   set => _function.pair.atom = value; }
		public Atom args       { get => _args.pair.atom;       set => _args.pair.atom = value; }
		public Atom temp1      { get => _temp1.pair.atom;      set => _temp1.pair.atom = value; }
		public Atom temp2      { get => _temp2.pair.atom;      set => _temp2.pair.atom = value; }
		public Atom temp3      { get => _temp3.pair.atom;      set => _temp3.pair.atom = value; }

		public StackFrame(Atom newContent)
		{
			content = newContent ?? Atoms.EMPTY;
			LinkElements();
		}

		public override string ToString()
		{
			return $"Stack frame: {content.Stringify()}";
		}
		
		public void SetState(Atom state)
		{
			_state.pair.atom = state;
		}

		private void EnsureLength(int len)
		{
			var iter = content;
			while (len-- > 0)
				iter = iter.pair.next ?? (iter.pair.next = new Atom(AtomType.Pair));
		}

		private void LinkElements()
		{
			EnsureLength(8);

			var iter = content;
			(_state, iter)      = (iter, iter.Next);
			(_expression, iter) = (iter, iter.Next);
			(_context, iter)    = (iter, iter.Next);
			(_function, iter)   = (iter, iter.Next);
			(_args, iter)       = (iter, iter.Next);
			(_temp1, iter)      = (iter, iter.Next);
			(_temp2, iter)      = (iter, iter.Next);
			(_temp3, iter)      = (iter, iter.Next);
		}
	}

	public class StackHandler
	{
		public Atom content { get; private set; }
		public Stack<StackFrame> stack;

		public bool IsEmpty => content == null;
		public StackFrame TopFrame => stack.Peek();

		private StringBuilder _dump;

		public StackHandler(Atom newContent)
		{
			stack = new Stack<StackFrame>();
			SetStackContent(newContent);
		}

		public void SetStackContent(Atom newContent)
		{
			stack.Clear();
			if (newContent != null)
			{
				var subStack = new Stack<StackFrame>();
				var iter     = newContent;
				while (iter.Next != null)
					subStack.Push(new StackFrame(iter.Head));
				// Reverse stack
				while (subStack.Count > 0)
					stack.Push(subStack.Pop());
			}
		}

		public void RemoveFrame()
		{
			stack.Pop();
			content = content.Next;
		}

		public StackFrame CreateFrame()
		{
			var frame = new StackFrame(null);
			stack.Push(frame);
			content = Atom.CreatePair(frame.content, content);
			return frame;
		}

		public StackFrame CreateFrame(Atom state, Atom expression, Atom context)
		{
			var frame = new StackFrame(null);
			frame.state      = state;
			frame.expression = expression;
			frame.context    = context;
			stack.Push(frame);
			content = Atom.CreatePair(frame.content, content);
			return frame;
		}

		public void Dump(int limit = Int32.MaxValue)
		{
			if (_dump == null)
				_dump = new StringBuilder();
			_dump.AppendLine("Stack:");
			
			var iter = content;
			while (iter != null && limit-->0)
			{
				_dump.AppendFormat("  {0}\n", iter.pair.atom.Stringify());
				iter = iter.pair.next;
			}
			Console.WriteLine(_dump.ToString());
			_dump.Clear();
		}
	}
}