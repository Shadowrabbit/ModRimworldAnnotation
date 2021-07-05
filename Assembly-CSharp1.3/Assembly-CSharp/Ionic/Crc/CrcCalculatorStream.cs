using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x02001842 RID: 6210
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x06009240 RID: 37440 RVA: 0x003492A6 File Offset: 0x003474A6
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06009241 RID: 37441 RVA: 0x003492B6 File Offset: 0x003474B6
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06009242 RID: 37442 RVA: 0x003492C6 File Offset: 0x003474C6
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06009243 RID: 37443 RVA: 0x003492E2 File Offset: 0x003474E2
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06009244 RID: 37444 RVA: 0x003492FE File Offset: 0x003474FE
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06009245 RID: 37445 RVA: 0x0034931C File Offset: 0x0034751C
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._innerStream = stream;
			this._crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x06009246 RID: 37446 RVA: 0x0034936C File Offset: 0x0034756C
		public long TotalBytesSlurped
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x06009247 RID: 37447 RVA: 0x00349379 File Offset: 0x00347579
		public int Crc
		{
			get
			{
				return this._crc32.Crc32Result;
			}
		}

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x06009248 RID: 37448 RVA: 0x00349386 File Offset: 0x00347586
		// (set) Token: 0x06009249 RID: 37449 RVA: 0x0034938E File Offset: 0x0034758E
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x0600924A RID: 37450 RVA: 0x00349398 File Offset: 0x00347598
		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num = this._lengthLimit - this._crc32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this._innerStream.Read(buffer, offset, count2);
			if (num2 > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		// Token: 0x0600924B RID: 37451 RVA: 0x00349406 File Offset: 0x00347606
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x0600924C RID: 37452 RVA: 0x00349428 File Offset: 0x00347628
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x0600924D RID: 37453 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x0600924E RID: 37454 RVA: 0x00349435 File Offset: 0x00347635
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x0600924F RID: 37455 RVA: 0x00349442 File Offset: 0x00347642
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06009250 RID: 37456 RVA: 0x0034944F File Offset: 0x0034764F
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06009251 RID: 37457 RVA: 0x0034936C File Offset: 0x0034756C
		// (set) Token: 0x06009252 RID: 37458 RVA: 0x00347025 File Offset: 0x00345225
		public override long Position
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06009253 RID: 37459 RVA: 0x00347025 File Offset: 0x00345225
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06009254 RID: 37460 RVA: 0x00347025 File Offset: 0x00345225
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06009255 RID: 37461 RVA: 0x00349470 File Offset: 0x00347670
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06009256 RID: 37462 RVA: 0x00349478 File Offset: 0x00347678
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x04005C60 RID: 23648
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x04005C61 RID: 23649
		private readonly Stream _innerStream;

		// Token: 0x04005C62 RID: 23650
		private readonly CRC32 _crc32;

		// Token: 0x04005C63 RID: 23651
		private readonly long _lengthLimit = -99L;

		// Token: 0x04005C64 RID: 23652
		private bool _leaveOpen;
	}
}
