namespace Client_Forms {
    partial class ChatForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.txtIn = new System.Windows.Forms.RichTextBox();
            this.lbUserlist = new System.Windows.Forms.ListBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtOut = new System.Windows.Forms.RichTextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconect = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtIn
            // 
            this.txtIn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtIn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtIn.Location = new System.Drawing.Point(118, 12);
            this.txtIn.Name = "txtIn";
            this.txtIn.ReadOnly = true;
            this.txtIn.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtIn.Size = new System.Drawing.Size(348, 186);
            this.txtIn.TabIndex = 0;
            this.txtIn.TabStop = false;
            this.txtIn.Text = "";
            // 
            // lbUserlist
            // 
            this.lbUserlist.FormattingEnabled = true;
            this.lbUserlist.Location = new System.Drawing.Point(12, 38);
            this.lbUserlist.Name = "lbUserlist";
            this.lbUserlist.Size = new System.Drawing.Size(100, 160);
            this.lbUserlist.Sorted = true;
            this.lbUserlist.TabIndex = 1;
            this.lbUserlist.TabStop = false;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(391, 204);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 51);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtOut
            // 
            this.txtOut.Location = new System.Drawing.Point(118, 204);
            this.txtOut.Name = "txtOut";
            this.txtOut.Size = new System.Drawing.Size(267, 51);
            this.txtOut.TabIndex = 0;
            this.txtOut.Text = "";
            this.txtOut.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatKeyDown);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(24, 204);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconect
            // 
            this.btnDisconect.Location = new System.Drawing.Point(24, 227);
            this.btnDisconect.Name = "btnDisconect";
            this.btnDisconect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconect.TabIndex = 3;
            this.btnDisconect.Text = "Disconnect";
            this.btnDisconect.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 4;
            this.txtName.Text = "Usuario";
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 262);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnDisconect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lbUserlist);
            this.Controls.Add(this.txtIn);
            this.Name = "ChatForm";
            this.Text = "Chat Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtIn;
        private System.Windows.Forms.ListBox lbUserlist;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtOut;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconect;
        private System.Windows.Forms.TextBox txtName;
    }
}

