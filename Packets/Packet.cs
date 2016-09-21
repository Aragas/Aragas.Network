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
        #region Packet Build
        private static TypeInfo GetTypeFromNameAndAbstract<T>(string className, IEnumerable<Assembly> assemblies) =>
            assemblies.SelectMany(assembly => assembly.DefinedTypes.Where(typeInfo => typeInfo.IsSubclassOf(typeof(T))).Select(typeInfo => typeInfo)).FirstOrDefault();

        public static Dictionary<int, Func<TPacket>> CreateIDList<TPacket>(Type packetEnumType, IEnumerable<Assembly> whereToFindPackets) where TPacket : Packet
        {
            var packetDictionary = new Dictionary<int, Func<TPacket>>();

            var typeNames = Enum.GetValues(packetEnumType);

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = GetTypeFromNameAndAbstract<TPacket>(typeName, whereToFindPackets);
                packetDictionary.Add((int) packetName, type != null ? (Func<TPacket>) (() => (TPacket) ActivatorCached.CreateInstance(type.AsType())) : null);
            }

            return packetDictionary;
        }
        #endregion Packet Build

        #region Packet Attribute Build
        private static IEnumerable<TypeInfo> GetTypeInfosFromAbstract<T>(IEnumerable<Assembly> assemblies) =>
            assemblies.SelectMany(assembly => assembly.DefinedTypes.Where(typeInfo => typeInfo.IsSubclassOf(typeof(T))));

        private static KeyValuePair<int, Func<TPacket>> GetPacketIDAndFunc<TPacket>(TypeInfo typeInfo) where TPacket : Packet =>
            new KeyValuePair<int, Func<TPacket>>(typeInfo.GetCustomAttribute<PacketAttribute>().ID, () => (TPacket) ActivatorCached.CreateInstance(typeInfo.AsType()));

        public static Dictionary<int, Func<TPacket>> CreateIDListByAttribute<TPacket>(IEnumerable<Assembly> whereToFindPackets) where TPacket : Packet
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
        #endregion Packet Attribute Build
    }

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