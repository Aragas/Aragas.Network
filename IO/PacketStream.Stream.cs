using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.Network.IO
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class PacketStream : Stream
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

        public override int Read(byte[] buffer, int offset, int count) { return BaseStream.Read(buffer, offset, count); }

        public override int ReadByte() { return BaseStream.ReadByte(); }

        public override long Seek(long offset, SeekOrigin origin) { return BaseStream.Seek(offset, origin); }

        public override void SetLength(long value) { BaseStream.SetLength(value); }

        public override void Write(byte[] buffer, int offset, int count) { BaseStream.Write(buffer, offset, count); }

        public override void WriteByte(byte value) { BaseStream.WriteByte(value); }

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