namespace Laborator_1
{
    public static class Vigenere
    {
        public static string Encrypt(string clearText, string key)
        {
            var result = "";
            var offset = new int[key.Length];

            for (var i = 0; i < key.Length; i++)
            {
                offset[i] = CharToInt(key[i]);
            }

            for (var i = 0; i < clearText.Length; i++)
            {
                result +=  (char) ('A' + ((CharToInt(clearText[i]) + offset[i % key.Length]) % 26));
            }

            return result;
        }

        private static int CharToInt(char c)
        {
            return c - 'A';
        }

        public static string Decrypt(string encryptedText, string key)
        {
            var result = "";
            var offset = new int[key.Length];
            
            for (var i = 0; i < key.Length; i++)
            {
                offset[i] = CharToInt(key[i]);
            }
            
            for (var i = 0; i < encryptedText.Length; i++)
            {
                if (encryptedText[i] != ' ')
                {
                    result +=  (char) ('A' + (26 + CharToInt(encryptedText[i]) - offset[i % key.Length]) % 26);
                }
                else
                {
                    result += encryptedText[i];
                }
            }

            return result;
        }

        public static char Encrypt(char clearText, char key)
        {
            return (char) ('A' + (CharToInt(clearText) + CharToInt(key)) % 26);
        }
        
        public static char Decrypt(char encryptedText, char key)
        {
            return (char) ('A' + (26 + CharToInt(encryptedText) - CharToInt(key)) % 26);
        }
    }
}