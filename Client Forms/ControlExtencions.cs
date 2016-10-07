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

        delegate void ChangeTextDelegate( Label label, Form invoker, string text );
        public static void ChangeText( this Label label, Form invoker, string text ) {

            if (!label.InvokeRequired) {
                label.Text = text;
            }
            else {
                ChangeTextDelegate changeText = new ChangeTextDelegate( ChangeText );
                invoker.Invoke( changeText, new object[] { label, invoker, text } );
            }

        }

        delegate void CloseDelegate( Form form, Form invoker );
        public static void Close(this Form form, Form invoker ) {
            if (!form.InvokeRequired) {
                form.Close();
            }
            else {
                CloseDelegate close = new CloseDelegate( Close );
                invoker.Invoke( close, new object[] { form, invoker } );
            }
        }

        delegate void ShowDelegate( Form form, Form parent );
        public static void ShowSafe(this Form form, Form parent ){

            if (!form.InvokeRequired) {
                ShowDelegate show = new ShowDelegate( ShowForm );
                parent.Invoke( show, new object[] { form, parent } );
            }
            else {
                ShowDelegate show = new ShowDelegate( ShowSafe );
                parent.Invoke( show, new object[] { form, parent } );
            }

        }

        static void ShowForm(Form form, Form parent) {
            form.Show( parent );
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
                rtb.ReadOnly = true;
                Emotes.mutex.ReleaseMutex();
            }

        }

    }
}
