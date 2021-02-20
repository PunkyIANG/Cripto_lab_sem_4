using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;

namespace Laborator_1
{
    public static class Nicodemus
    {
        public static int[] LetterMap(string keyword)
        {
            var keywordArr = keyword.ToUpper().ToCharArray();

            var result = new int[keyword.Length];
            for (var i = 0; i < keyword.Length; i++)
            {
                var minChar = char.MaxValue;
                var minCharId = int.MaxValue;
                for (var j = 0; j < keyword.Length; j++)
                {
                    var key = keywordArr[j];
                    if (key < minChar)
                    {
                        minChar = key;
                        minCharId = j;
                    }
                }

                result[minCharId] = i;
                keywordArr[minCharId] = char.MaxValue;
            }

            return result;
        }

        public static string Encrypt(string clearText, string key)
        {
            char[][] firstTable = new char[(int) Math.Ceiling((float) clearText.Length / key.Length + 1)][];

            for (int i = 0; i < firstTable.Length; i++)
            {
                firstTable[i] = new char[key.Length];
            }

            for (int j = 0; j < firstTable[0].Length; j++)
            {
                firstTable[0][j] = key[j];
            }

            for (int i = 1; i < firstTable.Length; i++)
            {
                for (int j = 0; j < firstTable[i].Length; j++)
                {
                    int tempId = (i - 1) * firstTable[i].Length + j;

                    if (tempId < clearText.Length)
                    {
                        firstTable[i][j] = clearText[tempId];
                    }
                    else
                    {
                        firstTable[i][j] = ' ';
                    }
                }
            }

            char[][] secondTable = new char[firstTable.Length][];
            int[] shuffle = LetterMap(key);

            for (int i = 0; i < firstTable.Length; i++)
            {
                secondTable[i] = new char[firstTable[i].Length];

                for (int j = 0; j < firstTable[i].Length; j++)
                {
                    secondTable[i][shuffle[j]] = firstTable[i][j];
                }
            }


            for (int i = 1; i < secondTable.Length; i++)
            {
                for (int j = 0; j < secondTable[i].Length; j++)
                {
                    if (secondTable[i][j] != ' ')
                        secondTable[i][j] = Vigenere.Encrypt(secondTable[i][j], secondTable[0][j]);
                }
            }

            string result = "";

            for (int multipleOfFive = 0;
                multipleOfFive < (int) Math.Ceiling((float) secondTable.Length / 5);
                multipleOfFive++)
            {
                for (int j = 0; j < secondTable[0].Length; j++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int x = i + multipleOfFive * 5 + 1;
                        if (x < secondTable.Length
                            && secondTable[x][j] != ' ')
                        {
                            result += secondTable[x][j];
                        }
                    }
                }
            }

            return result;
        }

        public static string Decrypt(string encryptedText, string key)
        {
            var secondTable = new char[(int) Math.Ceiling((float) encryptedText.Length / key.Length + 1)][];
            var invertedShuffle = Invert(LetterMap(key));


            for (int i = 0; i < secondTable.Length; i++)
            {
                secondTable[i] = new char[key.Length];
            }

            int block = 0;

            var sortedKey = new string (key.OrderBy(c => c).ToArray());

            for (int i = 0; i < secondTable[0].Length; i++)
            {
                secondTable[0][i] = sortedKey[i];
            }

            while (encryptedText.Length != 0)
            {
                if (encryptedText.Length >= key.Length * 5)
                {
                    //chop the entire block
                    
                    for (int j = 0; j < secondTable[0].Length; j++)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            //Console.Write(encryptedText[0]);
                            secondTable[block * 5 + i + 1][j] = Vigenere.Decrypt(encryptedText[0], secondTable[0][j]);
                            encryptedText = encryptedText.Remove(0, 1);
                        }
                    }

                    block++;
                }
                else
                {
                    //fiddle with the thing
                    
                    int fullRows = encryptedText.Length / key.Length;
                    int lastRowCount = encryptedText.Length - (fullRows * key.Length);
                    //Console.Write(fullRows + " ");
                    //Console.Write(lastRowCount + " ");
                    foreach (var i in invertedShuffle)
                    {
                        //Console.Write(i);
                    }
                    //Console.WriteLine();
                    
                    for (int j = 0; j < secondTable[0].Length; j++)
                    {
                        for (int i = 0; i <= fullRows; i++)
                        {
                            if (i < fullRows)
                            {
                                //Console.Write(encryptedText[0]);
                                secondTable[block * 5 + i + 1][j] = Vigenere.Decrypt(encryptedText[0], secondTable[0][j]);
                                encryptedText = encryptedText.Remove(0, 1);
                            }
                            else if (lastRowCount > 0)
                            {
                                if (invertedShuffle[j] < lastRowCount)
                                {
                                    //Console.Write(encryptedText[0]);
                                    secondTable[block * 5 + i + 1][j] = Vigenere.Decrypt(encryptedText[0], secondTable[0][j]);
                                    encryptedText = encryptedText.Remove(0, 1);
                                }
                                else
                                {
                                    //Console.Write(' ');
                                    secondTable[block * 5 + i + 1][j] = ' ';
                                }
                            }
                        }
                    }
                }
            }
            //Console.WriteLine();

            foreach (var row in secondTable)
            {
                foreach (var c in row)
                {
                    //Console.Write(c);
                }
                
                //Console.WriteLine();
            }
            //Console.WriteLine();
            
            char[][] firstTable = new char[secondTable.Length][];

            for (int i = 0; i < firstTable.Length; i++)
            {
                firstTable[i] = new char[secondTable[i].Length];
            }

            for (int i = 0; i < firstTable.Length; i++)
            {
                for (int j = 0; j < firstTable[i].Length; j++)
                {
                    firstTable[i][invertedShuffle[j]] = secondTable[i][j];
                }
            }

            string result = "";


            for (int i = 1; i < firstTable.Length; i++)
            {
                for (int j = 0; j < firstTable[i].Length; j++)
                {
                    result += firstTable[i][j];
                }
            }
            
            return result;
        }
        
        static int[] Invert(int[] arr)
        {
            int[] result = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                result[arr[i]] = i;
            }

            return result;
        }


    }
}