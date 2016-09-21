using System;
using System.Text;

using Org.BouncyCastle.Crypto.Digests;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Class that will store only password's hash. Hash type is Sha-512.
    /// </summary>
    public class PasswordStorage
    {
        private const string NoPassword = "PUT_PASSWORD_HERE";

        /// <summary>
        /// Password's hash. Hash type is Sha-512.
        /// </summary>
        public string Hash
        {
            get
            {
                if (!string.IsNullOrEmpty(Password))
                    HashPassword();
                
                return _hash;
            }
            private set { _hash = value; }
        }
        private string _hash = string.Empty;

        /// <summary>
        /// Temporary storage. Will be set empty after <see cref="Hash"/> is requested. 
        /// You can set the password here instead of creating a new instance.
        /// </summary>
        public string Password { get; set; } = NoPassword;


        /// <summary>
        /// Empty constructor. Set the password in <see cref="Password"/>.
        /// </summary>
        public PasswordStorage() { }
        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="data">The password/the password's Sha-512 hash.</param>
        /// <param name="doHash">Set true is password needs to be hashed.</param>
        public PasswordStorage(string data, bool doHash = true)
        {
            if (doHash)
            {
                Password = data;
                HashPassword();
            }
            else
            {
                Hash = data;
                Password = string.Empty;
            }
        }


        private void HashPassword()
        {
            if (string.IsNullOrEmpty(Password))
                Password = NoPassword;

            var pswBytes = Encoding.UTF8.GetBytes(Password);

            var sha512 = new Sha512Digest();
            var hashedPassword = new byte[sha512.GetDigestSize()];
            sha512.BlockUpdate(pswBytes, 0, pswBytes.Length);
            sha512.DoFinal(hashedPassword, 0);

            Hash = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();
            Password = string.Empty;
        }
        
        /// <summary>
        /// Returns <see cref="Hash"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Hash;
    }
}