using System.IO;

namespace Aragas.Network.Data
{
    internal abstract class Variant
    {
        protected internal static int VariantSize(ulong value)
        {
            var outputSize = 0;
            while (value > 127)
            {
                outputSize++;
                value >>= 7;
            }
            return ++outputSize;
        }

        protected internal static byte[] Encode(ulong value)
        {
            var size = VariantSize(value);
            var array = new byte[size];

            for (var i = 0; i < array.Length - 1; i++)
            {
                array[i] = (byte) (value & 127 | 128u);
                value >>= 7;
            }
            array[array.Length - 1] = (byte) value;

            return array;
        }

        protected internal static ulong Decode(byte[] buffer, int offset)
        {
            ulong decodedValue = 0;
            int index = 0, shiftAmount = 0;
            byte currByte;
            do
            {
                currByte = buffer[offset + index++];
                ulong lowByte = currByte & 127u;
                decodedValue |= lowByte << shiftAmount++ * 7;
            } while ((currByte & 128u) != 0);

            return decodedValue;
        }
        protected internal static ulong Decode(Stream stream)
        {
            ulong decodedValue = 0;
            int shiftAmount = 0;
            byte currByte;
            do
            {
                currByte = (byte) stream.ReadByte();
                ulong lowByte = currByte & 127u;
                decodedValue |= lowByte << shiftAmount++ * 7;
            } while ((currByte & 128u) != 0);

            return decodedValue;
        }

        protected internal static long ZigZagEncode(long value) => (value << 1) ^ (value >> 63);
        protected internal static long ZigZagDecode(long value)
        {
            var temp = (((value << 63) >> 63) ^ value) >> 1;
            return temp ^ (value & (1L << 63));
        }
    }
}