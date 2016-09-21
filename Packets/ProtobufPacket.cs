using Aragas.Network.Data;
using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// <see cref="Packet{TIDType, TPacketType, TReader, TWriter}"/> with .NET and Variant data types.
    /// </summary>
    public abstract class ProtobufPacket : Packet<VarInt, ProtobufPacket, ProtobufDataReader, ProtobufStream> { }

    /// <summary>
    /// <see cref="PacketWithAttribute{TPacketType, TReader, TWriter}"/> with .NET and Variant data types.
    /// </summary>
    public abstract class ProtobufPacketAttribute : PacketWithAttribute<ProtobufPacketAttribute, ProtobufDataReader, ProtobufStream> { }
}