using System;
using System.IO;

namespace Bombardo.Core
{
    public static partial class Names
    {
        // File operations
        public static readonly string FS_FILE_BIN_READ_UINT_8  = "readUInt8";   // fs.file.bin.readUInt8
        public static readonly string FS_FILE_BIN_READ_SINT_8  = "readSInt8";   // fs.file.bin.readSInt8
        public static readonly string FS_FILE_BIN_READ_UINT16  = "readUInt16";  // fs.file.bin.readUInt16
        public static readonly string FS_FILE_BIN_READ_SINT16  = "readSInt16";  // fs.file.bin.readSInt16
        public static readonly string FS_FILE_BIN_READ_UINT32  = "readUInt32";  // fs.file.bin.readUInt32
        public static readonly string FS_FILE_BIN_READ_SINT32  = "readSInt32";  // fs.file.bin.readSInt32
        public static readonly string FS_FILE_BIN_READ_UINT64  = "readUInt64";  // fs.file.bin.readUInt64
        public static readonly string FS_FILE_BIN_READ_SINT64  = "readSInt64";  // fs.file.bin.readSInt64
        public static readonly string FS_FILE_BIN_READ_SINGLE  = "readSingle";  // fs.file.bin.readSingle
        public static readonly string FS_FILE_BIN_READ_DOUBLE  = "readDouble";  // fs.file.bin.readDouble
        public static readonly string FS_FILE_BIN_READ_DECIMAL = "readDecimal"; // fs.file.bin.readDecimal
        public static readonly string FS_FILE_BIN_READ_STRING  = "readString";  // fs.file.bin.readString

        public static readonly string FS_FILE_BIN_WRITE         = "write";        // fs.file.bin.write
        public static readonly string FS_FILE_BIN_WRITE_UINT_8  = "writeUInt8";   // fs.file.bin.readUInt8
        public static readonly string FS_FILE_BIN_WRITE_SINT_8  = "writeSInt8";   // fs.file.bin.readSInt8
        public static readonly string FS_FILE_BIN_WRITE_UINT16  = "writeUInt16";  // fs.file.bin.readUInt16
        public static readonly string FS_FILE_BIN_WRITE_SINT16  = "writeSInt16";  // fs.file.bin.readSInt16
        public static readonly string FS_FILE_BIN_WRITE_UINT32  = "writeUInt32";  // fs.file.bin.readUInt32
        public static readonly string FS_FILE_BIN_WRITE_SINT32  = "writeSInt32";  // fs.file.bin.readSInt32
        public static readonly string FS_FILE_BIN_WRITE_UINT64  = "writeUInt64";  // fs.file.bin.readUInt64
        public static readonly string FS_FILE_BIN_WRITE_SINT64  = "writeSInt64";  // fs.file.bin.readSInt64
        public static readonly string FS_FILE_BIN_WRITE_SINGLE  = "writeSingle";  // fs.file.bin.readSingle
        public static readonly string FS_FILE_BIN_WRITE_DOUBLE  = "writeDouble";  // fs.file.bin.readDouble
        public static readonly string FS_FILE_BIN_WRITE_DECIMAL = "writeDecimal"; // fs.file.bin.readDecimal
        public static readonly string FS_FILE_BIN_WRITE_STRING  = "writeString";  // fs.file.bin.readString
    }

    public static class FSFileBinary
    {
        public static void Define(Context bin)
        {
            // @formatter:off
            // (fs.file.bin.readSInt8 [stream]) -> sint8
            // (fs.file.bin.readSInt16 [stream]) -> sint16
            // (fs.file.bin.readSInt32 [stream]) -> sint32
            // (fs.file.bin.readSInt64 [stream]) -> sint64
            // (fs.file.bin.readSingle [stream]) -> single
            // (fs.file.bin.readDouble [stream]) -> double
            // (fs.file.bin.readDecimal [stream]) -> decimal
            // (fs.file.bin.readString [stream]) -> string
            bin.DefineFunction(Names.FS_FILE_BIN_READ_UINT_8,  FileReadUInt8);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_SINT_8,  FileReadSInt8);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_UINT16,  FileReadUInt16);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_SINT16,  FileReadSInt16);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_UINT32,  FileReadUInt32);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_SINT32,  FileReadSInt32);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_UINT64,  FileReadUInt64);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_SINT64,  FileReadSInt64);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_SINGLE,  FileReadSingle);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_DOUBLE,  FileReadDouble);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_DECIMAL, FileReadDecimal);
            bin.DefineFunction(Names.FS_FILE_BIN_READ_STRING,  FileReadString);
            
            // (fs.file.bin.write [stream] value)
            // (fs.file.bin.writeSInt8 [stream] sint8)
            // (fs.file.bin.writeSInt16 [stream] sint16)
            // (fs.file.bin.writeSInt32 [stream] sint32)
            // (fs.file.bin.writeSInt64 [stream] sint64)
            // (fs.file.bin.writeSingle [stream] single)
            // (fs.file.bin.writeDouble [stream] double)
            // (fs.file.bin.writeDecimal [stream] decimal)
            // (fs.file.bin.writeString [stream] string)
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE,         FileWrite);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_UINT_8,  FileWriteUInt8);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_SINT_8,  FileWriteSInt8);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_UINT16,  FileWriteUInt16);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_SINT16,  FileWriteSInt16);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_UINT32,  FileWriteUInt32);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_SINT32,  FileWriteSInt32);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_UINT64,  FileWriteUInt64);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_SINT64,  FileWriteSInt64);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_SINGLE,  FileWriteSingle);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_DOUBLE,  FileWriteDouble);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_DECIMAL, FileWriteDecimal);
            bin.DefineFunction(Names.FS_FILE_BIN_WRITE_STRING,  FileWriteString);
            // @formatter:on
        }

