using System;
using System.Windows.Forms;

namespace Bombardo
{
    public partial class DialogWindow : Form
    {
        public Atom self;

        private Context ctx_;
        private Procedure send_;
        private Procedure close_;

        public DialogWindow(Atom sendCallback, Atom closeCallback, Context context)
        {
            self = new Atom(AtomType.Native, this);
            InitializeComponent();
            send_ = sendCallback?.value as Procedure;
            close_ = closeCallback?.value as Procedure;
            ctx_ = context;
        }
        
        private void SendButton_Click(object sender, EventArgs e)
        {
            if (send_!=null)
            {
                send_.Apply(
                    Atom.List(self, new Atom(AtomType.String, InputMessageField.Text)),
                    ctx_);
                InputMessageField.Text = "";
            }
        }

        public void Clear()
        {
            DialogueBox.Invoke(new Action(delegate () { DialogueBox.Clear(); }));
        }

        public void Write(string message)
        {
            if (!string.IsNullOrEmpty(message))
                DialogueBox.Invoke(new Action(delegate () { DialogueBox.AppendText("\r\n"+message); }));
        }

        public void SetTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
                this.Text = title;
        }

        private void DialogueWindow_Close(object sender, EventArgs e)
        {
            if (close_ != null)
                close_.Apply(Atom.List(self), ctx_);
        }
    }
}
