using System;

namespace Bombardo.Core
{
    public static partial class Names
    {
        public static readonly string LISP_MARKER      = "marker";
        public static readonly string LISP_TIMER_START = "timerStart";
        public static readonly string LISP_TIMER_END   = "timerEnd";
    }

    public static class DebugFunctions
    {
        public static DateTime mark;

        public static void Define(Context ctx)
        {
            // Created for debugging in visual studio
            //  (marker anything) -> null

            ctx.DefineFunction(Names.LISP_MARKER, Marker, false);
            ctx.DefineFunction(Names.LISP_TIMER_START, TimerStart, false);
            ctx.DefineFunction(Names.LISP_TIMER_END, TimerEnd, false);
        }

        private static void Marker(Evaluator eval, StackFrame frame)
        {
            var tag = frame.args?.Head;
            Console.WriteLine(tag == null ? "<Marker reached>" : $"<Marker reached: {tag.Stringify()}>");
            eval.Return(null);
        }

        private static void TimerStart(Evaluator eval, StackFrame frame)
        {
            mark = DateTime.Now;
            eval.Return(null);
        }

        private static void TimerEnd(Evaluator eval, StackFrame frame)
        {
            var delta = DateTime.Now - mark;
            Console.WriteLine($"<Debut time: {delta}>");
            eval.Return(null);
        }
    }
}