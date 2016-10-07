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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("", 10);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("", 9);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("", 8);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("", 7);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("", 6);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("", 5);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("", 4);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("", 3);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("", 2);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("", 1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("", 0);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrivateChatForm));
            this.lvEmoticons = new System.Windows.Forms.ListView();
            this.emoticonImageList = new System.Windows.Forms.ImageList(this.components);
            this.txtOut = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtIn = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblUsername = new System.Windows.Forms.Label();
            this.buzzTimer = new System.Windows.Forms.Timer(this.components);
            this.statusImageList = new System.Windows.Forms.ImageList(this.components);
            this.pbWebCamOut = new System.Windows.Forms.PictureBox();
            this.pbWebCamIn = new System.Windows.Forms.PictureBox();
            this.pbBuzzer = new System.Windows.Forms.PictureBox();
            this.pbFile = new System.Windows.Forms.PictureBox();
            this.pbEmoticons = new System.Windows.Forms.PictureBox();
            this.pbStatus = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbVideo = new System.Windows.Forms.PictureBox();
            this.pbGame = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebCamOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebCamIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuzzer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmoticons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).BeginInit();
            this.SuspendLayout();
            // 
            // lvEmoticons
            // 
            this.lvEmoticons.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvEmoticons.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvEmoticons.GridLines = true;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.ToolTipText = ";)";
            listViewItem2.ToolTipText = ":)";
            listViewItem3.ToolTipText = "D\':";
            listViewItem4.ToolTipText = ":(";
            listViewItem5.ToolTipText = ":\'(";
            listViewItem6.ToolTipText = ":l";
            listViewItem7.ToolTipText = ":P";
            listViewItem8.ToolTipText = ":laugh:";
            listViewItem9.ToolTipText = ":D";
            listViewItem10.ToolTipText = ":S";
            listViewItem11.ToolTipText = ":angry:";
            this.lvEmoticons.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11});
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
            this.txtOut.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatKeyDown);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(453, 469);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 51);
            this.btnSend.TabIndex = 19;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
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
            this.txtIn.TextChanged += new System.EventHandler(this.txtIn_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.panel1.Controls.Add(this.pbStatus);
            this.panel1.Controls.Add(this.lblUsername);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pbClose);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(813, 48);
            this.panel1.TabIndex = 26;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTitleBar_MouseDown);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Museo Sans 500", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(249)))), ((int)(((byte)(183)))));
            this.lblUsername.Location = new System.Drawing.Point(328, 13);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(100, 23);
            this.lblUsername.TabIndex = 20;
            this.lblUsername.Text = "Username";
            // 
            // buzzTimer
            // 
            this.buzzTimer.Interval = 10;
            this.buzzTimer.Tick += new System.EventHandler(this.buzzTimer_Tick);
            // 
            // statusImageList
            // 
            this.statusImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("statusImageList.ImageStream")));
            this.statusImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.statusImageList.Images.SetKeyName(0, "status-offline.png");
            this.statusImageList.Images.SetKeyName(1, "status-online.png");
            this.statusImageList.Images.SetKeyName(2, "status-busy.png");
            this.statusImageList.Images.SetKeyName(3, "status-away.png");
            // 
            // pbWebCamOut
            // 
            this.pbWebCamOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.pbWebCamOut.Location = new System.Drawing.Point(541, 297);
            this.pbWebCamOut.Name = "pbWebCamOut";
            this.pbWebCamOut.Size = new System.Drawing.Size(260, 223);
            this.pbWebCamOut.TabIndex = 28;
            this.pbWebCamOut.TabStop = false;
            // 
            // pbWebCamIn
            // 
            this.pbWebCamIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.pbWebCamIn.Location = new System.Drawing.Point(540, 74);
            this.pbWebCamIn.Name = "pbWebCamIn";
            this.pbWebCamIn.Size = new System.Drawing.Size(260, 208);
            this.pbWebCamIn.TabIndex = 27;
            this.pbWebCamIn.TabStop = false;
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
            this.pbBuzzer.Click += new System.EventHandler(this.pbBuzzer_Click);
            this.pbBuzzer.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbBuzzer.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
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
            this.pbFile.Click += new System.EventHandler(this.pbFile_Click);
            this.pbFile.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbFile.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
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
            this.pbEmoticons.Click += new System.EventHandler(this.pbEmoticons_Click);
            this.pbEmoticons.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbEmoticons.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            // 
            // pbStatus
            // 
            this.pbStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbStatus.Image = global::Client_Forms.Properties.Resources.status_online;
            this.pbStatus.Location = new System.Drawing.Point(311, 17);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(15, 15);
            this.pbStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbStatus.TabIndex = 21;
            this.pbStatus.TabStop = false;
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
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbClose.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            // 
            // pbVideo
            // 
            this.pbVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbVideo.Image = global::Client_Forms.Properties.Resources.icon_video;
            this.pbVideo.Location = new System.Drawing.Point(541, 529);
            this.pbVideo.Name = "pbVideo";
            this.pbVideo.Size = new System.Drawing.Size(40, 40);
            this.pbVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbVideo.TabIndex = 29;
            this.pbVideo.TabStop = false;
            this.pbVideo.Click += new System.EventHandler(this.pbVideo_Click);
            this.pbVideo.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbVideo.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            // 
            // pbGame
            // 
            this.pbGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbGame.Image = global::Client_Forms.Properties.Resources.icon_game;
            this.pbGame.Location = new System.Drawing.Point(146, 526);
            this.pbGame.Name = "pbGame";
            this.pbGame.Size = new System.Drawing.Size(40, 40);
            this.pbGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbGame.TabIndex = 30;
            this.pbGame.TabStop = false;
            this.pbGame.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbGame.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            // 
            // PrivateChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(42)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(809, 581);
            this.ControlBox = false;
            this.Controls.Add(this.pbGame);
            this.Controls.Add(this.pbVideo);
            this.Controls.Add(this.pbWebCamOut);
            this.Controls.Add(this.pbWebCamIn);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebCamOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebCamIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuzzer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmoticons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).EndInit();
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
        private System.Windows.Forms.PictureBox pbWebCamIn;
        private System.Windows.Forms.PictureBox pbWebCamOut;
        private System.Windows.Forms.ImageList emoticonImageList;
        private System.Windows.Forms.Timer buzzTimer;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.PictureBox pbStatus;
        private System.Windows.Forms.ImageList statusImageList;
        private System.Windows.Forms.PictureBox pbVideo;
        private System.Windows.Forms.PictureBox pbGame;
    }
}