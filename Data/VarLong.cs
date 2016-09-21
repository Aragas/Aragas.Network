using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Encoded Int64. Not optimal for negative values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VarLong : IEquatable<VarLong>
    {
        public int Size => Variant.VariantSize((ulong) _value);


        private readonly long _value;


        public VarLong(long value) { _value = value; }


        public byte[] Encode() => Encode(this);


        public override string ToString() => _value.ToString();

        public static VarLong Parse(string str) => new VarLong(long.Parse(str));

        public static byte[] Encode(VarLong value) => Variant.Encode((ulong) value._value);
        public static int Encode(VarLong value, byte[] buffer, int offset)
        {
            var encoded = value.Encode();
            Buffer.BlockCopy(encoded, 0, buffer, offset, encoded.Length);
            return encoded.Length;
        }
        public static int Encode(VarLong value, Stream stream)
        {
            var encoded = value.Encode();
            stream.Write(encoded, 0, encoded.Length);
            return encoded.Length;
        }

        public static VarLong Decode(byte[] buffer, int offset) => new VarLong((long) Variant.Decode(buffer, offset));
        public static VarLong Decode(Stream stream) => new VarLong((long) Variant.Decode(stream));
        public static int Decode(byte[] buffer, int offset, out VarLong result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarLong result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static explicit operator VarLong(short value) => new VarLong(value);
        public static explicit operator VarLong(int value) => new VarLong(value);
        public static explicit operator VarLong(long value) => new VarLong(value);

        public static implicit operator short(VarLong value) => (short) value._value;
        public static implicit operator int(VarLong value) => (int) value._value;
        public static implicit operator long(VarLong value) => value._value;
        public static implicit operator VarLong(Enum value) => new VarLong(Convert.ToInt64(value));


        public static bool operator !=(VarLong a, VarLong b) => !a.Equals(b);
        public static bool operator ==(VarLong a, VarLong b) => a.Equals(b);

        public bool Equals(VarLong value) => value._value.Equals(_value);
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((VarLong) obj);
        }
        public override int GetHashCode() => _value.GetHashCode();
    }
}