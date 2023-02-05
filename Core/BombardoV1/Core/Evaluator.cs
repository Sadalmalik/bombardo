using System;
using System.Collections.Generic;
using System.CodeDom;
using System.Linq;

namespace Bombardo.V1
{
	public class Evaluator
	{
		public static Atom EvaluateEach(Atom atom, Context context)
		{
			Atom result = null;

			for (Atom iter = atom; iter != null && iter.IsPair(); iter = iter.next)
			{
				if (iter.type != AtomType.Pair)
					throw new BombardoException($"S-Expression must be list: {iter} in {atom}");
				Atom sexp = (Atom) iter.value;
				if (sexp == null) continue;
				result = Evaluate(sexp, context);
			}

			return result;
		}

		public static Atom EvaluateList(Atom atom, Context context)
		{
			Atom head   = new Atom();
			Atom result = head;

			for (Atom iter = atom; iter != null && iter.IsPair(); iter = iter.next)
			{
				if (iter.type != AtomType.Pair)
					throw new BombardoException($"S-Expression must be list: {iter} in {atom}");
				Atom sexp = (Atom) iter.value;
				if (sexp != null)
					result.value = Evaluate(sexp, context);
				if (iter.next != null)
				{
					result.next = new Atom();
					result      = result.next;
				}
			}

			return head;
		}

		public static Atom Evaluate(Atom atom, Context context)
		{
			if (atom == null) return null;

			if (atom.IsSymbol())
			{
				string name = (string) atom.value;
				return ContextUtils.Get(context, name);
			}

			if (atom.IsPair())
			{
				Atom head = Evaluate((Atom) atom.value, context);
				Atom args = atom.next;

				if (head == null || !head.IsProcedure())
					throw new BombardoException(
						$"List head '{head?.ToString() ?? "null"}' is not a function or lambda!");

				var proc = (Procedure) head.value;

				if (proc.EvalArgs) args = EvaluateList(args, context);

				Atom result = null;
				try
				{
					result = proc.Apply(args, context);
				}
				catch (BombardoException sexc)
				{
					sexc.AddSection(atom, 0);
					throw;
				}
				catch (Exception exc)
				{
					throw new BombardoException(exc);
				}

				if (proc.EvalResult) result = Evaluate(result, context);
				return result;
			}

			return atom;
		}
	}
}