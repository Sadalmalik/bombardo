
namespace Bombardo
{
    public class ContextUtils
    {
        public static Atom Define(Context context, Atom value, string symbol)
        {
            if (symbol.Contains("."))
            {
                string[] path = symbol.Split('.');
                return Define(context, value, path, 0);
            }
            else
            {
                return context.Define(symbol, value);
            }
        }

        public static Atom Set(Context context, Atom value, string symbol)
        {
            if (symbol.Contains("."))
            {
                string[] path = symbol.Split('.');
                return Set(context, value, path, 0);
            }
            else
            {
                return context.Set(symbol, value);
            }
        }

        public static Atom Get(Context context, string symbol)
        {
            if (symbol.Contains("."))
            {
                string[] path = symbol.Split('.');
                return Get(context, path, 0);
            }
            else
            {
                return context.Get(symbol);
            }
        }

        public static Atom Define(Context context, Atom value, string[] path, int index = 0)
        {
            if (index == path.Length - 1)
            {
                if (value != null && value.type == AtomType.Procedure)
                {
                    var proc = value.value as Procedure;
                    if (proc!=null && (proc.Name=="??" || proc.Name== "λ"))
                        proc.Name = path[index];
                }

                return context.Define(path[index], value);
            }
            else
            {
                Atom dict = context.Get(path[index]);
                Context next = null;
                if (dict == null ||
                   !dict.IsNative() ||
                    (next = dict.value as Context) == null)
                {
                    throw new BombardoException(
                        string.Format(
                            "Atom '{0}' in '{1}' is not table!",
                            path[index], string.Join(".", path)
                        ));
                }
                return Define(next, value, path, index + 1);
            }
        }

        public static Atom Set(Context context, Atom value, string[] path, int index = 0)
        {
            if (index == path.Length - 1)
            {
                if (value != null && value.type == AtomType.Procedure)
                {
                    var proc = value.value as Procedure;
                    if (proc != null && (proc.Name == "??" || proc.Name == "λ"))
                        proc.Name = path[index];
                }

                return context.Set(path[index], value);
            }
            else
            {
                Atom dict = context.Get(path[index]);
                Context next = null;
                if (dict == null ||
                   !dict.IsNative() ||
                    (next = dict.value as Context) == null)
                {
                    throw new BombardoException(
                        string.Format(
                            "Atom '{0}' in '{1}' is not table!",
                            path[index], string.Join(".", path)
                        ));
                }
                return Set(next, value, path, index + 1);
            }
        }

        public static Atom Get(Context context, string[] path, int index = 0)
        {
            if (index == path.Length - 1)
            {
                return context.Get(path[index]);
            }
            else
            {
                Atom dict = context.Get(path[index]);
                Context next = null;
                if (dict == null ||
                   !dict.IsNative() ||
                    (next = dict.value as Context) == null)
                {
                    throw new BombardoException(
                        string.Format(
                            "Atom '{0}' in '{1}' is not table!",
                            path[index], string.Join(".", path)
                        ));
                }
                return Get(next, path, index + 1);
            }
        }

        public static void ImportSymbols(Context from, Context into, string[] names)
        {
            foreach (string name in names)
            {
                Atom value;
                if (from.TryGetValue(name, out value)) into.Define(name, value);
                else throw new BombardoException(string.Format("Table not contains symbol '{0}'!", name));
            }
        }

        public static void ImportAllSymbols(Context from, Context into)
        {
            foreach (var pair in from)
            {
                into.Define(pair.Key, pair.Value);
            }
        }
    }
}
