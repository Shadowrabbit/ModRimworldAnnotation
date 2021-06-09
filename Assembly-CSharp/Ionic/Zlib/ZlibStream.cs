using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02002225 RID: 8741
	public class ZlibStream : Stream
	{
		// Token: 0x0600BBCB RID: 48075 RVA: 0x00079967 File Offset: 0x00077B67
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600BBCC RID: 48076 RVA: 0x00079973 File Offset: 0x00077B73
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600BBCD RID: 48077 RVA: 0x0007997F File Offset: 0x00077B7F
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600BBCE RID: 48078 RVA: 0x0007998B File Offset: 0x00077B8B
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x17001C09 RID: 7177
		// (get) Token: 0x0600BBCF RID: 48079 RVA: 0x000799A8 File Offset: 0x00077BA8
		// (set) Token: 0x0600BBD0 RID: 48080 RVA: 0x000799B5 File Offset: 0x00077BB5
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
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17001C0A RID: 7178
		// (get) Token: 0x0600BBD1 RID: 48081 RVA: 0x000799D6 File Offset: 0x00077BD6
		// (set) Token: 0x0600BBD2 RID: 48082 RVA: 0x003626B4 File Offset: 0x003608B4
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
					throw new ObjectDisposedException("ZlibStream");
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

		// Token: 0x17001C0B RID: 7179
		// (get) Token: 0x0600BBD3 RID: 48083 RVA: 0x000799E3 File Offset: 0x00077BE3
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17001C0C RID: 7180
		// (get) Token: 0x0600BBD4 RID: 48084 RVA: 0x000799F5 File Offset: 0x00077BF5
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600BBD5 RID: 48085 RVA: 0x00362720 File Offset: 0x00360920
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

		// Token: 0x17001C0D RID: 7181
		// (get) Token: 0x0600BBD6 RID: 48086 RVA: 0x00079A07 File Offset: 0x00077C07
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17001C0E RID: 7182
		// (get) Token: 0x0600BBD7 RID: 48087 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C0F RID: 7183
		// (get) Token: 0x0600BBD8 RID: 48088 RVA: 0x00079A2C File Offset: 0x00077C2C
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600BBD9 RID: 48089 RVA: 0x00079A51 File Offset: 0x00077C51
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17001C10 RID: 7184
		// (get) Token: 0x0600BBDA RID: 48090 RVA: 0x0000713A File Offset: 0x0000533A
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17001C11 RID: 7185
		// (get) Token: 0x0600BBDB RID: 48091 RVA: 0x0036276C File Offset: 0x0036096C
		// (set) Token: 0x0600BBDC RID: 48092 RVA: 0x0000713A File Offset: 0x0000533A
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
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600BBDD RID: 48093 RVA: 0x00079A71 File Offset: 0x00077C71
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600BBDE RID: 48094 RVA: 0x0000713A File Offset: 0x0000533A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600BBDF RID: 48095 RVA: 0x0000713A File Offset: 0x0000533A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600BBE0 RID: 48096 RVA: 0x00079A94 File Offset: 0x00077C94
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600BBE1 RID: 48097 RVA: 0x003627B8 File Offset: 0x003609B8
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600BBE2 RID: 48098 RVA: 0x00362800 File Offset: 0x00360A00
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600BBE3 RID: 48099 RVA: 0x00362848 File Offset: 0x00360A48
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600BBE4 RID: 48100 RVA: 0x0036288C File Offset: 0x00360A8C
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x040080F8 RID: 33016
		internal ZlibBaseStream _baseStream;

		// Token: 0x040080F9 RID: 33017
		private bool _disposed;
	}
}
