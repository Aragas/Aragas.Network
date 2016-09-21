using System;

namespace Aragas.Network.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PacketAttribute : Attribute
    {
        public int ID { get; }

        public PacketAttribute(int id) { ID = id; }
    }
}