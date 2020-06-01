using System;
using System.Collections.Generic;
using System.Text;

namespace Bombardo.V2
{
	public class StackFrame
	{
		public Atom content;

		private Atom _state;
		private Atom _expression;
		private Atom _context;
		private Atom _function;
		private Atom _args;
		private Atom _meta;

		public Atom state
		{
			get => _state.atom;
			set => _state.value = value;
		}

		public Atom expression
		{
			get => _expression.atom;
			set => _expression.value = value;
		}

		public Atom context
		{
			get => _context.atom;
			set => _context.value = value;
		}

		public Atom function
		{
			get => _function.atom;
			set => _function.value = value;
		}

		public Atom args
		{
			get => _args.atom;
			set => _args.value = value;
		}

		public Atom meta
		{
			get => _meta.atom;
			set => _meta.value = value;
		}

		public StackFrame(Atom newContent)
		{
			content = newContent ?? new Atom();
			LinkElements();
		}

		private void EnsureLength(int len)
		{
			var iter = content;
			while (len-- > 0)
				iter = iter.next ?? (iter.next = new Atom());
		}

		private void LinkElements()
		{
			EnsureLength(6);

			var iter = content;
			(_state, iter)      = (iter, iter.next);
			(_expression, iter) = (iter, iter.next);
			(_context, iter)    = (iter, iter.next);
			(_function, iter)   = (iter, iter.next);
			(_args, iter)       = (iter, iter.next);
			(_meta, iter)       = (iter, iter.next);
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
				while (iter.next != null)
					subStack.Push(new StackFrame(iter.atom));
				// Reverse stack
				while (subStack.Count > 0)
					stack.Push(subStack.Pop());
			}
		}

		public void RemoveFrame()
		{
			stack.Pop();
			content = content.next;
		}

		public StackFrame CreateFrame()
		{
			var frame = new StackFrame(null);
			stack.Push(frame);
			content = new Atom(frame.content, content);
			return frame;
		}

		public StackFrame CreateFrame(string state, Atom expression, Context context)
		{
			var frame = new StackFrame(null);
			frame.state      = new Atom(state);
			frame.expression = expression;
			frame.context    = context.self;
			stack.Push(frame);
			content = new Atom(frame.content, content);
			return frame;
		}

		public StackFrame CreateFrame(string state, Atom expression, Atom context)
		{
			var frame = new StackFrame(null);
			frame.state      = new Atom(state);
			frame.expression = expression;
			frame.context    = context;
			stack.Push(frame);
			content = new Atom(frame.content, content);
			return frame;
		}

		public void Dump()
		{
			if (_dump == null)
				_dump = new StringBuilder();
			_dump.AppendLine("Stack:");
			var iter = content;
			while (iter != null)
			{
				_dump.AppendFormat("  {0}\n", iter.atom);
				iter = iter.next;
			}

			Console.WriteLine(_dump.ToString());
			_dump.Clear();
		}
	}
}