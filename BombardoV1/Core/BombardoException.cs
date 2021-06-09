using System;
using System.Collections.Generic;
using System.Text;

namespace Bombardo.V1
{
    internal class BombardoStack
    {
        public Atom sentence;
        public int index;

        public BombardoStack(Atom s, int i)
        {
            sentence = s;
            index = i;
        }
    }

    public class BombardoException : Exception
    {
        private string capturedStack_;
        private List<BombardoStack> stack_;

        public BombardoException(string message) : base(message)
        {
            stack_ = new List<BombardoStack>();
            capturedStack_ = base.StackTrace;
        }

        public BombardoException(Exception intern) : base(intern.Message, intern)
        {
            stack_ = new List<BombardoStack>();
            capturedStack_ = intern.StackTrace;
        }

        public BombardoException(string methodName, Exception intern) : base(string.Format("<{0}> {1}", methodName, intern.Message), intern)
        {
            stack_ = new List<BombardoStack>();
            capturedStack_ = intern.StackTrace;
        }

        public void AddSection(Atom sentence, int index)
        {
            stack_.Add(new BombardoStack(sentence, index));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (InnerException != null)
                sb.AppendFormat("InternalException: {0}\n", Message);
            else
                sb.AppendFormat("BombardoException: {0}\n", Message);
            foreach (var frame in stack_)
                sb.AppendFormat("  at atom {0} in sentence {1}\n",
                    frame.index, frame.sentence.Stringify(true));
            if(!string.IsNullOrEmpty(capturedStack_))
            {
                sb.AppendLine();
                sb.Append(capturedStack_);
            }
            return sb.ToString();
        }
    }
}
