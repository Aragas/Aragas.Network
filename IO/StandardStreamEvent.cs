using System;

using PCLExt.Network;

namespace Aragas.Network.IO
{
    /// <summary>
    ///
    /// </summary>
    public class StandardStreamEvent : StandardStream
    {
        public event PacketStreamConnectedEventArgs     Connected;
        public event PacketStreamDataReceivedEventArgs  DataReceived;
        public event PacketStreamDisconnectedEventArgs  Disconnected;

        public override string Host => Socket.RemoteEndPoint.IP;
        public override ushort Port => Socket.RemoteEndPoint.Port;
        public override bool IsConnected => Socket.IsConnected;
        public override int DataAvailable => Socket?.DataAvailable ?? 0;
     
        private ISocketClientEvent Socket { get; }


        public StandardStreamEvent(ISocketClientEvent socket, bool isServer = false) : base(socket, isServer)
        {
            Socket = socket;
            Socket.Connected    += (e) => Connected?.Invoke(new PacketStreamConnectedArgs(this));
            Socket.Disconnected += (e) => Disconnected?.Invoke(new PacketStreamDisconnectedArgs(this, e.Reason));
            Socket.DataReceived += (e) => DataReceived?.Invoke(new PacketStreamDataReceivedArgs(this, e.Data));
        }


        public void Send(byte[] buffer) { Socket.Write(buffer, 0, buffer.Length); }
        public byte[] Receive(int length) { throw new NotSupportedException(); }
    }
}