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

            while (count-->0)
            {
                Console.WriteLine($"Count: {count}");
            }
            
            TestRun();
        }
        
        public static string test = @"
        lang.block {
	        (context.define $CTX$ (context.getContext))
	        
	        (table.importAll context $CTX$)
	        (table.importAll console $CTX$)
	        (table.importAll math $CTX$)
	        (table.importAll lang $CTX$)
	        
	        (define RebuildTree (lambda [tree handler]
				(if [pair? tree]
					[cons	(RebuildTree (car tree) handler)
							(RebuildTree (cdr tree) handler)]
					(if [null? tree]
						null
						[handler tree]))
	        ))
	        
	        (define PreprocessNumbersHandler (lambda [symbol]
				(debug.marker PreprocessNumbersHandler:Start)
				(define number (tryParseNumber symbol))
				(if [number? number] number symbol)
			))
	        
	        (define PreprocessNumbers (lambda [expression]
				(debug.marker PreprocessNumbers:Start)
				(RebuildTree expression PreprocessNumbersHandler)
	        ))
	        
	        (define Identity (lambda [x] x))
	        
	        (debug.marker START_TEST__________________________________________________)
	        
	        (print (RebuildTree null Identity))
	        (print (RebuildTree [quote A] Identity))
	        (print (RebuildTree [quote (A B)] Identity))
	        (print (RebuildTree [quote (A B C)] Identity))
	        
	        (debug.marker END_TEST____________________________________________________)
	        
	        (define result (PreprocessNumbers
				[quote (print (+ 5 7 11))]
			))
			
			(cons result (eval result))
        }
        ";
        
        public static void TestRun()
        {
	        var bootContext = BuildBootContext();
	        
	        var bootProgram = BombardoLang.Parse(test);
	        
	        var eval = new Evaluator();
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
	        ListFunctions.Define(context);
	        ListSugarFunctions.Define(context);
	        ControlFunctions.Define(context);
	        TypePredicateFunctions.Define(context);
        }
        
        private static void AddSub(Context context, string name, Action<Context> Define)
        {
            Context builtIn = new Context();
            context.Define(name, builtIn.self);
            Define(builtIn);
        }
    }
}
