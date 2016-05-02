using System;
using System.Linq;
using System.Reflection;

using Aragas.Network.PacketHandlers;
using Aragas.Network.Packets;

namespace Aragas.Network.Extensions
{
    public static class EnumExtensions
    {
        /// <summary/>
        /// <typeparam name="T"/><param name="className"/><param name="assembly"/>
        /// <returns/>
        private static Type GetTypeFromNameAndInterface<T>(string className, Assembly assembly) => assembly.DefinedTypes.Where(typeInfo => typeInfo.Name == className)
            .SelectMany(typeInfo => typeInfo.ImplementedInterfaces, (typeInfo, type) => new {typeInfo, type}).Where(@t => @t.type == typeof (T)).Select(@t => @t.typeInfo.AsType()).FirstOrDefault();

        /// <summary/>
        /// <typeparam name="T"/><param name="className"/><param name="assembly"/>
        /// <returns/>
        private static Type GetTypeFromNameAndAbstract<T>(string className, Assembly assembly) => assembly.DefinedTypes.Where(typeInfo => typeInfo.Name == className && typeInfo.IsSubclassOf(typeof (T)))
            .Select(typeInfo => typeInfo.AsType()).FirstOrDefault();

        /// <summary/>
        /// <param name="className"/><param name="assembly"/>
        /// <returns/>
        private static Type GetTypeFromName(string className, Assembly assembly) => assembly.DefinedTypes.Where(typeInfo => typeInfo.Name == className)
            .Select(typeInfo => typeInfo.AsType()).FirstOrDefault();


        public static Func<TPacket>[] CreatePacketInstances<TPacket>(this Enum packetType, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            var packets = new Func<TPacket>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = GetTypeFromNameAndAbstract<TPacket>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<TPacket>) (() => (TPacket) ActivatorCached.CreateInstance(type)) : null;
            }

            return packets;
        }
        public static void CreatePacketInstancesOut<TPacket>(this Enum packetType, out Func<TPacket>[] packets, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            packets = new Func<TPacket>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = GetTypeFromNameAndAbstract<TPacket>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<TPacket>) (() => (TPacket) ActivatorCached.CreateInstance(type)) : null;
            }
        }
        public static void CreatePacketInstancesRef<TPacket>(this Enum packetType, ref Func<TPacket>[] packets, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());

            var size = typeNames.Cast<int>().Max() + 1;
            if (packets == null)
                packets = new Func<TPacket>[size];
            else
                Array.Resize(ref packets, size);

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = GetTypeFromNameAndAbstract<TPacket>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<TPacket>) (() => (TPacket) ActivatorCached.CreateInstance(type)) : null;
            }
        }


        public static Func<IPacketHandlerContext, ContextFunc<TPacket>>[] CreateHandlerInstances<TPacket>(this Enum packetType, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            var packets = new Func<IPacketHandlerContext, ContextFunc<TPacket>>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Handler";
                var type = GetTypeFromName(typeName, assembly);
                if (type != null)
                    packets[(int) packetName] = context => new ContextFunc<TPacket>((PacketHandler) ActivatorCached.CreateInstance(type)).SetContext(context);
                else
                    packets[(int) packetName] = null;
            }

            return packets;
        }
        public static void CreateHandlerInstancesOut<TPacket>(this Enum packetType, out Func<IPacketHandlerContext, ContextFunc<TPacket>>[] packets, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            packets = new Func<IPacketHandlerContext, ContextFunc<TPacket>>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Handler";
                var type = GetTypeFromName(typeName, assembly);
                if (type != null)
                    packets[(int) packetName] = context => new ContextFunc<TPacket>((PacketHandler) ActivatorCached.CreateInstance(type)).SetContext(context);
                else
                    packets[(int) packetName] = null;
            }
        }
        public static void CreateHandlerInstancesRef<TPacket>(this Enum packetType, ref Func<IPacketHandlerContext, ContextFunc<TPacket>>[] packets, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());

            var size = typeNames.Cast<int>().Max() + 1;
            if (packets == null)
                packets = new Func<IPacketHandlerContext, ContextFunc<TPacket>>[size];
            else
                Array.Resize(ref packets, size);

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Handler";
                var type = GetTypeFromName(typeName, assembly);
                if (type != null)
                    packets[(int) packetName] = context => new ContextFunc<TPacket>((PacketHandler) ActivatorCached.CreateInstance(type)).SetContext(context);
                else
                    packets[(int) packetName] = null;
            }
        }
    }
}