using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Utilities {
    public class Encryption {
       public static string EncryptString(string s, string key) {
            string result = "";
            int index = 0;
            for (int i = 0; i < s.Length; i++) {
                char c = (char)(s[i] ^ key[index]);
                result += c;
                index++;
                if (index >= key.Length)
                    index = 0;
            }
            return result;
        }
    }
}
