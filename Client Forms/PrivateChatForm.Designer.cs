namespace Client_Forms {
    partial class PrivateChatForm {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem("", 10);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem("", 9);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem("", 8);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem("", 7);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem("", 6);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem("", 5);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem("", 4);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem("", 3);
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem("", 2);
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem("", 1);
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem("", 0);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrivateChatForm));
            this.pbBuzzer = new System.Windows.Forms.PictureBox();
            this.pbFile = new System.Windows.Forms.PictureBox();
            this.lvEmoticons = new System.Windows.Forms.ListView();
            this.pbEmoticons = new System.Windows.Forms.PictureBox();
            this.txtOut = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtIn = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.buzzTimer = new System.Windows.Forms.Timer(this.components);
            this.emoticonImageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbBuzzer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmoticons)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pbBuzzer
            // 
            this.pbBuzzer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbBuzzer.Image = global::Client_Forms.Properties.Resources.icon_buzz;
            this.pbBuzzer.Location = new System.Drawing.Point(55, 526);
            this.pbBuzzer.Name = "pbBuzzer";
            this.pbBuzzer.Size = new System.Drawing.Size(40, 40);
            this.pbBuzzer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBuzzer.TabIndex = 25;
            this.pbBuzzer.TabStop = false;
            // 
            // pbFile
            // 
            this.pbFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbFile.Image = global::Client_Forms.Properties.Resources.icon_paperclip;
            this.pbFile.Location = new System.Drawing.Point(100, 526);
            this.pbFile.Name = "pbFile";
            this.pbFile.Size = new System.Drawing.Size(40, 40);
            this.pbFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFile.TabIndex = 24;
            this.pbFile.TabStop = false;
            // 
            // lvEmoticons
            // 
            this.lvEmoticons.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvEmoticons.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvEmoticons.GridLines = true;
            listViewItem12.StateImageIndex = 0;
            listViewItem12.ToolTipText = ";)";
            listViewItem13.ToolTipText = ":)";
            listViewItem14.ToolTipText = "D\':";
            listViewItem15.ToolTipText = ":(";
            listViewItem16.ToolTipText = ":\'(";
            listViewItem17.ToolTipText = ":l";
            listViewItem18.ToolTipText = ":P";
            listViewItem19.ToolTipText = ":laugh:";
            listViewItem20.ToolTipText = ":D";
            listViewItem21.ToolTipText = ":S";
            listViewItem22.ToolTipText = ":angry:";
            this.lvEmoticons.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19,
            listViewItem20,
            listViewItem21,
            listViewItem22});
            this.lvEmoticons.LabelWrap = false;
            this.lvEmoticons.LargeImageList = this.emoticonImageList;
            this.lvEmoticons.Location = new System.Drawing.Point(9, 432);
            this.lvEmoticons.MultiSelect = false;
            this.lvEmoticons.Name = "lvEmoticons";
            this.lvEmoticons.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lvEmoticons.Scrollable = false;
            this.lvEmoticons.ShowGroups = false;
            this.lvEmoticons.ShowItemToolTips = true;
            this.lvEmoticons.Size = new System.Drawing.Size(123, 95);
            this.lvEmoticons.TabIndex = 22;
            this.lvEmoticons.TabStop = false;
            this.lvEmoticons.TileSize = new System.Drawing.Size(30, 30);
            this.lvEmoticons.UseCompatibleStateImageBehavior = false;
            this.lvEmoticons.View = System.Windows.Forms.View.Tile;
            this.lvEmoticons.Visible = false;
            // 
            // pbEmoticons
            // 
            this.pbEmoticons.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEmoticons.Image = global::Client_Forms.Properties.Resources.icon_emoticons;
            this.pbEmoticons.Location = new System.Drawing.Point(9, 526);
            this.pbEmoticons.Name = "pbEmoticons";
            this.pbEmoticons.Size = new System.Drawing.Size(40, 40);
            this.pbEmoticons.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbEmoticons.TabIndex = 21;
            this.pbEmoticons.TabStop = false;
            // 
            // txtOut
            // 
            this.txtOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.txtOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOut.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOut.ForeColor = System.Drawing.Color.White;
            this.txtOut.Location = new System.Drawing.Point(9, 469);
            this.txtOut.Name = "txtOut";
            this.txtOut.Size = new System.Drawing.Size(438, 51);
            this.txtOut.TabIndex = 17;
            this.txtOut.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(453, 469);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 51);
            this.btnSend.TabIndex = 19;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // txtIn
            // 
            this.txtIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.txtIn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIn.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIn.ForeColor = System.Drawing.Color.White;
            this.txtIn.Location = new System.Drawing.Point(9, 72);
            this.txtIn.Name = "txtIn";
            this.txtIn.ReadOnly = true;
            this.txtIn.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtIn.Size = new System.Drawing.Size(519, 391);
            this.txtIn.TabIndex = 18;
            this.txtIn.TabStop = false;
            this.txtIn.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pbClose);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(813, 48);
            this.panel1.TabIndex = 26;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Client_Forms.Properties.Resources.icon_shark_logo_1;
            this.pictureBox1.Location = new System.Drawing.Point(8, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // pbClose
            // 
            this.pbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbClose.Image = global::Client_Forms.Properties.Resources.icon_logoff;
            this.pbClose.Location = new System.Drawing.Point(772, 8);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(30, 30);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbClose.TabIndex = 15;
            this.pbClose.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.pictureBox2.Location = new System.Drawing.Point(540, 74);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(260, 208);
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.pictureBox3.Location = new System.Drawing.Point(541, 297);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(260, 223);
            this.pictureBox3.TabIndex = 28;
            this.pictureBox3.TabStop = false;
            // 
            // buzzTimer
            // 
            this.buzzTimer.Interval = 10;
            // 
            // emoticonImageList
            // 
            this.emoticonImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("emoticonImageList.ImageStream")));
            this.emoticonImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.emoticonImageList.Images.SetKeyName(0, "emote_angry.png");
            this.emoticonImageList.Images.SetKeyName(1, "emote_badfeeling.png");
            this.emoticonImageList.Images.SetKeyName(2, "emote_happy.png");
            this.emoticonImageList.Images.SetKeyName(3, "emote_laugh.png");
            this.emoticonImageList.Images.SetKeyName(4, "emote_P.png");
            this.emoticonImageList.Images.SetKeyName(5, "emote_pokerface.png");
            this.emoticonImageList.Images.SetKeyName(6, "emote_sad.png");
            this.emoticonImageList.Images.SetKeyName(7, "emote_sad2.png");
            this.emoticonImageList.Images.SetKeyName(8, "emote_sad3.png");
            this.emoticonImageList.Images.SetKeyName(9, "emote_smile.png");
            this.emoticonImageList.Images.SetKeyName(10, "emote_winkyface.png");
            // 
            // PrivateChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(42)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(809, 581);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pbBuzzer);
            this.Controls.Add(this.pbFile);
            this.Controls.Add(this.lvEmoticons);
            this.Controls.Add(this.pbEmoticons);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtIn);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PrivateChatForm";
            this.Text = "PrivateChatForm";
            this.Load += new System.EventHandler(this.PrivateChatForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbBuzzer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmoticons)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbBuzzer;
        private System.Windows.Forms.PictureBox pbFile;
        private System.Windows.Forms.ListView lvEmoticons;
        private System.Windows.Forms.PictureBox pbEmoticons;
        private System.Windows.Forms.RichTextBox txtOut;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtIn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.ImageList emoticonImageList;
        private System.Windows.Forms.Timer buzzTimer;
    }
}