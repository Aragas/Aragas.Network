using System;

namespace Aragas.Network.IO
{
    public abstract class PacketStreamEventArgs : EventArgs
    {
        public PacketStream Stream { get; set; }

        public PacketStreamEventArgs(PacketStream stream) { Stream = stream; }
    }

    public delegate void PacketStreamConnectedEventArgs(PacketStreamConnectedArgs args);
    public class PacketStreamConnectedArgs : PacketStreamEventArgs
    {
        public PacketStreamConnectedArgs(PacketStream stream) : base(stream) { }
    }

    public delegate void PacketStreamDataReceivedEventArgs(PacketStreamDataReceivedArgs args);
    public class PacketStreamDataReceivedArgs : PacketStreamEventArgs
    {
        public byte[] Data { get; set; }

        public PacketStreamDataReceivedArgs(PacketStream stream, byte[] data) : base(stream) { Data = data; }
    }

    public delegate void PacketStreamDisconnectedEventArgs(PacketStreamDisconnectedArgs args);
    public class PacketStreamDisconnectedArgs : PacketStreamEventArgs
    {
        public string Reason { get; set; }

        public PacketStreamDisconnectedArgs(PacketStream stream, string reason) : base(stream) { Reason = reason; }
    }
}