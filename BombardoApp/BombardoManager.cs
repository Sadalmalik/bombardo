using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bombardo
{
    public class BombardoModule
    {
        public bool loading;
        public string currentPath;
        public Context moduleContext;
        
        public BombardoModule(string path, Context rootContext)
        {
            loading = false;
            currentPath = Path.GetDirectoryName(Path.GetFullPath(path));
            moduleContext = new Context(rootContext);
            moduleContext.Define("module", new Atom(AtomType.Native, new Context()));
        }

        public Atom GetModuleResult()
        {
            return moduleContext.Get("module");
        }
    }

    class BombardoManager : IDisposable
    {
        public static readonly string modulesFolder = "modules";

        private string programPath_;
        private BombardoLangClass bombardoLang_;
        private Dictionary<string, BombardoModule> modules_;
        private Stack<BombardoModule> modulesStack_;

        public BombardoManager()
        {
            programPath_ = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();

            //  (require "myModule.brd") => myModule
            //  (require "myModule.brd" as ultraModule) => ultraModule
            //  (require "myModule.brd" import stuff1 stuff2 stuff3) => stuff1 stuff2 stuff3

            bombardoLang_ = new BombardoLangClass(false);

            DialogueWindowContext.Setup(bombardoLang_.Global);

            BombardoLangClass.SetProcedure(bombardoLang_.Global, "require", Require, 1, false);
            BombardoLangClass.SetProcedure(bombardoLang_.Global, "export", Export, 1, false);
            
            bombardoLang_.WrapContext();

            modules_ = new Dictionary<string, BombardoModule>();
            modulesStack_ = new Stack<BombardoModule>();
        }
        
        private Atom Export(Atom args, Context context)
        {
            Atom symbol = args?.atom;

            if (symbol == null) throw new ArgumentException("argument must be symbol!");

            string name = (string)symbol.value;
            Atom value = context.Get(name);

            ContextUtils.Define(context, value, "module." + name);
            
            return null;
        }

        private Atom Require(Atom args, Context context)
        {
            BombardoModule currentModule = modulesStack_.Peek();
            Atom path = args.atom;
            if (path.type != AtomType.String && path.type != AtomType.Symbol)
                throw new ArgumentException("argument must be string or symbol!");
            string file = FSUtils.LookupModuleFile(programPath_, currentModule.currentPath, modulesFolder, (string)path.value);
            if (file==null) throw new ArgumentException("file not found!");

            //  Lifting exception up to first file
            BombardoModule module = ExecuteFile(file, false);
            Atom result = module.GetModuleResult();

            Atom rest = args.next;
            if (rest!=null && rest.IsPair())
            {
                Atom command = rest.atom;

                if(!command.IsSymbol()) throw new ArgumentException(string.Format("Unexpected symbol '{0}'!", command));

                switch((string)command.value)
                {
                    case "as":
                        Atom name = rest.next?.atom;
                        if (name == null || !name.IsSymbol()) throw new ArgumentException(string.Format("Unexpected symbol '{0}'!", name));
                        context.Define((string)name.value, result);
                        break;
                    case "import":
                        string[] nameList = CommonUtils.ListToStringArray(rest.next, "REQUIRE");
                        ContextUtils.ImportSymbols((Context)result.value, context, nameList);
                        break;
                    case "import-all":
                        ContextUtils.ImportAllSymbols((Context)result.value, context);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unexpected symbol '{0}'!", command));
                }
            }
            else
            {
                string name = Path.GetFileNameWithoutExtension((string)path.value);
                context.Define(name, result);
            }

            return result;
        }

        public BombardoModule ExecuteFile(string filePath, bool catchExceptions=true)
        {
            BombardoModule module;
            if (modules_.TryGetValue(filePath, out module))
            {
                if (module.loading)
                    throw new ArgumentException("reqursive requirement found!");
                return module;
            }

            module = new BombardoModule(filePath, bombardoLang_.Global);
            modules_.Add(filePath, module);
            modulesStack_.Push(module);

            if (catchExceptions)
            {
                try { ExecuteFile(filePath, module.moduleContext); }
                catch(BombardoException exc) { Console.WriteLine(exc.ToString()); }
            }
            else { ExecuteFile(filePath, module.moduleContext); }

            modulesStack_.Pop();

            return module;
        }

        private void ExecuteFile(string path, Context context)
        {
            string raw = File.ReadAllText(path);
            List<Atom> nodes = BombardoLangClass.Parse(raw);
            foreach (var node in nodes)
                Evaluator.Evaluate(node, context);
        }

        public void Dispose()
        {

        }
    }
}
