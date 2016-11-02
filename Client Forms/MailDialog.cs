using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Forms {
    public partial class MailDialog : Form {


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();


        public MailDialog() {
            InitializeComponent();
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

        private void btnOK_Click( object sender, EventArgs e ) {
            if (!String.IsNullOrEmpty( txtTo.Text ) || !String.IsNullOrEmpty( txtSubject.Text ) || !String.IsNullOrEmpty( txtMessage.Text )) {
                try {
                    DialogResult = DialogResult.OK;
                    SmtpClient smtpServer = new SmtpClient( "smtp.gmail.com" );

                    string From = "sharkcommunitymail@gmail.com";
                    string To = txtTo.Text;

                    MailMessage mail = new MailMessage( From, To );

                    MainForm owner = (MainForm)this.Owner;

                    mail.Subject = "Mail from: " + owner.client.sesionInfo.username + " | " + txtSubject.Text;
                    mail.Body =  txtMessage.Text + "\n\n ----- This mail was sent from the Shark desktop application. Please do not reply. -----";

                    smtpServer.Port = 587;
                    smtpServer.Credentials = new System.Net.NetworkCredential( "sharkcommunitymail@gmail.com", "laxx19954789" );
                    smtpServer.EnableSsl = true;
                    smtpServer.Send( mail );


                    Close();
                }
                catch(FormatException) {
                    MessageBox.Show( "That is not a valid e-mail.", "Invalid E-mail", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                }
            }
            else
                MessageBox.Show( "Can not be empty fields.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning );
        }

        private void btnCancel_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void pbClose_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public string GetValue() {
            return txtTo.Text;
        }

    }
}
