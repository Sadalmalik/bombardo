using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Bombardo
{
    class Program
    {
        private static BombardoManager manager_;

        public static void Main(string[] args)
        {
            if (args==null || args.Length==0)
            {
                REPL.Start();
            }
            else
            {
                if (args.Length>1)
                    Console.WriteLine("Bombardo not implements multyple arguments! Will be executed only one!");
                manager_ = new BombardoManager();
                manager_.ExecuteFile(Path.GetFullPath(args[0]), true);
                Console.ReadLine();
            }
        }
    }
}
