using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombardo
{
    public class Closure : Procedure
    {
        private Context context_;
        private Atom args_;
        private Atom body_;
        
        public Closure(Context context, Atom args, Atom body) : base(null, 0)
        {
            Name = "λ";
            context_ = context;
            args_ = args;
            body_ = body;
        }

        public override Atom Apply(Atom args, Context context)
        {
            Context innerContext = new Context(context_);
            innerContext.SetArgs(args_, args);
            Atom result = Evaluator.EvaluateEach(body_, innerContext);
            return result;
        }
        
        public Atom GetOriginal()
        {
            Atom head = new Atom();
            head.value = new Atom(AtomType.Symbol, EvalArgs ? "lambda" : "macros");
            head.next = new Atom();
            head.next.value = args_;
            head.next.next = new Atom();
            head.next.next.value = body_;
            return head;
        }
    }
}