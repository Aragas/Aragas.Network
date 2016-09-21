using System;
using System.Collections.Generic;

namespace Aragas.Network.IO
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PacketDataReader : IDisposable
    {
        public abstract bool IsServer { get; }


        #region ExtendRead

        private static readonly Dictionary<int, Func<PacketDataReader, int, object>> ReadExtendedList = new Dictionary<int, Func<PacketDataReader, int, object>>();

        public static void ExtendRead<T>(Func<PacketDataReader, int, T> func)
        {
            if(func != null)
                ReadExtendedList.Add(typeof(T).GetHashCode(), Transform(func));
        }
        private static Func<PacketDataReader, int, object> Transform<T>(Func<PacketDataReader, int, T> action) => (reader, length) => action(reader, length);

        protected static bool ExtendReadContains<T>() => ExtendReadContains(typeof(T));
        protected static bool ExtendReadContains(Type type) => ReadExtendedList.ContainsKey(type.GetHashCode());

        protected static T ExtendReadExecute<T>(PacketDataReader reader, int length = 0) => ExtendReadContains<T>() ? (T) ReadExtendedList[typeof (T).GetHashCode()](reader, length) : default(T);
        protected static bool ExtendReadTryExecute<T>(PacketDataReader reader, int length, out T value)
        {
            Func<PacketDataReader, int, object> func;
            var exist = ReadExtendedList.TryGetValue(typeof(T).GetHashCode(), out func);
            value = exist ? (T) func.Invoke(reader, length) : default(T);
            
            return exist;
        }

        #endregion ExtendRead


        public abstract T Read<T>(T value = default(T), int length = 0);

        public abstract int BytesLeft();

        
        public abstract void Dispose();
    }
}