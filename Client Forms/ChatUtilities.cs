using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Forms {

    /* 
    Handles some utilities from the chat windows.
    - Loads the emotes
    - Handles the mutex object to not conflict with the clipboard
    */
    public class ChatUtilities {

        Form owner;
        Dictionary<string, Image> emoticons;
        Mutex mutex;

        public ChatUtilities( Form form ) {
            owner = form;

            try {
                mutex = Mutex.OpenExisting( "ChatMutex" );
            }
            catch {
                mutex = new Mutex( false, "ChatMutex" );
            }

            emoticons = new Dictionary<string, Image>();
            emoticons.Add( ":)", Properties.Resources.emote_smile );
            emoticons.Add( ":D", Properties.Resources.emote_happy );
            emoticons.Add( ":P", Properties.Resources.emote_P );
            emoticons.Add( ":angry:", Properties.Resources.emote_angry );
            emoticons.Add( ":S", Properties.Resources.emote_badfeeling );
            emoticons.Add( ":laugh:", Properties.Resources.emote_laugh );
            emoticons.Add( ":l", Properties.Resources.emote_pokerface );
            emoticons.Add( ":(", Properties.Resources.emote_sad2 );
            emoticons.Add( ":'(", Properties.Resources.emote_sad );
            emoticons.Add( "D':", Properties.Resources.emote_sad3 );
            emoticons.Add( ";)", Properties.Resources.emote_winkyface );


        }

        delegate void WriteDelegate( RichTextBox output, string text, Color? color = null );
        public void Write( RichTextBox output, string text, Color? color = null) {

            Color textColor = color ?? Color.White;

            if (!output.InvokeRequired) {

                output.AppendText( text, textColor );

                SetEmoticons( output );

            }
            else {
                WriteDelegate write = new WriteDelegate( Write );
                owner.Invoke( write, new object[] { output, text, textColor } );
            }

        }

        delegate void WriteLineDelegate( RichTextBox output, string text, Color? color = null );
        public void WriteLine( RichTextBox output, string text, Color? color = null ) {

            Color textColor = color ?? Color.White;

            if (!output.InvokeRequired) {

                output.AppendText( text + "\n", textColor );

                SetEmoticons( output );

            }
            else {
                WriteLineDelegate writeLine = new WriteLineDelegate( WriteLine );
                owner.Invoke( writeLine, new object[] { output, text, textColor } );
            }

        }
        
        public void SetEmoticonsBackColor( Color backColor ) {
            List<string> keys = new List<string>( emoticons.Keys );
            foreach (string key in keys) {

                Bitmap emote = new Bitmap( emoticons[key].Width, emoticons[key].Height );
                Graphics graphics = Graphics.FromImage( emote );
                graphics.Clear( backColor );
                graphics.DrawImage( emoticons[key], new Rectangle( Point.Empty, new Size( emote.Width, emote.Height ) ) );
                emoticons[key] = emote;
            }
        }

        void SetEmoticons( RichTextBox rtb ) {
            mutex.WaitOne();
            rtb.ReadOnly = false;
            IDataObject currentClipboard = Clipboard.GetDataObject();

            foreach (string key in emoticons.Keys) {

                Clipboard.Clear();
                Clipboard.SetImage( emoticons[key] );

                while (rtb.Text.Contains( key )) {

                    int index = rtb.Text.IndexOf( key );
                    rtb.Select( index, key.Length );
                    
                    rtb.Paste();
                }

            }
            Clipboard.SetDataObject( currentClipboard );
            rtb.ReadOnly = false;
            mutex.ReleaseMutex();
        }



    }

    public static class RichTextBoxExtensions {

        public static void AppendText(this RichTextBox box, string text, Color color){

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText( text );
            box.SelectionColor = box.ForeColor;

        }

    }

}
