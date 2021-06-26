using System;
using System.Diagnostics;
using System.IO;

namespace Bombardo.V2
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			//GeneralV2Test.DoTests();

			// if (args==null || args.Length==0)
			// {
			//     REPL.Start();
			// }
			// else
			// {
			//     BombardoLangClass.Init(true);
			//     if (args.Length>1)
			//         Console.WriteLine("BombardoV1 not implements multyple arguments! Will be executed only one!");
			//
			//     BombardoLangClass.ExecuteFile(Path.GetFullPath(args[0]));
			//
			//     Console.ReadLine();
			// }

			int count = 5;

			while (count-- > 0)
			{
				Console.WriteLine($"Count: {count}");
			}

			TestRun();
		}

		public static string test = $@"
        lang.block [
			""test""
	        (context.define $CTX$ (context.getContext))
	        
	        (table.importAll context $CTX$)
	        (table.importAll console $CTX$)
	        (table.importAll math $CTX$)
	        (table.importAll lang $CTX$)
	        
	        (define RebuildTree (lambda [tree handler]
				(if [pair? tree]
					[cons	(RebuildTree (car tree) handler)
							(RebuildTree (cdr tree) handler)]
					(if [not-null? tree]
						[handler tree]))
	        ))
	        
	        (define Null [nope])
	        
	        (define PreprocessNullHandler (lambda [symbol]
				(if [eq? symbol (quote null)]
					Null
					symbol
				)
			))
	        
	        (define PreprocessNull (lambda [expression]
				(RebuildTree expression PreprocessNullHandler)
	        ))
	        
	        (define True (eq? lambda lambda)) # да, такой изврат, а шо делать если константы пока нигде не определены? :)
	        (define False (eq? lambda define))
	        
	        (define PreprocessBooleanHandler (lambda [symbol]
				(cond	[(eq? symbol (quote true)) True]	
						[(eq? symbol (quote false)) False]
						[True symbol]
				)
			))
	        
	        (define PreprocessBoolean (lambda [expression]
				(RebuildTree expression PreprocessBooleanHandler)
	        ))
	        
	        (define PreprocessNumbersHandler (lambda [symbol]
				(define number (tryParseNumber symbol))
				(if [number? number] number symbol)
			))
	        
	        (define PreprocessNumbers (lambda [expression]
				(RebuildTree expression PreprocessNumbersHandler)
	        ))
	        
	        
	        (define RebuildListTree (lambda [tree nodeHandler]
				(if [pair? tree]
					[nodeHandler tree]
					tree)
	        ))
	        
	        (define PreprocessQuotingHandler (lambda [node]
				(if [pair? node]
					[if	(and (eq? (car node) (quote `))
							 (pair? (cdr node)))
						()
					]
					[error ""Expect pair here!""])
			))
			
	        (define PreprocessQuoting (lambda [expression]
				(if [pair? expression]
					[block (
						(define iter Null)
						(while (not-null? expression)
							(if [and (eq? (car expression) (quote `))
									 (not-null? (cdr expression))]
								[block (
									(set! iter (cons [list (quote quote) (cadr expression)] iter))
									(set! expression (cddr expression))
								)]
								[block (
									(set! iter (cons (car expression) iter))
									(set! expression (cdr expression))
								)]
							)
						)
						(reverse iter)
					)]
					expression
				)
	        ))
	        
	        
	        (define PreprocessDottedPair (lambda [expression]
				(if [pair? tree]
					[block (
						()
					)]
					(if [not-null? tree]
						[handler tree]))
	        ))
	        
	        (define Identity (lambda [x] x))
	        
	        (debug.marker START_TEST__________________________________________________)
	        
	        (print (RebuildTree null Identity))
	        (print (RebuildTree [quote A] Identity))
	        (print (RebuildTree [quote (A B)] Identity))
	        (print (RebuildTree [quote (A B C)] Identity))
	        
	        (debug.marker END_TEST____________________________________________________)
	        
	        (define result (PreprocessNumbers
				[quote (+ 1 2 3 4)]
			))
			
			(cons result (eval result))
        ]
        ";

		public static void TestRun()
		{
			Console.WriteLine($"\n\n{test}\n\n\n");
			var bootContext = BuildBootContext();

			var bootProgram = BombardoLang.Parse(test);

			var eval       = new Evaluator();
			var bootResult = eval.Evaluate(bootProgram, bootContext);

			Console.WriteLine($"\nBootProgram: {bootProgram}");
			Console.WriteLine($"\nBootResult: {bootResult}");
		}

		public static Context BuildBootContext()
		{
			Context context = new Context();

			AddSub(context, "console", ConsoleFunctions.Define);
			AddSub(context, "context", ContextFunctions.Define);
			AddSub(context, "debug", DebugFunctions.Define);

			AddSub(context, "lang", DefineLang);

			AddSub(context, "fs", FileSystemFunctions.Define);
			AddSub(context, "math", MathFunctions.Define);
			AddSub(context, "string", StringFunctions.Define);

			AddSub(context, "table", TableFunctions.Define);

			context.@sealed = true;

			return new Context(context);
		}

		private static void DefineLang(Context context)
		{
			// Нет особого смысла разрывать эти определения по контекстам
			ListFunctions.Define(context);
			ListSugarFunctions.Define(context);
			ControlFunctions.Define(context);
			TypePredicateFunctions.Define(context);
			LogicFunctions.Define(context);
		}

		private static void AddSub(Context context, string name, Action<Context> Define)
		{
			Context builtIn = new Context();
			context.Define(name, builtIn.self);
			Define(builtIn);
		}
	}
}