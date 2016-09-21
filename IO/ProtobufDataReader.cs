using System;
using System.IO;
using System.Text;

using Aragas.Network.Data;

namespace Aragas.Network.IO
{
    /// <summary>
    /// Data reader that uses variants for length decoding.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public sealed class ProtobufDataReader : PacketDataReader
    {
        public override bool IsServer { get; }

        private Encoding Encoding { get; } = Encoding.UTF8;

        private readonly Stream _stream;
        
        public ProtobufDataReader(Stream stream, bool isServer = false)
        {
            _stream = stream;
            IsServer = isServer;
        }
        public ProtobufDataReader(byte[] data, bool isServer = false)
        {
            _stream = new MemoryStream(data);
            IsServer = isServer;
        }


        #region Read

        // -- Anything
        public override T Read<T>(T value = default(T), int length = 0)
        {
            T val;
            var type = value != null? value.GetType() : typeof(T);

            if (length > 0)
            {
                if (type == typeof (string))
                    return (T) (object) ReadString(length);

                if (type == typeof (string[]))
                    return (T) (object) ReadStringArray(length);
                if (type == typeof (VarShort[]))
                    return (T) (object) ReadVarShortArray(length);
                if (type == typeof (VarZShort[]))
                    return (T) (object) ReadVarZShortArray(length);
                if (type == typeof (VarInt[]))
                    return (T) (object) ReadVarIntArray(length);
                if (type == typeof (VarZInt[]))
                    return (T) (object) ReadVarZIntArray(length);
                if (type == typeof (VarLong[]))
                    return (T) (object) ReadVarLongArray(length);
                if (type == typeof (VarZLong[]))
                    return (T) (object) ReadVarZLongArray(length);
                if (type == typeof (int[]))
                    return (T) (object) ReadIntArray(length);
                if (type == typeof (byte[]))
                    return (T) (object) ReadByteArray(length);


                if(ExtendReadTryExecute(this, length, out val))
                    return val;


                throw new NotImplementedException($"Type {type} not found in extend methods.");
            }


            if (type == typeof (string))
                return (T) (object) ReadString();

            if (type == typeof (VarShort))
                return (T) (object) ReadVarShort();
            if (type == typeof (VarZShort))
                return (T) (object) ReadVarZShort();
            if (type == typeof (VarInt))
                return (T) (object) ReadVarInt();
            if (type == typeof (VarZInt))
                return (T) (object) ReadVarZInt();
            if (type == typeof (VarLong))
                return (T) (object) ReadVarLong();
            if (type == typeof (VarZLong))
                return (T) (object) ReadVarZLong();


            if (type == typeof (bool))
                return (T) (object) ReadBoolean();

            if (type == typeof (sbyte))
                return (T) (object) ReadSByte();
            if (type == typeof (byte))
                return (T) (object) ReadByte();

            if (type == typeof (short))
                return (T) (object) ReadShort();
            if (type == typeof (ushort))
                return (T) (object) ReadUShort();

            if (type == typeof (int))
                return (T) (object) ReadInt();
            if (type == typeof (uint))
                return (T) (object) ReadUInt();

            if (type == typeof (long))
                return (T) (object) ReadLong();
            if (type == typeof (ulong))
                return (T) (object) ReadULong();

            if (type == typeof (float))
                return (T) (object) ReadFloat();

            if (type == typeof (double))
                return (T) (object) ReadDouble();


            if (ExtendReadTryExecute(this, length, out val))
                return val;


            if (type == typeof (string[]))
                return (T) (object) ReadStringArray();
            if (type == typeof (VarShort[]))
                return (T) (object) ReadVarShortArray();
            if (type == typeof (VarZShort[]))
                return (T) (object) ReadVarZShortArray();
            if (type == typeof (VarInt[]))
                return (T) (object) ReadVarIntArray();
            if (type == typeof (VarZInt[]))
                return (T) (object) ReadVarZIntArray();
            if (type == typeof (VarLong[]))
                return (T) (object) ReadVarLongArray();
            if (type == typeof (VarZLong[]))
                return (T) (object) ReadVarZLongArray();
            if (type == typeof (int[]))
                return (T) (object) ReadIntArray();
            if (type == typeof (byte[]))
                return (T) (object) ReadByteArray();
            

            throw new NotImplementedException($"Type {type} not found in extend methods.");
        }

        // -- String
        private string ReadString(int length = 0)
        {
            if (length == 0)
                length = ReadVarInt();

            var stringBytes = ReadByteArray(length);

            return Encoding.GetString(stringBytes, 0, stringBytes.Length);
        }

        // -- Variants
        private VarShort ReadVarShort() { return VarShort.Decode(_stream); }
        private VarZShort ReadVarZShort() { return VarZShort.Decode(_stream); }

        private VarInt ReadVarInt() { return VarInt.Decode(_stream); }
        private VarZInt ReadVarZInt() { return VarZInt.Decode(_stream); }

        private VarLong ReadVarLong() { return VarLong.Decode(_stream); }
        private VarZLong ReadVarZLong() { return VarZLong.Decode(_stream); }

        // -- Boolean
        private bool ReadBoolean() { return Convert.ToBoolean(ReadByte()); }

        // -- SByte & Byte
        private sbyte ReadSByte() { return unchecked((sbyte) ReadByte()); }
        private byte ReadByte() { return (byte) _stream.ReadByte(); }

        // -- Short & UShort
        private short ReadShort()
        {
            var bytes = ReadByteArray(2);
            Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }
        private ushort ReadUShort()
        {
            return (ushort) ((ReadByte() << 8) | ReadByte());
        }

        // -- Int & UInt
        private int ReadInt()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }
        private uint ReadUInt()
        {
            return (uint) (
                (ReadByte() << 24) |
                (ReadByte() << 16) |
                (ReadByte() << 8) |
                (ReadByte()));
        }

