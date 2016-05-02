using System.IO;

namespace Aragas.Network.IO
{
    /// <summary>
    /// Object that implements AES.
    /// </summary>
    public abstract class AesStream : Stream
    {
        protected abstract Stream BaseStream { get; }
    }
}