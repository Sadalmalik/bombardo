using System;
using System.IO;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // Directory operations
        public static readonly string FS_DIRECTORY_READ   = "read";   // fs.directory.read
        public static readonly string FS_DIRECTORY_CREATE = "create"; // fs.directory.create
        public static readonly string FS_DIRECTORY_MOVE   = "move";   // fs.directory.move
        public static readonly string FS_DIRECTORY_DELETE = "delete"; // fs.directory.delete
    }
    
    public static class FSDirectory
    {
        public static void Define(Context directory)
        {
            //  (fs.directory.create "directoryPath") -> null
            //  (fs.directory.read "directoryPath") -> ( "file1" "file2" "file3" ... )
            //  (fs.directory.move "sourcePath" "destinationPath") -> null
            //  (fs.directory.delete "directoryPath") -> True|False
            
            directory.DefineFunction(Names.FS_DIRECTORY_READ, DirectoryRead);
            directory.DefineFunction(Names.FS_DIRECTORY_CREATE, DirectoryCreate);
            directory.DefineFunction(Names.FS_DIRECTORY_MOVE, DirectoryMove);
            directory.DefineFunction(Names.FS_DIRECTORY_DELETE, DirectoryDelete);
        }
        
        private static void DirectoryRead(Evaluator eval, StackFrame frame)
        {
            var (path, pattern, mode) = StructureUtils.Split3(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var directory = path.@string;

            var option = ArgUtils.GetEnum<SearchOption>(mode, 3);

            string[] dirs = null;
            if (pattern == null) dirs = Directory.GetDirectories(directory);
            else dirs                 = Directory.GetDirectories(directory, pattern.@string, option);

            string[] files = null;
            if (pattern == null) files = Directory.GetFiles(directory);
            else files                 = Directory.GetFiles(directory, pattern.@string, option);

            var elements                                                     = new Atom[dirs.Length + files.Length];
            for (var i = 0; i < dirs.Length; i++) elements[i]                = Atom.CreateString(dirs[i]);
            for (var i = 0; i < files.Length; i++) elements[i + dirs.Length] = Atom.CreateString(files[i]);

            eval.Return(StructureUtils.List(elements));
        }

        private static void DirectoryCreate(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");

            Directory.CreateDirectory(path.@string);

            eval.Return(null);
        }

        private static void DirectoryMove(Evaluator eval, StackFrame frame)
        {
            var (srcPath, dstPath) = StructureUtils.Split2(frame.args);

            if (srcPath == null || !srcPath.IsString)
                throw new ArgumentException("Source Path must be string!");
            if (dstPath == null || !dstPath.IsString)
                throw new ArgumentException("Destination Path must be string!");

            var src = srcPath.@string;
            var dst = dstPath.@string;
            Directory.Move(src, dst);

            eval.Return(null);
        }

        private static void DirectoryDelete(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");

            var directoryPath = path.@string;

            var dirs  = Directory.GetDirectories(directoryPath);
            var files = Directory.GetFiles(directoryPath);
            var empty = dirs.Length == 0 && files.Length == 0;

            if (empty) Directory.Delete(directoryPath);

            eval.Return(empty ? Atoms.TRUE : Atoms.FALSE);
        }
    }
}