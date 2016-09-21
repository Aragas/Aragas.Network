using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// <see cref="Packet{TIDType, TPacketType, TReader, TWriter}"/> with .NET data types.
    /// </summary>
    public abstract class StandardPacket : Packet<int, StandardPacket, StandardDataReader, StandardStream> { }

    /// <summary>
    /// <see cref="PacketWithAttribute{TPacketType, TReader, TWriter}"/> with .NET data types.
    /// </summary>
    public abstract class StandardPacketAttribute : PacketWithAttribute<StandardPacketAttribute, StandardDataReader, StandardStream> { }
}