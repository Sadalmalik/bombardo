using System.IO;

namespace Bombardo
{
    public class FSUtils
    {
        public static string FindFile(string name)
        {
            string file = null;
            int deph = 0;
            bool isRoot = false;
            do
            {
                string path = Path.GetFullPath("../".Repeat(deph) + ".");
                DirectoryInfo d = new DirectoryInfo(path);
                if (d.Parent == null) isRoot = true;
                path = Path.Combine(path, name);
                if (File.Exists(path)) file = path;
                deph++;
            }
            while (file == null && !isRoot);
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

        public static string LookupModuleFile(string programPath, string currentPath, string modulesFolder, string module)
        {
            string path = null;
            if (module.EndsWith(".brd"))
            {
                if (CheckFile(out path, currentPath, module) ||
                    CheckFile(out path, currentPath, modulesFolder, module) ||
                    CheckFile(out path, programPath, module) ||
                    CheckFile(out path, programPath, modulesFolder, module)) return path;
            }
            else
            {
                string moduleFile = module + ".brd";
                if (CheckFile(out path, currentPath, moduleFile) ||
                    CheckFile(out path, currentPath, modulesFolder, moduleFile) ||
                    CheckFile(out path, programPath, moduleFile) ||
                    CheckFile(out path, programPath, modulesFolder, moduleFile)) return path;

                string index = Path.Combine(module, "/index.brd");
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
