using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Encoded Int64. Optimal for negative values. Using zig-zag encoding. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VarZLong : IEquatable<VarZLong>
    {
        public int Size => Variant.VariantSize((ulong) Variant.ZigZagEncode(_value));


        private readonly long _value;


        public VarZLong(long value) { _value = value; }


        public byte[] Encode() => Encode(this);


        public static VarZLong Parse(string str) => new VarZLong(long.Parse(str));

        public static byte[] Encode(VarZLong value) => VarLong.Encode(new VarLong(Variant.ZigZagEncode(value)));

        public static VarZLong Decode(byte[] buffer, int offset) => new VarZLong(Variant.ZigZagDecode(VarLong.Decode(buffer, offset)));
        public static VarZLong Decode(Stream stream) => new VarZLong(Variant.ZigZagDecode(VarLong.Decode(stream)));
        public static int Decode(byte[] buffer, int offset, out VarZLong result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarZLong result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static explicit operator VarZLong(short value) => new VarZLong(value);
        public static explicit operator VarZLong(int value) => new VarZLong(value);
        public static explicit operator VarZLong(long value) => new VarZLong(value);

        public static implicit operator short(VarZLong value) => (short) value._value;
        public static implicit operator int(VarZLong value) => (int) value._value;
        public static implicit operator long(VarZLong value) => value._value;
        public static implicit operator VarZLong(Enum value) => new VarZLong(Convert.ToInt64(value));


        public static bool operator !=(VarZLong a, VarZLong b) => !a.Equals(b);
        public static bool operator ==(VarZLong a, VarZLong b) => a.Equals(b);

        public bool Equals(VarZLong value) => value._value.Equals(_value);
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((VarZLong) obj);
        }
        public override int GetHashCode() => _value.GetHashCode();
    }
}
