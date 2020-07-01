using System;
using System.Collections.Generic;
namespace Bombardo.V2.Lang
{
	public class Module
	{
		public bool loading;
		public Context content;
	}

	public class ModulesSystem
	{
		//	Загрузка модуля может идти двумя путями:
		//		Через предопределённый метод инициализации
		//		Загрузка из файла
		//	В обоих случаях модуль условно выполняется один раз, после чего результат его исполнения,
		//	сохранённый в module контексте, возвращается при всех следующих запросах
		
		public Dictionary<string, Action<Context>> builtInModules;
		
		public void Init()
		{
			
		}
		
		public void GetModule()
		{
			
		}
		
	}
}