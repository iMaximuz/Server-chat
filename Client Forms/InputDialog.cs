﻿using System;
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
    public partial class InputDialog : Form {


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();


        public InputDialog() {
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
            if (!String.IsNullOrEmpty( txtName.Text )) {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
                txtName.Focus();
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
            return txtName.Text;
        }

    }
}
