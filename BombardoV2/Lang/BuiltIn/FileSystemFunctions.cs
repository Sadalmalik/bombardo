namespace Bombardo.V2
{
	public static partial class Names
	{
        public static readonly string FS_LOAD = "load";
        public static readonly string FS_SAVE = "save";

        public static readonly string FS_EXISTS = "fsExist?"; // "fsExist?"

        public static readonly string FS_OPEN = "fsOpen";
        public static readonly string FS_FLUSH = "fsFlush";
        public static readonly string FS_CLOSE = "fsClose";

        public static readonly string FS_READ = "fsRead";
        public static readonly string FS_READ_LINE = "fsReadline";
        public static readonly string FS_WRITE = "fsWrite";

        public static readonly string FS_READ_TEXT = "fsReadText";
        public static readonly string FS_READ_LINES = "fsReadLines";
        public static readonly string FS_WRITE_TEXT = "fsWriteText";
        public static readonly string FS_WRITE_LINES = "fsWriteLines";
        public static readonly string FS_APPEND_TEXT = "fsAppendText";
        public static readonly string FS_APPEND_LINES = "fsAppendLines";
        
        public static readonly string FS_FILE_PRED = "fsIsFile?";
        public static readonly string FS_DIR_PRED = "fsIsDirectory?";
        public static readonly string FS_DIR_EMPTY_PRED = "fsDirectoryIsEmpty?";

        public static readonly string FS_READ_DIRECTORY = "fsReadDirectory";
        public static readonly string FS_CREATE_DIRECTORY = "fsCreateDirectory";
        public static readonly string FS_REMOVE_DIRECTORY = "fsRemoveDirectory";

        public static readonly string FS_PATH_COMBINE = "fsPathCombine";
        public static readonly string FS_PATH_GET_FULL = "fsPathGetFull";
        public static readonly string FS_PATH_GET_EXTENSION = "fsPathGetExtension";
        public static readonly string FS_PATH_GET_FILE_NAME = "fsPathGetFileName";
        public static readonly string FS_PATH_GET_DIR_NAME = "fsPathGetDirectoryName";
	}
	
	public class FileSystemFunctions
	{
		public static void Define(Context ctx)
        {
            //  (load "filepath") -> list
            //  (save "filepath" (symbol1 symbol2 symbol3 (expression1) (expression2) (expression3)))

            ctx.DefineFunction(Names.FS_LOAD, Load);
            ctx.DefineFunction(Names.FS_SAVE, Save);

            //  (fsExists "filepath") -> true|false
            //  (fsOpen "filepath" [read] [write] [|create|append]) -> handler
            //  (fsFlush handler)
            //  (fsClose handler)

            //  (fsRead handler) -> char
            //  (fsReadline handler) -> string
            //  (fsWrite handler char|string|symbol|number)

            //  (fsReadText "filepath") -> string
            //  (fsReadLines "filepath") -> ( string string string ... )
            //  (fsWriteText "filepath" string)
            //  (fsWriteLines "filepath" ( string string string ... ))
            //  (fsAppendText "filepath" string)
            //  (fsAppendLines "filepath" ( string string string ... ))

            ctx.DefineFunction(Names.FS_EXISTS, Exists);

            ctx.DefineFunction(Names.FS_OPEN, Open);
            ctx.DefineFunction(Names.FS_FLUSH, Flush);
            ctx.DefineFunction(Names.FS_CLOSE, Close);

            ctx.DefineFunction(Names.FS_READ, Read);
            ctx.DefineFunction(Names.FS_READ_LINE, ReadLine);
            ctx.DefineFunction(Names.FS_WRITE, Write);

            ctx.DefineFunction(Names.FS_READ_TEXT, ReadText);
            ctx.DefineFunction(Names.FS_READ_LINES, ReadLines);
            ctx.DefineFunction(Names.FS_WRITE_TEXT, WriteText);
            ctx.DefineFunction(Names.FS_WRITE_LINES, WriteLines);
            ctx.DefineFunction(Names.FS_APPEND_TEXT, AppendText);
            ctx.DefineFunction(Names.FS_APPEND_LINES, AppendLines);

            //  (fsFile? "path") -> true | false
            //  (fsDirectory? "path") -> true | false

            ctx.DefineFunction(Names.FS_FILE_PRED, FilePredicate);
            ctx.DefineFunction(Names.FS_DIR_PRED, DirectoryPredicate);
            ctx.DefineFunction(Names.FS_DIR_EMPTY_PRED, DirectoryEmptyPredicate);

            

            //  (fsReadDirectory "directoryPath") -> ( "file1" "file2" "file3" ... )
            //  (fsCreateDirectory "directoryPath") -> null
            //  (fsRemoveDirectory "directoryPath") -> null

            ctx.DefineFunction(Names.FS_READ_DIRECTORY, ReadDirectory);
            ctx.DefineFunction(Names.FS_CREATE_DIRECTORY, CreateDirectory);
            ctx.DefineFunction(Names.FS_REMOVE_DIRECTORY, RemoveDirectory);

            //  (fsPathCombine "path1" "path2" "path3") -> "path1/path2/path3"
            //  (fsPathGetFull "directoryPath.ext") -> "extended/directoryPath.ext"
            //  (fsPathGetExtension "directoryPath/file.ext") -> "ext"
            //  (fsPathGetFileName "directoryPath/file.ext") -> "file.ext"
            //  (fsPathGetDirectoryName "directoryPath/file.ext") -> "directoryPath"

            ctx.DefineFunction(Names.FS_PATH_COMBINE, PathCombile);
            ctx.DefineFunction(Names.FS_PATH_GET_FULL, PathGetFull);
            ctx.DefineFunction(Names.FS_PATH_GET_EXTENSION, PathGetExtension);
            ctx.DefineFunction(Names.FS_PATH_GET_FILE_NAME, PathGetFileName);
            ctx.DefineFunction(Names.FS_PATH_GET_DIR_NAME, PathGetDirectoryName);
        }

        private static void Load(Evaluator eval, StackFrame frame)
        {
            Atom path = (Atom)args?.value;

            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");

            string file = (string)path.value;
            if (File.Exists(file))
            {
                string raw = File.ReadAllText(file);
                List<Atom> nodes = BombardoLangClass.Parse(raw);
                return Atom.List(nodes.ToArray());
            }

            return null;
        }

        private static void Save(Evaluator eval, StackFrame frame)
        {
            Atom path = (Atom)args?.value;
            Atom list = (Atom)args?.next?.value;

            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            if (list == null || !list.IsPair())
                throw new ArgumentException("second argument must be list!");

            FileStream stream = File.Open((string)path.value, FileMode.Create);
            StreamWriter output = new StreamWriter(stream);

            while (list != null)
            {
                Atom item = (Atom)list?.value;
                output.WriteLine(item != null ? item.Stringify() : "");
                list = list.next;
            }

            output.Flush();
            output.Dispose();
            stream.Dispose();

            return null;
        }

        private static void Exists(Evaluator eval, StackFrame frame)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            return File.Exists(file) ? Atom.TRUE  : Atom.FALSE;
        }

        private static void Open(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;
            
            FileAccess access = ArgUtils.GetEnum<FileAccess>(args?.next?.atom, 1, FileAccess.Read);
            FileMode mode = ArgUtils.GetEnum<FileMode>(args?.next?.next?.atom, 2, FileMode.Open);
            
            if (access == FileAccess.Read)
            {
                StreamReader reader = new StreamReader(File.Open(file, mode, access));
                
                return new Atom(AtomType.Native, reader);
            }
            else if (access == FileAccess.Write)
            {
                StreamWriter writer = new StreamWriter(File.Open(file, mode, access));
                
                return new Atom(AtomType.Native, writer);
            }

            return null;
        }
        
        private static void Flush(Evaluator eval, StackFrame frame)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");
            
            StreamWriter writer = stream.value as StreamWriter;
            if (writer != null) writer.Flush();

            return null;
        }

        private static void Close(Evaluator eval, StackFrame frame)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            StreamReader reader = stream.value as StreamReader;
            if (reader != null) reader.Close();

            StreamWriter writer = stream.value as StreamWriter;
            if (writer != null) writer.Close();

            return null;
        }
        
        private static void Read(Evaluator eval, StackFrame frame)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            StreamReader reader = stream.value as StreamReader;
            if (reader == null)
                throw new ArgumentException("Argument must be stream!");

            return new Atom(AtomType.Number, reader.Read());
        }

        private static void ReadLine(Evaluator eval, StackFrame frame)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            StreamReader reader = stream.value as StreamReader;
            if (reader == null)
                throw new ArgumentException("Argument must be stream!");

            return new Atom(AtomType.String, reader.ReadLine());
        }

        private static void Write(Evaluator eval, StackFrame frame)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            StreamWriter writer = stream?.value as StreamWriter;
            if (writer == null) throw new ArgumentException("Argument must be stream!");

            Atom value = args?.next?.atom;

            if (value==null) throw new ArgumentException("Second argument can't be null!");

            switch(value.type)
            {
                case AtomType.Number:
                    var type = UNumber.NumberType(value?.value);
                    switch (type)
                    {
                        case UNumber.UINT_8: writer.Write(Convert.ToByte(value.value)); break;
                        case UNumber.SINT_8: writer.Write(Convert.ToSByte(value.value)); break;
                        case UNumber.UINT16: writer.Write(Convert.ToUInt16(value.value)); break;
                        case UNumber.SINT16: writer.Write(Convert.ToInt16(value.value)); break;
                        case UNumber._CHAR_: writer.Write(Convert.ToChar(value.value)); break;
                        case UNumber.UINT32: writer.Write(Convert.ToUInt32(value.value)); break;
                        case UNumber.SINT32: writer.Write(Convert.ToInt32(value.value)); break;
                        case UNumber.UINT64: writer.Write(Convert.ToUInt64(value.value)); break;
                        case UNumber.SINT64: writer.Write(Convert.ToInt64(value.value)); break;
                        case UNumber.FLO32: writer.Write(Convert.ToSingle(value.value)); break;
                        case UNumber.FLO64: writer.Write(Convert.ToDouble(value.value)); break;
                        default:
                            writer.Write(value?.value);
                            break;
                    }
                    break;
                default:
                    writer.Write(value?.value);
                    break;
            }

            return null;
        }
        
        private static void ReadText(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            string text = File.ReadAllText(file, System.Text.Encoding.UTF8);

            return new Atom(AtomType.String, text);
        }

        private static void ReadLines(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            string[]lines = File.ReadAllLines(file, System.Text.Encoding.UTF8);

            Atom[]atoms = new Atom[lines.Length];
            for (int i = 0; i < lines.Length; i++)
                atoms[i]= new Atom(AtomType.String, lines[i]);
            
            return Atom.List(atoms);
        }

        private static void WriteText(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;
            Atom text = args?.next?.atom;

            if (path == null || !path.IsString())
                throw new ArgumentException("First argument must be string!");
            string file = (string)path.value;

            if (text == null || !text.IsString())
                throw new ArgumentException("Second argument must be string!");
            string data = (string)text.value;

            File.WriteAllText(file, data, System.Text.Encoding.UTF8);

            return null;
        }

        private static void WriteLines(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;
            Atom list = args?.next?.atom;

            if (path == null || !path.IsString())
                throw new ArgumentException("First argument must be string!");
            string file = (string)path.value;

            if (list == null || !list.IsPair())
                throw new ArgumentException("Second argument must be list of strings!");

            List<string> lines = new List<string>();
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                Atom line = iter.atom;
                if (line == null || !line.IsString())
                    throw new ArgumentException("Second argument must be list of strings!");
                lines.Add((string)line.value);
            }
            
            File.WriteAllLines(file, lines, System.Text.Encoding.UTF8);

            return null;
        }

        private static void AppendText(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;
            Atom text = args?.next?.atom;

            if (path == null || !path.IsString())
                throw new ArgumentException("First argument must be string!");
            string file = (string)path.value;

            if (text == null || !text.IsString())
                throw new ArgumentException("Second argument must be string!");
            string data = (string)text.value;

            File.AppendAllText(file, data, System.Text.Encoding.UTF8);

            return null;
        }

        private static void AppendLines(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;
            Atom list = args?.next?.atom;

            if (path == null || !path.IsString())
                throw new ArgumentException("First argument must be string!");
            string file = (string)path.value;

            if (list == null || !list.IsPair())
                throw new ArgumentException("Second argument must be list of strings!");

            List<string> lines = new List<string>();
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                Atom line = iter.atom;
                if (line == null || !line.IsString())
                    throw new ArgumentException("Second argument must be list of strings!");
                lines.Add((string)line.value);
            }

            File.AppendAllLines(file, lines, System.Text.Encoding.UTF8);

            return null;
        }

        private static void FilePredicate(Evaluator eval, StackFrame frame)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            if(!File.Exists(file)) return Atom.FALSE;

            FileAttributes attr = File.GetAttributes(file);
            return attr.HasFlag(FileAttributes.Directory) ? Atom.FALSE : Atom.TRUE;
        }

        private static void DirectoryPredicate(Evaluator eval, StackFrame frame)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            if (!File.Exists(file)) return Atom.FALSE;

            FileAttributes attr = File.GetAttributes(file);
            return attr.HasFlag(FileAttributes.Directory) ? Atom.TRUE : Atom.FALSE;
        }

        private static void DirectoryEmptyPredicate(Evaluator eval, StackFrame frame)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string directoryPath = (string)path.value;

            string[] dirs = Directory.GetDirectories(directoryPath);
            string[] files = Directory.GetFiles(directoryPath);
            bool empty = dirs.Length == 0 && files.Length == 0;
            
            return empty ? Atom.TRUE : Atom.FALSE;
        }
        
        private static void ReadDirectory(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string directory = (string)path.value;
            Atom pattern = args?.next?.atom;
            Atom mode = args?.next?.next?.atom;
            SearchOption option = ArgUtils.GetEnum<SearchOption>(mode, 3);

            string[] dirs = null;
            if (pattern == null) dirs = Directory.GetDirectories(directory);
            else dirs = Directory.GetDirectories(directory, (string)pattern.value, option);

            string[] files = null;
            if (pattern == null) files = Directory.GetFiles(directory);
            else files = Directory.GetFiles(directory, (string)pattern.value, option);
            
            Atom[] elements = new Atom[dirs.Length + files.Length];
            for (int i = 0; i < dirs.Length; i++) elements[i] = new Atom(AtomType.String, dirs[i]);
            for (int i = 0; i < files.Length; i++) elements[i + dirs.Length] = new Atom(AtomType.String, files[i]);
            
            return Atom.List(elements);
        }

        private static void CreateDirectory(Evaluator eval, StackFrame frame)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string directoryPath = (string)path.value;

            Directory.CreateDirectory(directoryPath);

            return null;
        }

        private static void RemoveDirectory(Evaluator eval, StackFrame frame)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string directoryPath = (string)path.value;

            string[] dirs = Directory.GetDirectories(directoryPath);
            string[] files = Directory.GetFiles(directoryPath);
            bool empty = dirs.Length == 0 && files.Length == 0;

            if(empty) Directory.Delete(directoryPath);

            return empty ? Atom.TRUE : Atom.FALSE;
        }

        private static void PathCombile(Evaluator eval, StackFrame frame)
        {
            int i = 0;
            string[] parts = new string[args.ListLength()];

            for(Atom iter= args; iter!=null; iter= iter.next)
            {
                if (!iter.atom.IsString())
                    throw new ArgumentException("All arguments must be strings!");
                parts[i] = (string)iter.atom?.value;
                i++;
            }
            
            return new Atom(AtomType.String, Path.Combine(parts));
        }

        private static void PathGetFull(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;

            if (!path.IsString())
                throw new ArgumentException("Path must be string!");

            return new Atom(AtomType.String, Path.GetFullPath((string)path.value));
        }

        private static void PathGetExtension(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;

            if (!path.IsString())
                throw new ArgumentException("Path must be string!");

            return new Atom(AtomType.String, Path.GetExtension((string)path.value));
        }

        private static void PathGetFileName(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;

            if (!path.IsString())
                throw new ArgumentException("Path must be string!");

            return new Atom(AtomType.String, Path.GetFileName((string)path.value));
        }

        private static void PathGetDirectoryName(Evaluator eval, StackFrame frame)
        {
            Atom path = args?.atom;

            if (!path.IsString())
                throw new ArgumentException("Path must be string!");

            return new Atom(AtomType.String, Path.GetDirectoryName((string)path.value));
        }
	}
}