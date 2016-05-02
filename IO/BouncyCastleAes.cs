using System.IO;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

using PCLExt.Network;

namespace Aragas.Network.IO
{
    /// <summary>
    /// BouncyCastle's AesStream implementation.
    /// </summary>
    public partial class BouncyCastleAes : AesStream
    {
        private Stream Stream { get; }
        protected override Stream BaseStream => Stream;

        private BufferedBlockCipher DecryptCipher { get; }
        private BufferedBlockCipher EncryptCipher { get; }

        public BouncyCastleAes(ITCPClient tcp, byte[] key)
        {
            Stream = new TCPClientStream(tcp);

            EncryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            EncryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            DecryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            DecryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }

        public BouncyCastleAes(Stream stream, byte[] key)
        {
            Stream = stream;

            EncryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            EncryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            DecryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            DecryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }
    }
}
