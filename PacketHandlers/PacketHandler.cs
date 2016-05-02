using Aragas.Network.Packets;

namespace Aragas.Network.PacketHandlers
{
    /// <summary>
    /// Basic class.
    /// </summary>
    public abstract class PacketHandler { }
    /// <summary>
    /// The "constructor" for a custom <see cref="PacketHandler"/>. Use this to create a new <see cref="PacketHandler"/>.
    /// </summary>
    /// <typeparam name="TRequestPacket">The <see cref="Packet"/> that is handled.</typeparam>
    /// <typeparam name="TReplyPacket">The <see cref="Packet"/> that will be returned as a response.</typeparam>
    /// <typeparam name="TContext">The context in which the <see cref="PacketHandler"/> will operate.</typeparam>
    public abstract class PacketHandler<TRequestPacket, TReplyPacket, TContext> : PacketHandler where TRequestPacket : Packet where TReplyPacket : Packet where TContext : IPacketHandlerContext
    {
        /// <summary>
        /// The <see cref="PacketHandler"/>'s context.
        /// </summary>
        public TContext Context { protected get; set; }

        /// <summary>
        /// The actual handle function.
        /// </summary>
        public abstract TReplyPacket Handle(TRequestPacket packet);
    }
}
