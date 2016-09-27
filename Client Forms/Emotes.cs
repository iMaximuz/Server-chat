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
    public static class Emotes {
        public static bool isInitialized = false;
        public static Dictionary<string, Image> emoticons;
        public static Mutex mutex;

        public static void LoadEmotes( Color backColor ) {

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

            List<string> keys = new List<string>( emoticons.Keys );
            foreach (string key in keys) {

                Bitmap emote = new Bitmap( emoticons[key].Width, emoticons[key].Height );
                Graphics graphics = Graphics.FromImage( emote );
                graphics.Clear( backColor );
                graphics.DrawImage( emoticons[key], new Rectangle( Point.Empty, new Size( emote.Width, emote.Height ) ) );
                emoticons[key] = emote;
            }

            isInitialized = true;

        }

    }
}
