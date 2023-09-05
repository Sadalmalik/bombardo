namespace Bombardo.Core
{
	public class Closure : Function
	{
		private Context context_;
		private Atom args_;
		private Atom body_;

		public Closure(Context context, Atom args, Atom body, string closureTag) : base(null)
		{
			Name     = "λ";
			context_ = context;
			args_    = args;
			body_    = body;
			tag      = Atom.CreateSymbol(closureTag);
		}

		public override void Apply(Evaluator eval, StackFrame frame)
		{
			Context innerContext = new Context(context_);
			innerContext.SetArgs(args_, frame.args);
			eval.CreateFrame(Atoms.STATE_EVAL_BLOCK, body_, innerContext.self);
		}
	}
}