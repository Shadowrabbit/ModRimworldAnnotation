using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x02002227 RID: 8743
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x0600BBF8 RID: 48120 RVA: 0x00079B35 File Offset: 0x00077D35
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x0600BBF9 RID: 48121 RVA: 0x00079B45 File Offset: 0x00077D45
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x0600BBFA RID: 48122 RVA: 0x00079B55 File Offset: 0x00077D55
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600BBFB RID: 48123 RVA: 0x00079B71 File Offset: 0x00077D71
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600BBFC RID: 48124 RVA: 0x00079B8D File Offset: 0x00077D8D
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600BBFD RID: 48125 RVA: 0x00362D00 File Offset: 0x00360F00
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

		// Token: 0x17001C14 RID: 7188
		// (get) Token: 0x0600BBFE RID: 48126 RVA: 0x00079BAA File Offset: 0x00077DAA
		public long TotalBytesSlurped
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
		}

		// Token: 0x17001C15 RID: 7189
		// (get) Token: 0x0600BBFF RID: 48127 RVA: 0x00079BB7 File Offset: 0x00077DB7
		public int Crc
		{
			get
			{
				return this._crc32.Crc32Result;
			}
		}

		// Token: 0x17001C16 RID: 7190
		// (get) Token: 0x0600BC00 RID: 48128 RVA: 0x00079BC4 File Offset: 0x00077DC4
		// (set) Token: 0x0600BC01 RID: 48129 RVA: 0x00079BCC File Offset: 0x00077DCC
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

		// Token: 0x0600BC02 RID: 48130 RVA: 0x00362D50 File Offset: 0x00360F50
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

		// Token: 0x0600BC03 RID: 48131 RVA: 0x00079BD5 File Offset: 0x00077DD5
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17001C17 RID: 7191
		// (get) Token: 0x0600BC04 RID: 48132 RVA: 0x00079BF7 File Offset: 0x00077DF7
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17001C18 RID: 7192
		// (get) Token: 0x0600BC05 RID: 48133 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C19 RID: 7193
		// (get) Token: 0x0600BC06 RID: 48134 RVA: 0x00079C04 File Offset: 0x00077E04
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x0600BC07 RID: 48135 RVA: 0x00079C11 File Offset: 0x00077E11
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x0600BC08 RID: 48136 RVA: 0x00079C1E File Offset: 0x00077E1E
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

		// Token: 0x17001C1B RID: 7195
		// (get) Token: 0x0600BC09 RID: 48137 RVA: 0x00079BAA File Offset: 0x00077DAA
		// (set) Token: 0x0600BC0A RID: 48138 RVA: 0x0000713A File Offset: 0x0000533A
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

		// Token: 0x0600BC0B RID: 48139 RVA: 0x0000713A File Offset: 0x0000533A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600BC0C RID: 48140 RVA: 0x0000713A File Offset: 0x0000533A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600BC0D RID: 48141 RVA: 0x00079C3F File Offset: 0x00077E3F
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x0600BC0E RID: 48142 RVA: 0x00079C47 File Offset: 0x00077E47
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x04008100 RID: 33024
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x04008101 RID: 33025
		private readonly Stream _innerStream;

		// Token: 0x04008102 RID: 33026
		private readonly CRC32 _crc32;

		// Token: 0x04008103 RID: 33027
		private readonly long _lengthLimit = -99L;

		// Token: 0x04008104 RID: 33028
		private bool _leaveOpen;
	}
}
