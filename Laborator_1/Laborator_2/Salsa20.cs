using System;
using System.Runtime.InteropServices;

namespace Laborator_2
{
    public class Salsa20
    {
        private const int ROUNDS = 20;
        
        public enum ESalsa20
        {
            Bit16,
            Bit32
        }

        public static uint ROTL(uint a, int b)
        {
            return a << b | a >> (32 - b);
        }

        public static void QR(ref uint a, ref uint b, ref uint c, ref uint d)
        {
            b ^= ROTL(a + d, 7);
            c ^= ROTL(b + a, 9);
            d ^= ROTL(c + b, 13);
            a ^= ROTL(d + c, 18);
        }

        public static uint[] Salsa20Block(uint[] input)
        {
            var x = new uint[16];
            var output = new uint[16];

            for (int i = 0; i < 16; i++)
            {
                x[i] = input[i];
            }

            for (int i = 0; i < ROUNDS; i += 2)
            {
                QR(ref x[0], ref x[4], ref x[8], ref x[12]); // column 1
                QR(ref x[5], ref x[9], ref x[13], ref x[1]); // column 2
                QR(ref x[10], ref x[14], ref x[2], ref x[6]); // column 3
                QR(ref x[15], ref x[3], ref x[7], ref x[11]); // column 4
                // Even round
                QR(ref x[0], ref x[1], ref x[2], ref x[3]); // row 1
                QR(ref x[5], ref x[6], ref x[7], ref x[4]); // row 2
                QR(ref x[10], ref x[11], ref x[8], ref x[9]); // row 3
                QR(ref x[15], ref x[12], ref x[13], ref x[14]); // row 4
            }

            for (int i = 0; i < 16; i++)
            {
                output[i] = x[i] + input[i];
            }

            //PrintArr(x);

            return output;
        }

        public static void PrintArr(uint[] arr)
        {
            foreach (var u in arr)
            {
                Console.WriteLine(u);
            }

            Console.WriteLine();
        }

        public static void PrintArr(byte[] arr)
        {
            foreach (var b in arr)
            {
                Console.WriteLine(b);
            }

            Console.WriteLine();
        }

        public static uint[] ByteToUInt(byte[] input)
        {
            uint[] result = new uint[16];

            for (int i = 0; i < 16; i++)
            {
                result[i] = (uint) (input[4 * i]
                        + (input[4 * i + 1] << 8)
                        + (input[4 * i + 2] << 16)
                        + (input[4 * i + 3] << 24));
            }

            return result;
        }

        public static byte[] UIntToByte(uint[] input)
        {
            byte[] result = new byte[64];

            for (int i = 0; i < 16; i++)
            {
                result[4 * i] = (byte) input[i];
                result[4 * i + 1] = (byte) (input[i] >> 8);
                result[4 * i + 2] = (byte) (input[i] >> 16);
                result[4 * i + 3] = (byte) (input[i] >> 24);
            }

            return result;
        }
        
        public static void Expand16(byte[] key, byte[] nonce, byte[] keystream)
        {
            //Console.WriteLine("expand 16-byte k");
            var t = new char[4, 4]
            {
                {'e', 'x', 'p', 'a'},
                {'n', 'd', ' ', '1'},
                {'6', '-', 'b', 'y'},
                {'t', 'e', ' ', 'k'}
            };
            //Console.WriteLine("done");

            for (int i = 0; i < 64; i += 20)
            for (int j = 0; j < 4; j++)
                keystream[i + j] = (byte) t[i / 20, j];

            //Console.WriteLine("set k");

            for (int i = 0; i < 16; ++i)
            {
                keystream[4 + i] = i < key.Length ? key[i] : (byte) 0;
                keystream[44 + i] = i < key.Length ? key[i] : (byte) 0;
                keystream[24 + i] = i < nonce.Length && i < 8 ? nonce[i] : (byte) 0;
            }

            //Console.WriteLine("key done");
        }
        
        public static void Expand32(byte[] key, byte[] nonce, byte[] keystream)
        {
            //Console.WriteLine("expand 16-byte k");
            var t = new char[4, 4]
            {
                {'e', 'x', 'p', 'a'},
                {'n', 'd', ' ', '3'},
                {'2', '-', 'b', 'y'},
                {'t', 'e', ' ', 'k'}
            };
            //Console.WriteLine("done");

            for (int i = 0; i < 64; i += 20)
            for (int j = 0; j < 4; j++)
                keystream[i + j] = (byte) t[i / 20, j];

            //Console.WriteLine("set k");

            for (int i = 0; i < 16; ++i)
            {
                keystream[4 + i] = i < key.Length ? key[i] : (byte) 0;
                keystream[44 + i] = i < key.Length ? key[i] : (byte) 0;
                keystream[24 + i] = i < nonce.Length ? nonce[i] : (byte) 0;
            }

            //Console.WriteLine("key done");
        }

        public static void Encrypt(byte[] key, byte[] nonce, byte[] text, ESalsa20 encrType)
        {
            byte[] keystream = new byte[64];

            if (encrType == ESalsa20.Bit32)
            {
                Expand32(key, nonce, keystream);
            }
            else
            {
                Expand16(key, nonce, keystream);
            }


            uint[] uintInputKeystream = ByteToUInt(keystream);

            uint[] uintOutputKeystream = Salsa20Block(uintInputKeystream);

            keystream = UIntToByte(uintOutputKeystream);
            
            for (int i = 0; i < text.Length; i++)
            {
                text[i] ^= keystream[i % 64];
            }
        }
    }
}