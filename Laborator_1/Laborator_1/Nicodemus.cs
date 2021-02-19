using System;
using System.Collections.Generic;
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
                    secondTable[i][j] = Vigenere.Encrypt(secondTable[i][j], secondTable[0][j]);
                }
            }
            
            string result = "";

            for (int j = 0; j < secondTable[0].Length; j++)
            {
                
                for (int i = 1; i < secondTable.Length; i++)
                {
                    if (secondTable[i][j] != ' ')
                    {
                        result += secondTable[i][j];
                    }
                }
            }

            return result;
        }

        public static string Decrypt(string encryptedText, string key)
        {
            return string.Empty;
        }
    }
}