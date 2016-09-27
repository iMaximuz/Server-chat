using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Forms {
    public static class ControlExtencions {

        public static void AppendText( this RichTextBox box, string text, Color color ) {

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText( text );
            box.SelectionColor = box.ForeColor;

        }

        delegate void WriteDelegate( RichTextBox output, Form invoker, string text, Color? color = null );
        public static void Write( this RichTextBox output, Form invoker, string text, Color? color = null ) {

            Color textColor = color ?? Color.White;

            if (!output.InvokeRequired) {

                output.AppendText( text, textColor );

                output.SetEmoticons();

            }
            else {
                WriteDelegate write = new WriteDelegate( Write );
                invoker.Invoke( write, new object[] { output, invoker, text, textColor } );
            }

        }

        delegate void WriteLineDelegate( RichTextBox output, Form invoker, string text, Color? color = null );
        public static void WriteLine( this RichTextBox output, Form invoker, string text, Color? color = null ) {

            Color textColor = color ?? Color.White;

            if (!output.InvokeRequired) {

                output.AppendText( text + '\n', textColor );

                output.SetEmoticons();

            }
            else {
                WriteLineDelegate write = new WriteLineDelegate( WriteLine );
                invoker.Invoke( write, new object[] { output, invoker, text, textColor } );
            }

        }

        static void SetEmoticons( this RichTextBox rtb ) {
            if (Emotes.isInitialized) {
                Emotes.mutex.WaitOne();
                rtb.ReadOnly = false;
                IDataObject currentClipboard = Clipboard.GetDataObject();

                foreach (string key in Emotes.emoticons.Keys) {

                    Clipboard.Clear();
                    Clipboard.SetImage( Emotes.emoticons[key] );

                    while (rtb.Text.Contains( key )) {

                        int index = rtb.Text.IndexOf( key );
                        rtb.Select( index, key.Length );

                        rtb.Paste();
                    }

                }
                Clipboard.SetDataObject( currentClipboard );
                rtb.ReadOnly = false;
                Emotes.mutex.ReleaseMutex();
            }

        }

    }
}
