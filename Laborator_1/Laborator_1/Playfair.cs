using System;

namespace Laborator_1
{
    public class Playfair
    {
        private const string Alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

        public static char[][] GetAlphabetTable(string key, string alphabet = Alphabet, int x = 5, int y = 5)
        {
            char[][] result = new char[x][];
            for (int i = 0; i < 5; i++)
            {
                result[i] = new char[y];
            }

            int counterI = 0;
            int counterJ = 0;

            foreach (var c in key)
            {
                if (alphabet.Contains(c))
                {
                    result[counterI][counterJ] = c;
                    alphabet = alphabet.Remove(alphabet.IndexOf(c), 1);

                    Increment(ref counterI, ref counterJ, x, y);
                }
            }

            foreach (var c in alphabet)
            {
                result[counterI][counterJ] = c;
                Increment(ref counterI, ref counterJ, x, y);
            }

            return result;
        }

        private static void Increment(ref int counterI, ref int counterJ, int x, int y)
        {
            counterJ++;
            counterI += counterJ / 5;
            counterJ %= 5;
        }

        private static string EncryptBigram(string bigram, char[][] alphabet)
        {
            CharToInt(bigram[0], alphabet, out var firstX, out var firstY);
            CharToInt(bigram[1], alphabet, out var secondX, out var secondY);

            if (firstX == secondX        //error
                && firstY == secondY)
            {
                Console.WriteLine("Shit's fucked man");
                Console.WriteLine("Equal bigram given to EncryptBigram: " + bigram);
                return string.Empty;
            }

            if (firstX == secondX         //same row
                && firstY != secondY)
            {
                var newFirstY = (firstY + 1) % alphabet.Length;
                var newSecondY = (secondY + 1) % alphabet.Length;

                return "" + alphabet[firstX][newFirstY] + alphabet[secondX][newSecondY];
            }

            if (firstX != secondX        //same column
                && firstY == secondY)
            {
                var newFirstX = (firstX + 1) % alphabet[0].Length;
                var newSecondX = (secondX + 1) % alphabet[0].Length;

                return "" + alphabet[newFirstX][firstY] + alphabet[newSecondX][secondY];
            }
            
            if (firstX != secondX        //diff row/col
                && firstY != secondY)
            {
                return "" + alphabet[firstX][secondY] + alphabet[secondX][firstY];
            }
            
            Console.WriteLine("Shit's fucked man");
            Console.WriteLine("Not expected case: " + bigram);
            return string.Empty;
        }

        private static void CharToInt(char c, char[][] alphabet, out int x, out int y)
        {
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (alphabet[i][j] == c)
                    {
                        x = i;
                        y = j;
                        return;
                    }
                }
            }

            Console.WriteLine("Nonexistent char given at CharToInt: " + c);
            x = 0;
            y = 0;
        }
    }
}