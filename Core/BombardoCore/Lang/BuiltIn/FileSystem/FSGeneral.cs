using System;
using System.IO;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // General
        public static readonly string FS_LOAD   = "load";   // fx.load
        public static readonly string FS_SAVE   = "save";   // fs.save
        public static readonly string FS_FIND   = "find";   // fs.find
        public static readonly string FS_LOOKUP = "lookup"; // fs.lookup

        // Predicates
        public static readonly string FS_PRED_EXISTS = "exist?";            // fs.exist?
        public static readonly string FS_PRED_FILE   = "isFile?";           // fs.isFile?
        public static readonly string FS_PRED_DIR    = "isDirectory?";      // fs.isDirectory?
        public static readonly string FS_PRED_EMPTY  = "isDirectoryEmpty?"; // fs.isDirectoryEmpty?
    }

    public static class FSGeneral
    {
        public static void Define(Context ctx)
        {
            // Main Operations
            //  (fs.load "filepath") -> list
            //  (fs.save "filepath" (symbol1 symbol2 symbol3 (expression1) (expression2) (expression3)))
            //  (fs.find "filepath") -> founded path
            //  (fs.lookup "filepath") -> founded path

            ctx.DefineFunction(Names.FS_LOAD, Load);
            ctx.DefineFunction(Names.FS_SAVE, Save);
            ctx.DefineFunction(Names.FS_FIND, Find);
            ctx.DefineFunction(Names.FS_LOOKUP, LookUp);

            //  (fs.exist? "filepath") -> true|false
            //  (fs.isFile? "path") -> true | false
            //  (fs.isDirectory? "path") -> true | false
            //  (fs.isDirectoryEmpty? "path") -> true | false

            ctx.DefineFunction(Names.FS_PRED_EXISTS, Exists);
            ctx.DefineFunction(Names.FS_PRED_FILE, FilePredicate);
            ctx.DefineFunction(Names.FS_PRED_DIR, DirectoryPredicate);
            ctx.DefineFunction(Names.FS_PRED_EMPTY, DirectoryEmptyPredicate);
        }

#region Special

        private static void Load(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;

            var path = args?.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");

            var file = path.@string;
            if (File.Exists(file))
            {
                var raw   = File.ReadAllText(file);
                var nodes = BombardoLang.Parse(raw);

                eval.Return(nodes);
                return;
            }

            eval.Return(null);
        }

        private static void Save(Evaluator eval, StackFrame frame)
        {
            var (path, list) = StructureUtils.Split2(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            if (list == null || !list.IsPair)
                throw new ArgumentException("second argument must be list!");

            var stream = File.Open(path.@string, FileMode.Create);
            var output = new StreamWriter(stream);

            while (list != null)
            {
                var item = list.Head;
                output.Write(item.Stringify());
                output.Write(item.IsPair ? "\n" : " ");
                list = list.Next;
            }

            output.Flush();
            output.Dispose();
            stream.Dispose();

            eval.Return(null);
        }

        private static void Find(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");

            var file = FSUtils.FindFile(path.@string);
            if (!string.IsNullOrEmpty(file))
            {
                eval.Return(Atom.CreateString(file));
                return;
            }

            eval.Return(null);
        }

        private static void LookUp(Evaluator eval, StackFrame frame)
        {
            var (programPath,
                currentPath,
                modulesFolder,
                module,
                moduleRoot) = StructureUtils.Split5(frame.args);

            var file = FSUtils.LookupModuleFile(
                programPath.@string,
                currentPath.@string,
                modulesFolder.@string,
                module.@string,
                moduleRoot.@string
            );
            if (!string.IsNullOrEmpty(file))
            {
                eval.Return(Atom.CreateString(file));
                return;
            }

            eval.Return(null);
        }

#endregion


#region Predicates

        private static void Exists(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var file = path.@string;

            eval.Return(File.Exists(file) ? Atoms.TRUE : Atoms.FALSE);
        }


        private static void FilePredicate(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var file = path.@string;

            if (!File.Exists(file))
            {
                eval.Return(Atoms.FALSE);
                return;
            }

            var attr = File.GetAttributes(file);

            var hasDirectoryFlag = ((int)attr & (int)FileAttributes.Directory) != 0;
            eval.Return(hasDirectoryFlag ? Atoms.FALSE : Atoms.TRUE);
        }

        private static void DirectoryPredicate(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var file = path.@string;

            if (!File.Exists(file))
            {
                eval.Return(Atoms.FALSE);
                return;
            }

            var attr = File.GetAttributes(file);

            var hasDirectoryFlag = ((int)attr & (int)FileAttributes.Directory) != 0;
            eval.Return(hasDirectoryFlag ? Atoms.TRUE : Atoms.FALSE);
        }

        private static void DirectoryEmptyPredicate(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var directoryPath = path.@string;

            var dirs  = Directory.GetDirectories(directoryPath);
            var files = Directory.GetFiles(directoryPath);
            var empty = dirs.Length == 0 && files.Length == 0;

            eval.Return(empty ? Atoms.TRUE : Atoms.FALSE);
        }

#endregion
    }
}