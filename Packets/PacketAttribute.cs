using System.Reflection;

using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// A <see cref="Packet"/> with <see langword="int"/> <seealso cref="PacketWithAttribute{TPacketType, TReader, TWriter}.ID"/>. Must have an <seealso cref="PacketAttribute"/>.
    /// </summary>
    /// <typeparam name="TPacketType">Put here the new <see cref="Packet"/> type.</typeparam>
    /// <typeparam name="TReader"><see cref="PacketDataReader"/>. You can create a custom one or use <see cref="StandardDataReader"/> and <see cref="ProtobufDataReader"/></typeparam>
    /// <typeparam name="TWriter"><see cref="PacketStream"/>. You can create a custom one or use <see cref="StandardStream"/> and <see cref="ProtobufStream"/></typeparam>
    public abstract class PacketWithAttribute<TPacketType, TReader, TWriter> : Packet<int, TPacketType, TReader, TWriter> where TPacketType : Packet where TReader : PacketDataReader where TWriter : PacketStream
    {
        private int? _id;
        public sealed override int ID
        {
            get
            {
                if (_id != null)
                    return _id.Value;

                _id = GetType().GetTypeInfo().GetCustomAttribute<PacketAttribute>().ID;
                return _id.Value;
            }
        }
    }
}