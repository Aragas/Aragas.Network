using System;
using System.Collections.Generic;
using System.IO;

using Aragas.Network.Packets;

namespace Aragas.Network.IO
{
    public interface IEncryptedStream
    {
        bool EncryptionEnabled { get; }

        void InitializeEncryption(byte[] key);
    }

    public interface ICompressedStream
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public abstract partial class PacketStream : IDisposable
    {
        public abstract bool IsServer { get; }
        public abstract bool IsConnected { get; }
        public abstract string Host { get; }
        public abstract ushort Port { get; }
        public abstract int DataAvailable { get; }

        protected abstract Stream BaseStream { get; }

        public abstract void Connect(string ip, ushort port);
        public abstract void Disconnect();

        public abstract void SendPacket(Packet packet);


        #region ExtendWrite

        private static readonly Dictionary<int, Action<PacketStream, object, bool>> WriteExtendedList = new Dictionary<int, Action<PacketStream, object, bool>>();

        public static void ExtendWrite<T>(Action<PacketStream, T, bool> action)
        {
            if(action != null)
                WriteExtendedList.Add(typeof(T).GetHashCode(), Transform(action));
        }

        private static Action<PacketStream, object, bool> Transform<T>(Action<PacketStream, T, bool> action) => (stream, value, writedef) => action(stream, (T) value, writedef);
        
        protected static bool ExtendWriteContains<T>() => ExtendWriteContains(typeof(T));
        protected static bool ExtendWriteContains(Type type) => WriteExtendedList.ContainsKey(type.GetHashCode());

        protected static void ExtendWriteExecute<T>(PacketStream stream, T value, bool writeDefaultLength = true)
        {
            Action<PacketStream, object, bool> action;
            if (WriteExtendedList.TryGetValue(typeof(T).GetHashCode(), out action))
                action.Invoke(stream, value, writeDefaultLength);
        }

        #endregion ExtendWrite

        public abstract void Write<TDataType>(TDataType value = default(TDataType), bool writeDefaultLength = true);

        public abstract byte[] GetBuffer();

        public abstract void Dispose();
    }
}