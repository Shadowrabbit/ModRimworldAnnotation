using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02001829 RID: 6185
	public class DeflateStream : Stream
	{
		// Token: 0x0600914D RID: 37197 RVA: 0x00342751 File Offset: 0x00340951
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600914E RID: 37198 RVA: 0x0034275D File Offset: 0x0034095D
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600914F RID: 37199 RVA: 0x00342769 File Offset: 0x00340969
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06009150 RID: 37200 RVA: 0x00342775 File Offset: 0x00340975
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x06009151 RID: 37201 RVA: 0x00342799 File Offset: 0x00340999
		// (set) Token: 0x06009152 RID: 37202 RVA: 0x003427A6 File Offset: 0x003409A6
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

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x06009153 RID: 37203 RVA: 0x003427C7 File Offset: 0x003409C7
		// (set) Token: 0x06009154 RID: 37204 RVA: 0x003427D4 File Offset: 0x003409D4
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

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x06009155 RID: 37205 RVA: 0x00342840 File Offset: 0x00340A40
		// (set) Token: 0x06009156 RID: 37206 RVA: 0x0034284D File Offset: 0x00340A4D
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

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x06009157 RID: 37207 RVA: 0x0034286E File Offset: 0x00340A6E
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x06009158 RID: 37208 RVA: 0x00342880 File Offset: 0x00340A80
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06009159 RID: 37209 RVA: 0x00342894 File Offset: 0x00340A94
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

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x0600915A RID: 37210 RVA: 0x003428E0 File Offset: 0x00340AE0
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

		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x0600915B RID: 37211 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x0600915C RID: 37212 RVA: 0x00342905 File Offset: 0x00340B05
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

		// Token: 0x0600915D RID: 37213 RVA: 0x0034292A File Offset: 0x00340B2A
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x0600915E RID: 37214 RVA: 0x0002974C File Offset: 0x0002794C
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x0600915F RID: 37215 RVA: 0x0034294C File Offset: 0x00340B4C
		// (set) Token: 0x06009160 RID: 37216 RVA: 0x0002974C File Offset: 0x0002794C
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

		// Token: 0x06009161 RID: 37217 RVA: 0x00342998 File Offset: 0x00340B98
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06009162 RID: 37218 RVA: 0x0002974C File Offset: 0x0002794C
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009163 RID: 37219 RVA: 0x0002974C File Offset: 0x0002794C
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009164 RID: 37220 RVA: 0x003429BB File Offset: 0x00340BBB
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06009165 RID: 37221 RVA: 0x003429E0 File Offset: 0x00340BE0
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

		// Token: 0x06009166 RID: 37222 RVA: 0x00342A28 File Offset: 0x00340C28
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

		// Token: 0x06009167 RID: 37223 RVA: 0x00342A70 File Offset: 0x00340C70
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

		// Token: 0x06009168 RID: 37224 RVA: 0x00342AB4 File Offset: 0x00340CB4
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

		// Token: 0x04005B65 RID: 23397
		internal ZlibBaseStream _baseStream;

		// Token: 0x04005B66 RID: 23398
		internal Stream _innerStream;

		// Token: 0x04005B67 RID: 23399
		private bool _disposed;
	}
}
