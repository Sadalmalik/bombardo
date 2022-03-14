using System;
using System.Collections.Generic;
using System.IO;

namespace Bombardo.V2
{
	public static class Program
	{
		public static void Main(string[] argsArray)
		{
			// Мне тут нужны:
			// Путь до интерпретатора
			// Путь до ядра (бут-скрипт)
			// Путь до исполняемого скрипта
			
			string pathToApp     = System.Reflection.Assembly.GetEntryAssembly().Location;
			string basePath = Path.GetDirectoryName(pathToApp);
			string pathToBoot = Path.Combine(basePath, "bombardo.boot.brd");
			string pathToScript = null;
			string pathToWorkDirectory = Directory.GetCurrentDirectory();

			var args = new Queue<string>(argsArray);
			while (args.Count>0)
			{
				var arg = args.Dequeue();
				if (arg.StartsWith("boot:"))
				{
					pathToBoot = Path.GetFullPath(arg.Substring(5));
					continue;
				}
				pathToScript = Path.GetFullPath(arg);
			}
			
			if (args.Count>0)
				Console.WriteLine($"Too match arguments!\nWill be ignored: {string.Join(" ", args)}");
			
			pathToBoot = FSUtils.FindFile(pathToBoot);
			
			if (string.IsNullOrEmpty(pathToBoot))
			{
				Console.WriteLine($"File not found: {pathToBoot}");
				return;
			}
			
			var bootScript = File.ReadAllText(pathToBoot);
			
			var bootContext = BuildBootContext();
			bootContext.Define("pathToApp", CreateString(pathToApp));
			bootContext.Define("basePath", CreateString(basePath));
			bootContext.Define("pathToBoot", CreateString(pathToBoot));
			bootContext.Define("pathToScript", CreateString(pathToScript));
			bootContext.Define("pathToWorkDirectory", CreateString(pathToWorkDirectory));
			bootContext.@sealed = true;
			bootContext = new Context(bootContext);
			
			var bootProgram = BombardoLang.Parse(bootScript);

			var  eval       = new Evaluator();
			Atom bootResult = null;
			try
			{
				bootResult = eval.Evaluate(bootProgram, bootContext, "-eval-block-");
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc);
				Console.WriteLine();
				eval.Stack.Dump();
			}

			Console.WriteLine($"Boot result: {bootResult}");
		}
		
		private static Atom CreateString(string value)
		{
			if (string.IsNullOrEmpty(value))
				return null;
			return new Atom(AtomType.String, value);
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

			return context;
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