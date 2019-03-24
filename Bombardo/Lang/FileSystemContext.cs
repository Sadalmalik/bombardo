using System.Collections.Generic;
using System.IO;

namespace Bombardo
{
    class FileSystemContext
    {
        public static void Setup(Context context)
        {
            //  (fsExists "filepath") -> true|false
            //  (fsOpen "filepath" [read] [write] [|create|append]) -> handler

            //  (fsRead handler) -> char
            //  (fsReadline handler) -> string
            //  (fsWrite handler char|string|symbol|number)
            //  (fsFlush handler)
            //  (fsClose handler)

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
            Atom path = (Atom)args?.value;
            if (path == null || !path.IsString())
                throw new BombardoException("<fsOpen> Path must be string!");
            string file = (string)path.value;



            File.Open(file,FileMode.Open,FileAccess.Read)

            return File.Exists(file) ? Atom.TRUE : Atom.FALSE;
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
