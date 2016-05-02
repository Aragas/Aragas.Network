using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// Base class.
    /// </summary>
    public abstract class Packet { }

    /// <summary>
    /// The "constructor" for a custom <see cref="Packet"/>. Use this to create a new <see cref="Packet"/>.
    /// </summary>
    /// <typeparam name="TIDType"><see cref="Packet"/>'s unique ID type. It will be used to differentiate <see cref="Packet"/>'s</typeparam>
    /// <typeparam name="TPacketType">Put here the new <see cref="Packet"/> type.</typeparam>
    /// <typeparam name="TReader"><see cref="PacketDataReader"/>. You can create a custom one or use <see cref="StandardDataReader"/> and <see cref="ProtobufDataReader"/></typeparam>
    /// <typeparam name="TWriter"><see cref="PacketStream"/>. You can create a custom one or use <see cref="StandardStream"/> and <see cref="ProtobufStream"/></typeparam>
    public abstract class Packet<TIDType, TPacketType, TReader, TWriter> : Packet where TPacketType : Packet where TReader : PacketDataReader where TWriter : PacketStream
    {
        public abstract TIDType ID { get; }

        /// <summary>
        /// Read packet from <see cref="PacketDataReader"/>.
        /// </summary>
        public abstract TPacketType ReadPacket(TReader reader);

        /// <summary>
        /// Write packet to <see cref="PacketStream"/>.
        /// </summary>
        public abstract TPacketType WritePacket(TWriter writer);
    }
}
