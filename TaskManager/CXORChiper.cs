namespace TaskManager
{
    public static class CXORChiper
    {
        private static string GetRepeatKey(string s, int n)
        {
            string r = s;

            while (r.Length < n)
                r += r;

            return r.Substring(0, n);
        }

        private static string Cipher(string text, string secretKey)
        {
            string currentKey = GetRepeatKey(secretKey, text.Length);

            string res = string.Empty;

            for (var i = 0; i < text.Length; i++)
                res += ((char)(text[i] ^ currentKey[i])).ToString();

            return res;
        }

        public static string Encrypt(string plainText, string password)
        => Cipher(plainText, password);

        public static string Decrypt(string encryptedText, string password)
        => Cipher(encryptedText, password);
    }
}
