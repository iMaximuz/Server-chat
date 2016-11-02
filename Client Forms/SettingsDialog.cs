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
    public partial class SettingsDialog : Form {


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();


        public SettingsDialog() {
            InitializeComponent();

        }

        private void SettingsDialog_Load( object sender, EventArgs e ) {
            cbEncrypt.Checked = Properties.Settings.Default.EncryptMessages;
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


        private void pbClose_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click( object sender, EventArgs e ) {
            DialogResult = DialogResult.Cancel;
            Properties.Settings.Default.EncryptMessages = cbEncrypt.Checked;
            Properties.Settings.Default.Save();
            Close();
        }
    }
}
