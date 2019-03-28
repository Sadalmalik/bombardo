using System;
using System.Threading;
using System.Windows.Forms;

namespace Bombardo
{
    class DialogueWindowContext
    {
        public static void Setup(Context context)
        {
            //  (CreateDialogueWindow "Title" (lambda [$window $text] ... )) -> window
            //  (DialogueWindowClose window)
            //  (DialogueWindowRename window "New Title")
            //  (DialogueWindowClear window)
            //  (DialogueWindowWrite window "message")

            BombardoLangClass.SetProcedure(context, "CreateDialogueWindow", CreateDialogueWindow, 2);
            BombardoLangClass.SetProcedure(context, "DialogueWindowClose", DialogueWindowClose, 1);
            BombardoLangClass.SetProcedure(context, "DialogueWindowRename", DialogueWindowRename, 2);
            BombardoLangClass.SetProcedure(context, "DialogueWindowClear", DialogueWindowClear, 1);
            BombardoLangClass.SetProcedure(context, "DialogueWindowWrite", DialogueWindowWrite, 2);
        }

        private static void StartWindowThread(DialogWindow window)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(window);
        }

        private static DialogWindow GetWindow(Atom win)
        {
            if (win == null || win.type != AtomType.Native)
                throw new ArgumentException("First argument must be dialogue window");

            DialogWindow window = win?.value as DialogWindow;
            if (window == null)
                throw new ArgumentException("First argument must be dialogue window");

            return window;
        }

        public static Atom CreateDialogueWindow(Atom args, Context context)
        {
            Atom title = args?.atom;
            Atom sendCallback = args?.next?.atom;
            Atom closeCallback = args?.next?.next?.atom;

            if (title == null || title.type != AtomType.String)
                throw new ArgumentException("First argument must be string");

            if (sendCallback == null || sendCallback.type != AtomType.Procedure)
                throw new ArgumentException("Second argument must be function");

            if (closeCallback != null && closeCallback.type != AtomType.Procedure)
                throw new ArgumentException("Third argument must be function or null");

            DialogWindow window = new DialogWindow(sendCallback, closeCallback, context);
            window.SetTitle((string)title?.value);

            Thread thread = new Thread(() => StartWindowThread(window));
            thread.Start();

            return window.self;
        }

        public static Atom DialogueWindowClose(Atom args, Context context)
        {
            DialogWindow window = GetWindow(args?.atom);
            
            window.Close();

            return null;
        }

        public static Atom DialogueWindowRename(Atom args, Context context)
        {
            DialogWindow window = GetWindow(args?.atom);

            window.SetTitle((string)args?.next?.atom?.value);

            return null;
        }

        public static Atom DialogueWindowClear(Atom args, Context context)
        {
            DialogWindow window = GetWindow(args?.atom);

            window.Clear();

            return null;
        }

        public static Atom DialogueWindowWrite(Atom args, Context context)
        {
            DialogWindow window = GetWindow(args?.atom);

            window.Write((string)args?.next?.atom?.value);

            return null;
        }
    }
}
