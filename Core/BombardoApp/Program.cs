using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bombardo.Core.Lang;

namespace Bombardo.Core
{
    public static class Program
    {
        public const string BOOT_NAME = "boot.brd";

        private static string pathToCore;
        private static string pathToBase;
        private static string pathToBoot;
        private static string pathToScript;
        private static string pathToWorkDir;


        public static void Main(string[] argsArray)
        {
            InitEnvironment();
            ParseArguments(argsArray);
            FindBootScriprt();

            var bootContext = BuildBootContext();

            var bootScript  = File.ReadAllText(pathToBoot);
            var bootProgram = BombardoLang.Parse(bootScript);

            var  eval       = new Evaluator();
            Atom bootResult = null;
            try
            {
                bootResult = eval.Evaluate(bootProgram, bootContext, "-eval-block-");

                if (bootResult != null)
                    Console.WriteLine(bootResult.ToString());
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Boot result: Exception!");
                Console.WriteLine(exc);
                Console.WriteLine();
                eval.Stack.Dump();
            }
        }

        private static void InitEnvironment()
        {
            // Основные пути
            pathToCore   = Assembly.GetEntryAssembly().Location;
            pathToBase   = Path.GetDirectoryName(pathToCore);   // Путь до интерпретатора
            pathToBoot   = Path.Combine(pathToBase, BOOT_NAME); // Путь до ядра (бут-скрипт)
            pathToScript = null;                                // Путь до исполняемого скрипта
        }

        private static void FindBootScriprt()
        {
            pathToBoot = FSUtils.FindFile(pathToBoot);
            if (string.IsNullOrEmpty(pathToBoot))
            {
                Console.WriteLine($"File not found: {pathToBoot}");
                return;
            }

            pathToWorkDir = Path.GetDirectoryName(pathToBoot); // Путь до рабочей папки
        }

        private static void ParseArguments(string[] argsArray)
        {
            var args = new Queue<string>(argsArray);
            while (args.Count > 0)
            {
                var arg = args.Dequeue();

                if (arg.StartsWith("boot:"))
                {
                    pathToBoot = Path.GetFullPath(arg.Substring(5));
                    continue;
                }

                pathToScript = Path.GetFullPath(arg);
            }

            if (args.Count > 0)
                Console.WriteLine($"Too match arguments!\nWill be ignored: {string.Join(" ", args)}");
        }

        public static Context BuildBootContext()
        {
            var context = new Context();

            AddSub(context, "lang", subContext =>
            {
                // Нет особого смысла разрывать эти определения по контекстам
                ListFunctions.Define(subContext);
                ListSugarFunctions.Define(subContext);
                ControlFunctions.Define(subContext);
                TypePredicateFunctions.Define(subContext);
                LogicFunctions.Define(subContext);
            });

            AddSub(context, "context", ContextFunctions.Define);
            AddSub(context, "console", ConsoleFunctions.Define);
            AddSub(context, "debug", DebugFunctions.Define);

            AddSub(context, "fs", FileSystemFunctions.Define);
            AddSub(context, "math", MathFunctions.Define);
            AddSub(context, "string", StringFunctions.Define);

            AddSub(context, "table", TableFunctions.Define);

            AddSub(context, "env", envContext =>
            {
                envContext.Define("pathToCore", Atom.CreateString(pathToCore));
                envContext.Define("pathToBase", Atom.CreateString(pathToBase));
                envContext.Define("pathToBoot", Atom.CreateString(pathToBoot));
                envContext.Define("pathToScript", string.IsNullOrEmpty(pathToScript) ? null : Atom.CreateString(pathToScript));
                envContext.Define("pathToWorkDirectory", Atom.CreateString(pathToWorkDir));
            });

            context.@sealed = true;
            return new Context(context);
        }

        private static void AddSub(Context context, string name, Action<Context> Define)
        {
            var builtIn = new Context();
            context.Define(name, builtIn.self);
            Define(builtIn);
            builtIn.@sealed = true;
        }
    }
}