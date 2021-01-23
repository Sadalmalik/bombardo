using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bombardo.V2.Lang
{
	public class Module
	{
		public bool loading;
		public string currentPath;
		public Context moduleContext;

		public Atom Result => moduleContext.Get(Names.MODULE, true);

		public Module(string fullPath, Context rootContext)
		{
			loading       = false;
			currentPath   = fullPath;
			moduleContext = new Context(rootContext);
			moduleContext.Define(Names.MODULE_PATH, new Atom(AtomType.String, currentPath));
			moduleContext.Define(Names.MODULE, new Atom(AtomType.Native, new Context()));
		}
	}

	public static class ModulesSystem
	{
		//	Загрузка модуля может идти двумя путями:
		//		Через предопределённый метод инициализации
		//		Загрузка из файла
		//	В обоих случаях модуль условно выполняется один раз, после чего результат его исполнения,
		//	сохранённый в module контексте, возвращается при всех следующих запросах

		private static Dictionary<string, Action<Context>> _builtInModules;
		private static Dictionary<string, Module> _modules;
		private static Stack<Module> _modulesStack;

		public static string programPath;
		public static Context baseContext;

		public static Module CurrentModule => _modulesStack.Count > 0 ? _modulesStack.Peek() : null;


		public static void Init()
		{
			_builtInModules = new Dictionary<string, Action<Context>>();
			_modules        = new Dictionary<string, Module>();
			_modulesStack   = new Stack<Module>();

			SetupBuiltInModules();

			programPath = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();

			baseContext = new Context();
			Define(baseContext);
		}

		private static void SetupBuiltInModules()
		{
			// Была мысль как-то ещё поделить, но я не придумал как...
			// Все эти функции - основа языка
			_builtInModules.Add("lang", context =>
			{
				ListFunctions.Define(context);
				ListSugarFunctions.Define(context);
				ControlFunctions.Define(context);
				TypePredicateFunctions.Define(context);
			});

			// Более того, следующие три набора функций так же можно было бы объединить с lang...
			// Но я их всё же вынес, потому как язык может работать и без них
			_builtInModules.Add("context", ContextFunctions.Define);
			_builtInModules.Add("console", ConsoleFunctions.Define);
			_builtInModules.Add("math", MathFunctions.Define);

			// А вот этот модуль вполне удобно иметь отдельным, потому как тогда имена его функций упрощаются
			_builtInModules.Add("table", TableFunctions.Define);
		}

		public static void Define(Context ctx)
		{
			ctx.DefineFunction(Names.MODULE_REQUIRE, Require, false);
			ctx.DefineFunction(Names.MODULE_EXPORT, Export, false);
		}
		
		public static Module GetModule(string fullPath)
		{
			Module module;
			
			if (!_modules.TryGetValue(fullPath, out module))
			{
				module = new Module(fullPath, baseContext);
				_modules.Add(fullPath, module);
			}

			return module;
		}
		
		private static (string, Atom, Atom, Atom) ReadArguments(Atom args)
		{
			var (path, command, rest) = StructureUtils.Split2Next(args);

			if (path.type != AtomType.String && path.type != AtomType.Symbol)
				throw new ArgumentException("argument must be string or symbol!");
			string name = (string) path.value;
			
			return (name, path, command, rest);
		}
		
		private static void Require(Evaluator eval, StackFrame frame)
		{
			var context = frame.context.value as Context;
			var (name, path, command, rest) = ReadArguments(frame.args);
			
			if (eval.HaveReturn())
			{
				ImportSymbols(context, null, path, command, rest);
				eval.SetReturn(null);
			}
			else
			{
				
			}
			
				frame.temp1 = eval.TakeReturn();
			
			var cur = CurrentModule;
			string fullPath = FSUtils.LookupModuleFile(programPath, cur.currentPath, Names.MODULES_FOLDER, name);
			
			if (fullPath != null)
			{
				var module = GetModule(fullPath);
				if (module!=null)
				{
					
				}
				
				var content = File.ReadAllText(fullPath);
				var atoms = BombardoLang.Parse(content);
				
				eval.Stack.CreateFrame("-eval-block-", atoms, )
			}
			else
			{
				
			}
			
			ImportSymbols(context, null, path, command, rest);
		}

		private static void ImportSymbols(Context context, Atom result, Atom path, Atom command, Atom rest)
		{
			if (command == null)
			{
				string name = Path.GetFileNameWithoutExtension((string) path.value);
				context.Define(name, result);
			}
			else
			{
				if (!command.IsSymbol)
					throw new ArgumentException($"Unexpected symbol '{command}'!");

				switch ((string) command.value)
				{
					case Names.MODULE_REQUIRE_AS:
						Atom name = rest.next?.atom;
						if (name == null || !name.IsSymbol)
							throw new ArgumentException($"Unexpected symbol '{name}'!");
						context.Define((string) name.value, result);
						break;
					case Names.MODULE_REQUIRE_IMPORT:
						string[] nameList = StructureUtils.ListToStringArray(rest, "REQUIRE");
						ContextUtils.ImportSymbols((Context) result.value, context, nameList);
						break;
					case Names.MODULE_REQUIRE_IMPORT_ALL:
						ContextUtils.ImportAllSymbols((Context) result.value, context);
						break;
					default:
						throw new ArgumentException($"Unexpected symbol '{command}'!");
				}
			}
		}

		private static void Export(Evaluator eval, StackFrame frame)
		{
		}
	}
}