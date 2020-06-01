using System;
using System.IO;
using Bombardo.V2;

namespace Bombardo.V1
{
    class Program
    {
        public static void Main(string[] args)
        {
            GeneralV2Test.DoTests();
        
            // if (args==null || args.Length==0)
            // {
            //     REPL.Start();
            // }
            // else
            // {
            //     BombardoLangClass.Init(true);
            //     if (args.Length>1)
            //         Console.WriteLine("Bombardo not implements multyple arguments! Will be executed only one!");
            //
            //     BombardoLangClass.ExecuteFile(Path.GetFullPath(args[0]));
            //
            //     Console.ReadLine();
            // }
        }
    }
}