        // -- Long & ULong
        private long ReadLong()
        {
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }
        private ulong ReadULong()
        {
            return ((ulong) ReadByte() << 56) |
                   ((ulong) ReadByte() << 48) |
                   ((ulong) ReadByte() << 40) |
                   ((ulong) ReadByte() << 32) |
                   ((ulong) ReadByte() << 24) |
                   ((ulong) ReadByte() << 16) |
                   ((ulong) ReadByte() << 8) |
                    (ulong) ReadByte();
        }

        // -- Floats
        private float ReadFloat()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        // -- Doubles
        private double ReadDouble()
        {
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }

        // -- StringArray
        private string[] ReadStringArray()
        {
            var length = ReadVarInt();
            return ReadStringArray(length);
        }
        private string[] ReadStringArray(int length)
        {
            var myStrings = new string[length];

            for (var i = 0; i < length; i++)
                myStrings[i] = ReadString();

            return myStrings;
        }

        // -- Variant Array
        private VarShort[] ReadVarShortArray()
        {
            var length = ReadVarInt();
            return ReadVarShortArray(length);
        }
        private VarShort[] ReadVarShortArray(int length)
        {
            var myInts = new VarShort[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarShort();

            return myInts;
        }

        private VarZShort[] ReadVarZShortArray()
        {
            var length = ReadVarInt();
            return ReadVarZShortArray(length);
        }
        private VarZShort[] ReadVarZShortArray(int length)
        {
            var myInts = new VarZShort[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarZShort();

            return myInts;
        }

        private VarInt[] ReadVarIntArray()
        {
            var length = ReadVarInt();
            return ReadVarIntArray(length);
        }
        private VarInt[] ReadVarIntArray(int length)
        {
            var myInts = new VarInt[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarInt();

            return myInts;
        }

        private VarZInt[] ReadVarZIntArray()
        {
            var length = ReadVarInt();
            return ReadVarZIntArray(length);
        }
        private VarZInt[] ReadVarZIntArray(int length)
        {
            var myInts = new VarZInt[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarZInt();

            return myInts;
        }

        private VarLong[] ReadVarLongArray()
        {
            var length = ReadVarInt();
            return ReadVarLongArray(length);
        }
        private VarLong[] ReadVarLongArray(int length)
        {
            var myInts = new VarLong[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarLong();

            return myInts;
        }

        private VarZLong[] ReadVarZLongArray()
        {
            var length = ReadVarInt();
            return ReadVarZLongArray(length);
        }
        private VarZLong[] ReadVarZLongArray(int length)
        {
            var myInts = new VarZLong[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarZLong();

            return myInts;
        }

        // -- IntArray
        private int[] ReadIntArray()
        {
            var length = ReadVarInt();
            return ReadIntArray(length);
        }
        private int[] ReadIntArray(int length)
        {
            var myInts = new int[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadInt();

            return myInts;
        }

        // -- ByteArray
        private byte[] ReadByteArray()
        {
            var length = ReadVarInt();
            return ReadByteArray(length);
        }
        private byte[] ReadByteArray(int length)
        {
            if (length == 0)
                return new byte[length];

            var msg = new byte[length];
            var readSoFar = 0;
            while (readSoFar < length)
            {
                var read = _stream.Read(msg, readSoFar, msg.Length - readSoFar);
                readSoFar += read;
                if (read == 0)
                    break;   // connection was broken
            }

            return msg;
        }

        #endregion Read


        public override int BytesLeft() => (int)(_stream.Length - _stream.Position);


        public override void Dispose()
        {
            _stream?.Dispose();
        }
    }
}