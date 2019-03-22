
namespace Bombardo
{
    partial class DialogWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DialogueBox = new System.Windows.Forms.TextBox();
            this.InputMessageField = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DialogueBox
            // 
            this.DialogueBox.Location = new System.Drawing.Point(12, 12);
            this.DialogueBox.Multiline = true;
            this.DialogueBox.Name = "DialogueBox";
            this.DialogueBox.ReadOnly = true;
            this.DialogueBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DialogueBox.ShortcutsEnabled = false;
            this.DialogueBox.Size = new System.Drawing.Size(680, 362);
            this.DialogueBox.TabIndex = 0;
            // 
            // InputMessageField
            // 
            this.InputMessageField.Location = new System.Drawing.Point(12, 380);
            this.InputMessageField.Multiline = true;
            this.InputMessageField.Name = "InputMessageField";
            this.InputMessageField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InputMessageField.Size = new System.Drawing.Size(680, 58);
            this.InputMessageField.TabIndex = 2;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(556, 444);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(136, 28);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // DialogueWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(704, 484);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.InputMessageField);
            this.Controls.Add(this.DialogueBox);
            this.Name = "DialogueWindow";
            this.Text = "AI 0.1.001 Dialogue window";
            this.Closed += new System.EventHandler(this.DialogueWindow_Close);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DialogueBox;
        private System.Windows.Forms.TextBox InputMessageField;
        private System.Windows.Forms.Button SendButton;
    }
}