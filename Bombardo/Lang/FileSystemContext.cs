using System.Collections.Generic;
using System.IO;

namespace Bombardo
{
    class FileSystemContext
    {
        public static void Setup(Context context)
        {
            //  (load "filename") -> list
            //  (save "filename" (symbol1 symbol2 symbol3 (expression1) (expression2) (expression3)))

            BombardoLangClass.SetProcedure(context, "load", Load, 1);
            BombardoLangClass.SetProcedure(context, "save", Save, 2);
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
