using System;
using System.Collections.Generic;
using System.Threading;

namespace Bombardo.Core
{
    public static partial class Names
    {
        public static readonly string LISP_SET_INTERVAL        = "setInterval";
        public static readonly string LISP_CLEAR_INTERVAL      = "clearInterval";
        public static readonly string LISP_CLEAR_ALL_INTERVALS = "clearAllIntervals";
    }

    public class Interval : IDisposable
    {
        public Atom Context;
        public Atom Callback;
        public Atom Delay;
        public Atom Limit;

        private int       _callLimit;
        private int       _exceptionsLimit;
        private Timer     _timer;
        private Evaluator _evaluator;

        public void Start()
        {
            _evaluator       = new Evaluator();
            _callLimit       = Limit != null ? Limit.number.ToSInt() : -1;
            _exceptionsLimit = 100;
            
            Console.WriteLine($"[TEST] Start interval: {Delay.number.ToSInt()} ms");
            
            _timer           = new Timer(HandleTimerElapsed, null, 0, Delay.number.ToSInt());
        }

        private void HandleTimerElapsed(object state)
        {
            if (_callLimit > 0)
            {
                _callLimit--;
            }
            
            try
            {
                var sexp = Atom.CreatePair(Callback, null);
                _evaluator.Evaluate(sexp, Context.context);
            }
            catch (Exception exc)
            {
                _exceptionsLimit--;
                Console.WriteLine("Exception in timer!");
                Console.WriteLine(exc);
                Console.WriteLine();
                _evaluator.Stack.Dump();
            }

            if (_exceptionsLimit <= 0)
            {
                IntervalsFunctions.Intervals.Remove(this);
                _timer.Dispose();
            }
            
            if (_callLimit == 0)
            {
                IntervalsFunctions.Intervals.Remove(this);
                _timer.Dispose();
                return;
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

    public static class IntervalsFunctions
    {
        public static DateTime mark;

        public static readonly HashSet<Interval> Intervals = new HashSet<Interval>();

        public static void Define(Context ctx)
        {
            // Created for debugging in visual studio
            //  (marker anything) -> null

            ctx.DefineFunction(Names.LISP_SET_INTERVAL, SetInterval);
            ctx.DefineFunction(Names.LISP_CLEAR_INTERVAL, ClearInterval);
            ctx.DefineFunction(Names.LISP_CLEAR_ALL_INTERVALS, ClearAllIntervals);
        }

        public static bool IsTicking()
        {
            return Intervals.Count > 0;
        }

        private static void SetInterval(Evaluator eval, StackFrame frame)
        {
            var (callback, delay, limit) = StructureUtils.Split3(frame.args);

            if (delay == null)
                throw new ArgumentException("First argument must be Interval delay!");
            
            if (callback == null)
                throw new ArgumentException("Second argument must be Interval callback!");
            
            var interval = new Interval
            {
                Context  = frame.context,
                Callback = callback,
                Delay    = delay,
                Limit    = limit
            };
            Intervals.Add(interval);
            interval.Start();

            eval.Return(Atom.CreateNative(interval));
        }

        private static void ClearInterval(Evaluator eval, StackFrame frame)
        {
            var atom = frame.args.Head;

            if (!atom.IsNative)
                throw new ArgumentException("Argument must be Interval!");

            var interval = atom.@object as Interval;

            if (interval == null)
                throw new ArgumentException("Argument must be Interval!");

            Intervals.Remove(interval);
            interval.Dispose();

            eval.Return(null);
        }

        private static void ClearAllIntervals(Evaluator eval, StackFrame frame)
        {
            foreach (var interval in Intervals)
            {
                interval.Dispose();
            }

            Intervals.Clear();

            eval.Return(null);
        }
    }
}