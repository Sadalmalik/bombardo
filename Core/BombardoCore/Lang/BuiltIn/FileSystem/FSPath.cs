using System;
using System.IO;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // Path operations
        public static readonly string FS_PATH_COMBINE       = "combine";          // fs.path.combine
        public static readonly string FS_PATH_GET_FULL      = "getFull";          // fs.path.getFull
        public static readonly string FS_PATH_GET_RELATIVE  = "getRelative";      // fs.path.getFull
        public static readonly string FS_PATH_GET_EXTENSION = "getExtension";     // fs.path.getExtension
        public static readonly string FS_PATH_GET_FILENAME  = "getFileName";      // fs.path.getFileName
        public static readonly string FS_PATH_GET_DIRNAME   = "getDirectoryName"; // fs.path.getDirectoryName
    }
    
    public static class FSPath
    {
        public static void Define(Context path)
        {
            // Path operations
            //  (fs.path.combine          "path1" "path2" "path3") -> "path1/path2/path3"
            //  (fs.path.getFull          "directoryPath.ext") -> "extended/directoryPath.ext"
            //  (fs.path.getExtension     "directoryPath/file.ext") -> "ext"
            //  (fs.path.getFileName      "directoryPath/file.ext") -> "file.ext"
            //  (fs.path.getDirectoryName "directoryPath/file.ext") -> "directoryPath"

            path.DefineFunction(Names.FS_PATH_COMBINE, PathCombine);
            path.DefineFunction(Names.FS_PATH_GET_FULL, PathGetFull);
            path.DefineFunction(Names.FS_PATH_GET_RELATIVE, PathGetRelative);
            path.DefineFunction(Names.FS_PATH_GET_EXTENSION, PathGetExtension);
            path.DefineFunction(Names.FS_PATH_GET_FILENAME, PathGetFileName);
            path.DefineFunction(Names.FS_PATH_GET_DIRNAME, PathGetDirectoryName);
        }

        private static void PathCombine(Evaluator eval, StackFrame frame)
        {
            var args = frame.args;

            var i     = 0;
            var parts = new string[args.ListLength()];

            for (var iter = args; iter != null; iter = iter.Next)
            {
                var item = iter.Head;
                if (!item.IsString)
                    throw new ArgumentException("All arguments must be strings!");
                parts[i] = item.@string;
                i++;
            }

            eval.Return(Atom.CreateString(Path.Combine(parts)));
        }

        private static void PathGetFull(Evaluator eval, StackFrame frame)
        {
            var (atomPath, atomBase) = StructureUtils.Split2(frame.args);

            if (!atomPath.IsString)
                throw new ArgumentException("Path must be string!");

            string path = atomPath.@string;
            string root = atomBase == null || !atomBase.IsString ? null : atomBase.@string;

            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path unexpectedly is null!");

            if (!string.IsNullOrEmpty(root))
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(root, path);

            path = Path.GetFullPath(path);

            eval.Return(Atom.CreateString(path));
        }

        private static void PathGetRelative(Evaluator eval, StackFrame frame)
        {
            var (basePath, relatedPath) = StructureUtils.Split2(frame.args);

            if (!basePath.IsString)
                throw new ArgumentException("Base Path must be string!");
            if (!relatedPath.IsString)
                throw new ArgumentException("Related Path must be string!");

            Uri bPath   = new Uri(basePath.@string);
            Uri rPath   = new Uri(relatedPath.@string);
            var newPath = bPath.MakeRelativeUri(rPath).ToString();

            eval.Return(Atom.CreateString(newPath));
        }

        private static void PathGetExtension(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (!path.IsString)
                throw new ArgumentException("Path must be string!");

            eval.Return(Atom.CreateString(Path.GetExtension(path.@string)));
        }

        private static void PathGetFileName(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (!path.IsString)
                throw new ArgumentException("Path must be string!");

            eval.Return(Atom.CreateString(Path.GetFileNameWithoutExtension(path.@string)));
        }

        private static void PathGetDirectoryName(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (!path.IsString)
                throw new ArgumentException("Path must be string!");

            eval.Return(Atom.CreateString(Path.GetDirectoryName(path.@string)));
        }
    }
}