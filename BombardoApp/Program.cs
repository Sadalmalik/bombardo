using System;
using System.IO;

namespace Bombardo
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args==null || args.Length==0)
            {
                REPL.Start();
            }
            else
            {
                BombardoLangClass.Init(true);
                if (args.Length>1)
                    Console.WriteLine("Bombardo not implements multyple arguments! Will be executed only one!");

                BombardoLangClass.ExecuteFile(Path.GetFullPath(args[0]));

                Console.ReadLine();
            }
        }
    }
}
