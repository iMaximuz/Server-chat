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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Usuario 1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Room 01", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Room 02");
            this.cmsUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendPrivateMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtIn = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtOut = new System.Windows.Forms.RichTextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.buzzTimer = new System.Windows.Forms.Timer(this.components);
            this.lvEmoticons = new System.Windows.Forms.ListView();
            this.emoticonImageList = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.cmsRooms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createRoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbEmoticons = new System.Windows.Forms.PictureBox();
            this.pbTitleBar = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbProfile = new System.Windows.Forms.PictureBox();
            this.cmsUser.SuspendLayout();
            this.cmsRooms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmoticons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitleBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfile)).BeginInit();
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
            // 
            // txtIn
            // 
            this.txtIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.txtIn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIn.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIn.ForeColor = System.Drawing.Color.White;
            this.txtIn.Location = new System.Drawing.Point(200, 48);
            this.txtIn.Name = "txtIn";
            this.txtIn.ReadOnly = true;
            this.txtIn.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtIn.Size = new System.Drawing.Size(598, 405);
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
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(39, 487);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(256, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(182, 20);
            this.txtName.TabIndex = 4;
            this.txtName.Text = "Usuario";
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
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(17)))), ((int)(((byte)(26)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.ContextMenuStrip = this.cmsRooms;
            this.treeView1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.emoticonImageList;
            this.treeView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeView1.ItemHeight = 25;
            this.treeView1.Location = new System.Drawing.Point(12, 48);
            this.treeView1.Name = "treeView1";
            treeNode1.ContextMenuStrip = this.cmsUser;
            treeNode1.Name = "Node2";
            treeNode1.StateImageIndex = 1;
            treeNode1.Text = "Usuario 1";
            treeNode2.Name = "Node0";
            treeNode2.Text = "Room 01";
            treeNode3.Name = "Node1";
            treeNode3.Text = "Room 02";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3});
            this.treeView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowLines = false;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(182, 405);
            this.treeView1.StateImageList = this.emoticonImageList;
            this.treeView1.TabIndex = 7;
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
            // 
            // pbClose
            // 
            this.pbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbClose.Image = global::Client_Forms.Properties.Resources.icon_logoff;
            this.pbClose.Location = new System.Drawing.Point(775, 5);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(30, 30);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbClose.TabIndex = 10;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            this.pbClose.MouseHover += new System.EventHandler(this.PictureBoxHover);
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
            this.pbEmoticons.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            this.pbEmoticons.MouseHover += new System.EventHandler(this.PictureBoxHover);
            // 
            // pbTitleBar
            // 
            this.pbTitleBar.Location = new System.Drawing.Point(-6, -6);
            this.pbTitleBar.Name = "pbTitleBar";
            this.pbTitleBar.Size = new System.Drawing.Size(824, 48);
            this.pbTitleBar.TabIndex = 11;
            this.pbTitleBar.TabStop = false;
            this.pbTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTitleBar_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Client_Forms.Properties.Resources.icon_shark_logo_1;
            this.pictureBox1.Location = new System.Drawing.Point(8, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // pbProfile
            // 
            this.pbProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbProfile.Image = global::Client_Forms.Properties.Resources.icon_profile;
            this.pbProfile.Location = new System.Drawing.Point(737, 5);
            this.pbProfile.Name = "pbProfile";
            this.pbProfile.Size = new System.Drawing.Size(30, 30);
            this.pbProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbProfile.TabIndex = 13;
            this.pbProfile.TabStop = false;
            this.pbProfile.MouseLeave += new System.EventHandler(this.PictureBoxLeave);
            this.pbProfile.MouseHover += new System.EventHandler(this.PictureBoxHover);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(42)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(810, 569);
            this.Controls.Add(this.pbProfile);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pbClose);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.lvEmoticons);
            this.Controls.Add(this.pbEmoticons);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtIn);
            this.Controls.Add(this.pbTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.cmsUser.ResumeLayout(false);
            this.cmsRooms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmoticons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitleBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtIn;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtOut;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Timer buzzTimer;
        private System.Windows.Forms.PictureBox pbEmoticons;
        private System.Windows.Forms.ListView lvEmoticons;
        private System.Windows.Forms.ImageList emoticonImageList;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip cmsUser;
        private System.Windows.Forms.ToolStripMenuItem sendPrivateMessageToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsRooms;
        private System.Windows.Forms.ToolStripMenuItem createRoomToolStripMenuItem;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbTitleBar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbProfile;
    }
}

