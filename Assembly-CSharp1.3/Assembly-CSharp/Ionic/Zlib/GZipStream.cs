using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x0200182A RID: 6186
	public class GZipStream : Stream
	{
		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x06009169 RID: 37225 RVA: 0x00342AF8 File Offset: 0x00340CF8
		// (set) Token: 0x0600916A RID: 37226 RVA: 0x00342B00 File Offset: 0x00340D00
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

		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x0600916B RID: 37227 RVA: 0x00342B1C File Offset: 0x00340D1C
		// (set) Token: 0x0600916C RID: 37228 RVA: 0x00342B24 File Offset: 0x00340D24
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

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x0600916D RID: 37229 RVA: 0x00342BC3 File Offset: 0x00340DC3
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x0600916E RID: 37230 RVA: 0x00342BCB File Offset: 0x00340DCB
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600916F RID: 37231 RVA: 0x00342BD7 File Offset: 0x00340DD7
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06009170 RID: 37232 RVA: 0x00342BE3 File Offset: 0x00340DE3
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06009171 RID: 37233 RVA: 0x00342BEF File Offset: 0x00340DEF
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x06009172 RID: 37234 RVA: 0x00342C0C File Offset: 0x00340E0C
		// (set) Token: 0x06009173 RID: 37235 RVA: 0x00342C19 File Offset: 0x00340E19
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

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x06009174 RID: 37236 RVA: 0x00342C3A File Offset: 0x00340E3A
		// (set) Token: 0x06009175 RID: 37237 RVA: 0x00342C48 File Offset: 0x00340E48
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

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x06009176 RID: 37238 RVA: 0x00342CB4 File Offset: 0x00340EB4
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x06009177 RID: 37239 RVA: 0x00342CC6 File Offset: 0x00340EC6
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06009178 RID: 37240 RVA: 0x00342CD8 File Offset: 0x00340ED8
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

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x06009179 RID: 37241 RVA: 0x00342D38 File Offset: 0x00340F38
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

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x0600917A RID: 37242 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x0600917B RID: 37243 RVA: 0x00342D5D File Offset: 0x00340F5D
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

		// Token: 0x0600917C RID: 37244 RVA: 0x00342D82 File Offset: 0x00340F82
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x0600917D RID: 37245 RVA: 0x0002974C File Offset: 0x0002794C
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x0600917E RID: 37246 RVA: 0x00342DA4 File Offset: 0x00340FA4
		// (set) Token: 0x0600917F RID: 37247 RVA: 0x0002974C File Offset: 0x0002794C
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

		// Token: 0x06009180 RID: 37248 RVA: 0x00342E08 File Offset: 0x00341008
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

		// Token: 0x06009181 RID: 37249 RVA: 0x0002974C File Offset: 0x0002794C
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009182 RID: 37250 RVA: 0x0002974C File Offset: 0x0002794C
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009183 RID: 37251 RVA: 0x00342E68 File Offset: 0x00341068
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

		// Token: 0x06009184 RID: 37252 RVA: 0x00342EC8 File Offset: 0x003410C8
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

		// Token: 0x06009185 RID: 37253 RVA: 0x00343064 File Offset: 0x00341264
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

		// Token: 0x06009186 RID: 37254 RVA: 0x003430AC File Offset: 0x003412AC
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

		// Token: 0x06009187 RID: 37255 RVA: 0x003430F4 File Offset: 0x003412F4
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

		// Token: 0x06009188 RID: 37256 RVA: 0x00343138 File Offset: 0x00341338
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

		// Token: 0x04005B68 RID: 23400
		public DateTime? LastModified;

		// Token: 0x04005B69 RID: 23401
		private int _headerByteCount;

		// Token: 0x04005B6A RID: 23402
		internal ZlibBaseStream _baseStream;

		// Token: 0x04005B6B RID: 23403
		private bool _disposed;

		// Token: 0x04005B6C RID: 23404
		private bool _firstReadDone;

		// Token: 0x04005B6D RID: 23405
		private string _FileName;

		// Token: 0x04005B6E RID: 23406
		private string _Comment;

		// Token: 0x04005B6F RID: 23407
		private int _Crc32;

		// Token: 0x04005B70 RID: 23408
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04005B71 RID: 23409
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
