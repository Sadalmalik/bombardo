using System.IO;

namespace Bombardo.V2
{
	public class FSUtils
	{
		/// <summary>
		/// Ищет файл в данной папке и во всех родительских папках.<br />
		/// Если файл найден - возвращает полный путь к нему<br />
		/// Если не найден - null
		/// </summary>
		/// <param name="name">Имя искомого файоа</param>
		/// <returns>Путь к файлу или null</returns>
		public static string FindFile(string name)
		{
			string file   = null;
			int    deph   = 0;
			bool   isRoot = false;
			do
			{
				string        path           = Path.GetFullPath("../".Repeat(deph) + ".");
				DirectoryInfo d              = new DirectoryInfo(path);
				if (d.Parent == null) isRoot = true;
				path = Path.Combine(path, name);
				if (File.Exists(path)) file = path;
				deph++;
			} while (file == null && !isRoot);

			return file;
		}

		//  search order for (require "somepath/something")
		//  
		//  if "somepath/something" contains extensiom .brd
		//      try find file relative to current path "currentpath/somepath/something.brd"
		//      try find file relative to program path "programmPath/somepath/something.brd"
		//  else
		//      try find file relative to current path "currentpath/somepath/something/index.brd"
		//      try find file relative to modules path "currentpath/modulesFolder/somepath/something/index.brd"
		//      try find file relative to program path "programmPath/somepath/something/index.brd"
		//      try find file relative to modules path "programmPath/modulesFolder/somepath/something/index.brd"

		private static bool CheckFile(out string path, params string[] paths)
		{
			path = Path.Combine(paths);
			return File.Exists(path);
		}

		public static string LookupModuleFile(
			string programPath,
			string currentPath,
			string modulesFolder,
			string module)
		{
			string path;
			if (module.EndsWith(".brd"))
			{
				if (CheckFile(out path, currentPath, module) ||
				    CheckFile(out path, currentPath, modulesFolder, module) ||
				    CheckFile(out path, programPath, module) ||
				    CheckFile(out path, programPath, modulesFolder, module))
					return path;
			}
			else
			{
				string moduleFile = module + ".brd";
				if (CheckFile(out path, currentPath, moduleFile) ||
				    CheckFile(out path, currentPath, modulesFolder, moduleFile) ||
				    CheckFile(out path, programPath, moduleFile) ||
				    CheckFile(out path, programPath, modulesFolder, moduleFile))
					return path;

				string index = Path.Combine(module, "/module.brd");
				if (CheckFile(out path, currentPath, index) ||
				    CheckFile(out path, currentPath, modulesFolder, index) ||
				    CheckFile(out path, programPath, index) ||
				    CheckFile(out path, programPath, modulesFolder, index))
					return path;
			}

			return null;
		}
	}
}