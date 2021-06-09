using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02002223 RID: 8739
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x17001C08 RID: 7176
		// (get) Token: 0x0600BBB4 RID: 48052 RVA: 0x00079770 File Offset: 0x00077970
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x0600BBB5 RID: 48053 RVA: 0x00079778 File Offset: 0x00077978
		public ZlibCodec()
		{
		}

		// Token: 0x0600BBB6 RID: 48054 RVA: 0x00362428 File Offset: 0x00360628
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				if (this.InitializeDeflate() != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				if (this.InitializeInflate() != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x0600BBB7 RID: 48055 RVA: 0x0007978F File Offset: 0x0007798F
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x0600BBB8 RID: 48056 RVA: 0x0007979D File Offset: 0x0007799D
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x0600BBB9 RID: 48057 RVA: 0x000797AC File Offset: 0x000779AC
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x0600BBBA RID: 48058 RVA: 0x000797BD File Offset: 0x000779BD
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x0600BBBB RID: 48059 RVA: 0x000797F2 File Offset: 0x000779F2
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x0600BBBC RID: 48060 RVA: 0x00079813 File Offset: 0x00077A13
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x0600BBBD RID: 48061 RVA: 0x0007983A File Offset: 0x00077A3A
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x0600BBBE RID: 48062 RVA: 0x0007985A File Offset: 0x00077A5A
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600BBBF RID: 48063 RVA: 0x00079863 File Offset: 0x00077A63
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600BBC0 RID: 48064 RVA: 0x00079873 File Offset: 0x00077A73
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600BBC1 RID: 48065 RVA: 0x00079883 File Offset: 0x00077A83
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600BBC2 RID: 48066 RVA: 0x0007989A File Offset: 0x00077A9A
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600BBC3 RID: 48067 RVA: 0x00362484 File Offset: 0x00360684
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x0600BBC4 RID: 48068 RVA: 0x000798B1 File Offset: 0x00077AB1
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x0600BBC5 RID: 48069 RVA: 0x000798D2 File Offset: 0x00077AD2
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x0600BBC6 RID: 48070 RVA: 0x000798EF File Offset: 0x00077AEF
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x0600BBC7 RID: 48071 RVA: 0x0007990F File Offset: 0x00077B0F
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x0600BBC8 RID: 48072 RVA: 0x00079931 File Offset: 0x00077B31
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x0600BBC9 RID: 48073 RVA: 0x003624DC File Offset: 0x003606DC
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x0600BBCA RID: 48074 RVA: 0x00362628 File Offset: 0x00360828
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x040080DF RID: 32991
		public byte[] InputBuffer;

		// Token: 0x040080E0 RID: 32992
		public int NextIn;

		// Token: 0x040080E1 RID: 32993
		public int AvailableBytesIn;

		// Token: 0x040080E2 RID: 32994
		public long TotalBytesIn;

		// Token: 0x040080E3 RID: 32995
		public byte[] OutputBuffer;

		// Token: 0x040080E4 RID: 32996
		public int NextOut;

		// Token: 0x040080E5 RID: 32997
		public int AvailableBytesOut;

		// Token: 0x040080E6 RID: 32998
		public long TotalBytesOut;

		// Token: 0x040080E7 RID: 32999
		public string Message;

		// Token: 0x040080E8 RID: 33000
		internal DeflateManager dstate;

		// Token: 0x040080E9 RID: 33001
		internal InflateManager istate;

		// Token: 0x040080EA RID: 33002
		internal uint _Adler32;

		// Token: 0x040080EB RID: 33003
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x040080EC RID: 33004
		public int WindowBits = 15;

		// Token: 0x040080ED RID: 33005
		public CompressionStrategy Strategy;
	}
}