#region Reading

        private static void FileReadUInt8(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            byte value = reader.ReadByte();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadSInt8(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            sbyte value = reader.ReadSByte();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadUInt16(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            ushort value = reader.ReadUInt16();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadSInt16(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            short value = reader.ReadInt16();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadUInt32(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            uint value = reader.ReadUInt32();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadSInt32(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            int value = reader.ReadInt32();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadUInt64(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            ulong value = reader.ReadUInt64();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadSInt64(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            long value = reader.ReadInt64();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadSingle(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            float value = reader.ReadSingle();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadDouble(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            double value = reader.ReadDouble();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadDecimal(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            decimal value = reader.ReadDecimal();

            eval.SetReturn(Atom.CreateNumber(value));
        }

        private static void FileReadString(Evaluator eval, StackFrame frame)
        {
            var stream = StructureUtils.Split1(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryReader reader))
                throw new ArgumentException("Argument must be Stream Reader!");

            string value = reader.ReadString();

            eval.SetReturn(Atom.CreateString(value));
        }

#endregion


#region Writing

        private static void FileWrite(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null)
            {
                eval.SetReturn(null);
                return;
            }

            if (value.IsNumber)
            {
                var number = value.number;
                switch (number.type)
                {
                    // @formatter:off
                    case AtomNumberType.UINT_8:  writer.Write(number.val_uint8);   break;
                    case AtomNumberType.SINT_8:  writer.Write(number.val_sint8);   break;
                    case AtomNumberType.UINT16:  writer.Write(number.val_uint16);  break;
                    case AtomNumberType.SINT16:  writer.Write(number.val_uint16);  break;
                    case AtomNumberType._CHAR_:  writer.Write(number.val_char);    break;
                    case AtomNumberType.UINT32:  writer.Write(number.val_uint32);  break;
                    case AtomNumberType.SINT32:  writer.Write(number.val_uint32);  break;
                    case AtomNumberType.UINT64:  writer.Write(number.val_uint64);  break;
                    case AtomNumberType.SINT64:  writer.Write(number.val_sint64);  break;
                    case AtomNumberType.SINGLE:  writer.Write(number.val_single);  break;
                    case AtomNumberType.DOUBLE:  writer.Write(number.val_double);  break;
                    case AtomNumberType.DECIMAL: writer.Write(number.val_decimal); break;
                    // @formatter:on
                }
                eval.SetReturn(null);
            }

            if (value.IsString)
            {
                writer.Write(value.@string);
                eval.SetReturn(null);
            }
            
            throw new ArgumentException("Value must be Number or String!");
        }

        private static void FileWriteUInt8(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToUByte());
            
            eval.SetReturn(null);
        }

        private static void FileWriteSInt8(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToSByte());
            
            eval.SetReturn(null);
        }

        private static void FileWriteUInt16(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToUShort());
            
            eval.SetReturn(null);
        }

        private static void FileWriteSInt16(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToSShort());
            
            eval.SetReturn(null);
        }

        private static void FileWriteUInt32(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToUInt());

            eval.SetReturn(null);
        }

        private static void FileWriteSInt32(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToSInt());

            eval.SetReturn(null);
        }

        private static void FileWriteUInt64(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToULong());

            eval.SetReturn(null);
        }

        private static void FileWriteSInt64(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToULong());

            eval.SetReturn(null);
        }

        private static void FileWriteSingle(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToSingle());

            eval.SetReturn(null);
        }

        private static void FileWriteDouble(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToDouble());

            eval.SetReturn(null);
        }

        private static void FileWriteDecimal(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsNumber)
                throw new ArgumentException("Value must be Number!");
            
            writer.Write(value.number.ToDecimal());

            eval.SetReturn(null);
        }

        private static void FileWriteString(Evaluator eval, StackFrame frame)
        {
            var (stream, value) = StructureUtils.Split2(frame.args);

            if (!stream.IsNative ||
                !(stream.@object is BinaryWriter writer))
                throw new ArgumentException("Argument must be Stream Writer!");

            if (value == null || !value.IsString)
                throw new ArgumentException("Value must be String!");
            
            writer.Write(value.@string);

            eval.SetReturn(null);
        }

#endregion
    }
}