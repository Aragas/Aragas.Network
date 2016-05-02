using System;
using System.IO;

using PCLExt.Network;

namespace Aragas.Network.IO
{
    public sealed class TCPClientStream : Stream
    {
        public override bool CanRead { get; }
        public override bool CanSeek { get; }
        public override bool CanWrite { get; }
        public override long Length { get; }
        public override long Position { get; set; }

        private readonly ITCPClient _client;


        public TCPClientStream(ITCPClient client) { _client = client; }


        public override void Write(byte[] buffer, int offset, int count) { _client.Write(buffer, offset, count); }
        public override int Read(byte[] buffer, int offset, int count) { return _client.Read(buffer, offset, count); }

        public override void Flush() { throw new NotImplementedException(); }
        public override long Seek(long offset, SeekOrigin origin) { throw new NotImplementedException(); }
        public override void SetLength(long value) { throw new NotImplementedException(); }
    }
}