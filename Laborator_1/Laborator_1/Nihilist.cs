namespace Laborator_1
{
    public static class Nihilist
    {
        public static char[][] GetAlphabetTable(string key)
        {
            char[][] result = new char[5][];
            for (int i = 0; i < 5; i++)
            {
                result[i] = new char[5];
            }
            
            var _alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            int counterI = 0;
            int counterJ = 0;

            foreach (var c in key)
            {
                if (_alphabet.Contains(c))
                {
                    result[counterI][counterJ] = c;
                    _alphabet = _alphabet.Remove(_alphabet.IndexOf(c),1);
                    
                    Increment(ref counterI, ref counterJ);
                }
            }

            foreach (var c in _alphabet)
            {
                result[counterI][counterJ] = c;
                Increment(ref counterI, ref counterJ);
            }

            return result;
        }

        private static void Increment(ref int counterI, ref int counterJ)
        {
            counterJ++;
            counterI += counterJ % 5;
            counterJ /= 5;
        }
    }
}