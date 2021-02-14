using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Laborator_1
{
    public static class Myszkowski
    {
        public static int[] LetterMap(string keyword)
        {
            var keywordArr = keyword.ToUpper().ToCharArray();
            
            var result = new int[keyword.Length];
            for (var i = 0; i < keyword.Length; i++)
            {
                var minChar = char.MaxValue;
                var minCharIds = new List<int>();
                for (int j = 0; j < keyword.Length; j++)
                {
                    var key = keywordArr[j];
                    if (key < minChar)
                    {
                        minChar = key;
                        minCharIds.Clear();
                        minCharIds.Add(j);
                    }
                    else if (key == minChar)
                    {
                        minCharIds.Add(j);
                    }
                }

                foreach (var minCharId in minCharIds)
                {
                    result[minCharId] = i;
                    keywordArr[minCharId] = char.MaxValue;
                }


                var skip = true;
                foreach (var c in keywordArr)
                {
                    if (c != char.MaxValue)
                    {
                        skip = false;
                    }
                }

                if (skip)
                {
                    break;
                }
            }

            return result;
        }

        public static char[][] EncryptTable(string clearText, string key)
        {
            char[][] result = new char[(clearText.Length / key.Length) + 1][];
            Random random = new Random();
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new char[key.Length];
                for (int j = 0; j < result[i].Length; j++)
                {
                    int nextId = i * key.Length + j;
                    if (nextId < clearText.Length)
                    {
                        result[i][j] = clearText[i * key.Length + j];
                    }
                    else
                    {
                        result[i][j] = (char)('A' + random.Next(26));
                    }
                }
            }

            // foreach (var cArr in result)
            // {
            //     foreach (var c in cArr)
            //     {
            //         Console.Write(c);
            //     }
            //     Console.WriteLine();
            // }
            // Console.WriteLine();

            return result;
        }

        public static char[][] DecryptTable(string encryptedText, string key)
        {
            char[][] result = new char[(int)Math.Ceiling((float)encryptedText.Length / key.Length)][];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new char[key.Length];
            }
            
            var colPos = LetterMap(key);
            int counter = 0;
            

            for (int i = 0; i <= colPos.Max(); i++)
            {
                for (int j = 0; j < result.Length; j++)
                {
                    for (int k = 0; k < result[j].Length; k++)
                    {
                        if (colPos[k] != i)
                        {
                            continue;
                        }

                        result[j][k] = counter < encryptedText.Length ? encryptedText[counter] : ' ';
                        counter++;
                    }
                }
            }

            return result;
        }
        
        
        
        public static string Encrypt(string clearText, string key)
        {
            var table = EncryptTable(clearText, key);
            var result = "";
            var colPos = LetterMap(key);
            

            for (int i = 0; i <= colPos.Max(); i++)
            {
                for (int j = 0; j < table.Length; j++)
                {
                    for (int k = 0; k < table[j].Length; k++)
                    {
                        if (colPos[k] != i || table[j][k] == ' ')
                        {
                            continue;
                        }

                        result += table[j][k];
                    }
                }
            }
            
            return result;
        }

        public static string Decrypt(string encryptedText, string key)
        {
            var result = "";
            foreach (var row in DecryptTable(encryptedText, key))
            {
                result += new string(row);
            }

            return result;
        }

    }
}