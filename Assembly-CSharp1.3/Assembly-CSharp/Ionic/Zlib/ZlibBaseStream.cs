using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x0200183D RID: 6205
	internal class ZlibBaseStream : Stream
	{
		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x060091E3 RID: 37347 RVA: 0x00347982 File Offset: 0x00345B82
		internal int Crc32
		{
			get
			{
				if (this.crc == null)
				{
					return 0;
				}
				return this.crc.Crc32Result;
			}
		}

		// Token: 0x060091E4 RID: 37348 RVA: 0x0034799C File Offset: 0x00345B9C
		public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen)
		{
			this._flushMode = FlushType.None;
			this._stream = stream;
			this._leaveOpen = leaveOpen;
			this._compressionMode = compressionMode;
			this._flavor = flavor;
			this._level = level;
			if (flavor == ZlibStreamFlavor.GZIP)
			{
				this.crc = new CRC32();
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x060091E5 RID: 37349 RVA: 0x00347A0D File Offset: 0x00345C0D
		protected internal bool _wantCompress
		{
			get
			{
				return this._compressionMode == CompressionMode.Compress;
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x060091E6 RID: 37350 RVA: 0x00347A18 File Offset: 0x00345C18
		private ZlibCodec z
		{
			get
			{
				if (this._z == null)
				{
					bool flag = this._flavor == ZlibStreamFlavor.ZLIB;
					this._z = new ZlibCodec();
					if (this._compressionMode == CompressionMode.Decompress)
					{
						this._z.InitializeInflate(flag);
					}
					else
					{
						this._z.Strategy = this.Strategy;
						this._z.InitializeDeflate(this._level, flag);
					}
				}
				return this._z;
			}
		}

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x060091E7 RID: 37351 RVA: 0x00347A88 File Offset: 0x00345C88
		private byte[] workingBuffer
		{
			get
			{
				if (this._workingBuffer == null)
				{
					this._workingBuffer = new byte[this._bufferSize];
				}
				return this._workingBuffer;
			}
		}

		// Token: 0x060091E8 RID: 37352 RVA: 0x00347AAC File Offset: 0x00345CAC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, count);
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				this._streamMode = ZlibBaseStream.StreamMode.Writer;
			}
			else if (this._streamMode != ZlibBaseStream.StreamMode.Writer)
			{
				throw new ZlibException("Cannot Write after Reading.");
			}
			if (count == 0)
			{
				return;
			}
			this.z.InputBuffer = buffer;
			this._z.NextIn = offset;
			this._z.AvailableBytesIn = count;
			for (;;)
			{
				this._z.OutputBuffer = this.workingBuffer;
				this._z.NextOut = 0;
				this._z.AvailableBytesOut = this._workingBuffer.Length;
				int num = this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode);
				if (num != 0 && num != 1)
				{
					break;
				}
				this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
				bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
				if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
				{
					flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
				}
				if (flag)
				{
					return;
				}
			}
			throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this._z.Message);
		}

		// Token: 0x060091E9 RID: 37353 RVA: 0x00347C34 File Offset: 0x00345E34
		private void finish()
		{
			if (this._z == null)
			{
				return;
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Writer)
			{
				int num;
				for (;;)
				{
					this._z.OutputBuffer = this.workingBuffer;
					this._z.NextOut = 0;
					this._z.AvailableBytesOut = this._workingBuffer.Length;
					num = (this._wantCompress ? this._z.Deflate(FlushType.Finish) : this._z.Inflate(FlushType.Finish));
					if (num != 1 && num != 0)
					{
						break;
					}
					if (this._workingBuffer.Length - this._z.AvailableBytesOut > 0)
					{
						this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
					}
					bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
					if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
					{
						flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
					}
					if (flag)
					{
						goto Block_12;
					}
				}
				string text = (this._wantCompress ? "de" : "in") + "flating";
				if (this._z.Message == null)
				{
					throw new ZlibException(string.Format("{0}: (rc = {1})", text, num));
				}
				throw new ZlibException(text + ": " + this._z.Message);
				Block_12:
				this.Flush();
				if (this._flavor == ZlibStreamFlavor.GZIP)
				{
					if (this._wantCompress)
					{
						int crc32Result = this.crc.Crc32Result;
						this._stream.Write(BitConverter.GetBytes(crc32Result), 0, 4);
						int value = (int)(this.crc.TotalBytesRead & (long)((ulong)-1));
						this._stream.Write(BitConverter.GetBytes(value), 0, 4);
						return;
					}
					throw new ZlibException("Writing with decompression is not supported.");
				}
			}
			else if (this._streamMode == ZlibBaseStream.StreamMode.Reader && this._flavor == ZlibStreamFlavor.GZIP)
			{
				if (this._wantCompress)
				{
					throw new ZlibException("Reading with compression is not supported.");
				}
				if (this._z.TotalBytesOut == 0L)
				{
					return;
				}
				byte[] array = new byte[8];
				if (this._z.AvailableBytesIn < 8)
				{
					Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, this._z.AvailableBytesIn);
					int num2 = 8 - this._z.AvailableBytesIn;
					int num3 = this._stream.Read(array, this._z.AvailableBytesIn, num2);
					if (num2 != num3)
					{
						throw new ZlibException(string.Format("Missing or incomplete GZIP trailer. Expected 8 bytes, got {0}.", this._z.AvailableBytesIn + num3));
					}
				}
				else
				{
					Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, array.Length);
				}
				int num4 = BitConverter.ToInt32(array, 0);
				int crc32Result2 = this.crc.Crc32Result;
				int num5 = BitConverter.ToInt32(array, 4);
				int num6 = (int)(this._z.TotalBytesOut & (long)((ulong)-1));
				if (crc32Result2 != num4)
				{
					throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", crc32Result2, num4));
				}
				if (num6 != num5)
				{
					throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", num6, num5));
				}
			}
		}

		// Token: 0x060091EA RID: 37354 RVA: 0x00347F84 File Offset: 0x00346184
		private void end()
		{
			if (this.z == null)
			{
				return;
			}
			if (this._wantCompress)
			{
				this._z.EndDeflate();
			}
			else
			{
				this._z.EndInflate();
			}
			this._z = null;
		}

		// Token: 0x060091EB RID: 37355 RVA: 0x00347FB8 File Offset: 0x003461B8
		public override void Close()
		{
			if (this._stream == null)
			{
				return;
			}
			try
			{
				this.finish();
			}
			finally
			{
				this.end();
				if (!this._leaveOpen)
				{
					this._stream.Close();
				}
				this._stream = null;
			}
		}

		// Token: 0x060091EC RID: 37356 RVA: 0x00348008 File Offset: 0x00346208
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x060091ED RID: 37357 RVA: 0x0002974C File Offset: 0x0002794C
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060091EE RID: 37358 RVA: 0x00348015 File Offset: 0x00346215
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x060091EF RID: 37359 RVA: 0x00348024 File Offset: 0x00346224
		private string ReadZeroTerminatedString()
		{
			List<byte> list = new List<byte>();
			bool flag = false;
			while (this._stream.Read(this._buf1, 0, 1) == 1)
			{
				if (this._buf1[0] == 0)
				{
					flag = true;
				}
				else
				{
					list.Add(this._buf1[0]);
				}
				if (flag)
				{
					byte[] array = list.ToArray();
					return GZipStream.iso8859dash1.GetString(array, 0, array.Length);
				}
			}
			throw new ZlibException("Unexpected EOF reading GZIP header.");
		}

		// Token: 0x060091F0 RID: 37360 RVA: 0x00348090 File Offset: 0x00346290
		private int _ReadAndValidateGzipHeader()
		{
			int num = 0;
			byte[] array = new byte[10];
			int num2 = this._stream.Read(array, 0, array.Length);
			if (num2 == 0)
			{
				return 0;
			}
			if (num2 != 10)
			{
				throw new ZlibException("Not a valid GZIP stream.");
			}
			if (array[0] != 31 || array[1] != 139 || array[2] != 8)
			{
				throw new ZlibException("Bad GZIP header.");
			}
			int num3 = BitConverter.ToInt32(array, 4);
			this._GzipMtime = GZipStream._unixEpoch.AddSeconds((double)num3);
			num += num2;
			if ((array[3] & 4) == 4)
			{
				num2 = this._stream.Read(array, 0, 2);
				num += num2;
				short num4 = (short)((int)array[0] + (int)array[1] * 256);
				byte[] array2 = new byte[(int)num4];
				num2 = this._stream.Read(array2, 0, array2.Length);
				if (num2 != (int)num4)
				{
					throw new ZlibException("Unexpected end-of-file reading GZIP header.");
				}
				num += num2;
			}
			if ((array[3] & 8) == 8)
			{
				this._GzipFileName = this.ReadZeroTerminatedString();
			}
			if ((array[3] & 16) == 16)
			{
				this._GzipComment = this.ReadZeroTerminatedString();
			}
			if ((array[3] & 2) == 2)
			{
				this.Read(this._buf1, 0, 1);
			}
			return num;
		}

		// Token: 0x060091F1 RID: 37361 RVA: 0x003481B0 File Offset: 0x003463B0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._stream.CanRead)
				{
					throw new ZlibException("The stream is not readable.");
				}
				this._streamMode = ZlibBaseStream.StreamMode.Reader;
				this.z.AvailableBytesIn = 0;
				if (this._flavor == ZlibStreamFlavor.GZIP)
				{
					this._gzipHeaderByteCount = this._ReadAndValidateGzipHeader();
					if (this._gzipHeaderByteCount == 0)
					{
						return 0;
					}
				}
			}
			if (this._streamMode != ZlibBaseStream.StreamMode.Reader)
			{
				throw new ZlibException("Cannot Read after Writing.");
			}
			if (count == 0)
			{
				return 0;
			}
			if (this.nomoreinput && this._wantCompress)
			{
				return 0;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (offset < buffer.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.GetLength(0))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._z.OutputBuffer = buffer;
			this._z.NextOut = offset;
			this._z.AvailableBytesOut = count;
			this._z.InputBuffer = this.workingBuffer;
			int num;
			for (;;)
			{
				if (this._z.AvailableBytesIn == 0 && !this.nomoreinput)
				{
					this._z.NextIn = 0;
					this._z.AvailableBytesIn = this._stream.Read(this._workingBuffer, 0, this._workingBuffer.Length);
					if (this._z.AvailableBytesIn == 0)
					{
						this.nomoreinput = true;
					}
				}
				num = (this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode));
				if (this.nomoreinput && num == -5)
				{
					break;
				}
				if (num != 0 && num != 1)
				{
					goto Block_20;
				}
				if (((this.nomoreinput || num == 1) && this._z.AvailableBytesOut == count) || this._z.AvailableBytesOut <= 0 || this.nomoreinput || num != 0)
				{
					goto IL_20A;
				}
			}
			return 0;
			Block_20:
			throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", this._wantCompress ? "de" : "in", num, this._z.Message));
			IL_20A:
			if (this._z.AvailableBytesOut > 0)
			{
				if (num == 0)
				{
					int availableBytesIn = this._z.AvailableBytesIn;
				}
				if (this.nomoreinput && this._wantCompress)
				{
					num = this._z.Deflate(FlushType.Finish);
					if (num != 0 && num != 1)
					{
						throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", num, this._z.Message));
					}
				}
			}
			num = count - this._z.AvailableBytesOut;
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, num);
			}
			return num;
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x060091F2 RID: 37362 RVA: 0x0034844E File Offset: 0x0034664E
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x060091F3 RID: 37363 RVA: 0x0034845B File Offset: 0x0034665B
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x060091F4 RID: 37364 RVA: 0x00348468 File Offset: 0x00346668
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x060091F5 RID: 37365 RVA: 0x00348475 File Offset: 0x00346675
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x060091F6 RID: 37366 RVA: 0x0002974C File Offset: 0x0002794C
		// (set) Token: 0x060091F7 RID: 37367 RVA: 0x0002974C File Offset: 0x0002794C
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060091F8 RID: 37368 RVA: 0x00348484 File Offset: 0x00346684
		public static void CompressString(string s, Stream compressor)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			try
			{
				compressor.Write(bytes, 0, bytes.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((IDisposable)compressor).Dispose();
				}
			}
		}

		// Token: 0x060091F9 RID: 37369 RVA: 0x003484C8 File Offset: 0x003466C8
		public static void CompressBuffer(byte[] b, Stream compressor)
		{
			try
			{
				compressor.Write(b, 0, b.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((IDisposable)compressor).Dispose();
				}
			}
		}

		// Token: 0x060091FA RID: 37370 RVA: 0x00348500 File Offset: 0x00346700
		public static string UncompressString(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			Encoding utf = Encoding.UTF8;
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((IDisposable)decompressor).Dispose();
					}
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
				result = new StreamReader(memoryStream, utf).ReadToEnd();
			}
			return result;
		}

		// Token: 0x060091FB RID: 37371 RVA: 0x00348590 File Offset: 0x00346790
		public static byte[] UncompressBuffer(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((IDisposable)decompressor).Dispose();
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x04005C2D RID: 23597
		protected internal ZlibCodec _z;

		// Token: 0x04005C2E RID: 23598
		protected internal ZlibBaseStream.StreamMode _streamMode = ZlibBaseStream.StreamMode.Undefined;

		// Token: 0x04005C2F RID: 23599
		protected internal FlushType _flushMode;

		// Token: 0x04005C30 RID: 23600
		protected internal ZlibStreamFlavor _flavor;

		// Token: 0x04005C31 RID: 23601
		protected internal CompressionMode _compressionMode;

		// Token: 0x04005C32 RID: 23602
		protected internal CompressionLevel _level;

		// Token: 0x04005C33 RID: 23603
		protected internal bool _leaveOpen;

		// Token: 0x04005C34 RID: 23604
		protected internal byte[] _workingBuffer;

		// Token: 0x04005C35 RID: 23605
		protected internal int _bufferSize = 16384;

		// Token: 0x04005C36 RID: 23606
		protected internal byte[] _buf1 = new byte[1];

		// Token: 0x04005C37 RID: 23607
		protected internal Stream _stream;

		// Token: 0x04005C38 RID: 23608
		protected internal CompressionStrategy Strategy;

		// Token: 0x04005C39 RID: 23609
		private CRC32 crc;

		// Token: 0x04005C3A RID: 23610
		protected internal string _GzipFileName;

		// Token: 0x04005C3B RID: 23611
		protected internal string _GzipComment;

		// Token: 0x04005C3C RID: 23612
		protected internal DateTime _GzipMtime;

		// Token: 0x04005C3D RID: 23613
		protected internal int _gzipHeaderByteCount;

		// Token: 0x04005C3E RID: 23614
		private bool nomoreinput;

		// Token: 0x02002AA0 RID: 10912
		internal enum StreamMode
		{
			// Token: 0x0400A09F RID: 41119
			Writer,
			// Token: 0x0400A0A0 RID: 41120
			Reader,
			// Token: 0x0400A0A1 RID: 41121
			Undefined
		}
	}
}
