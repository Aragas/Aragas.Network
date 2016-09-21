using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Encoded Int16. Optimal for negative values. Using zig-zag encoding.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VarZShort : IEquatable<VarZShort>
    {
        public int Size => Variant.VariantSize((ushort) Variant.ZigZagEncode(_value));


        private readonly short _value;


        public VarZShort(short value) { _value = value; }


        public byte[] Encode() => Encode(this);


        public static VarZShort Parse(string str) => new VarZShort(short.Parse(str));

        public static byte[] Encode(VarZShort value) => VarShort.Encode(new VarShort((short) Variant.ZigZagEncode(value)));

        public static VarZShort Decode(byte[] buffer, int offset) => new VarZShort((short) Variant.ZigZagDecode(VarShort.Decode(buffer, offset)));
        public static VarZShort Decode(Stream stream) => new VarZShort((short) Variant.ZigZagDecode(VarShort.Decode(stream)));
        public static int Decode(byte[] buffer, int offset, out VarZShort result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarZShort result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static explicit operator VarZShort(short value) => new VarZShort(value);

        public static implicit operator short(VarZShort value) => value._value;
        public static implicit operator int(VarZShort value) => value._value;
        public static implicit operator long(VarZShort value) => value._value;
        public static implicit operator VarZShort(Enum value) => new VarZShort(Convert.ToInt16(value));


        public static bool operator !=(VarZShort a, VarZShort b) => !a.Equals(b);
        public static bool operator ==(VarZShort a, VarZShort b) => a.Equals(b);

        public bool Equals(VarZShort value) => value._value.Equals(_value);
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((VarZShort) obj);
        }
        public override int GetHashCode() => _value.GetHashCode();
    }
}