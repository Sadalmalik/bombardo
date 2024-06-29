using System;
using System.Runtime.CompilerServices;

namespace Bombardo.Core
{
    public class ContextUtils
    {
        public static Atom Define(Context context, Atom value, string symbol)
        {
            if (symbol.Contains("."))
            {
                string[] path = symbol.Split('.');
                return Define(context, value, path);
            }
            else
            {
                return context.Define(symbol, value);
            }
        }

        public static Atom Undefine(Context context, string symbol)
        {
            if (symbol.Contains("."))
            {
                string[] path = symbol.Split('.');
                return Undefine(context, path);
            }
            else
            {
                return context.Undefine(symbol);
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

        private static Atom Define(Context context, Atom value, string[] path, int index = 0)
        {
            if (index == path.Length - 1)
            {
                if (value != null && value.type == AtomType.Function)
                {
                    var proc = value.function;
                    if (proc != null && (proc.Name == "??" || proc.Name == "λ"))
                        proc.Name = path[index];
                }

                return context.Define(path[index], value);
            }

            Atom dict = context.Get(path[index]);

            if (dict == null || !dict.IsContext)
                throw new BombardoException($"Atom '{path[index]}' in '{string.Join(".", path)}' is not table!");

            return Define(dict.context, value, path, index + 1);
        }

        private static Atom Undefine(Context context, string[] path, int index = 0)
        {
            if (index == path.Length - 1)
            {
                return context.Undefine(path[index]);
            }

            Atom dict = context.Get(path[index]);

            if (dict == null || !dict.IsContext)
                throw new BombardoException($"Atom '{path[index]}' in '{string.Join(".", path)}' is not table!");

            return Undefine(dict.context, path, index + 1);
        }

        private static Atom Set(Context context, Atom value, string[] path, int index = 0)
        {
            if (index == path.Length - 1)
            {
                if (value != null && value.type == AtomType.Function)
                {
                    var proc = value.function;
                    if (proc != null && (proc.Name == "??" || proc.Name == "λ"))
                        proc.Name = path[index];
                }

                return context.Set(path[index], value);
            }
            else
            {
                Atom dict = context.Get(path[index]);

                if (dict == null || !dict.IsContext)
                    throw new BombardoException($"Atom '{path[index]}' in '{string.Join(".", path)}' is not table!");

                return Set(dict.context, value, path, index + 1);
            }
        }

        private static Atom Get(Context context, string[] path, int index = 0)
        {
            if (index == path.Length - 1)
            {
                return context.Get(path[index]);
            }
            else
            {
                Atom dict = context.Get(path[index]);

                if (dict == null || !dict.IsContext)
                    throw new BombardoException($"Atom '{path[index]}' in '{string.Join(".", path)}' is not table!");

                return Get(dict.context, path, index + 1);
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


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Context GetContext(Atom context, StackFrame frame)
        {
            var ctx = frame.context.context;
            if (context != null)
            {
                if (!context.IsContext)
                    throw new ArgumentException("Definition context must be context!");
                ctx = context.context;
            }

            return ctx;
        }
    }
}