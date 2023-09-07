using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // File operations
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
    }

    public static class FileSystemFunctions
    {
        public static void Define(Context ctx)
        {
            // General functions
            FSGeneral.Define(ctx);
            
            // Path functions
            var path = new Context();
            ctx.Define(Names.FS_SUBMODULE_PATH, path.self);
            FSPath.Define(path);

            // Directory operations

            var directory = new Context();
            ctx.Define(Names.FS_SUBMODULE_DIRECTORY, directory.self);
            FSDirectory.Define(directory);

            // File operations


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
            ctx.Define(Names.FS_SUBMODULE_FILE, file.self);


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