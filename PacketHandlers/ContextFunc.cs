using Aragas.Network.Packets;

namespace Aragas.Network.PacketHandlers
{
    /// <summary>
    /// The usage is <see cref="ContextFunc{TPacket}"/>.SetContext(<see cref="IPacketHandlerContext"/>).Handle(<see cref="Packet"/>).
    /// This means before calling Handle(<see cref="Packet"/>) call SetContext(<see cref="IPacketHandlerContext"/>)!
    /// </summary>
    /// <typeparam name="TPacket"></typeparam>
    public class ContextFunc<TPacket> where TPacket : Packet
    {
        private readonly dynamic _instance;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public ContextFunc(PacketHandler instance) { _instance = instance; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public TPacket Handle(dynamic packet) { return _instance.Handle(packet); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ContextFunc<TPacket> SetContext(dynamic context) { _instance.Context = context; return this; }
    }
}