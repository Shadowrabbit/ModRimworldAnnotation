using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x0200220A RID: 8714
	public class DeflateStream : Stream
	{
		// Token: 0x0600BB05 RID: 47877 RVA: 0x000790D8 File Offset: 0x000772D8
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600BB06 RID: 47878 RVA: 0x000790E4 File Offset: 0x000772E4
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600BB07 RID: 47879 RVA: 0x000790F0 File Offset: 0x000772F0
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600BB08 RID: 47880 RVA: 0x000790FC File Offset: 0x000772FC
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x17001BDE RID: 7134
		// (get) Token: 0x0600BB09 RID: 47881 RVA: 0x00079120 File Offset: 0x00077320
		// (set) Token: 0x0600BB0A RID: 47882 RVA: 0x0007912D File Offset: 0x0007732D
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17001BDF RID: 7135
		// (get) Token: 0x0600BB0B RID: 47883 RVA: 0x0007914E File Offset: 0x0007734E
		// (set) Token: 0x0600BB0C RID: 47884 RVA: 0x0035CC20 File Offset: 0x0035AE20
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x17001BE0 RID: 7136
		// (get) Token: 0x0600BB0D RID: 47885 RVA: 0x0007915B File Offset: 0x0007735B
		// (set) Token: 0x0600BB0E RID: 47886 RVA: 0x00079168 File Offset: 0x00077368
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x17001BE1 RID: 7137
		// (get) Token: 0x0600BB0F RID: 47887 RVA: 0x00079189 File Offset: 0x00077389
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17001BE2 RID: 7138
		// (get) Token: 0x0600BB10 RID: 47888 RVA: 0x0007919B File Offset: 0x0007739B
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600BB11 RID: 47889 RVA: 0x0035CC8C File Offset: 0x0035AE8C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17001BE3 RID: 7139
		// (get) Token: 0x0600BB12 RID: 47890 RVA: 0x000791AD File Offset: 0x000773AD
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17001BE4 RID: 7140
		// (get) Token: 0x0600BB13 RID: 47891 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BE5 RID: 7141
		// (get) Token: 0x0600BB14 RID: 47892 RVA: 0x000791D2 File Offset: 0x000773D2
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600BB15 RID: 47893 RVA: 0x000791F7 File Offset: 0x000773F7
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17001BE6 RID: 7142
		// (get) Token: 0x0600BB16 RID: 47894 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17001BE7 RID: 7143
		// (get) Token: 0x0600BB17 RID: 47895 RVA: 0x0035CCD8 File Offset: 0x0035AED8
		// (set) Token: 0x0600BB18 RID: 47896 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600BB19 RID: 47897 RVA: 0x00079217 File Offset: 0x00077417
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600BB1A RID: 47898 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600BB1B RID: 47899 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600BB1C RID: 47900 RVA: 0x0007923A File Offset: 0x0007743A
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600BB1D RID: 47901 RVA: 0x0035CD24 File Offset: 0x0035AF24
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600BB1E RID: 47902 RVA: 0x0035CD6C File Offset: 0x0035AF6C
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600BB1F RID: 47903 RVA: 0x0035CDB4 File Offset: 0x0035AFB4
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600BB20 RID: 47904 RVA: 0x0035CDF8 File Offset: 0x0035AFF8
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04007FD4 RID: 32724
		internal ZlibBaseStream _baseStream;

		// Token: 0x04007FD5 RID: 32725
		internal Stream _innerStream;

		// Token: 0x04007FD6 RID: 32726
		private bool _disposed;
	}
}
