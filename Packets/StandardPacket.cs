using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// <see cref="Packet"/> with .NET data types.
    /// </summary>
    public abstract class StandardPacket : Packet<ProtobufPacket, StandardDataReader, StandardStream> { }
}