using System;

using Aragas.Network.Data;
using Aragas.Network.IO;

using static Aragas.Network.IO.PacketStream;
using static Aragas.Network.IO.PacketDataReader;

namespace Aragas.Network.Extensions
{
    public static class PacketExtensions
    {
        private static void Extend<T>(Func<PacketDataReader, int, T> readFunc, Action<PacketStream, T, bool> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        public static void Init()
        {
            Extend<TimeSpan>(ReadTimeSpan, WriteTimeSpan);
            Extend<DateTime>(ReadDateTime, WriteDateTime);
            Extend<Vector2>(ReadVector2, WriteVector2);
            Extend<Vector3>(ReadVector3, WriteVector3);
        }

        private static void WriteTimeSpan(PacketStream stream, TimeSpan value, bool writeDefaultLength = true)
        {
            stream.Write(value.Ticks);
        }
        private static TimeSpan ReadTimeSpan(PacketDataReader reader, int length = 0)
        {
            return new TimeSpan(reader.Read<long>());
        }

        private static void WriteDateTime(PacketStream stream, DateTime value, bool writeDefaultLength = true)
        {
            stream.Write(value.Ticks);
        }
        private static DateTime ReadDateTime(PacketDataReader reader, int length = 0)
        {
            return new DateTime(reader.Read<long>());
        }

        private static void WriteVector2(PacketStream stream, Vector2 value, bool writeDefaultLength = true)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
        }
        private static Vector2 ReadVector2(PacketDataReader reader, int length = 0)
        {
            return new Vector2(reader.Read<float>(), reader.Read<float>());
        }

        private static void WriteVector3(PacketStream stream, Vector3 value, bool writeDefaultLength = true)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
            stream.Write(value.Z);
        }
        private static Vector3 ReadVector3(PacketDataReader reader, int length = 0)
        {
            return new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
        }
    }
}