using System;

namespace Laborator_2
{
    public class Trivium
    {
        public byte[] internalState;

        public Trivium(byte[] key, byte[] IV)
        {
            internalState = new byte[36];

            int minKeyLength = 10 < key.Length ? 10 : key.Length; 

            for (var i = 0; i < minKeyLength; i++)
            {
                internalState[i] = i < key.Length ? key[i] : (byte) 0;
            }
            
            for (int i = 93; i < 173; i++)
            {
                var IVPos = i - 93;
                var IVByte = IVPos / 8;
                var IVBit = IVPos % 8;
                
                if (IV.Length <= IVByte)
                    break;

                SetBit((uint)i, GetBit(IV[IVByte], IVBit));
            }

            for (uint i = 285; i < 288; i++)
            {
                SetBit(i, 1);
            }
            
            for (int i = 0; i < 288 * 4; i++)
            {
                Cycle();
            }
        }

        public byte Cycle()
        {
            var t1 = (byte)(GetBit(65) ^ GetBit(92));
            var t2 = (byte)(GetBit(161) ^ GetBit(176));
            var t3 = (byte)(GetBit(242) ^ GetBit(287));
            
            var z = (byte)(t1 ^ t2 ^ t3);

            t1 = (byte)(t1 ^ GetBit(90) & GetBit(91) ^ GetBit(170));
            t2 = (byte)(t2 ^ GetBit(174) & GetBit(175) ^ GetBit(263));
            
            t3 = (byte)(t3 ^ GetBit(285) & GetBit(286) ^ GetBit(68));

            for (int i = 92; i >= 1; i--)
            {
                SetBit(i, GetBit(i-1));
            }
            SetBit(0, t3);
            
            for (int i = 176; i >= 94; i--)
            {
                SetBit(i, GetBit(i-1));
            }
            SetBit(93, t1);
            
            for (int i = 287; i >= 178; i--)
            {
                SetBit(i, GetBit(i-1));
            }
            SetBit(177, t2);

            return z;
        }

        public void Encrypt(byte[] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    text[i] ^= (byte)(Cycle() << j);
                }
            }
            
            Console.WriteLine("encrypted");
        }

        public void SetBit(uint id, byte bit)
        {
            if (bit > 1)
            {
                Console.WriteLine("Invalid value " + bit + " given to bit " + id);
                return;
            }

            if (id >= 288)
            {
                Console.WriteLine("Id " + id + " out of bounds [0, 287]");
                return;
            }

            var byteIndex = id / 8;
            var bitIndex = (int) id % 8;
            
            internalState[byteIndex] = (byte) (internalState[byteIndex] & ((1 << bitIndex) ^ 255) | bit << bitIndex);
        }

        public byte GetBit(uint id)
        {
            if (id >= 288)
            {
                Console.WriteLine("Id " + id + " out of bounds [0, 287]");
            }
            
            var byteIndex = id / 8;
            var bitIndex = (int) id % 8;

            return (byte)(internalState[byteIndex] & (1 << bitIndex));
        }
        
        public void SetBit(int id, byte bit)
        {
            if (bit > 1)
            {
                Console.WriteLine("Invalid value " + bit + " given to bit " + id);
                return;
            }

            if (id >= 288 || id < 0)
            {
                Console.WriteLine("Id " + id + " out of bounds [0, 287]");
                return;
            }

            var byteIndex = id / 8;
            var bitIndex = (int) id % 8;
            
            internalState[byteIndex] = (byte) (internalState[byteIndex] & ((1 << bitIndex) ^ 255) | bit << bitIndex);
        }

        public byte GetBit(int id)
        {
            if (id >= 288 || id < 0)
            {
                Console.WriteLine("Id " + id + " out of bounds [0, 287]");
            }
            
            var byteIndex = id / 8;
            var bitIndex = (int) id % 8;

            return (byte)((internalState[byteIndex] >> bitIndex) & 1);
        }


        public byte GetBit(byte num,int id)
        {
            if (id >= 8)
            {
                Console.WriteLine("Id " + id + " out of bounds [0, 7]");
                return 0;
            }
            
            //return (byte)(num & (1 << id));
            return (byte)((num >> id) & 1);
        }
        
        
    }
}