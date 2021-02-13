using System;

namespace Laborator_1
{
    public static class Nihilist
    {
        private const string Alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        public static char[][] GetAlphabetTable(string key)
        {
            char[][] result = new char[5][];
            for (int i = 0; i < 5; i++)
            {
                result[i] = new char[5];
            }
            
            var _alphabet = Alphabet;
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
            counterI += counterJ / 5;
            counterJ %= 5;
        }

        public static int[][] GetEncryptTable(string clearText, string cryptKey, string alphabetKey)
        {
            var alphabetTable = GetAlphabetTable(alphabetKey);
            char[][] table = new char[(int) Math.Ceiling((float) clearText.Length / cryptKey.Length + 1)][];

            for (int i = 0; i < table.Length; i++)
            {
                table[i] = new char[cryptKey.Length];
            }

            for (int i = 0; i < cryptKey.Length; i++)
            {
                table[0][i] = cryptKey[i];
            }

            for (int i = 1; i < table.Length; i++)
            {
                for (int j = 0; j < table[i].Length; j++)
                {
                    int stringId = (i - 1) * table[i].Length + j;

                    if (stringId < clearText.Length)
                    {
                        table[i][j] = clearText[stringId];
                    }
                    else
                    {
                        table[i][j] = Char.MaxValue;    //kind of null
                    }
                }
            }
            
            int[][] result = new int[table.Length][];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new int[table[i].Length];
            }

            for (int i = 0; i < result[0].Length; i++)
            {
                result[0][i] = CharToInt(table[0][i], alphabetTable);
            }

            for (int i = 1; i < result.Length; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                {
                    if (table[i][j] != char.MaxValue)
                    {
                        result[i][j] = CharToInt(table[i][j], alphabetTable) +
                                       CharToInt(table[0][j], alphabetTable);
                        result[i][j] %= 100;
                    }
                    else
                    {
                        result[i][j] = 0;
                    }
                }
            }

            return result;
        }

        private static int CharToInt(char c, char[][] alphabetTable)
        {
            for (int i = 0; i < alphabetTable.Length; i++)
            {
                for (int j = 0; j < alphabetTable[i].Length; j++)
                {
                    if (c == alphabetTable[i][j])
                    {
                        return (i + 1) * 10 + (j + 1);
                    }
                }
            }
            
            Console.WriteLine("Shit's fucked man");
            Console.WriteLine("Nonexistent letter given to CharToInt converter " + c);
            return 0;
        }
    }
}