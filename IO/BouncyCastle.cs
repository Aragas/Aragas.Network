using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Aragas.Network.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class BouncyCastle
    {
        private BufferedBlockCipher DecryptCipher { get; }
        private BufferedBlockCipher EncryptCipher { get; }


        public BouncyCastle(byte[] key)
        {
            EncryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            EncryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            DecryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            DecryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }


        public byte[] Decrypt(byte[] buffer, int offset, int count) => DecryptCipher.ProcessBytes(buffer, offset, count);
        public byte[] Encrypt(byte[] buffer, int offset, int count) => EncryptCipher.ProcessBytes(buffer, offset, count);
    }
}