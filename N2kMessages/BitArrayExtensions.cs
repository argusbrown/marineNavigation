using System;
using System.Collections;

namespace N2kMessages
{
    public static class BitArrayExtensions
    {
        public static int ToInteger(this BitArray source)
        {
            int[] result = new int[1];
            source.CopyTo(result, 0);
            return result[0];
        }

        public static ushort ToUnsignedShort(this BitArray source)
        {
            byte[] result = new byte[2];
            source.CopyTo(result, 0);
            return (ushort)(result[0] + (result[1] << 8));
        }

        public static byte[] ToBytes(this BitArray source)
        {
            if ((source.Count % 8) != 0)
            {
                throw new ArgumentException("Only a bit array with a multiple of 8 bits can be converted", "source");
            }

            byte[] bytes = new byte[source.Count / 8];
            source.CopyTo(bytes, 0);
            return bytes;
        }

        public static byte ToByte(this BitArray source)
        {
            if (source.Count != 8)
            {
                throw new ArgumentException("Only a bit array with 8 bits can be converted", "source");
            }

            byte[] bytes = new byte[1];
            source.CopyTo(bytes, 0);
            return bytes[0];
        }
    }
}