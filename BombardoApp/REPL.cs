﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Bombardo
{
    internal class REPL
    {
        private static bool loop_;
        private static BombardoLangClass bombardo_;
        private static string storage_ = "";

        public static void Start()
        {
            Console.WriteLine("Basic Bombardo lang");
            Console.WriteLine("07.2018 by Kaleb Sadalmalik");
            Console.WriteLine();
            Console.WriteLine("  enter (about) to see details");
            Console.WriteLine("  enter (exit) or (e) to exit app");
            Console.WriteLine("  enter (halp) or (h) to see list of everything");
            Console.WriteLine("  enter (load \"file\") or (l \"file\") to load and evaluate file");
            Console.WriteLine("  enter (debug-check-all-implementations) for check implementations");
            Console.WriteLine();

            loop_ = true;
            bombardo_ = new BombardoLangClass(false);

            //  Add to basic context
            Context system = bombardo_.Global;
            system.Define("#path", new Atom(AtomType.String, System.AppDomain.CurrentDomain.BaseDirectory));
            BombardoLangClass.SetProcedure(system, "about", ShowAbout, 0);
            BombardoLangClass.SetProcedure(system, "exit", Exit, 0);
            BombardoLangClass.SetProcedure(system, "e", Exit, 0);
            BombardoLangClass.SetProcedure(system, "halp", Halp, 0);
            BombardoLangClass.SetProcedure(system, "h", Halp, 0);
            BombardoLangClass.SetProcedure(system, "load", LoadEval, 1);
            BombardoLangClass.SetProcedure(system, "l", LoadEval, 1);
            bombardo_.WrapContext();

            Console.CancelKeyPress += ResetConsole;

            while (loop_)
            {
                try
                {
                    Console.Write(":> ");
                    storage_ += Console.ReadLine();
                    List<Atom> nodes;
                    if (TryParse(storage_, out nodes))
                    {
                        storage_ = "";
                        foreach (var node in nodes)
                        {
                            Atom result = Evaluator.Evaluate(node, bombardo_.Global);
                            Console.WriteLine(result != null ? result.ToString() : AllNames.NULL_SYMBOL);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
            }
        }

        private static bool TryParse(string str, out List<Atom> atoms)
        {
            var tokens = Lexer.Handle(str);
            if (Parser.ValidateBrackets(tokens))
            {
                atoms = Parser.Handle(tokens);
                return true;
            }
            atoms = null;
            return false;
        }

        private static void ResetConsole(object sender, ConsoleCancelEventArgs e)
        {
            storage_ = "";
            Console.WriteLine("drop stuff");
        }

        private static Atom ShowAbout(Atom args, Context context)
        {
            Console.WriteLine(@"

                            (@),,;;,,(@)
         *-----*               /||||\               *-----*
        /     / \    <*>      [ /||\ ]      <*>    / \     \
       /     /   \    |      [  -/\__ ]      |    /   \     \
      /     /     *===*\    [_- -__  - ]    /*===*     \     \
           /     /    \*===[ ___ --  __-]===*/    \     \
          /     /     /     [    __    ]     \     \     \
         /     /     /     [ --  ][   --]     \     \     \
                    /      [ _ -]  [ __ ]      \
                           [-  ]    [   ]
                           [ _]      [ -]
                          ^[^]^      ^[^]^

(note: ""make better ASCII"")

");

            return null;
        }

        private static Atom Exit(Atom args, Context context)
        {
            loop_ = false;
            return null;
        }

        private static Atom Halp(Atom args, Context context)
        {
            Context curr = (Context)bombardo_.Global;
            Context prev = (Context)curr.parent;

            Console.WriteLine("All available stuff:");
            Console.WriteLine("System context (readonly):");
            foreach (var pair in prev)
            {
                Console.WriteLine(string.Format("  {0,-32} {1}", pair.Key, pair.Value?.value?.ToString()));
            }
            Console.WriteLine();

            Console.WriteLine("Work context:");
            foreach (var pair in curr)
            {
                Console.WriteLine(string.Format("  {0,-32} {1}", pair.Key, pair.Value?.value?.ToString()));
            }
            Console.WriteLine();

            return null;
        }

        private static Atom LoadEval(Atom args, Context context)
        {
            if (args == null)
            {
                Console.WriteLine("load-eval requires string parameter!!!");
                return Atom.FALSE;
            }

            Atom atom = args.atom;
            if (atom.type != AtomType.String)
            {
                Console.WriteLine("load-eval requires string parameter!!!");
                return Atom.FALSE;
            }

            string name = atom.value as string;
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("load-eval requires string parameter!!!");
                return Atom.FALSE;
            }

            string path = FSUtils.FindFile(name);
            string raw = File.ReadAllText(path);
            var nodes = Bombardo.BombardoLangClass.Parse(raw);

            foreach (var node in nodes)
            {
                try
                {
                    Evaluator.Evaluate(node, bombardo_.Global);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
            }

            return null;
        }
    }
}