using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // Submodules
        public static readonly string FS_SUBMODULE_PATH      = "path";      // submodule
        public static readonly string FS_SUBMODULE_DIRECTORY = "directory"; // submodule
        public static readonly string FS_SUBMODULE_FILE      = "file";      // submodule

        // File Submodules
        public static readonly string FS_FILE_SUBMODULE_TEXT = "text"; // fs.file.text
        public static readonly string FS_FILE_SUBMODULE_BIN  = "bin";  // fs.file.bin
    }

    public static class FileSystemFunctions
    {
        public static void Define(Context ctx)
        {
            // General functions
            FSGeneral.Define(ctx);

            // Path functions
            var path = new Context();
            FSPath.Define(path);
            ctx.Define(Names.FS_SUBMODULE_PATH, path.self);

            // Directory functions
            var directory = new Context();
            FSDirectory.Define(directory);
            ctx.Define(Names.FS_SUBMODULE_DIRECTORY, directory.self);

            // File functions
            var file = new Context();
            FSFile.Define(file);
            {
                var text = new Context();
                FSFileText.Define(text);
                file.Define(Names.FS_FILE_SUBMODULE_TEXT, text.self);

                var bin = new Context();
                FSFileBinary.Define(bin);
                file.Define(Names.FS_FILE_SUBMODULE_BIN, bin.self);
            }
            ctx.Define(Names.FS_SUBMODULE_FILE, file.self);
        }


#region File operations

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
    }
}