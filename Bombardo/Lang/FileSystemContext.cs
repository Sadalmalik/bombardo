using System.IO;
using System.Collections.Generic;
using Bombardo.Utils;

namespace Bombardo
{
    class FileSystemContext
    {
        public static void Setup(Context context)
        {
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

            //  (load "filepath") -> list
            //  (save "filepath" (symbol1 symbol2 symbol3 (expression1) (expression2) (expression3)))

            BombardoLangClass.SetProcedure(context, "fsExists", Exists, 1);

            BombardoLangClass.SetProcedure(context, "fsOpen", Open, 1);
            BombardoLangClass.SetProcedure(context, "fsFlush", Flush, 1);
            BombardoLangClass.SetProcedure(context, "fsClose", Close, 1);

            BombardoLangClass.SetProcedure(context, "fsRead", Read, 1);
            BombardoLangClass.SetProcedure(context, "fsReadline", ReadLine, 1);
            BombardoLangClass.SetProcedure(context, "fsWrite", Write, 2);

            BombardoLangClass.SetProcedure(context, "fsReadText", ReadText, 1);
            BombardoLangClass.SetProcedure(context, "fsReadLines", ReadLines, 1);
            BombardoLangClass.SetProcedure(context, "fsWriteText", WriteText, 2);
            BombardoLangClass.SetProcedure(context, "fsWriteLines", WriteLines, 2);
            BombardoLangClass.SetProcedure(context, "fsAppendText", AppendText, 2);
            BombardoLangClass.SetProcedure(context, "fsAppendLines", AppendLines, 2);
            
            BombardoLangClass.SetProcedure(context, "load", Load, 1);
            BombardoLangClass.SetProcedure(context, "save", Save, 2);
        }

        private static Atom Exists(Atom args, Context context)
        {
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new BombardoException("<fsExists> Path must be string!");
            string file = (string)path.value;

            return File.Exists(file) ? Atom.TRUE  : Atom.FALSE;
        }

        private static Atom Open(Atom args, Context context)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new BombardoException("<fsOpen> Path must be string!");
            string file = (string)path.value;
            
            FileAccess access = ArgUtils.GetEnum<FileAccess>(
                args?.next?.atom, 1, "fsOpen");
            
            FileMode mode = ArgUtils.GetEnum<FileMode>(
                args?.next?.next?.atom, 2, "fsOpen");

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
                throw new BombardoException("<fsFlush> Argument must be stream!");
            
            StreamWriter writer = stream.value as StreamWriter;
            if (writer != null) writer.Flush();

            return null;
        }

        private static Atom Close(Atom args, Context context)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new BombardoException("<fsClose> Argument must be stream!");

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
                throw new BombardoException("<fsRead> Argument must be stream!");

            StreamReader reader = stream.value as StreamReader;
            if (reader == null) throw new BombardoException("<fsRead> Argument must be stream!");

            return new Atom(AtomType.Number, reader.Read());
        }

        private static Atom ReadLine(Atom args, Context context)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new BombardoException("<fsReadline> Argument must be stream!");

            StreamReader reader = stream.value as StreamReader;
            if (reader == null) throw new BombardoException("<fsReadline> Argument must be stream!");

            return new Atom(AtomType.String, reader.ReadLine());
        }

        private static Atom Write(Atom args, Context context)
        {
            Atom stream = args?.atom;

            if (stream.type != AtomType.Native)
                throw new BombardoException("<fsReadline> Argument must be stream!");

            StreamWriter writer = stream?.value as StreamWriter;
            if (writer == null) throw new BombardoException("<fsReadline> Argument must be stream!");

            Atom value = args?.next?.atom;

            if (value==null) throw new BombardoException("<fsReadline> Second argument can't be null!");
            writer.Write(value?.value);

            return null;
        }
        
        private static Atom ReadText(Atom args, Context context)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new BombardoException("<fsReadText> Path must be string!");
            string file = (string)path.value;

            string text = File.ReadAllText(file);

            return new Atom(AtomType.String, text);
        }

        private static Atom ReadLines(Atom args, Context context)
        {
            Atom path = args?.atom;
            if (path == null || !path.IsString())
                throw new BombardoException("<fsReadLines> Path must be string!");
            string file = (string)path.value;

            string[]lines = File.ReadAllLines(file);

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
                throw new BombardoException("<fsWriteText> First argument must be string!");
            string file = (string)path.value;

            if (text == null || !text.IsString())
                throw new BombardoException("<fsWriteText> Second argument must be string!");
            string data = (string)text.value;

            File.WriteAllText(file, data);

            return null;
        }

        private static Atom WriteLines(Atom args, Context context)
        {
            Atom path = args?.atom;
            Atom list = args?.next?.atom;

            if (path == null || !path.IsString())
                throw new BombardoException("<fsWriteLines> First argument must be string!");
            string file = (string)path.value;

            if (list == null || !list.IsPair())
                throw new BombardoException("<fsWriteLines> Second argument must be list of strings!");

            List<string> lines = new List<string>();
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                Atom line = iter.atom;
                if (line == null || !line.IsString())
                    throw new BombardoException("<fsWriteLines> Second argument must be list of strings!");
                lines.Add((string)line.value);
            }
            
            File.WriteAllLines(file, lines);

            return null;
        }

        private static Atom AppendText(Atom args, Context context)
        {
            Atom path = args?.atom;
            Atom text = args?.next?.atom;

            if (path == null || !path.IsString())
                throw new BombardoException("<fsWriteText> First argument must be string!");
            string file = (string)path.value;

            if (text == null || !text.IsString())
                throw new BombardoException("<fsWriteText> Second argument must be string!");
            string data = (string)text.value;

            File.AppendAllText(file, data);

            return null;
        }

        private static Atom AppendLines(Atom args, Context context)
        {
            Atom path = args?.atom;
            Atom list = args?.next?.atom;

            if (path == null || !path.IsString())
                throw new BombardoException("<fsWriteLines> First argument must be string!");
            string file = (string)path.value;

            if (list == null || !list.IsPair())
                throw new BombardoException("<fsWriteLines> Second argument must be list of strings!");

            List<string> lines = new List<string>();
            for (Atom iter = list; iter != null; iter = iter.next)
            {
                Atom line = iter.atom;
                if (line == null || !line.IsString())
                    throw new BombardoException("<fsWriteLines> Second argument must be list of strings!");
                lines.Add((string)line.value);
            }

            File.AppendAllLines(file, lines);

            return null;
        }
        
        private static Atom Load(Atom args, Context context)
        {
            Atom path = (Atom)args?.value;

            if (path == null || !path.IsString())
                throw new BombardoException("<LOAD> Path must be string!");

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
                throw new BombardoException("<LOAD> Path must be string!");

            if (list == null || !list.IsPair())
                throw new BombardoException("<LOAD> second argument must be list!");

            FileStream stream = File.Open((string)path.value, FileMode.Create);
            StreamWriter output = new StreamWriter(stream);

            while(list!=null)
            {
                Atom item = (Atom)list?.value;
                output.WriteLine( item!=null ? item.Stringify() : "" );
                list = list.next;
            }

            output.Flush();
            output.Dispose();
            stream.Dispose();

            return null;
        }
    }
}
