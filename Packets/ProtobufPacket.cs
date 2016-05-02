using Aragas.Network.Data;
using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// <see cref="Packet"/> with .NET and Variant data types.
    /// </summary>
    public abstract class ProtobufPacket : Packet<VarInt, ProtobufPacket, ProtobufDataReader, ProtobufStream> { }
}