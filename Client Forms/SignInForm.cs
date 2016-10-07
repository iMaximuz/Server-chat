using Server_Utilities;
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
    public partial class SignInForm : Form {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();

        MainForm owner;

        public SignInForm() {
            InitializeComponent();
        }

        private void SignInForm_Load( object sender, EventArgs e ) {
            owner = (MainForm)Owner;
        }


        private void btnSignIn_Click( object sender, EventArgs e ) {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string passConfirm = txtConfirmPasswore.Text;


            if ( !String.IsNullOrEmpty( username ) && !String.IsNullOrEmpty( password ) && !String.IsNullOrEmpty( passConfirm ) ) {
                if (password == passConfirm) {

                    Packet p = new Packet( PacketType.Client_SignIn, owner.client.ID );
                    p.data.Add( "username", username );
                    p.data.Add( "password", password );

                    owner.client.SendPacket( p );

                }
                else {
                    MessageBox.Show( "The passwords do not match.", "Sign In error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                }
            }
            else {
                MessageBox.Show( "Fields cannot be empty", "Sign In error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
            }

        }

        delegate void SuccessDelegate();
        public void Success() {
            if (!this.InvokeRequired) {
                MessageBox.Show( "Your account has been created! \n Please Log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information );
                this.Close( owner );
            }
            else {
                SuccessDelegate success = new SuccessDelegate(Success);
                this.Invoke( success );
            }
        }

        public void Fail() {
            MessageBox.Show( "That account already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
        }


        private void pbClose_Click( object sender, EventArgs e ) {
            this.Close();
        }

        private void PictureBoxHover( object sender, EventArgs e ) {
            ((PictureBox)sender).BackColor = ColorTranslator.FromHtml( "#035e7c" ); ;
        }

        private void PictureBoxLeave( object sender, EventArgs e ) {
            ((PictureBox)sender).BackColor = Color.Transparent;
        }

        private void pbTitleBar_MouseDown( object sender, MouseEventArgs e ) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage( Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0 );
            }
        }




    }
}
