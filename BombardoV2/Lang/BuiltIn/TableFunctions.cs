using System;
using System.Diagnostics;
using System.Linq;

namespace Bombardo.V2
{
	public static partial class Names
	{
		public static readonly string LISP_TABLE_CREATE = "create";        // "table";
		public static readonly string LISP_TABLE_GET = "get";              // "tableGet";
		public static readonly string LISP_TABLE_SET = "set";              // "tableSet";
		public static readonly string LISP_TABLE_REMOVE = "rem";           // "tableRemove";
		public static readonly string LISP_TABLE_CLEAR = "clear";          // "tableClear";
		public static readonly string LISP_TABLE_IMPORT = "import";        // "tableImport";
		public static readonly string LISP_TABLE_IMPORT_ALL = "importAll"; // "tableImportAll";
		public static readonly string LISP_TABLE_EACH = "each";            // "tableEach";

		public static readonly string LISP_TABLE_PRED = "table?";
	}

	public class TableFunctions
	{
		public static void Define(Context ctx)
		{
			// 
			ctx.DefineFunction(Names.LISP_TABLE_CREATE, TableCreate);
			ctx.DefineFunction(Names.LISP_TABLE_GET, TableGet);
			ctx.DefineFunction(Names.LISP_TABLE_SET, TableSet);
			ctx.DefineFunction(Names.LISP_TABLE_REMOVE, TableRemove);
			ctx.DefineFunction(Names.LISP_TABLE_CLEAR, TableClear);
			ctx.DefineFunction(Names.LISP_TABLE_IMPORT, TableImport);
			ctx.DefineFunction(Names.LISP_TABLE_IMPORT_ALL, TableImportAll);
			ctx.DefineFunction(Names.LISP_TABLE_EACH, TableEach);
			ctx.DefineFunction(Names.LISP_TABLE_PRED, TablePred);
		}

#region Methods

		private static void TableCreate(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var dict = new Context();
			FillDictionary(dict, args);
			eval.Return(new Atom(AtomType.Native, dict));
		}

		private static void TableGet(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom dic  = (Atom) args?.value;
			Atom key  = (Atom) args?.next?.value;

			Context dictionary = GetDictionary(dic);

			if (key == null || (key.type != AtomType.String && key.type != AtomType.Symbol))
				throw new BombardoException("Table key must be string or symbol!!!");

			dictionary.TryGetValue((string) key?.value, out var value);

			eval.Return(value);
		}

		private static void TableSet(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom dic  = (Atom) args?.value;

			Context dictionary = GetDictionary(dic);

			FillDictionary(dictionary, args.next);

			eval.Return(null);
		}

		private static void TableRemove(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom dic  = (Atom) args?.value;
			Atom key  = (Atom) args?.next?.value;

			Context dictionary = GetDictionary(dic);

			if (key.type != AtomType.String && key.type != AtomType.Symbol)
				throw new BombardoException("Table key must be string or symbol!!!");

			dictionary.Remove((string) key.value);

			eval.Return(null);
		}

		private static void TableClear(Evaluator eval, StackFrame frame)
		{
			var  args = frame.args;
			Atom dic  = (Atom) args?.value;

			Context dictionary = GetDictionary(dic);

			dictionary.Clear();

			eval.Return(null);
		}

		private static void TableImport(Evaluator eval, StackFrame frame)
		{
			var (src, dst, names) = StructureUtils.Split3(frame.args);
			
			if (names==null && dst.IsPair)
			{
				names = dst;
				dst = frame.context.atom;
			}
			
			var srcCtx = GetDictionary(src);
			var dstCtx = GetDictionary(dst);

			string[] nameList = StructureUtils.ListToStringArray(names, "TABLE");
			
			ContextUtils.ImportSymbols(srcCtx, dstCtx, nameList);

			eval.Return(null);
		}

		private static void TableImportAll(Evaluator eval, StackFrame frame)
		{
			var (src, dst) = StructureUtils.Split2(frame.args);

			if (dst==null || dst.IsPair)
				dst = frame.context.atom;
			
			var srcCtx = GetDictionary(src);
			var dstCtx = GetDictionary(dst);

			ContextUtils.ImportAllSymbols(srcCtx, dstCtx);

			eval.Return(null);
		}

		// TODO: Осознать, что тут происходит
		private static void TableEach(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;
			var (dict, func) = StructureUtils.Split2(args);
			Context  dictionary = GetDictionary(dict);
			Function proc       = func?.value as Function;

			if (dictionary == null)
				throw new ArgumentException("First argument must be table!");
			if (proc == null)
				throw new ArgumentException("Second argument must be procedure!");

			switch (frame.state.value)
			{
				case "-eval-sexp-body-":
					var list = dictionary
					          .Select(pair => StructureUtils.List(new Atom(AtomType.String, pair.Key), pair.Value))
					          .ToArray();
					frame.temp1 = StructureUtils.List(list);
					frame.state = new Atom("-built-in-table-each-");
					break;
				case "-built-in-table-each-":
					if (eval.HaveReturn())
					{
						frame.temp2 = StructureUtils.BuildListContainer(frame.temp2, eval.TakeReturn());
					}

					if (frame.temp1 != null)
					{
						var pair = frame.temp1.atom;
						frame.temp1 = frame.temp1.next;
						var newFrame = eval.CreateFrame(
							"-eval-sexp-args-",
							new Atom(func, pair),
							frame.context);
						newFrame.function = func;
						newFrame.args     = frame.temp1.atom;
					}
					else
					{
						eval.SetReturn(null);
						frame.state = new Atom("-eval-sexp-body-");
					}

					break;
			}
		}

		private static void TablePred(Evaluator eval, StackFrame frame)
		{
			var atom   = frame.args.atom;
			var result = atom != null && atom.type == AtomType.Native;
			if (!result) eval.SetReturn(Atoms.FALSE);
			else
			{
				var table = atom.value as Context;
				eval.Return(table != null ? Atoms.TRUE : Atoms.FALSE);
			}
		}

#endregion

#region Internal

		// Как сейчас работает словарь?
		private static void FillDictionary(Context dict, Atom args)
		{
			for (Atom iter = args; iter != null && iter.next != null; iter = iter.next.next)
			{
				Atom key   = (Atom) iter.value;
				Atom value = (Atom) iter.next.atom.value;
				if (key.type != AtomType.String && key.type != AtomType.Symbol)
					throw new BombardoException("Table key must be string or symbol!!!");
				dict.Add((string) key.value, value);
			}
		}

		private static Context GetDictionary(Atom atom)
		{
			if (atom == null) return null;
			if (atom.type != AtomType.Native) return null;
			return atom.value as Context;
		}

#endregion
	}
}