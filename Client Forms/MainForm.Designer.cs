namespace Client_Forms {
    partial class MainForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem("Online", 1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem("Away", 2);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem("Busy", 3);
            this.cmsUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendPrivateMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtIn = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtOut = new System.Windows.Forms.RichTextBox();
            this.buzzTimer = new System.Windows.Forms.Timer(this.components);
            this.lvEmoticons = new System.Windows.Forms.ListView();
            this.emoticonImageList = new System.Windows.Forms.ImageList(this.components);
            this.tvRooms = new System.Windows.Forms.TreeView();
            this.cmsRooms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createRoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusImageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbStatus = new System.Windows.Forms.PictureBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.pbProfile = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.cmsRoom = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsRoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbBuzzer = new System.Windows.Forms.PictureBox();
            this.pbEmoticons = new System.Windows.Forms.PictureBox();
            this.lvStatus = new System.Windows.Forms.ListView();
            this.cmsUser.SuspendLayout();
            this.cmsRooms.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.cmsRoom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuzzer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmoticons)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsUser
            // 
            this.cmsUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendPrivateMessageToolStripMenuItem});
            this.cmsUser.Name = "cmsUser";
            this.cmsUser.Size = new System.Drawing.Size(137, 26);
            // 
            // sendPrivateMessageToolStripMenuItem
            // 
            this.sendPrivateMessageToolStripMenuItem.Name = "sendPrivateMessageToolStripMenuItem";
            this.sendPrivateMessageToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.sendPrivateMessageToolStripMenuItem.Text = "Private chat";
            this.sendPrivateMessageToolStripMenuItem.Click += new System.EventHandler(this.sendPrivateMessageToolStripMenuItem_Click);
            // 
            // txtIn
            // 
            this.txtIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.txtIn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIn.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIn.ForeColor = System.Drawing.Color.White;
            this.txtIn.Location = new System.Drawing.Point(200, 62);
            this.txtIn.Name = "txtIn";
            this.txtIn.ReadOnly = true;
            this.txtIn.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtIn.Size = new System.Drawing.Size(598, 391);
            this.txtIn.TabIndex = 0;
            this.txtIn.TabStop = false;
            this.txtIn.Text = "";
            this.txtIn.TextChanged += new System.EventHandler(this.txtIn_TextChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(723, 459);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 51);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtOut
            // 
            this.txtOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.txtOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOut.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOut.ForeColor = System.Drawing.Color.White;
            this.txtOut.Location = new System.Drawing.Point(200, 459);
            this.txtOut.Name = "txtOut";
            this.txtOut.Size = new System.Drawing.Size(517, 51);
            this.txtOut.TabIndex = 0;
            this.txtOut.Text = "";
            this.txtOut.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatKeyDown);
            // 
            // buzzTimer
            // 
            this.buzzTimer.Interval = 10;
            this.buzzTimer.Tick += new System.EventHandler(this.buzzTimer_Tick);
            // 
            // lvEmoticons
            // 
            this.lvEmoticons.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvEmoticons.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvEmoticons.BackColor = System.Drawing.Color.White;
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
            this.lvEmoticons.Location = new System.Drawing.Point(200, 415);
            this.lvEmoticons.MultiSelect = false;
            this.lvEmoticons.Name = "lvEmoticons";
            this.lvEmoticons.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lvEmoticons.Scrollable = false;
            this.lvEmoticons.ShowGroups = false;
            this.lvEmoticons.ShowItemToolTips = true;
            this.lvEmoticons.Size = new System.Drawing.Size(123, 95);
            this.lvEmoticons.SmallImageList = this.emoticonImageList;
            this.lvEmoticons.TabIndex = 6;
            this.lvEmoticons.TabStop = false;
            this.lvEmoticons.TileSize = new System.Drawing.Size(30, 30);
            this.lvEmoticons.UseCompatibleStateImageBehavior = false;
            this.lvEmoticons.View = System.Windows.Forms.View.Tile;
            this.lvEmoticons.Visible = false;
            this.lvEmoticons.SelectedIndexChanged += new System.EventHandler(this.lvEmoticons_SelectedIndexChanged);
            this.lvEmoticons.Leave += new System.EventHandler(this.lvEmoticons_Leave);
            this.lvEmoticons.MouseLeave += new System.EventHandler(this.lvEmoticons_MouseLeave);
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
            // tvRooms
            // 
            this.tvRooms.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.tvRooms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvRooms.ContextMenuStrip = this.cmsRooms;
            this.tvRooms.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvRooms.ForeColor = System.Drawing.Color.White;
            this.tvRooms.ImageIndex = 0;
            this.tvRooms.ImageList = this.statusImageList;
            this.tvRooms.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tvRooms.ItemHeight = 25;
            this.tvRooms.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(249)))), ((int)(((byte)(183)))));
            this.tvRooms.Location = new System.Drawing.Point(12, 62);
            this.tvRooms.Name = "tvRooms";
            this.tvRooms.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tvRooms.SelectedImageIndex = 0;
            this.tvRooms.ShowLines = false;
            this.tvRooms.ShowPlusMinus = false;
            this.tvRooms.Size = new System.Drawing.Size(182, 494);
            this.tvRooms.TabIndex = 7;
            this.tvRooms.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRooms_NodeMouseClick);
            // 
            // cmsRooms
            // 
            this.cmsRooms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createRoomToolStripMenuItem});
            this.cmsRooms.Name = "cmsRooms";
            this.cmsRooms.Size = new System.Drawing.Size(141, 26);
            // 
            // createRoomToolStripMenuItem
            // 
            this.createRoomToolStripMenuItem.Name = "createRoomToolStripMenuItem";
            this.createRoomToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.createRoomToolStripMenuItem.Text = "Create room";
            this.createRoomToolStripMenuItem.Click += new System.EventHandler(this.createRoomToolStripMenuItem_Click);
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
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.panel1.Controls.Add(this.pbStatus);
            this.panel1.Controls.Add(this.lblUsername);
            this.panel1.Controls.Add(this.pbProfile);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pbClose);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(813, 48);
            this.panel1.TabIndex = 16;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTitleBar_MouseDown);
            // 
            // pbStatus
            // 
            this.pbStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbStatus.Image = global::Client_Forms.Properties.Resources.status_online;
            this.pbStatus.Location = new System.Drawing.Point(312, 17);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(15, 15);
            this.pbStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbStatus.TabIndex = 17;
            this.pbStatus.TabStop = false;
            this.pbStatus.Click += new System.EventHandler(this.pbStatus_Click);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Museo Sans 500", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(249)))), ((int)(((byte)(183)))));
            this.lblUsername.Location = new System.Drawing.Point(328, 13);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(100, 23);
            this.lblUsername.TabIndex = 19;
            this.lblUsername.Text = "Username";
            // 
            // pbProfile
            // 
            this.pbProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbProfile.Image = global::Client_Forms.Properties.Resources.icon_profile;
            this.pbProfile.Location = new System.Drawing.Point(733, 9);
            this.pbProfile.Name = "pbProfile";
            this.pbProfile.Size = new System.Drawing.Size(30, 30);
            this.pbProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbProfile.TabIndex = 17;
            this.pbProfile.TabStop = false;
            this.pbProfile.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbProfile.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
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
            // cmsRoom
            // 
            this.cmsRoom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsRoomToolStripMenuItem});
            this.cmsRoom.Name = "cmsRoom";
            this.cmsRoom.Size = new System.Drawing.Size(131, 26);
            // 
            // cmsRoomToolStripMenuItem
            // 
            this.cmsRoomToolStripMenuItem.Name = "cmsRoomToolStripMenuItem";
            this.cmsRoomToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.cmsRoomToolStripMenuItem.Text = "Join Room";
            this.cmsRoomToolStripMenuItem.Click += new System.EventHandler(this.cmsRoomToolStripMenuItem_Click);
            // 
            // pbBuzzer
            // 
            this.pbBuzzer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbBuzzer.Image = global::Client_Forms.Properties.Resources.icon_buzz;
            this.pbBuzzer.Location = new System.Drawing.Point(245, 516);
            this.pbBuzzer.Name = "pbBuzzer";
            this.pbBuzzer.Size = new System.Drawing.Size(40, 40);
            this.pbBuzzer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBuzzer.TabIndex = 15;
            this.pbBuzzer.TabStop = false;
            this.pbBuzzer.Click += new System.EventHandler(this.pbBuzzer_Click);
            this.pbBuzzer.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbBuzzer.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            // 
            // pbEmoticons
            // 
            this.pbEmoticons.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEmoticons.Image = global::Client_Forms.Properties.Resources.icon_emoticons;
            this.pbEmoticons.Location = new System.Drawing.Point(199, 516);
            this.pbEmoticons.Name = "pbEmoticons";
            this.pbEmoticons.Size = new System.Drawing.Size(40, 40);
            this.pbEmoticons.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbEmoticons.TabIndex = 5;
            this.pbEmoticons.TabStop = false;
            this.pbEmoticons.Click += new System.EventHandler(this.pbEmoticons_Click);
            this.pbEmoticons.MouseEnter += new System.EventHandler(this.PictureBoxHover);
            this.pbEmoticons.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            // 
            // lvStatus
            // 
            this.lvStatus.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvStatus.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvStatus.BackColor = System.Drawing.Color.White;
            this.lvStatus.GridLines = true;
            this.lvStatus.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem12,
            listViewItem13,
            listViewItem14});
            this.lvStatus.LabelWrap = false;
            this.lvStatus.LargeImageList = this.statusImageList;
            this.lvStatus.Location = new System.Drawing.Point(311, 33);
            this.lvStatus.MultiSelect = false;
            this.lvStatus.Name = "lvStatus";
            this.lvStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lvStatus.Scrollable = false;
            this.lvStatus.ShowGroups = false;
            this.lvStatus.ShowItemToolTips = true;
            this.lvStatus.Size = new System.Drawing.Size(65, 65);
            this.lvStatus.TabIndex = 17;
            this.lvStatus.TabStop = false;
            this.lvStatus.TileSize = new System.Drawing.Size(50, 20);
            this.lvStatus.UseCompatibleStateImageBehavior = false;
            this.lvStatus.View = System.Windows.Forms.View.Tile;
            this.lvStatus.Visible = false;
            this.lvStatus.SelectedIndexChanged += new System.EventHandler(this.lvStatus_SelectedIndexChanged);
            this.lvStatus.Leave += new System.EventHandler(this.lvStatus_Leave);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(42)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(810, 569);
            this.Controls.Add(this.lvStatus);
            this.Controls.Add(this.pbBuzzer);
            this.Controls.Add(this.tvRooms);
            this.Controls.Add(this.lvEmoticons);
            this.Controls.Add(this.pbEmoticons);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtIn);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.cmsUser.ResumeLayout(false);
            this.cmsRooms.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.cmsRoom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbBuzzer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmoticons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtIn;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtOut;
        private System.Windows.Forms.Timer buzzTimer;
        private System.Windows.Forms.PictureBox pbEmoticons;
        private System.Windows.Forms.ListView lvEmoticons;
        private System.Windows.Forms.ImageList emoticonImageList;
        private System.Windows.Forms.TreeView tvRooms;
        private System.Windows.Forms.ContextMenuStrip cmsUser;
        private System.Windows.Forms.ToolStripMenuItem sendPrivateMessageToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsRooms;
        private System.Windows.Forms.ToolStripMenuItem createRoomToolStripMenuItem;
        private System.Windows.Forms.PictureBox pbBuzzer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbProfile;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.ContextMenuStrip cmsRoom;
        private System.Windows.Forms.ToolStripMenuItem cmsRoomToolStripMenuItem;
        private System.Windows.Forms.ImageList statusImageList;
        private System.Windows.Forms.PictureBox pbStatus;
        private System.Windows.Forms.ListView lvStatus;
    }
}

