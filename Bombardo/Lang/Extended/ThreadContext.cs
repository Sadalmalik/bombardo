using System;
using System.Collections.Generic;

namespace Bombardo
{
    class ThreadSafeQueue
    {
        private Queue<Atom> queue_;
        public ThreadSafeQueue() { queue_ = new Queue<Atom>(); }
        public int Count { get { return queue_.Count; } }
        public void Enqueue(Atom atom) { lock (queue_) queue_.Enqueue(atom); }
        public Atom Dequeue() { lock (queue_) return (queue_.Count > 0) ? queue_.Dequeue() : null; }
    }

    class ThreadContext
    {
        public static void Setup(Context context)
        {
            //  (concurentQueueCreate) -> queue
            //  (concurentQueueEnqueue queue atom)
            //  (concurentQueueDequeue queue) -> atom
            //  (concurentQueueCount queue) -> count

            //  (concurentQueue? queue) -> true
            //  (concurentQueue? anything-else) -> false

            BombardoLangClass.SetProcedure(context, AllNames.CONCURENCY_QUEUE_CREATE, CreateThreadSafeQueue, 0);
            BombardoLangClass.SetProcedure(context, AllNames.CONCURENCY_QUEUE_ENQUEUE, ThreadSafeEnqueue, 1);
            BombardoLangClass.SetProcedure(context, AllNames.CONCURENCY_QUEUE_DEQUEUE, ThreadSafeDequeue, 1);
            BombardoLangClass.SetProcedure(context, AllNames.CONCURENCY_QUEUE_COUNT, ThreadSafeCount, 1);

            BombardoLangClass.SetProcedure(context, AllNames.CONCURENCY_QUEUE_PRED, ThreadSafeCount, 1);
        }

        public static Atom CreateThreadSafeQueue(Atom args, Context context)
        {
            return new Atom(AtomType.Native, new ThreadSafeQueue());
        }

        public static Atom ThreadSafeEnqueue(Atom args, Context context)
        {
            Atom queue = args?.atom;

            if (queue == null || queue.type != AtomType.Native)
                throw new ArgumentException("Argument must be queue");

            Atom atom = args?.next?.atom;

            ThreadSafeQueue que = queue.value as ThreadSafeQueue;

            que.Enqueue(atom);

            return null;
        }

        public static Atom ThreadSafeDequeue(Atom args, Context context)
        {
            Atom queue = args?.atom;

            if (queue == null || queue.type != AtomType.Native)
                throw new ArgumentException("Argument must be queue");

            ThreadSafeQueue que = queue.value as ThreadSafeQueue;

            return que.Dequeue();
        }

        public static Atom ThreadSafeCount(Atom args, Context context)
        {
            Atom queue = args?.atom;

            if (queue == null || queue.type != AtomType.Native)
                throw new ArgumentException("Argument must be queue");

            ThreadSafeQueue que = queue.value as ThreadSafeQueue;

            return new Atom(AtomType.Number, que.Count);
        }

        public static Atom ThreadSafePredicate(Atom args, Context context)
        {
            Atom queue = args?.atom;

            if (queue == null || queue.type != AtomType.Native)
                return Atom.FALSE;

            ThreadSafeQueue que = queue.value as ThreadSafeQueue;

            return que==null ? Atom.FALSE : Atom.TRUE;
        }
    }
}
