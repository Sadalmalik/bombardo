using System;

namespace Bombardo.Core
{
    public class Evaluator
    {
        private Atom _retValue;
        private bool _haveReturn;

        public StackHandler Stack        { get; }
        public string       ErrorMessage { get; private set; } = null;

        public Evaluator()
        {
            Stack = new StackHandler(null);
        }

        public void SetError(string error)
        {
            ErrorMessage = error;
        }

        public void SetReturn(Atom value)
        {
            _retValue   = value;
            _haveReturn = true;
        }

        public bool HaveReturn()
        {
            return _haveReturn;
        }

        public Atom TakeReturn()
        {
            var temp = _retValue;
            _retValue   = null;
            _haveReturn = false;
            return temp;
        }

        public StackFrame CreateFrame(Atom state, Atom expression, Atom context)
        {
            return Stack.CreateFrame(state, expression, context);
        }

        public void CloseFrame()
        {
            Stack.RemoveFrame();
        }

        public void Call(Atom state, Atom expression, Atom context)
        {
            Stack.CreateFrame(state, expression, context);
        }

        public void Return(Atom value)
        {
            _retValue   = value;
            _haveReturn = true;
            Stack.RemoveFrame();
        }

        private static string GetType(Atom atom)
        {
            if (atom == null) return "<-:->";
            var strType = AtomType.ToString(atom.type);
            return $"<{atom.type}:{strType}>";
        }

        public Atom Evaluate(Atom atom, Context current_context, string startState = null)
        {
            _retValue   = null;
            _haveReturn = false;
            Atom initialState = string.IsNullOrEmpty(startState) ? Atoms.STATE_EVAL : Atom.CreateString(startState);
            Stack.CreateFrame(initialState, atom, current_context.self);

            while (Stack.stack.Count > 0)
            {
                if (ErrorMessage != null)
                {
                    Console.WriteLine($"[ERROR] {ErrorMessage}");
                    Stack.Dump();
                    ErrorMessage = null;
                    return null;
                }

                StackFrame frame = Stack.TopFrame;
                //Console.WriteLine($"Frame expression: {frame}");
                //Stack.Dump(); Console.WriteLine("\n\n");
                switch (frame.state.@string)
                {
                    case "-eval-":
                        State_Eval(frame);
                        continue;
                    case "-eval-each-":
                        State_EvalEach(frame);
                        continue;
                    case "-eval-block-":
                        State_EvalBlock(frame);
                        continue;

                    case "-eval-sexp-head-":
                        State_EvalSExpHead(frame);
                        continue;
                    case "-eval-sexp-args-":
                        State_EvalSExpArgs(frame);
                        continue;
                    case "-eval-sexp-body-":
                        State_EvalSExpBody(frame);
                        continue;
                    case "-eval-sexp-result-":
                        State_EvalSExpResult(frame);
                        continue;
                }

                //	  Ни один из указанных стейтов, значит:
                //  Либо это стейты функции и тогда её надо вызывать
                if (frame.function != null)
                {
                    Function func = frame.function.function;
                    func.Apply(this, frame);
                    continue;
                }

                //	Либо если нет функции - то это ошибка интерпретации
                SetError($"Wrong evaluation state: {frame.state.@string}");
            }

            if (HaveReturn())
                return TakeReturn();

            return null;
        }

        private void State_Eval(StackFrame frame)
        {
            if (frame.expression == null)
            {
                Return(frame.expression);
                return;
            }

            if (frame.expression.IsPair)
            {
                frame.SetState(Atoms.STATE_EVAL_SEXP_HEAD);
                return;
            }

            if (frame.expression.IsSymbol)
            {
                var  name = frame.expression.@string;
                Atom value;
                try
                {
                    value = ContextUtils.Get(frame.context.context, name);
                }
                catch (BombardoException bex)
                {
                    ErrorMessage = bex.Message;
                    return;
                }

                Return(value);
                return;
            }

            Return(frame.expression);
        }

        private void State_EvalEach(StackFrame frame)
        {
            if (HaveReturn())
            {
                frame.temp1 = StructureUtils.BuildListContainer(frame.temp1, TakeReturn());
            }

            if (frame.expression != null)
            {
                var subExpression = frame.expression.Head;
                frame.expression = frame.expression.Next;
                Call(Atoms.STATE_EVAL, subExpression, frame.context);
                return;
            }

            Return(frame.temp1.Head);
        }

        private void State_EvalBlock(StackFrame frame)
        {
            if (HaveReturn())
            {
                frame.temp1 = TakeReturn();
            }

            if (frame.expression != null)
            {
                var subExpression = frame.expression.Head;
                frame.expression = frame.expression.Next;
                Call(Atoms.STATE_EVAL, subExpression, frame.context);
                return;
            }

            Return(frame.temp1);
        }

        private void State_EvalSExpHead(StackFrame frame)
        {
            if (!HaveReturn())
            {
                Call(Atoms.STATE_EVAL, frame.expression.Head, frame.context);
                return;
            }

            frame.function = TakeReturn();
            if (frame.function == null ||
                !frame.function.IsFunction)
            {
                ErrorMessage = $"Head is not function: {frame.function} in {frame.expression.Stringify()}";
                return;
            }

            frame.SetState(Atoms.STATE_EVAL_SEXP_ARGS);
        }

        private void State_EvalSExpArgs(StackFrame frame)
        {
            if (!HaveReturn())
            {
                var func = frame.function.function;
                if (func.EvalArgs && frame.expression.Next != null)
                {
                    Call(Atoms.STATE_EVAL_EACH, frame.expression.Next, frame.context);
                    return;
                }

                frame.args = frame.expression.Next;
                frame.SetState(Atoms.STATE_EVAL_SEXP_BODY);
                return;
            }

            frame.args = TakeReturn();
            frame.SetState(Atoms.STATE_EVAL_SEXP_BODY);
        }

        private void State_EvalSExpBody(StackFrame frame)
        {
            var func = frame.function.function;

            if (!HaveReturn())
            {
                func.Apply(this, frame);
                return;
            }

            if (func.EvalResult)
            {
                frame.SetState(Atoms.STATE_EVAL_SEXP_RESULT);
                frame.temp1 = TakeReturn();
                return;
            }

            CloseFrame();
        }

        private void State_EvalSExpResult(StackFrame frame)
        {
            if (!HaveReturn())
            {
                Call(Atoms.STATE_EVAL, frame.temp1, frame.context);
                return;
            }

            Return(TakeReturn());
        }
    }
}