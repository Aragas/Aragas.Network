using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.Network.IO
{
    /// <summary>
    /// BouncyCastle's AesStream implementation.
    /// </summary>
    public partial class BouncyCastleAes
    {
        public override bool CanRead => BaseStream.CanRead;
        public override bool CanSeek => BaseStream.CanSeek;
        public override bool CanWrite => BaseStream.CanWrite;
        public override long Length => BaseStream.Length;

        public override long Position { get { return BaseStream.Position; } set { BaseStream.Position = value; } }

        public override int WriteTimeout { get { return BaseStream.WriteTimeout; } set { BaseStream.WriteTimeout = value; } }

        public override int ReadTimeout { get { return BaseStream.ReadTimeout; } set { BaseStream.ReadTimeout = value; } }

        public override bool CanTimeout => BaseStream.CanTimeout;

        public override void Flush() { BaseStream.Flush(); }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var length = Stream.Read(buffer, offset, count);
            var decrypted = DecryptCipher.ProcessBytes(buffer, 0, length);
            Buffer.BlockCopy(decrypted, 0, buffer, offset, decrypted.Length);
            return length;
        }

        public override int ReadByte()
        {
            var @byte = new byte[1];
            var length = Read(@byte, 0, 1);
            return @byte[0];
        }

        public override long Seek(long offset, SeekOrigin origin) { return BaseStream.Seek(offset, origin); }

        public override void SetLength(long value) { BaseStream.SetLength(value); }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var encrypted = EncryptCipher.ProcessBytes(buffer, offset, count);
            Stream.Write(encrypted, 0, encrypted.Length);
        }

        public override void WriteByte(byte value)
        {
            var encrypted = EncryptCipher.ProcessByte(value);
            Stream.Write(encrypted, 0, encrypted.Length);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) { return BaseStream.WriteAsync(buffer, offset, count, cancellationToken); }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) { return BaseStream.ReadAsync(buffer, offset, count, cancellationToken); }

        public override Task FlushAsync(CancellationToken cancellationToken) { return BaseStream.FlushAsync(cancellationToken); }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) { return BaseStream.CopyToAsync(destination, bufferSize, cancellationToken); }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            BaseStream?.Dispose();
        }
    }
}