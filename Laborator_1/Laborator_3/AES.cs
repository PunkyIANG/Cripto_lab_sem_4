﻿using System;
using System.Windows.Navigation;

namespace Laborator_3
{
    public class AES
    {
        public enum Mode
        {
            Bit128 = 0,
            Bit192 = 1,
            Bit256 = 2
        }
        
        private static readonly int[] key_bits =
        {
            /* AES_CYPHER_128 */ 128,
            /* AES_CYPHER_192 */ 192,
            /* AES_CYPHER_256 */ 256,
        };

        private static readonly int[] number_of_rounds =
        {
            /* AES_CYPHER_128 */ 10,
            /* AES_CYPHER_192 */ 12,
            /* AES_CYPHER_256 */ 14,
        };

        public static readonly int[] key_words =
        {
            4,
            6,
            8
        };
        
        static readonly byte[] rcon =
        {
            0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80,
            0x1b, 0x36, 0x6c, 0xd8, 0xab, 0xed, 0x9a
        };
        
        static readonly uint[] rconWord = {
            0x01000000, 0x02000000, 0x04000000, 0x08000000, 0x10000000, 0x20000000, 0x40000000, 0x80000000,
            0x1b000000, 0x36000000, 0x6c000000, 0xd8000000, 0xab000000, 0xed000000, 0x9a000000
        };

        static readonly byte[] sbox =
        {
            /* 0     1     2     3     4     5     6     7     8     9     A     B     C     D     E     F  */
            0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
            0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
            0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
            0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
            0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
            0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
            0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
            0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
            0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
            0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
            0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
            0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
            0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
            0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
            0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
            0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
        };

        static readonly byte[] sboxInverted =
        {
            /* 0     1     2     3     4     5     6     7     8     9     A     B     C     D     E     F  */
            0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
            0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
            0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
            0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
            0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
            0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
            0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
            0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
            0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
            0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
            0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
            0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
            0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
            0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
            0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
            0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d
        };

        public static void SubBytes(ref byte[] s)
        {
            s = SubBytes(s);
        }
        
        public static byte[] SubBytes(byte[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = sbox[s[i]];
            }

            return s;
        }        
        

        public static void InvSubBytes(ref byte[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = sboxInverted[s[i]];
            }
        }

        public static uint SubWord(uint i)
        {
            return GetWord(SubBytes(GetBytes(i)));
        } //TODO: invert

        public static void ShiftRows(ref byte[] s)
        {
            for (var l = 0; l < s.Length / 16; l++)
            {
                for (var i = 1; i < 4; i++) 
                {
                    for (var j = 0; j < i; j++) 
                    {
                        var tmp = s[i + 16*l];
                        for (var r = 0; r < 3; r++) 
                        {
                            s[i + r * 4 + 16*l] = s[i + (r + 1) * 4 + 16*l];
                        }
                        s[i + 3 * 4 + 16*l] = tmp;
                    }
                }        
            }
        }

        public static void InvShiftRows(ref byte[] s)
        {
            for (var l = 0; l < s.Length / 16; l++)
            {
                for (var i = 1; i < 4; i++)
                {
                    for (var j = 0; j < 4 - i; j++)
                    {
                        var tmp = s[i + 16*l];
                        for (var r = 0; r < 3; r++)
                        {
                            s[i + r * 4 + 16*l] = s[i + (r + 1) * 4 + 16*l];
                        }

                        s[i + (3) * 4 + 16*l] = tmp;
                    }
                }
            }
        }

        private static byte XTime(byte x)
        {
            return (byte) ((x << 1) ^ ((x >> 7) * 0x1b));
        }

        private static byte XTimes(byte x, int ts)
        {
            while (ts-- > 0)
            {
                x = XTime(x);
            }

            return x;
        }

        private static byte Multiply(byte x, byte y)
        {
            /*
             * encrypt: y has only 2 bits: can be 1, 2 or 3
             * decrypt: y could be any value of 9, b, d, or e
             */

            return (byte) ((((y >> 0) & 1) * XTimes(x, 0)) ^
                           (((y >> 1) & 1) * XTimes(x, 1)) ^
                           (((y >> 2) & 1) * XTimes(x, 2)) ^
                           (((y >> 3) & 1) * XTimes(x, 3)) ^
                           (((y >> 4) & 1) * XTimes(x, 4)) ^
                           (((y >> 5) & 1) * XTimes(x, 5)) ^
                           (((y >> 6) & 1) * XTimes(x, 6)) ^
                           (((y >> 7) & 1) * XTimes(x, 7)));
        }

        public static void MixColumns(ref byte[] state)
        {
            var y = new byte[] {2, 3, 1, 1, 1, 2, 3, 1, 1, 1, 2, 3, 3, 1, 1, 2};
            var s = new byte[4];

            for (int l = 0; l < s.Length / 16; l++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        s[r] = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            s[r] = (byte)(s[r] ^ Multiply(state[i * 4 + j + 16 * l], y[r * 4 + j]));
                        }
                    }

