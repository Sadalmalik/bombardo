using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bombardo.V2.Lang
{
	public class Module
	{
		public bool loading;
		public Context moduleContext;
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
		
		public static void Init()
		{
            programPath = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
            
			_builtInModules = new Dictionary<string, Action<Context>>();
			_modules = new Dictionary<string, Module>();
			_modulesStack = new Stack<Module>();
			
			_builtInModules.Add("lang", context =>
			{
				ListMethods.Define(context);
				ListSugarMethods.Define(context);
				ControlMethods.Define(context);
				TypePredicatesMethods.Define(context);
			});
			_builtInModules.Add("context", ContextMethods.Define);
			_builtInModules.Add("console", ConsoleMethods.Define);
			_builtInModules.Add("math", MathMethods.Define);
			_builtInModules.Add("table", TableMethods.Define);
		}
		
		public static void GetModule()
		{
			
		}
		
	}
}