using System;

namespace Aragas.Network.Attributes
{
    public class PacketAttribute : Attribute
    {
        public int ID { get; }

        public PacketAttribute(int id) { ID = id; }
    }
}
