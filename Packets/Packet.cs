using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// Base class.
    /// </summary>
    public abstract class Packet
    {
        private static IEnumerable<TypeInfo> GetTypeInfosFromAbstract<T>(IEnumerable<Assembly> assemblies) =>
            assemblies.SelectMany(assembly => assembly.DefinedTypes.Where(typeInfo => typeInfo.IsSubclassOf(typeof(T))));

        private static KeyValuePair<int, Func<TPacket>> GetPacketIDAndFunc<TPacket>(TypeInfo typeInfo) where TPacket : Packet =>
            new KeyValuePair<int, Func<TPacket>>(typeInfo.GetCustomAttribute<PacketAttribute>().ID, () => (TPacket) ActivatorCached.CreateInstance(typeInfo.AsType()));

        public static Dictionary<int, Func<TPacket>> CreateIDList<TPacket>(IEnumerable<Assembly> whereToFindPackets) where TPacket : Packet
        {
            var typeInfos = GetTypeInfosFromAbstract<TPacket>(whereToFindPackets);
            var typeInfosWithAttribute = typeInfos.Where(typeInfo => typeInfo.IsDefined(typeof(PacketAttribute), false));

            var packetDictionary = new Dictionary<int, Func<TPacket>>();
            foreach (var typeInfo in typeInfosWithAttribute)
            {
                var pair = GetPacketIDAndFunc<TPacket>(typeInfo);
                packetDictionary.Add(pair.Key, pair.Value);
            }
            return packetDictionary;
        }
    }

    /// <summary>
    /// The "constructor" for a custom <see cref="Packet"/>. Use this to create a new <see cref="Packet"/>.
    /// </summary>
    /// <typeparam name="TPacketType">Put here the new <see cref="Packet"/> type.</typeparam>
    /// <typeparam name="TReader"><see cref="PacketDataReader"/>. You can create a custom one or use <see cref="StandardDataReader"/> and <see cref="ProtobufDataReader"/></typeparam>
    /// <typeparam name="TWriter"><see cref="PacketStream"/>. You can create a custom one or use <see cref="StandardStream"/> and <see cref="ProtobufStream"/></typeparam>
    public abstract class Packet<TPacketType, TReader, TWriter> : Packet where TPacketType : Packet where TReader : PacketDataReader where TWriter : PacketStream
    {
        private int? _id;
        public virtual int ID
        {
            get
            {
                if (_id != null)
                    return _id.Value;

                _id = GetType().GetTypeInfo().GetCustomAttribute<PacketAttribute>().ID;
                return _id.Value;
            }
        }

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