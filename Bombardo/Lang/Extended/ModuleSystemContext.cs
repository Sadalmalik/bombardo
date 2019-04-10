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

        public BombardoModule(string path, Context rootContext, bool addModule = true)
        {
            loading = false;
            currentPath = Path.GetDirectoryName(Path.GetFullPath(path));
            moduleContext = new Context(rootContext);
            moduleContext.Define(AllNames.MODULE_PATH, new Atom(AtomType.String, currentPath));

            if (addModule)
                moduleContext.Define(AllNames.MODULE, new Atom(AtomType.Native, new Context()));
        }

        public Atom GetModuleResult()
        {
            return moduleContext.Get(AllNames.MODULE);
        }
    }

    class ModuleSystemContext
    {
        private static string programPath_;
        private static Context bombardoLang_;
        private static Dictionary<string, BombardoModule> modules_;
        private static Stack<BombardoModule> modulesStack_;

        public static void Setup(Context context)
        {
            bombardoLang_ = context;

            programPath_ = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
            modules_ = new Dictionary<string, BombardoModule>();
            modulesStack_ = new Stack<BombardoModule>();

            //  (require "myModule.brd") => myModule
            //  (require "myModule.brd" as ultraModule) => ultraModule
            //  (require "myModule.brd" import stuff1 stuff2 stuff3) => stuff1 stuff2 stuff3

            BombardoLangClass.SetProcedure(bombardoLang_, AllNames.MODULE_REQUIRE, Require, 1, false);
            BombardoLangClass.SetProcedure(bombardoLang_, AllNames.MODULE_EXPORT, Export, 1, false);

            modulesStack_.Push(new BombardoModule(programPath_, context, false));
        }

        private static Atom Export(Atom args, Context context)
        {
            Atom symbol = args?.atom;

            if (symbol == null) throw new ArgumentException("argument must be symbol!");

            string name = (string)symbol.value;
            Atom value = context.Get(name);

            ContextUtils.Define(context, value, string.Format("{0}.{1}", AllNames.MODULE, name));

            return null;
        }

        private static Atom Require(Atom args, Context context)
        {
            BombardoModule currentModule = modulesStack_.Peek();
            Atom path = args.atom;
            if (path.type != AtomType.String && path.type != AtomType.Symbol)
                throw new ArgumentException("argument must be string or symbol!");
            string file = FSUtils.LookupModuleFile(programPath_, currentModule.currentPath, AllNames.MODULES_FOLDER, (string)path.value);
            if (file == null) throw new ArgumentException("file not found!");

            //  Lifting exception up to first file
            BombardoModule module = ExecuteFile(file, false);
            Atom result = module.GetModuleResult();

            Atom rest = args.next;
            if (rest != null && rest.IsPair())
            {
                Atom command = rest.atom;

                if (!command.IsSymbol()) throw new ArgumentException(string.Format("Unexpected symbol '{0}'!", command));

                switch ((string)command.value)
                {
                    case AllNames.MODULE_REQUIRE_AS:
                        Atom name = rest.next?.atom;
                        if (name == null || !name.IsSymbol()) throw new ArgumentException(string.Format("Unexpected symbol '{0}'!", name));
                        context.Define((string)name.value, result);
                        break;
                    case AllNames.MODULE_REQUIRE_IMPORT:
                        string[] nameList = CommonUtils.ListToStringArray(rest.next, "REQUIRE");
                        ContextUtils.ImportSymbols((Context)result.value, context, nameList);
                        break;
                    case AllNames.MODULE_REQUIRE_IMPORT_ALL:
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

        public static BombardoModule ExecuteFile(string filePath, bool catchExceptions = true)
        {
            BombardoModule module;
            if (modules_.TryGetValue(filePath, out module))
            {
                if (module.loading)
                    throw new ArgumentException("reqursive requirement found!");
                return module;
            }

            module = new BombardoModule(filePath, bombardoLang_);
            modules_.Add(filePath, module);
            modulesStack_.Push(module);

            if (catchExceptions)
            {
                try { ExecuteFile(filePath, module.moduleContext); }
                catch (BombardoException exc) { Console.WriteLine(exc.ToString()); }
            }
            else { ExecuteFile(filePath, module.moduleContext); }

            modulesStack_.Pop();

            return module;
        }

        private static void ExecuteFile(string path, Context context)
        {
            string raw = File.ReadAllText(path);
            List<Atom> nodes = BombardoLangClass.Parse(raw);
            foreach (var node in nodes)
                Evaluator.Evaluate(node, context);
        }
    }
}
