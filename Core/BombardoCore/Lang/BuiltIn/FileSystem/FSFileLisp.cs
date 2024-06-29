using System;
using System.IO;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // File operations
        public static readonly string FS_FILE_LISP_READ  = "read";  // fs.file.lisp.read
        public static readonly string FS_FILE_LISP_WRITE = "write"; // fs.file.lisp.write
    }

    public static class FSFileLisp
    {
        public static void Define(Context file)
        {
            // (fs.file.lisp.read [stream]) -> [sexp] до закрывающей скобки ?
            // (fs.file.lisp.write [stream] [sexp])
            
            // @formatter:off
            file.DefineFunction(Names.FS_FILE_LISP_READ,  FileRead);
            file.DefineFunction(Names.FS_FILE_LISP_WRITE, FileWrite);
            // @formatter:on
        }

        private static void FileRead(Evaluator eval, StackFrame frame)
        {
            var stream = frame.args.Head;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be Stream!");
            var reader = stream.@object as StreamReader;
            if (reader == null)
                throw new ArgumentException("Argument must be Stream Reader!");

            // А тут дилемма... я сказал что хочу читать по одному символу из файла, но...
            // парсер не умеет работать с потоками.
            // Соответственно здась надо ЯВНО читать входящий поток на нужное число символов.
            // Ну или надо перепродумывать парсер.
            throw new BombardoException("fs.file.lisp.read not implemented!");

            eval.Return(null);
        }

        private static void FileWrite(Evaluator eval, StackFrame frame)
        {
            var stream = frame.args.Head;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be Stream!");
            var reader = stream.@object as StreamWriter;
            if (reader == null)
                throw new ArgumentException("Argument must be Stream Writer!");

            throw new BombardoException("fs.file.lisp.write not implemented!");

            eval.Return(null);
        }
    }
}