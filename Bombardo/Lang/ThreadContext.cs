using System.Collections.Generic;

namespace Bombardo
{
    class ThreadSafeQueue
    {
        private Queue<Atom> queue_;

        public ThreadSafeQueue()
        {
            queue_ = new Queue<Atom>();
        }

        public int Count
        {
            get { return queue_.Count; }
        }

        public void Enqueue(Atom atom)
        {
            lock (queue_)
                queue_.Enqueue(atom);
        }

        public Atom Dequeue()
        {
            lock (queue_)
                return (queue_.Count > 0) ? queue_.Dequeue() : null;
        }
    }

    class ThreadContext
    {
        public static void Setup(Context context)
        {
            //  (CreateThreadSafeQueue) -> queue
            //  (ThreadSafeEnqueue queue atom)
            //  (ThreadSafeDequeue queue) -> atom
            //  (ThreadSafeCount queue) -> count

            BombardoLangClass.SetProcedure(context, "CreateThreadSafeQueue", CreateThreadSafeQueue, 0);
            BombardoLangClass.SetProcedure(context, "ThreadSafeEnqueue", ThreadSafeEnqueue, 1);
            BombardoLangClass.SetProcedure(context, "ThreadSafeDequeue", ThreadSafeDequeue, 1);
            BombardoLangClass.SetProcedure(context, "ThreadSafeCount", ThreadSafeCount, 1);
        }

        public static Atom CreateThreadSafeQueue(Atom args, Context context)
        {
            return new Atom(AtomType.Native, new ThreadSafeQueue());
        }

        public static Atom ThreadSafeEnqueue(Atom args, Context context)
        {
            Atom queue = args?.atom;

            if (queue == null || queue.type != AtomType.Native)
                throw new BombardoException("<ThreadSafeEnqueue> Argument must be queue");

            Atom atom = args?.next?.atom;

            ThreadSafeQueue que = queue.value as ThreadSafeQueue;

            que.Enqueue(atom);

            return null;
        }

        public static Atom ThreadSafeDequeue(Atom args, Context context)
        {
            Atom queue = args?.atom;

            if (queue == null || queue.type != AtomType.Native)
                throw new BombardoException("<ThreadSafeEnqueue> Argument must be queue");

            ThreadSafeQueue que = queue.value as ThreadSafeQueue;

            return que.Dequeue();
        }

        public static Atom ThreadSafeCount(Atom args, Context context)
        {
            Atom queue = args?.atom;

            if (queue == null || queue.type != AtomType.Native)
                throw new BombardoException("<ThreadSafeEnqueue> Argument must be queue");

            ThreadSafeQueue que = queue.value as ThreadSafeQueue;

            return new Atom(AtomType.Number, que.Count);
        }
    }
}
