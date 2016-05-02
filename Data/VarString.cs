using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Encoded String. Using VarLong as length.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VarString : IEquatable<VarString>
    {
        public int Size => Length.Size + _value.Length;

        private VarInt Length => new VarInt(_value.Length);
        private readonly string _value;


        public VarString(string value) { _value = value; }


        public byte[] Encode() => Encode(this);


        public override string ToString() => _value;

        public static byte[] Encode(VarString value)
        {
            var lengthArray = value.Length.Encode();
            var stringArray = Encoding.UTF8.GetBytes(value._value);
            var result = new byte[lengthArray.Length + stringArray.Length];

            Buffer.BlockCopy(lengthArray, 0, result, 0, lengthArray.Length);
            Buffer.BlockCopy(stringArray, 0, result, lengthArray.Length, stringArray.Length);

            return result;
        }
        public static int Encode(VarString value, byte[] buffer, int offset)
        {
            var encoded = value.Encode();
            Buffer.BlockCopy(encoded, 0, buffer, offset, encoded.Length);
            return encoded.Length;
        }
        public static int Encode(VarString value, Stream stream)
        {
            var encoded = value.Encode();
            stream.Write(encoded, 0, encoded.Length);
            return encoded.Length;
        }

        public static VarString Decode(byte[] buffer, int offset)
        {
            var length = VarLong.Decode(buffer, offset);
            var stringArray = new byte[length];
            Buffer.BlockCopy(buffer, length.Size + offset, stringArray, 0, stringArray.Length);
            return new VarString(Encoding.UTF8.GetString(stringArray, 0, stringArray.Length));
        }
        public static VarString Decode(Stream stream)
        {
            var length = VarInt.Decode(stream);
            var stringArray = new byte[length];
            stream.Read(stringArray, 0, length);
            return new VarString(Encoding.UTF8.GetString(stringArray, 0, stringArray.Length));
        }
        public static int Decode(byte[] buffer, int offset, out VarString result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarString result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static implicit operator VarString(string value) => new VarString(value);
        public static implicit operator string(VarString value) => value._value;


        public static bool operator !=(VarString a, VarString b) => !a.Equals(b);
        public static bool operator ==(VarString a, VarString b) => a.Equals(b);

        public bool Equals(VarString value) => value._value.Equals(_value);
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((VarString) obj);
        }
        public override int GetHashCode() => _value.GetHashCode();
    }
}
