﻿using Server_Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Client_Forms {

    public partial class LoginForm : Form {


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();

        MainForm owner;

        public SignInForm signIn;

        private void LoginForm_Load( object sender, EventArgs e ) {
            owner = (MainForm)Owner;
            //DialogResult = DialogResult.OK;
        }

        public LoginForm() {
            InitializeComponent();
        }

        private void pbClose_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnLogIn_Click( object sender, EventArgs e ) {

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (!String.IsNullOrEmpty( username ) && !String.IsNullOrEmpty( password )) {
                Packet p = new Packet( PacketType.Client_LogIn, owner.client.ID );

                p.data.Add( "username", username );
                p.data.Add( "password", password );

                owner.client.SendPacket( p );
            }
            else {
                MessageBox.Show( "Fields cannot be empty", "Sign In error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
            }

        }

        delegate void FailDelegate();
        public void Fail() {
            if (!this.InvokeRequired) {
                MessageBox.Show( "Incorrect Username or Password", "Log In error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );

            }
            else {
                FailDelegate fail = new FailDelegate( Fail );
                this.Invoke( fail );
            }
        }
        delegate void SuccessDelegate();
        public void Success() {
             if (!this.InvokeRequired) {
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else {
                SuccessDelegate success = new SuccessDelegate( Success );
                this.Invoke( success );
            }
            
        }

        private void pbTitleBar_MouseDown( object sender, MouseEventArgs e ) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage( Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0 );
            }
        }

        private void PictureBoxHover( object sender, EventArgs e ) {
            ((PictureBox)sender).BackColor = ColorTranslator.FromHtml( "#035e7c" ); ;
        }

        private void PictureBoxLeave( object sender, EventArgs e ) {

            ((PictureBox)sender).BackColor = Color.Transparent;

        }

        private void btnSignIn_Click( object sender, EventArgs e ) {
            signIn = new SignInForm();
            signIn.ShowDialog( owner );
        }



    }
}
