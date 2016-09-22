using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Forms {
    public partial class ChatForm : Form {

        MainForm parent;

        public ChatForm() {
            InitializeComponent();

            parent = (MainForm)Owner;


        }

        public void SetText( string text ) {
            this.Text = text;

        }
        


        public void DispatchPachet() {

        }

    }

}
