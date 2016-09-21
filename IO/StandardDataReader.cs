using System;
using System.IO;
using System.Text;

namespace Aragas.Network.IO
{
    /// <summary>
    /// Data reader that uses int for length decoding.
    /// </summary>
    public sealed class StandardDataReader : PacketDataReader
    {
        public override bool IsServer { get; }

        private Encoding Encoding { get; } = Encoding.UTF8;

        private readonly Stream _stream;
        
        public StandardDataReader(Stream stream, bool isServer = false)
        {
            _stream = stream;
            IsServer = isServer;
        }
        public StandardDataReader(byte[] data, bool isServer = false)
        {
            _stream = new MemoryStream(data);
            IsServer = isServer;
        }


        #region Read

        // -- Anything
        public override T Read<T>(T value = default(T), int length = 0)
        {
            T val;
            var type = value != null ? value.GetType() : typeof(T);

            if (length > 0)
            {
                if (type == typeof (string))
                    return (T) (object) ReadString(length);

                if (type == typeof (string[]))
                    return (T) (object) ReadStringArray(length);
                if (type == typeof (int[]))
                    return (T) (object) ReadIntArray(length);
                if (type == typeof (byte[]))
                    return (T) (object) ReadByteArray(length);


                if (ExtendReadTryExecute(this, length, out val))
                    return val;


                throw new NotImplementedException($"Type {type} not found in extend methods.");
            }


            if (type == typeof (string))
                return (T) (object) ReadString();

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
                length = ReadInt();

            var stringBytes = ReadByteArray(length);

            return Encoding.GetString(stringBytes, 0, stringBytes.Length);
        }

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
            return unchecked(
                   ((ulong) ReadByte() << 56) |
                   ((ulong) ReadByte() << 48) |
                   ((ulong) ReadByte() << 40) |
                   ((ulong) ReadByte() << 32) |
                   ((ulong) ReadByte() << 24) |
                   ((ulong) ReadByte() << 16) |
                   ((ulong) ReadByte() << 8) |
                    (ulong) ReadByte());
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
            var length = ReadInt();
            return ReadStringArray(length);
        }
        private string[] ReadStringArray(int length)
        {
            var myStrings = new string[length];

            for (var i = 0; i < length; i++)
                myStrings[i] = ReadString();

            return myStrings;
        }

        // -- IntArray
        private int[] ReadIntArray()
        {
            var length = ReadInt();
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
            var length = ReadInt();
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