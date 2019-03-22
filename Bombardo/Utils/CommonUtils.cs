using System;
using System.Collections.Generic;
using System.Text;

namespace Bombardo
{
    public class CommonUtils
    {
        public static string[] ListToStringArray(Atom names, string tag)
        {
            List<string> nameList = new List<string>();

            while (names != null)
            {
                Atom key = (Atom)names.value;
                if (key.type != AtomType.String && key.type != AtomType.Symbol)
                    throw new BombardoException(string.Format("<{0}> key must be string or symbol!", tag));
                nameList.Add((string)key.value);
                names = names.next;
            }

            return nameList.ToArray();
        }
    }
}
