using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Encoded Int32. Not optimal for negative values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VarInt : IEquatable<VarInt>
    {
        public int Size => Variant.VariantSize((uint) _value);


        private readonly int _value;


        public VarInt(int value) { _value = value; }


        public byte[] Encode() => Encode(this);


        public override string ToString() => _value.ToString();

        public static VarInt Parse(string str) => new VarInt(int.Parse(str));
        
        public static byte[] Encode(VarInt value) => Variant.Encode((uint) value._value);
        public static int Encode(VarInt value, byte[] buffer, int offset)
        {
            var encoded = value.Encode();
            Buffer.BlockCopy(encoded, 0, buffer, offset, encoded.Length);
            return encoded.Length;
        }
        public static int Encode(VarInt value, Stream stream)
        {
            var encoded = value.Encode();
            stream.Write(encoded, 0, encoded.Length);
            return encoded.Length;
        }

        public static VarInt Decode(byte[] buffer, int offset) => new VarInt((int) Variant.Decode(buffer, offset));
        public static VarInt Decode(Stream stream) => new VarInt((int) Variant.Decode(stream));
        public static int Decode(byte[] buffer, int offset, out VarInt result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarInt result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static explicit operator VarInt(short value) => new VarInt(value);
        public static explicit operator VarInt(int value) => new VarInt(value);

        public static implicit operator short(VarInt value) => (short) value._value;
        public static implicit operator int(VarInt value) => value._value;
        public static implicit operator long(VarInt value) => value._value;
        public static implicit operator VarInt(Enum value) => new VarInt(Convert.ToInt32(value));


        public static bool operator !=(VarInt a, VarInt b) => !a.Equals(b);
        public static bool operator ==(VarInt a, VarInt b) => a.Equals(b);

        public bool Equals(VarInt value) => value._value.Equals(_value);
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((VarInt) obj);
        }
        public override int GetHashCode() => _value.GetHashCode();
    }
}