using System;

using PCLExt.Network;

namespace Aragas.Network.IO
{
    /// <summary>
    /// Stream that uses variant for length encoding.
    /// </summary>
    public class ProtobufStreamEvent : ProtobufStream
    {
        public event PacketStreamConnectedEventArgs     Connected;
        public event PacketStreamDataReceivedEventArgs  DataReceived;
        public event PacketStreamDisconnectedEventArgs  Disconnected;

        public override string Host => Socket.RemoteEndPoint.IP;
        public override ushort Port => Socket.RemoteEndPoint.Port;
        public override bool IsConnected => Socket.IsConnected;
        public override int DataAvailable => Socket?.DataAvailable ?? 0;

        private ISocketClientEvent Socket { get; }

        private BouncyCastle BouncyCastle { get; set; }


        public ProtobufStreamEvent(ISocketClientEvent socket, bool isServer = false) : base(socket, isServer)
        {
            Socket = socket;
            Socket.Connected += (e) => Connected?.Invoke(new PacketStreamConnectedArgs(this));
            Socket.DataReceived += (e) => DataReceived?.Invoke(new PacketStreamDataReceivedArgs(this, Socket_DataReceived(e)));
            Socket.Disconnected += (e) => Disconnected?.Invoke(new PacketStreamDisconnectedArgs(this, e.Reason));
        }
        private byte[] Socket_DataReceived(SocketDataReceivedArgs args)
        {
            if (EncryptionEnabled)
                return BouncyCastle.Decrypt(args.Data, 0, args.Data.Length);
            else
                return args.Data;
        }


        public override void InitializeEncryption(byte[] key)
        {
            BouncyCastle = new BouncyCastle(key);
            EncryptionEnabled = true;
        }

        public void Send(byte[] buffer)
        {
            if (EncryptionEnabled)
            {
                var decrypted = BouncyCastle.Decrypt(buffer, 0, buffer.Length);
                Socket.Write(decrypted, 0, decrypted.Length);
            }
            else
                Socket.Write(buffer, 0, buffer.Length);
        }
        public byte[] Receive(int length) { throw new NotSupportedException(); }
    }
}