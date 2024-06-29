using System;
using System.IO;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // File operations
        public static readonly string FS_FILE_TEXT_READ        = "read";       // fs.file.text.read
        public static readonly string FS_FILE_TEXT_READ_LINE   = "readLine";   // fs.file.text.readLine
        public static readonly string FS_FILE_TEXT_READ_LINES  = "readLines";  // fs.file.text.readLines
        public static readonly string FS_FILE_TEXT_WRITE       = "write";      // fs.file.text.write
        public static readonly string FS_FILE_TEXT_WRITE_LINE  = "writeLine";  // fs.file.text.writeLine
        public static readonly string FS_FILE_TEXT_WRITE_LINES = "writeLines"; // fs.file.text.writeLines
    }

    public static class FSFileText
    {
        public static void Define(Context file)
        {
            // (fs.file.text.read [stream] count) -> text, если count==0 то весь файл
            // (fs.file.text.readLine [stream]) -> string
            // (fs.file.text.readLines [stream] count) -> (string1 string2 string3)
            // (fs.file.text.write [stream] value)
            // (fs.file.text.writeLine [stream] value)
            // (fs.file.text.writeLines [stream] (value1 value2 value3))

            // @formatter:off
            file.DefineFunction(Names.FS_FILE_TEXT_READ,        FileRead);
            file.DefineFunction(Names.FS_FILE_TEXT_READ_LINE,   FileReadLine);
            file.DefineFunction(Names.FS_FILE_TEXT_READ_LINES,  FileReadLines);
            file.DefineFunction(Names.FS_FILE_TEXT_WRITE,       FileWrite);
            file.DefineFunction(Names.FS_FILE_TEXT_WRITE_LINE,  FileWriteLine);
            file.DefineFunction(Names.FS_FILE_TEXT_WRITE_LINES, FileWriteLines);
            // @formatter:on
        }

        private static void FileRead(Evaluator eval, StackFrame frame)
        {
            var (stream, count) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is StreamReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            int size = 0;
            if (count != null)
            {
                if (!count.IsNumber)
                    throw new ArgumentException("Second argument must be number!");
                size = count.number.ToSInt();
            }

            if (size == 0)
            {
                var text = reader.ReadToEnd();
                eval.Return(Atom.CreateString(text));
            }
            else
            {
                var buffer = new char[size];
                var readed = reader.Read(buffer, 0, size);
                if (readed < size)
                    Array.Resize(ref buffer, readed);
                var str = new string(buffer);
                eval.Return(Atom.CreateString(str));
            }
        }

        private static void FileReadLine(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is StreamReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            var line = reader.ReadLine();
            eval.Return(Atom.CreateString(line));

            eval.Return(null);
        }

        private static void FileReadLines(Evaluator eval, StackFrame frame)
        {
            var (stream, count) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is StreamReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            Atom head = null, tail = null;
            while (!reader.EndOfStream)
            {
                var line = Atom.CreateString(reader.ReadLine());

                if (head == null)
                    head = tail = Atom.CreatePair(line, null);
                else
                    tail = tail.Next = Atom.CreatePair(line, tail);
            }

            eval.Return(head);
        }

        private static void FileWrite(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is StreamWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            writer.Write(value);

            eval.Return(null);
        }

        private static void FileWriteLine(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is StreamWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            writer.WriteLine(value);

            eval.Return(null);
        }

        private static void FileWriteLines(Evaluator eval, StackFrame frame)
        {
            var (stream, values) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is StreamWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (values == null)
                throw new ArgumentException("Values can not be null!");

            for (Atom iter = values; iter != null; iter = iter.Next)
            {
                var element = iter.Head;
                if (element != null)
                    writer.WriteLine(element);
            }

            eval.Return(null);
        }
    }
}