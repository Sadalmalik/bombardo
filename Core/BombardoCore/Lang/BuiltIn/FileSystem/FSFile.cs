using System;
using System.IO;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // File operations
        public static readonly string FS_FILE_INPUT  = "input";  // fs.file.input
        public static readonly string FS_FILE_OUTPUT = "output"; // fs.file.output
        public static readonly string FS_FILE_SEEK   = "seek";   // fs.file.seek
        public static readonly string FS_FILE_FLUSH  = "flush";  // fs.file.flush
        public static readonly string FS_FILE_CLOSE  = "close";  // fs.file.close
        public static readonly string FS_FILE_MOVE   = "move";   // fs.file.move
        public static readonly string FS_FILE_DELETE = "delete"; // fs.file.delete
    }

    public static class FSFile
    {
        public static void Define(Context file)
        {
            //  (fs.file.input "filePath" [text/bin] [encoding]) -> [stream]
            //  (fs.file.output "filePath" [text/bin] [encoding]) -> [stream]
            //  (fs.file.seek [stream] offset [start/curr/end])
            //  (fs.file.flush [stream])
            //  (fs.file.close [stream])
            //  (fs.file.move "sourcePath" "destinationPath")
            //  (fs.file.delete "filePath")

            // @formatter:off
            file.DefineFunction(Names.FS_FILE_INPUT,  FileInput);
            file.DefineFunction(Names.FS_FILE_OUTPUT, FileOutput);
            file.DefineFunction(Names.FS_FILE_SEEK,   FileSeek);
            file.DefineFunction(Names.FS_FILE_FLUSH,  FileFlush);
            file.DefineFunction(Names.FS_FILE_CLOSE,  FileClose);
            file.DefineFunction(Names.FS_FILE_MOVE,   FileMove);
            file.DefineFunction(Names.FS_FILE_DELETE, FileDelete);
            // @formatter:on
        }

        private static void FileInput(Evaluator eval, StackFrame frame)
        {
            var (path, fileMode, fileEncoding) = StructureUtils.Split3(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var file = path.@string;

            var mode     = ArgUtils.GetEnum(fileMode, 2, FSMode.text);
            var encoding = ArgUtils.GetEnum(fileEncoding, 3, FSEncoding.utf8);

            if (mode == FSMode.text)
            {
                var stream = File.Open(file, FileMode.Open, FileAccess.Read);
                var reader = new StreamReader(stream, encoding.GetEncoding());
                eval.Return(Atom.CreateNative(reader));
                return;
            }

            if (mode == FSMode.bin)
            {
                var stream = File.Open(file, FileMode.Open, FileAccess.Read);
                var reader = new BinaryReader(stream, encoding.GetEncoding());
                eval.Return(Atom.CreateNative(reader));
                return;
            }

            eval.Return(null);
        }

        private static void FileOutput(Evaluator eval, StackFrame frame)
        {
            var (path, fileMode, fileEncoding) = StructureUtils.Split3(frame.args);

            if (path == null || !path.IsString)
                throw new ArgumentException("Path must be string!");
            var file = path.@string;

            var mode     = ArgUtils.GetEnum(fileMode, 2, FSMode.text);
            var encoding = ArgUtils.GetEnum(fileEncoding, 3, FSEncoding.utf8);

            if (mode == FSMode.text)
            {
                var stream = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write);
                var reader = new StreamWriter(stream, encoding.GetEncoding());
                eval.Return(Atom.CreateNative(reader));
                return;
            }

            if (mode == FSMode.bin)
            {
                var stream = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write);
                var reader = new BinaryWriter(stream, encoding.GetEncoding());
                eval.Return(Atom.CreateNative(reader));
                return;
            }

            eval.Return(null);
        }

        private static void FileSeek(Evaluator eval, StackFrame frame)
        {
            var (stream, offset, mode) = StructureUtils.Split3(frame.args);

            if (!stream.IsNative)
                throw new ArgumentException("First argument must be Stream!");
            if (!offset.IsNumber)
                throw new ArgumentException("Second argument must be number!");
            if (offset != null && !offset.IsSymbol)
                throw new ArgumentException("Third argument must be symbol!");
            var seekMode = ArgUtils.GetEnum(mode, 3, SeekOrigin.Current);

            Stream baseStream = null;

            if (stream.@object is StreamWriter streamWriter) baseStream = streamWriter.BaseStream;
            if (stream.@object is BinaryWriter binaryWriter) baseStream = binaryWriter.BaseStream;
            if (stream.@object is StreamReader streamReader) baseStream = streamReader.BaseStream;
            if (stream.@object is BinaryReader binaryReader) baseStream = binaryReader.BaseStream;

            if (baseStream == null)
                throw new ArgumentException("First argument must be Stream!");
            baseStream.Seek(offset.number.ToSInt(), seekMode);

            eval.Return(null);
        }

        private static void FileFlush(Evaluator eval, StackFrame frame)
        {
            var stream = frame.args.Head;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be Stream!");

            switch (stream.@object)
            {
                case StreamWriter streamWriter:
                    streamWriter.Flush();
                    break;
                case BinaryWriter binaryWriter:
                    binaryWriter.Flush();
                    break;
                default:
                    throw new ArgumentException("Argument must be Stream Writer!");
            }

            eval.Return(null);
        }

        private static void FileClose(Evaluator eval, StackFrame frame)
        {
            var stream = frame.args?.Head;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            switch (stream.@object)
            {
                case StreamWriter streamWriter:
                    streamWriter.Close();
                    break;
                case BinaryWriter binaryWriter:
                    binaryWriter.Close();
                    break;
                case StreamReader streamReader:
                    streamReader.Close();
                    break;
                case BinaryReader binaryReader:
                    binaryReader.Close();
                    break;
                default:
                    throw new ArgumentException("Argument must be Stream!");
            }

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
    }
}