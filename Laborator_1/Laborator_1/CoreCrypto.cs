using System;
using System.Collections.Generic;
using System.Linq;

namespace Laborator_1
{
    public static class CoreCrypto
    {
        public static int[] MyszkowskiLetterMap(string keyword)
        {
            var keywordArr = keyword.ToUpper().ToCharArray();
            Dictionary<char, int> usedKeys = new Dictionary<char, int>();
            
            var result = new int[keyword.Length];
            for (int i = 0; i < keyword.Length; i++)
            {
                var minChar = char.MaxValue;
                var minCharId = 0;
                for (int j = 0; j < keyword.Length; j++)
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
        
        
    }
}