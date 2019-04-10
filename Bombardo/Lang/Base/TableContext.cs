
using System;

namespace Bombardo
{
    public class TableContext
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TABLE_CREATE, TableCreate, 0);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TABLE_GET, TableGet, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TABLE_SET, TableSet, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TEBLE_REMOVE, TebleRemove, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TABLE_CLEAR, TableClear, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TABLE_IMPORT, TableImport, 2);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TABLE_IMPORT_ALL, TableImportAll, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_TABLE_EACH, TableEach, 2);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_TABLE_PRED, TablePred, 1);
        }
        
        private static void FillDictionary(Context dict, Atom args)
        {
            for (Atom iter = args; iter != null; iter = iter.next)
            {
                Atom pair = (Atom)iter.value;
                if (pair == null) continue;
                if (pair.type != AtomType.Pair)
                    throw new BombardoException("Table values must be pairs (key value)!");
                Atom key = (Atom)pair.value;
                Atom value = (Atom)pair.next.value;
                if (key.type != AtomType.String && key.type != AtomType.Symbol)
                    throw new BombardoException("Table key must be string or symbol!!!");
                dict.Add((string)key.value, value);
            }
        }

        public static Context GetDictionary(Atom atom)
        {
            if (atom == null) return null;
            if (atom.type != AtomType.Native) return null;
            return atom.value as Context;
        }

        public static Atom TableCreate(Atom args, Context context)
        {
            var dict = new Context();
            FillDictionary(dict, args);
            return new Atom(AtomType.Native, dict);
        }

        public static Atom TableGet(Atom args, Context context)
        {
            Atom dic = (Atom)args?.value;
            Atom key = (Atom)args?.next?.value;

            Context dictionary = GetDictionary(dic);

            if (key==null || (key.type != AtomType.String && key.type != AtomType.Symbol))
                throw new BombardoException("Table key must be string or symbol!!!");

            Atom value = null;
            dictionary.TryGetValue((string)key?.value, out value);

            return value;
        }

        public static Atom TableSet(Atom args, Context context)
        {
            Atom dic = (Atom)args?.value;

            Context dictionary = GetDictionary(dic);

            FillDictionary(dictionary, args.next);

            return null;
        }

        public static Atom TebleRemove(Atom args, Context context)
        {
            Atom dic = (Atom)args?.value;
            Atom key = (Atom)args?.next?.value;

            Context dictionary = GetDictionary(dic);

            if (key.type != AtomType.String && key.type != AtomType.Symbol)
                throw new BombardoException("Table key must be string or symbol!!!");

            dictionary.Remove((string)key.value);

            return null;
        }

        public static Atom TableClear(Atom args, Context context)
        {
            Atom dic = (Atom)args?.value;

            Context dictionary = GetDictionary(dic);

            dictionary.Clear();

            return null;
        }

        public static Atom TableImport(Atom args, Context context)
        {
            Atom dict = (Atom)args?.value;
            Atom names = (Atom)args?.next?.value;

            Context dictionary = GetDictionary(dict);

            string[] nameList = CommonUtils.ListToStringArray(names, "TABLE");
            ContextUtils.ImportSymbols(dictionary, context, nameList);

            return null;
        }

        public static Atom TableImportAll(Atom args, Context context)
        {
            Atom dict = (Atom)args?.value;

            Context dictionary = GetDictionary(dict);

            ContextUtils.ImportAllSymbols(dictionary, context);

            return null;
        }
        
        public static Atom TableEach(Atom args, Context context)
        {
            Atom dic = (Atom)args?.value;

            Context dictionary = GetDictionary(dic);

            Atom procedure = args.next?.value as Atom;
            Procedure proc = procedure?.value as Procedure;

            if (dictionary == null)
                throw new ArgumentException("First argument must be table!");
            if (proc==null)
                throw new ArgumentException("Second argument must be procedure!");

            foreach(var pair in dictionary)
            {
                Atom arg = Atom.List(new Atom(AtomType.String, pair.Key), pair.Value);
                proc.Apply(arg, context);
            }

            return null;
        }
        
        public static Atom TablePred(Atom args, Context context)
        {
            Atom atom = (Atom)args?.value;

            if (atom == null)
                return Atom.FALSE;

            if (atom.type != AtomType.Native)
                return Atom.FALSE;

            Context dict = atom.value as Context;
            if(dict!=null) return Atom.TRUE;

            return Atom.FALSE;
        }
    }
}