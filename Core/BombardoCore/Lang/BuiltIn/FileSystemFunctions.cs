using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // Generalized stuff
        public static readonly string FS_LOAD   = "load";   // fx.load
        public static readonly string FS_SAVE   = "save";   // fs.save
        public static readonly string FS_FIND   = "find";   // fs.find
        public static readonly string FS_LOOKUP = "lookup"; // fs.lookup

        // Path operations

        public static readonly string FS_PATH_SUBMODULE     = "path";             // submodule
        public static readonly string FS_PATH_COMBINE       = "combine";          // fs.path.combine
        public static readonly string FS_PATH_GET_FULL      = "getFull";          // fs.path.getFull
        public static readonly string FS_PATH_GET_RELATIVE  = "getRelative";      // fs.path.getFull
        public static readonly string FS_PATH_GET_EXTENSION = "getExtension";     // fs.path.getExtension
        public static readonly string FS_PATH_GET_FILENAME  = "getFileName";      // fs.path.getFileName
        public static readonly string FS_PATH_GET_DIRNAME   = "getDirectoryName"; // fs.path.getDirectoryName

        // Directory operations

        public static readonly string FS_DIRECTORY_SUBMODULE = "directory"; // submodule
        public static readonly string FS_DIRECTORY_READ      = "read";      // fs.directory.read
        public static readonly string FS_DIRECTORY_CREATE    = "create";    // fs.directory.create
        public static readonly string FS_DIRECTORY_MOVE      = "move";      // fs.directory.move
        public static readonly string FS_DIRECTORY_DELETE    = "delete";    // fs.directory.delete

        // File operations

        public static readonly string FS_FILE_SUBMODULE = "file";   // submodule
        public static readonly string FS_FILE_OPEN      = "open";   // fs.file.open
        public static readonly string FS_FILE_FLUSH     = "flush";  // fs.file.flush
        public static readonly string FS_FILE_CLOSE     = "close";  // fs.file.close
        public static readonly string FS_FILE_MOVE      = "move";   // fs.file.move
        public static readonly string FS_FILE_DELETE    = "delete"; // fs.file.delete

        public static readonly string FS_FILE_READ         = "read";        // fs.file.read
        public static readonly string FS_FILE_WRITE        = "write";       // fs.file.write
        public static readonly string FS_FILE_READ_LINE    = "readLine";    // fs.file.readLine
        public static readonly string FS_FILE_WRITE_LINE   = "writeLine";   // fs.file.writeLine
        public static readonly string FS_FILE_READ_TEXT    = "readText";    // fs.file.readText
        public static readonly string FS_FILE_WRITE_TEXT   = "writeText";   // fs.file.writeText
        public static readonly string FS_FILE_READ_LINES   = "readLines";   // fs.file.readLines
        public static readonly string FS_FILE_WRITE_LINES  = "writeLines";  // fs.file.writeLines
        public static readonly string FS_FILE_APPEND_TEXT  = "appendText";  // fs.file.appendText
        public static readonly string FS_FILE_APPEND_LINES = "appendLines"; // fs.file.appendLines

        // Predicates

        public static readonly string FS_PRED_EXISTS = "exist?";            // fs.exist?
        public static readonly string FS_PRED_FILE   = "isFile?";           // fs.isFile?
        public static readonly string FS_PRED_DIR    = "isDirectory?";      // fs.isDirectory?
        public static readonly string FS_PRED_EMPTY  = "isDirectoryEmpty?"; // fs.isDirectoryEmpty?
    }

    public class FileSystemFunctions
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

            // Path operations
            //  (fs.path.combine          "path1" "path2" "path3") -> "path1/path2/path3"
            //  (fs.path.getFull          "directoryPath.ext") -> "extended/directoryPath.ext"
            //  (fs.path.getExtension     "directoryPath/file.ext") -> "ext"
            //  (fs.path.getFileName      "directoryPath/file.ext") -> "file.ext"
            //  (fs.path.getDirectoryName "directoryPath/file.ext") -> "directoryPath"

            var path = new Context();
            ctx.Define(Names.FS_PATH_SUBMODULE, path.self);
            path.DefineFunction(Names.FS_PATH_COMBINE, PathCombine);
            path.DefineFunction(Names.FS_PATH_GET_FULL, PathGetFull);
            path.DefineFunction(Names.FS_PATH_GET_RELATIVE, PathGetRelative);
            path.DefineFunction(Names.FS_PATH_GET_EXTENSION, PathGetExtension);
            path.DefineFunction(Names.FS_PATH_GET_FILENAME, PathGetFileName);
            path.DefineFunction(Names.FS_PATH_GET_DIRNAME, PathGetDirectoryName);

            // Directory operations
            //  (fs.directory.create "directoryPath") -> null
            //  (fs.directory.read "directoryPath") -> ( "file1" "file2" "file3" ... )
            //  (fs.directory.move "sourcePath" "destinationPath") -> null
            //  (fs.directory.delete "directoryPath") -> True|False

            var dir = new Context();
            ctx.Define(Names.FS_DIRECTORY_SUBMODULE, dir.self);
            dir.DefineFunction(Names.FS_DIRECTORY_READ, DirectoryRead);
            dir.DefineFunction(Names.FS_DIRECTORY_CREATE, DirectoryCreate);
            dir.DefineFunction(Names.FS_DIRECTORY_MOVE, DirectoryMove);
            dir.DefineFunction(Names.FS_DIRECTORY_DELETE, DirectoryDelete);

            // File operations

            //  (fs.file.open "filePath" [read] [write] [|create|append]) -> handler
            //  (fs.file.flush handler)
            //  (fs.file.close handler)
            //  (fs.file.move "sourcePath" "destinationPath")
            //  (fs.file.delete "filePath")

            //  (fs.file.read handler) -> char
            //  (fs.file.readLine handler) -> string
            //  (fs.file.write handler char|string|symbol|number)
            //  (fs.file.writeLine handler char|string|symbol|number)

            //  (fs.file.readText "filepath") -> string
            //  (fs.file.writeText "filepath") -> ( string string string ... )
            //  (fs.file.readLines "filepath" string)
            //  (fs.file.writeLines "filepath" ( string string string ... ))
            //  (fs.file.appendText "filepath" string)
            //  (fs.file.appendLines "filepath" ( string string string ... ))

            var file = new Context();
            ctx.Define(Names.FS_FILE_SUBMODULE, file.self);

            file.DefineFunction(Names.FS_FILE_OPEN, FileOpen);
            file.DefineFunction(Names.FS_FILE_FLUSH, FileFlush);
            file.DefineFunction(Names.FS_FILE_CLOSE, FileClose);
            file.DefineFunction(Names.FS_FILE_MOVE, FileMove);
            file.DefineFunction(Names.FS_FILE_DELETE, FileDelete);

            file.DefineFunction(Names.FS_FILE_READ, FileRead);
            file.DefineFunction(Names.FS_FILE_READ_LINE, FileReadLine);
            file.DefineFunction(Names.FS_FILE_WRITE, FileWrite);
            file.DefineFunction(Names.FS_FILE_WRITE_LINE, FileWriteLine);

            file.DefineFunction(Names.FS_FILE_READ_TEXT, FileReadText);
            file.DefineFunction(Names.FS_FILE_READ_LINES, FileReadLines);
            file.DefineFunction(Names.FS_FILE_WRITE_TEXT, FileWriteText);
            file.DefineFunction(Names.FS_FILE_WRITE_LINES, FileWriteLines);
            file.DefineFunction(Names.FS_FILE_APPEND_TEXT, FileAppendText);
            file.DefineFunction(Names.FS_FILE_APPEND_LINES, FileAppendLines);

            // Predicates

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


