using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Encoded Int32. Optimal for negative values. Using zig-zag encoding. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VarZInt : IEquatable<VarZInt>
    {
        public int Size => Variant.VariantSize((uint) Variant.ZigZagEncode(_value));


        private readonly int _value;


        public VarZInt(int value) { _value = value; }


        public byte[] Encode() => Encode(this);


        public static VarZInt Parse(string str) => new VarZInt(int.Parse(str));

        public static byte[] Encode(VarZInt value) => VarInt.Encode(new VarInt((int) Variant.ZigZagEncode(value._value)));

        public static VarZInt Decode(byte[] buffer, int offset) => new VarZInt((int) Variant.ZigZagDecode(VarInt.Decode(buffer, offset)));
        public static VarZInt Decode(Stream stream) => new VarZInt((int) Variant.ZigZagDecode(VarInt.Decode(stream)));
        public static int Decode(byte[] buffer, int offset, out VarZInt result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarZInt result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static explicit operator VarZInt(short value) => new VarZInt(value);
        public static explicit operator VarZInt(int value) => new VarZInt(value);

        public static implicit operator short(VarZInt value) => (short) value._value;
        public static implicit operator int(VarZInt value) => value._value;
        public static implicit operator long(VarZInt value) => value._value;
        public static implicit operator VarZInt(Enum value) => new VarZInt(Convert.ToInt32(value));


        public static bool operator !=(VarZInt a, VarZInt b) => !a.Equals(b);
        public static bool operator ==(VarZInt a, VarZInt b) => a.Equals(b);

        public bool Equals(VarZInt value) => value._value.Equals(_value);
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((VarZInt) obj);
        }
        public override int GetHashCode() => _value.GetHashCode();
    }
}