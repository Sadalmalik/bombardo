using System;
using System.Collections.Generic;

namespace Bombardo
{
    public class BombardoLangClass
    {
        private Context system_ = null;
        public Context Global { get; private set; }

        public BombardoLangClass(bool wrapped=true)
        {
            if(wrapped)
            {
                system_ = new Context();
                SetupLang(system_);
                system_.@sealed = true;
                //  Изолируем языковые примитивы, что бы пользователь системы не мог их окончательно переопределить
                Global = new Context(system_);
            }
            else
            {
                system_ = null;
                Global = new Context();
                SetupLang(Global);
            }
        }

        public void WrapContext()
        {
            if(system_==null)
            {
                system_ = Global;
                system_.@sealed = true;
                Global = new Context(system_);
            }
        }

        public static void SetupLang(Context context)
        {
            ListContext.Setup(context);
            ListSugarContext.Setup(context);
            GeneralContext.Setup(context);
            TypePredicatesContextcs.Setup(context);
            ControlContext.Setup(context);
            LogicContext.Setup(context);
            MathContext.Setup(context);
            TextContext.Setup(context);
            TableContext.Setup(context);
            TimersContext.Setup(context);
            FileSystemContext.Setup(context);
            ThreadContext.Setup(context);
        }

        public static void SetProcedure(Context context, string name, Func<Atom, Context, Atom> proc, int args = 0, bool evalArgs = true, bool evalResult = false)
        {
            Procedure p = new Procedure(name, proc, args, evalArgs, evalResult);
            context.Define(name, new Atom(AtomType.Procedure, p));
        }

        public static List<Atom> Parse(string raw)
        {
            var tokens = Lexer.Handle(raw);
            return Parser.Handle(tokens);
        }
    }
}