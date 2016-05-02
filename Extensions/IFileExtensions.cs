using System;

using Org.BouncyCastle.Crypto.Digests;

using PCLExt.FileStorage;

namespace Aragas.Network.Extensions
{
    public static class IFileExtensions
    {
        public static string MD5Hash(this IFile file)
        {
            using (var fileStream = file.OpenAsync(FileAccess.Read).Result)
            {
                var data = fileStream.ReadFully();

                var md5 = new MD5Digest();
                var md5Hash = new byte[md5.GetDigestSize()];
                md5.BlockUpdate(data, 0, data.Length);
                md5.DoFinal(md5Hash, 0);
                md5.Reset();

                return BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            }
        }
    }
}
