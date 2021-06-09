using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x0200220B RID: 8715
	public class GZipStream : Stream
	{
		// Token: 0x17001BE8 RID: 7144
		// (get) Token: 0x0600BB21 RID: 47905 RVA: 0x0007925D File Offset: 0x0007745D
		// (set) Token: 0x0600BB22 RID: 47906 RVA: 0x00079265 File Offset: 0x00077465
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x17001BE9 RID: 7145
		// (get) Token: 0x0600BB23 RID: 47907 RVA: 0x00079281 File Offset: 0x00077481
		// (set) Token: 0x0600BB24 RID: 47908 RVA: 0x0035CE3C File Offset: 0x0035B03C
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x17001BEA RID: 7146
		// (get) Token: 0x0600BB25 RID: 47909 RVA: 0x00079289 File Offset: 0x00077489
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x0600BB26 RID: 47910 RVA: 0x00079291 File Offset: 0x00077491
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600BB27 RID: 47911 RVA: 0x0007929D File Offset: 0x0007749D
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600BB28 RID: 47912 RVA: 0x000792A9 File Offset: 0x000774A9
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600BB29 RID: 47913 RVA: 0x000792B5 File Offset: 0x000774B5
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x17001BEB RID: 7147
		// (get) Token: 0x0600BB2A RID: 47914 RVA: 0x000792D2 File Offset: 0x000774D2
		// (set) Token: 0x0600BB2B RID: 47915 RVA: 0x000792DF File Offset: 0x000774DF
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
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17001BEC RID: 7148
		// (get) Token: 0x0600BB2C RID: 47916 RVA: 0x00079300 File Offset: 0x00077500
		// (set) Token: 0x0600BB2D RID: 47917 RVA: 0x0035CEDC File Offset: 0x0035B0DC
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
					throw new ObjectDisposedException("GZipStream");
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

		// Token: 0x17001BED RID: 7149
		// (get) Token: 0x0600BB2E RID: 47918 RVA: 0x0007930D File Offset: 0x0007750D
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17001BEE RID: 7150
		// (get) Token: 0x0600BB2F RID: 47919 RVA: 0x0007931F File Offset: 0x0007751F
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600BB30 RID: 47920 RVA: 0x0035CF48 File Offset: 0x0035B148
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17001BEF RID: 7151
		// (get) Token: 0x0600BB31 RID: 47921 RVA: 0x00079331 File Offset: 0x00077531
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17001BF0 RID: 7152
		// (get) Token: 0x0600BB32 RID: 47922 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BF1 RID: 7153
		// (get) Token: 0x0600BB33 RID: 47923 RVA: 0x00079356 File Offset: 0x00077556
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600BB34 RID: 47924 RVA: 0x0007937B File Offset: 0x0007757B
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17001BF2 RID: 7154
		// (get) Token: 0x0600BB35 RID: 47925 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17001BF3 RID: 7155
		// (get) Token: 0x0600BB36 RID: 47926 RVA: 0x0035CFA8 File Offset: 0x0035B1A8
		// (set) Token: 0x0600BB37 RID: 47927 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600BB38 RID: 47928 RVA: 0x0035D00C File Offset: 0x0035B20C
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x0600BB39 RID: 47929 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600BB3A RID: 47930 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600BB3B RID: 47931 RVA: 0x0035D06C File Offset: 0x0035B26C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600BB3C RID: 47932 RVA: 0x0035D0CC File Offset: 0x0035B2CC
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			byte[] array3 = new byte[10 + num + num2];
			int num3 = 0;
			array3[num3++] = 31;
			array3[num3++] = 139;
			array3[num3++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num3++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			Array.Copy(BitConverter.GetBytes((int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds), 0, array3, num3, 4);
			num3 += 4;
			array3[num3++] = 0;
			array3[num3++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num3, num2 - 1);
				num3 += num2 - 1;
				array3[num3++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num3, num - 1);
				num3 += num - 1;
				array3[num3++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x0600BB3D RID: 47933 RVA: 0x0035D268 File Offset: 0x0035B468
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600BB3E RID: 47934 RVA: 0x0035D2B0 File Offset: 0x0035B4B0
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600BB3F RID: 47935 RVA: 0x0035D2F8 File Offset: 0x0035B4F8
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600BB40 RID: 47936 RVA: 0x0035D33C File Offset: 0x0035B53C
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04007FD7 RID: 32727
		public DateTime? LastModified;

		// Token: 0x04007FD8 RID: 32728
		private int _headerByteCount;

		// Token: 0x04007FD9 RID: 32729
		internal ZlibBaseStream _baseStream;

		// Token: 0x04007FDA RID: 32730
		private bool _disposed;

		// Token: 0x04007FDB RID: 32731
		private bool _firstReadDone;

		// Token: 0x04007FDC RID: 32732
		private string _FileName;

		// Token: 0x04007FDD RID: 32733
		private string _Comment;

		// Token: 0x04007FDE RID: 32734
		private int _Crc32;

		// Token: 0x04007FDF RID: 32735
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04007FE0 RID: 32736
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