                    for (int r = 0; r < 4; r++)
                    {
                        state[i * 4 + r + 16 * l] = s[r];
                    }
                }
            }
        }
        
        public static void InvMixColumns(ref byte[] state)
        {
            var y = new byte[]
            {
                0x0e, 0x0b, 0x0d, 0x09, 0x09, 0x0e, 0x0b, 0x0d,
                0x0d, 0x09, 0x0e, 0x0b, 0x0b, 0x0d, 0x09, 0x0e
            };
            var s = new byte[4];
            
            for (int l = 0; l < s.Length / 16; l++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        s[r] = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            s[r] = (byte) (s[r] ^ Multiply(state[i * 4 + j + 16 * l], y[r * 4 + j]));
                        }
                    }

                    for (int r = 0; r < 4; r++)
                    {
                        state[i * 4 + r + 16 * l] = s[r];
                    }
                }
            }        
        }

        public static void AddRoundKey(byte[] extendedKey, int id, ref byte[] block)
        {
            for (int i = 0; i < block.Length; i++)
            {
                block[i] ^= extendedKey[id * 16 + i % 16];
            }
        }

        public static void PrintBlock(byte[] block)
        {
            const int rows = 4;
            var cols = block.Length / 4;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write("{0:X} ", block[i + j * 4]);

                var finalElem = i + cols * 4;

                if (finalElem < block.Length)
                    Console.Write("{0:X} ", block[finalElem]);

                Console.WriteLine();
            }
            
            Console.WriteLine();
        }

        public static byte[] ExpandKey(byte[] key, int mode)
        {
            var result = new byte[(number_of_rounds[mode] + 1) * 16];
            Array.Copy(key, 0, result, 0, 16);

            for (int i = 0; i < number_of_rounds[mode]; i++)
            {
                byte[] prev4 = new byte[4];
                byte[] mult4 = new byte[4];
                byte[] rcon4 = new byte[4];
                Array.Copy(result, i * 16, prev4, 0, 4);

                mult4[0] = result[i * 16 + 13];
                mult4[1] = result[i * 16 + 14];
                mult4[2] = result[i * 16 + 15];
                mult4[3] = result[i * 16 + 12];

                SubBytes(ref mult4);

                rcon4[0] = rcon[i];

                for (int j = 0; j < 4; j++)
                {
                    result[(i + 1) * 16 + j] = (byte) (mult4[j] ^ prev4[j] ^ rcon4[j]);
                }

                for (int j = 0; j < 12; j++)
                {
                    result[(i + 1) * 16 + 4 + j] = (byte) (result[(i + 1) * 16 + j] ^ result[(i + 1) * 16 - 12 + j]);
                }
            }

            return result;
        }

        public static uint GetWord(byte a, byte b, byte c, byte d)
        {
            return (uint) (a
                           | b << 8
                           | c << 16
                           | d << 24);
        }

        public static uint GetWord(byte[] s)
        {
            return GetWord(s[0], s[1], s[2], s[3]);
        }

        public static byte[] GetBytes(uint w)
        {
            return new byte[4]
            {
                (byte) w,
                (byte) (w >> 8),
                (byte) (w >> 16),
                (byte) (w >> 24)
            };
        }

        public static byte[] GetBytes(uint[] w)
        {
            var res = new byte[w.Length * 4];

            for (int i = 0; i < w.Length; i++)
            {
                Array.Copy(GetBytes(w[i]), 0, res, i * 4, 4);
            }

            return res;
        }

        public static uint RotWord(uint w)
        {
            return (w >> 8) | (w << 24);
        }

        public static byte[] KeyExpansion(byte[] key, int mode)
        {
            var nk = key_words[mode];
            var nr = number_of_rounds[mode];
            var nb = 4; // constant in AES
            uint temp = 0;
            var i = 0;
            var w = new uint[nb * (nr + 1)];
            

            while (i < nk)
            {
                w[i] = GetWord(key[4 * i], key[4 * i + 1], key[4 * i + 2], key[4 * i + 3]);
                i++;
            }

            i = nk;

            while (i < nb * (nr + 1))
            {
                temp = w[i - 1];
                
                if (i % nk == 0)
                    temp = SubWord(RotWord(temp)) ^ rcon[i / nk];
                else if (nk > 6 && i % nk == 4)
                    temp = SubWord(temp);

                w[i] = w[i - nk] ^ temp;
                i++;
            }

            return GetBytes(w);
        }

        public static void Encrypt(ref byte[] text, byte[] key, int mode)
        {
            var extendedKey = KeyExpansion(key, mode);
            
            // PrintBlock(text);
            
            AddRoundKey(extendedKey, 0, ref text);
            
            // PrintBlock(text);

            for (int i = 1; i < number_of_rounds[mode]; i++)
            {
                SubBytes(ref text);
                // PrintBlock(text);
                
                ShiftRows(ref text);
                // PrintBlock(text);

                MixColumns(ref text);
                // PrintBlock(text);
                
                AddRoundKey(extendedKey, i, ref text);
                // PrintBlock(text);
            }
            
            SubBytes(ref text);
            // PrintBlock(text);
            
            ShiftRows(ref text);
            // PrintBlock(text);

            AddRoundKey(extendedKey, number_of_rounds[mode], ref text);
        }

        public static void Decrypt(ref byte[] text, byte[] key, int mode)
        {
            var extendedKey = KeyExpansion(key, mode);
            
            AddRoundKey(extendedKey, number_of_rounds[mode], ref text);
            // PrintBlock(text);
            
            InvShiftRows(ref text);
            // PrintBlock(text);
            
            InvSubBytes(ref text);
            // PrintBlock(text);
            
            for (int i = number_of_rounds[mode] - 1; i > 0; i--)
            {
                AddRoundKey(extendedKey, i, ref text);
                // PrintBlock(text);

                InvMixColumns(ref text);
                // PrintBlock(text);

                InvShiftRows(ref text);
                // PrintBlock(text);

                InvSubBytes(ref text);
                // PrintBlock(text);

            }
            
            AddRoundKey(extendedKey, 0, ref text);
        }
    }
}