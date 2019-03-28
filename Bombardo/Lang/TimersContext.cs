using System;
using System.Collections.Generic;
using System.Threading;

namespace Bombardo
{
    internal class Interval
    {
        public Atom tag;
        public Atom action;
        public Context context;
        public bool repeat;
        private Procedure proc;
        
        public Timer timer;

        public event Action elapsed;

        public Interval(Atom tag, Atom action, Context context, int delay, bool repeat)
        {
            this.tag = tag;
            this.action = action;
            this.context = context;
            this.repeat = repeat;

            timer = null;
            proc = action.value as Procedure;
            if (proc != null) timer = new Timer(Callback, null, delay, repeat ? delay : System.Threading.Timeout.Infinite);
            else throw new ArgumentException("callback must be procedure!");
        }

        private void Callback(object state)
        {
            proc.Apply(Atom.EMPTY, context);
            elapsed?.Invoke();
        }

        public void Stop()
        {
            timer.Dispose();
        }
    }

    internal class TimersContext
    {
        private static List<Interval> timeouts;
        private static List<Interval> intervals;
        
        public static void Setup(Context context)
        {
            timeouts = new List<Interval>();
            intervals = new List<Interval>();

            //  (interval-set 1500 (lambda () (print "yo!"))) -> interval
            //  (interval-set `(tag 1500) (lambda () (print "yo!"))) -> interval
            //  (interval-clear interval) -> stops interval
            //
            //  AllIntervals -> list of intervals
            //  (timeout-set 1500 (lambda () (print "yo!"))) -> timeout
            //  (timeout-set `(tag 1500) (lambda () (print "yo!"))) -> interval
            //  (timeout-clear timeout) -> stops timeout
            //
            //  AllTimeouts -> list of timeouts

            BombardoLangClass.SetProcedure(context, AllNames.LISP_GET_TIME, GetTime, 0);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_INTERVAL_PRED, IntervalPred, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TIMEOUT_PRED, TimeoutPred, 1);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_INTERVAL_SET, IntervalSet, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_INTERVAL_CLEAR, IntervalClear, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_INTERVAL_TAG, IntervalTag, 1);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_TIMEOUT_SET, TimeoutSet, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TIMEOUT_CLEAR, TimeoutClear, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TIMEOUT_TAG, IntervalTag, 1);
        }

        private static Atom GetTime(Atom args, Context context)
        {
            return new Atom(AtomType.Number, (double)DateTime.Now.Ticks / (double)TimeSpan.TicksPerSecond);
        }

        private static void UnpackArgs(Atom argument, out Atom tag, out int delay)
        {
            tag = null;
            delay = -1;
            if (argument.type==AtomType.Number)
            {
                tag = null;
                if (argument.type != AtomType.Number)
                    throw new ArgumentException("delay must be number!");
                delay = Convert.ToInt32(argument.value);
            }
            else if(argument.type == AtomType.Pair)
            {
                Atom t = (Atom)argument.value;
                Atom d = (Atom)argument.next.value;

                if(t.type != AtomType.Symbol && t.type != AtomType.String)
                    throw new ArgumentException("tag must be symbol or string!");

                if (d.type != AtomType.Number)
                    throw new ArgumentException("delay must be number!");

                tag = t;
                delay = Convert.ToInt32(d.value);
            }
            else throw new ArgumentException("Argument must be number or (symbol number) or ('string' number)!");
        }

        public static Atom IntervalTag(Atom args, Context context)
        {
            Atom argument = (Atom)args.value;
            Interval interval = argument.value as Interval;
            if (interval == null) throw new ArgumentException("Argument must be interval!");
            return interval.tag;
        }

        #region Predicates

        public static Atom IntervalPred(Atom args, Context context)
        {
            Atom argument = (Atom)args.value;
            Interval interval = argument.value as Interval;
            if (interval == null) return Atom.FALSE;
            return interval.repeat ? Atom.TRUE : Atom.FALSE;
        }

        public static Atom TimeoutPred(Atom args, Context context)
        {
            Atom argument = (Atom)args.value;
            Interval interval = argument.value as Interval;
            if (interval == null) return Atom.FALSE;
            return interval.repeat ? Atom.FALSE : Atom.TRUE;
        }

        #endregion

        #region Intervals

        public static Atom IntervalSet(Atom args, Context context)
        {
            Atom tag; int delay;
            UnpackArgs((Atom)args.value, out tag, out delay);
            Atom action = (Atom)args.next.value;

            Interval interval = new Interval(tag, action, context, delay, true);
            intervals.Add(interval);
            return new Atom(AtomType.Native, interval);
        }

        public static Atom IntervalClear(Atom args, Context context)
        {
            Atom argument = (Atom)args.value;
            if (argument.type != AtomType.Native)
                throw new ArgumentException("Argument must be interval!");
            Interval interval = argument.value as Interval;
            if (interval==null)
                throw new ArgumentException("Argument must be interval!");
            RemoveInterval(interval);
            return null;
        }

        private static void RemoveInterval(Interval interval)
        {
            interval.Stop();
            intervals.Remove(interval);
        }

        #endregion

        #region Timeouts

        public static Atom TimeoutSet(Atom args, Context context)
        {
            Atom tag; int delay;
            UnpackArgs((Atom)args.value, out tag, out delay);
            Atom action = (Atom)args.next.value;

            Interval timeout = new Interval(tag, action, context, delay, false);
            timeouts.Add(timeout);
            timeout.elapsed += () => RemoveTimeout(timeout);
            return new Atom(AtomType.Native, timeout);
        }

        public static Atom TimeoutClear(Atom args, Context context)
        {
            Atom argument = (Atom)args.value;
            if (argument.type != AtomType.Native)
                throw new ArgumentException("Argument must be interval!");
            Interval interval = argument.value as Interval;
            if (interval == null)
                throw new ArgumentException("Argument must be interval!");
            RemoveTimeout(interval);
            return null;
        }
        
        private static void RemoveTimeout(Interval timeout)
        {
            timeout.Stop();
            timeouts.Remove(timeout);
        }

        #endregion
    }
}
