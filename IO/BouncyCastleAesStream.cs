using System.IO;

using PCLExt.Network;

namespace Aragas.Network.IO
{
    /// <summary>
    /// BouncyCastle's AesStream implementation.
    /// </summary>
    public partial class BouncyCastleAesStream : AesStream
    {
        private Stream Stream { get; set; }

        private BouncyCastle BouncyCastle { get; }


        public BouncyCastleAesStream(ISocketClient client, byte[] key)
        {
            Stream = new SocketClientStream(client);

            BouncyCastle = new BouncyCastle(key);
        }

        public BouncyCastleAesStream(Stream stream, byte[] key)
        {
            Stream = stream;

            BouncyCastle = new BouncyCastle(key);
        }
    }
}