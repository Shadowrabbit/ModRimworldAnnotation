using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02001840 RID: 6208
	public class ZlibStream : Stream
	{
		// Token: 0x06009213 RID: 37395 RVA: 0x00348A86 File Offset: 0x00346C86
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06009214 RID: 37396 RVA: 0x00348A92 File Offset: 0x00346C92
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06009215 RID: 37397 RVA: 0x00348A9E File Offset: 0x00346C9E
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06009216 RID: 37398 RVA: 0x00348AAA File Offset: 0x00346CAA
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x06009217 RID: 37399 RVA: 0x00348AC7 File Offset: 0x00346CC7
		// (set) Token: 0x06009218 RID: 37400 RVA: 0x00348AD4 File Offset: 0x00346CD4
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

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x06009219 RID: 37401 RVA: 0x00348AF5 File Offset: 0x00346CF5
		// (set) Token: 0x0600921A RID: 37402 RVA: 0x00348B04 File Offset: 0x00346D04
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

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x0600921B RID: 37403 RVA: 0x00348B70 File Offset: 0x00346D70
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x0600921C RID: 37404 RVA: 0x00348B82 File Offset: 0x00346D82
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600921D RID: 37405 RVA: 0x00348B94 File Offset: 0x00346D94
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

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x0600921E RID: 37406 RVA: 0x00348BE0 File Offset: 0x00346DE0
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

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x0600921F RID: 37407 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x06009220 RID: 37408 RVA: 0x00348C05 File Offset: 0x00346E05
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

		// Token: 0x06009221 RID: 37409 RVA: 0x00348C2A File Offset: 0x00346E2A
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x06009222 RID: 37410 RVA: 0x00347025 File Offset: 0x00345225
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x06009223 RID: 37411 RVA: 0x00348C4C File Offset: 0x00346E4C
		// (set) Token: 0x06009224 RID: 37412 RVA: 0x00347025 File Offset: 0x00345225
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

		// Token: 0x06009225 RID: 37413 RVA: 0x00348C98 File Offset: 0x00346E98
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06009226 RID: 37414 RVA: 0x00347025 File Offset: 0x00345225
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06009227 RID: 37415 RVA: 0x00347025 File Offset: 0x00345225
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06009228 RID: 37416 RVA: 0x00348CBB File Offset: 0x00346EBB
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06009229 RID: 37417 RVA: 0x00348CE0 File Offset: 0x00346EE0
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

		// Token: 0x0600922A RID: 37418 RVA: 0x00348D28 File Offset: 0x00346F28
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

		// Token: 0x0600922B RID: 37419 RVA: 0x00348D70 File Offset: 0x00346F70
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

		// Token: 0x0600922C RID: 37420 RVA: 0x00348DB4 File Offset: 0x00346FB4
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

		// Token: 0x04005C58 RID: 23640
		internal ZlibBaseStream _baseStream;

		// Token: 0x04005C59 RID: 23641
		private bool _disposed;
	}
}
