using System;
using System.Linq;

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

            var alphabet = Alphabet;
            int counterI = 0;
            int counterJ = 0;

            foreach (var c in key)
            {
                if (alphabet.Contains(c))
                {
                    result[counterI][counterJ] = c;
                    alphabet = alphabet.Remove(alphabet.IndexOf(c), 1);

                    Increment(ref counterI, ref counterJ);
                }
            }

            foreach (var c in alphabet)
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

        private static char IntToChar(int id, char[][] alphabetTable)
        {
            Console.WriteLine(id);
            int i = (id / 10) - 1;
            int j = (id % 10) - 1;

            if (i < 5 && i >= 0
                      && j < 5 && j >= 0)
            {
                return alphabetTable[i][j];
            }
            
            Console.WriteLine("Shit's fucked man");
            Console.WriteLine("Nonexistent id given to IntToChar converter " + id);
            return '?';
        }
        
        public static string Encrypt(string clearText, string cryptKey, string alphabetKey, out char[][] table, out int[][] result)
        {
            var alphabetTable = GetAlphabetTable(alphabetKey);
            table = new char[(int) Math.Ceiling((float) clearText.Length / cryptKey.Length + 1)][];

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
                        table[i][j] = char.MaxValue; //kind of null
                    }
                }
            }

            result = new int[table.Length][];

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
                        result[i][j] = 100;
                    }
                }
            }

            string encryptedText = "";

            for (int i = 1; i < result.Length; i++)
            {
                foreach (var j in result[i])
                {
                    if (j == 100)
                    {
                        continue;
                    }

                    if (j < 10)
                    {
                        encryptedText += "0";
                    }

                    encryptedText += j + " ";
                }
            }

            return encryptedText;
        }

        public static string Decrypt(string encryptedText, string cryptKey, string alphabetKey, out char[][] charGrid, out int[][] intTable)
        {
            var alphabetTable = GetAlphabetTable(alphabetKey);
            int[] intArray = encryptedText.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            intTable = new int[(int) Math.Ceiling((float) intArray.Length / cryptKey.Length + 1)][];

            for (int i = 0; i < intTable.Length; i++)
            {
                intTable[i] = new int[cryptKey.Length];
            }

            for (int i = 0; i < intTable[0].Length; i++)
            {
                intTable[0][i] = CharToInt(cryptKey[i], alphabetTable);
                Console.Write(intTable[0][i] + " ");
            }

            Console.WriteLine();
            charGrid = new char[intTable.Length][];

            for (int i = 1; i < intTable.Length; i++)
            {
                for (int j = 0; j < intTable[0].Length; j++)
                {
                    int arrId = (i - 1) * intTable[i].Length + j;

                    if (arrId < intArray.Length)
                    {
                        intTable[i][j] = intArray[arrId];
                    }
                    else
                    {
                        intTable[i][j] = 100;
                    }

                    Console.Write(intTable[i][j] + " ");
                }

                Console.WriteLine();
            }


            string clearText = "";

            for (int i = 0; i < intTable.Length; i++)
            {
                charGrid[i] = new char[intTable[i].Length];
                
                for (int j = 0; j < intTable[i].Length; j++)
                {
                    if (i == 0)
                    {
                        charGrid[i][j] = IntToChar(intTable[i][j], alphabetTable);
                    }
                    else
                    {
                        if (intTable[i][j] == 100)
                        {
                            continue;
                        }

                        int actualId = intTable[i][j] - intTable[0][j];

                        if (actualId <= 0)
                        {
                            actualId += 100;
                        }

                        clearText += IntToChar(actualId, alphabetTable);
                        charGrid[i][j] = IntToChar(actualId, alphabetTable);
                    }
                }
            }

            return clearText;
        }
    }
}