#region Path operations

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

#endregion


#region Directory operations

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

#endregion


#region File operations

        private static void FileOpen(Evaluator eval, StackFrame frame)
        {
            var (path, accessType, modeType) = StructureUtils.Split3(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");

            var file = path.@string;

            var access = ArgUtils.GetEnum(accessType, 1, FileAccess.Read);
            var mode   = ArgUtils.GetEnum(modeType, 2, FileMode.Open);

            Console.WriteLine($"fs.file.open args:\n\taccess = {access}\n\tmode = {mode}");

            if (access == FileAccess.Read)
            {
                var reader = new StreamReader(File.Open(file, mode, access));

                eval.Return(Atom.CreateNative(reader));
                return;
            }

            if (access == FileAccess.Write)
            {
                var writer = new StreamWriter(File.Open(file, mode, access));

                eval.Return(Atom.CreateNative(writer));
                return;
            }

            eval.Return(null);
        }

        private static void FileFlush(Evaluator eval, StackFrame frame)
        {
            var stream = frame.args?.Head;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be Stream!");

            var writer = stream.@object as StreamWriter;

            if (writer == null)
                throw new ArgumentException("Argument must be Stream Writer!");

            writer.Flush();

            eval.Return(null);
        }

        private static void FileClose(Evaluator eval, StackFrame frame)
        {
            var stream = frame.args?.Head;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            var reader = stream.@object as StreamReader;
            reader?.Close();

            var writer = stream.@object as StreamWriter;
            writer?.Close();

            eval.Return(null);
        }

        private static void FileMove(Evaluator eval, StackFrame frame)
        {
            var (srcPath, dstPath) = StructureUtils.Split2(frame.args);

            if (srcPath == null || !srcPath.IsString)
                throw new ArgumentException("Source Path must be string!");
            if (dstPath == null || !dstPath.IsString)
                throw new ArgumentException("Destination Path must be string!");

            File.Move(srcPath.@string, dstPath.@string);

            eval.Return(null);
        }

        private static void FileDelete(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");

            File.Delete(path.@string);

            eval.Return(null);
        }

        private static void FileRead(Evaluator eval, StackFrame frame)
        {
            var stream = frame.args.Head;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            var reader = stream.@object as StreamReader;
            if (reader == null)
                throw new ArgumentException("Argument must be Stream Reader!");

            eval.Return(Atom.CreateNumber(reader.Read()));
        }

        private static void FileReadLine(Evaluator eval, StackFrame frame)
        {
            var stream = frame.args.Head;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be Stream!");

            var reader = stream.@object as StreamReader;
            if (reader == null)
                throw new ArgumentException("Argument must be Stream Reader!");

            eval.Return(Atom.CreateString(reader.ReadLine()));
        }

        private static void FileWrite(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            var writer = stream.@object as StreamWriter;
            if (writer == null) throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null) throw new ArgumentException("Second argument can't be null!");

            Console.WriteLine($"fs.file.write args:\n\tvalue = {value}");
            writer.Write(value.ToString());

            eval.Return(null);
        }

        private static void FileWriteLine(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            var writer = stream.@object as StreamWriter;
            if (writer == null) throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null) throw new ArgumentException("Second argument can't be null!");

            writer.WriteLine(value.Stringify());

            eval.Return(null);
        }

        private static void FileReadText(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var file = path.@string;

            var text = File.ReadAllText(file, Encoding.UTF8);

            eval.Return(Atom.CreateString(text));
        }

        private static void FileReadLines(Evaluator eval, StackFrame frame)
        {
            var path = frame.args.Head;

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var file = path.@string;

            var lines = File.ReadAllLines(file, Encoding.UTF8);

            var atoms = new Atom[lines.Length];
            for (var i = 0; i < lines.Length; i++)
                atoms[i] = Atom.CreateString(lines[i]);

            eval.Return(StructureUtils.List(atoms));
        }

        private static void FileWriteText(Evaluator eval, StackFrame frame)
        {
            var (path, text) = StructureUtils.Split2(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("First argument must be string!");
            var file = path.@string;

            if (text == null || !text.IsString)
                throw new ArgumentException("Second argument must be string!");
            var data = text.@string;

            File.WriteAllText(file, data, Encoding.UTF8);

            eval.Return(null);
        }

        private static void FileWriteLines(Evaluator eval, StackFrame frame)
        {
            var (path, list) = StructureUtils.Split2(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("First argument must be string!");
            var file = path.@string;

            if (list == null || !list.IsPair)
                throw new ArgumentException("Second argument must be list of strings!");

            var lines = new List<string>();
            for (var iter = list; iter != null; iter = iter.Next)
            {
                var line = iter.Head;
                if (line == null || !line.IsString)
                    throw new ArgumentException("Second argument must be list of strings!");
                lines.Add(line.@string);
            }

            File.WriteAllLines(file, lines, Encoding.UTF8);

            eval.Return(null);
        }

        private static void FileAppendText(Evaluator eval, StackFrame frame)
        {
            var (path, text) = StructureUtils.Split2(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("First argument must be string!");
            var file = path.@string;

            if (text == null || !text.IsString)
                throw new ArgumentException("Second argument must be string!");
            var data = text.@string;

            File.AppendAllText(file, data, Encoding.UTF8);

            eval.Return(null);
        }

        private static void FileAppendLines(Evaluator eval, StackFrame frame)
        {
            var (path, list) = StructureUtils.Split2(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("First argument must be string!");
            var file = path.@string;

            if (list == null || !list.IsPair)
                throw new ArgumentException("Second argument must be list of strings!");

            var lines = new List<string>();
            for (var iter = list; iter != null; iter = iter.Next)
            {
                var line = iter.Head;
                if (line == null || !line.IsString)
                    throw new ArgumentException("Second argument must be list of strings!");
                lines.Add(line.@string);
            }

            File.AppendAllLines(file, lines, Encoding.UTF8);

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

            var hasDirectoryFlag = ((int) attr & (int) FileAttributes.Directory) != 0;
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

            var hasDirectoryFlag = ((int) attr & (int) FileAttributes.Directory) != 0;
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