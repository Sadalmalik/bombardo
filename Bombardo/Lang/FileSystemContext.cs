using System.IO;
using System.Collections.Generic;
using Bombardo.Utils;
using System;

namespace Bombardo
{
    class FileSystemContext
    {
        public static void Setup(Context context)
        {
            //  (load "filepath") -> list
            //  (save "filepath" (symbol1 symbol2 symbol3 (expression1) (expression2) (expression3)))

            BombardoLangClass.SetProcedure(context, AllNames.FS_LOAD, Load, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_SAVE, Save, 2);

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

            BombardoLangClass.SetProcedure(context, AllNames.FS_EXISTS, Exists, 1);

            BombardoLangClass.SetProcedure(context, AllNames.FS_OPEN, Open, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_FLUSH, Flush, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_CLOSE, Close, 1);

            BombardoLangClass.SetProcedure(context, AllNames.FS_READ, Read, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_READ_LINE, ReadLine, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_WRITE, Write, 2);

            BombardoLangClass.SetProcedure(context, AllNames.FS_READ_TEXT, ReadText, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_READ_LINES, ReadLines, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_WRITE_TEXT, WriteText, 2);
            BombardoLangClass.SetProcedure(context, AllNames.FS_WRITE_LINES, WriteLines, 2);
            BombardoLangClass.SetProcedure(context, AllNames.FS_APPEND_TEXT, AppendText, 2);
            BombardoLangClass.SetProcedure(context, AllNames.FS_APPEND_LINES, AppendLines, 2);

            //  (fsFile? "path") -> true | false
            //  (fsDirectory? "path") -> true | false

            BombardoLangClass.SetProcedure(context, AllNames.FS_FILE_PRED, FilePredicate, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_DIR_PRED, DirectoryPredicate, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_DIR_EMPTY_PRED, DirectoryEmptyPredicate, 1);

            

            //  (fsReadDirectory "directoryPath") -> ( "file1" "file2" "file3" ... )
            //  (fsCreateDirectory "directoryPath") -> null
            //  (fsRemoveDirectory "directoryPath") -> null

            BombardoLangClass.SetProcedure(context, AllNames.FS_READ_DIRECTORY, ReadDirectory, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_CREATE_DIRECTORY, CreateDirectory, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_REMOVE_DIRECTORY, RemoveDirectory, 1);

            //  (fsPathCombine "path1" "path2" "path3") -> "path1/path2/path3"
            //  (fsPathGetFull "directoryPath.ext") -> "extended/directoryPath.ext"
            //  (fsPathGetExtension "directoryPath/file.ext") -> "ext"
            //  (fsPathGetFileName "directoryPath/file.ext") -> "file.ext"
            //  (fsPathGetDirectoryName "directoryPath/file.ext") -> "directoryPath"

            BombardoLangClass.SetProcedure(context, AllNames.FS_PATH_COMBINE, PathCombile, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_PATH_GET_FULL, PathGetFull, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_PATH_GET_EXTENSION, PathGetExtension, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_PATH_GET_FILE_NAME, PathGetFileName, 1);
            BombardoLangClass.SetProcedure(context, AllNames.FS_PATH_GET_DIR_NAME, PathGetDirectoryName, 1);
        }

        private static Atom Load(Atom args, Context context)
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

        private static Atom Save(Atom args, Context context)
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

        private static Atom Exists(Atom args, Context context)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            return File.Exists(file) ? Atom.TRUE  : Atom.FALSE;
        }

        private static Atom Open(Atom args, Context context)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;
            
            FileAccess access = ArgUtils.GetEnum<FileAccess>(args?.next?.atom, 1);
            FileMode mode = ArgUtils.GetEnum<FileMode>(args?.next?.next?.atom, 2);

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
        
        private static Atom Flush(Atom args, Context context)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");
            
            StreamWriter writer = stream.value as StreamWriter;
            if (writer != null) writer.Flush();

            return null;
        }

        private static Atom Close(Atom args, Context context)
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
        
        private static Atom Read(Atom args, Context context)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            StreamReader reader = stream.value as StreamReader;
            if (reader == null)
                throw new ArgumentException("Argument must be stream!");

            return new Atom(AtomType.Number, reader.Read());
        }

        private static Atom ReadLine(Atom args, Context context)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            StreamReader reader = stream.value as StreamReader;
            if (reader == null)
                throw new ArgumentException("Argument must be stream!");

            return new Atom(AtomType.String, reader.ReadLine());
        }

        private static Atom Write(Atom args, Context context)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new ArgumentException("Argument must be stream!");

            StreamWriter writer = stream?.value as StreamWriter;
            if (writer == null) throw new ArgumentException("Argument must be stream!");

            Atom value = args?.next?.atom;

            if (value==null) throw new ArgumentException("Second argument can't be null!");

            writer.Write(value?.value);

            return null;
        }
        
        private static Atom ReadText(Atom args, Context context)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            string text = File.ReadAllText(file, System.Text.Encoding.UTF8);

            return new Atom(AtomType.String, text);
        }

        private static Atom ReadLines(Atom args, Context context)
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

        private static Atom WriteText(Atom args, Context context)
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

        private static Atom WriteLines(Atom args, Context context)
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

        private static Atom AppendText(Atom args, Context context)
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

        private static Atom AppendLines(Atom args, Context context)
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

        private static Atom FilePredicate(Atom args, Context context)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            if(!File.Exists(file)) return Atom.FALSE;

            FileAttributes attr = File.GetAttributes(file);
            return attr.HasFlag(FileAttributes.Directory) ? Atom.FALSE : Atom.TRUE;
        }

        private static Atom DirectoryPredicate(Atom args, Context context)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string file = (string)path.value;

            if (!File.Exists(file)) return Atom.FALSE;

            FileAttributes attr = File.GetAttributes(file);
            return attr.HasFlag(FileAttributes.Directory) ? Atom.TRUE : Atom.FALSE;
        }

        private static Atom DirectoryEmptyPredicate(Atom args, Context context)
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
        
        private static Atom ReadDirectory(Atom args, Context context)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string directory = (string)path.value;
            Atom pattern = args?.next?.atom;
            Atom mode = args?.next?.next?.atom;
            SearchOption option = ArgUtils.GetEnum<SearchOption>(
                 mode, 3);

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

        private static Atom CreateDirectory(Atom args, Context context)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new ArgumentException("Path must be string!");
            string directoryPath = (string)path.value;

            Directory.CreateDirectory(directoryPath);

            return null;
        }

        private static Atom RemoveDirectory(Atom args, Context context)
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

        private static Atom PathCombile(Atom args, Context context)
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

        private static Atom PathGetFull(Atom args, Context context)
        {
            Atom path = args?.atom;

            if (!path.IsString())
                throw new ArgumentException("Path must be string!");

            return new Atom(AtomType.String, Path.GetFullPath((string)path.value));
        }

        private static Atom PathGetExtension(Atom args, Context context)
        {
            Atom path = args?.atom;

            if (!path.IsString())
                throw new ArgumentException("Path must be string!");

            return new Atom(AtomType.String, Path.GetExtension((string)path.value));
        }

        private static Atom PathGetFileName(Atom args, Context context)
        {
            Atom path = args?.atom;

            if (!path.IsString())
                throw new ArgumentException("Path must be string!");

            return new Atom(AtomType.String, Path.GetFileName((string)path.value));
        }

        private static Atom PathGetDirectoryName(Atom args, Context context)
        {
            Atom path = args?.atom;

            if (!path.IsString())
                throw new ArgumentException("Path must be string!");

            return new Atom(AtomType.String, Path.GetDirectoryName((string)path.value));
        }
    }
}
