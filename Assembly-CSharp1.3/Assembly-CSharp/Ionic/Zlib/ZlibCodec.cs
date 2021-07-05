using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200183E RID: 6206
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x060091FC RID: 37372 RVA: 0x00348608 File Offset: 0x00346808
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x060091FD RID: 37373 RVA: 0x00348610 File Offset: 0x00346810
		public ZlibCodec()
		{
		}

		// Token: 0x060091FE RID: 37374 RVA: 0x00348628 File Offset: 0x00346828
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

		// Token: 0x060091FF RID: 37375 RVA: 0x00348682 File Offset: 0x00346882
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x06009200 RID: 37376 RVA: 0x00348690 File Offset: 0x00346890
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x06009201 RID: 37377 RVA: 0x0034869F File Offset: 0x0034689F
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x06009202 RID: 37378 RVA: 0x003486B0 File Offset: 0x003468B0
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

		// Token: 0x06009203 RID: 37379 RVA: 0x003486E5 File Offset: 0x003468E5
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x06009204 RID: 37380 RVA: 0x00348706 File Offset: 0x00346906
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

		// Token: 0x06009205 RID: 37381 RVA: 0x0034872D File Offset: 0x0034692D
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x06009206 RID: 37382 RVA: 0x0034874D File Offset: 0x0034694D
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06009207 RID: 37383 RVA: 0x00348756 File Offset: 0x00346956
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06009208 RID: 37384 RVA: 0x00348766 File Offset: 0x00346966
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x06009209 RID: 37385 RVA: 0x00348776 File Offset: 0x00346976
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600920A RID: 37386 RVA: 0x0034878D File Offset: 0x0034698D
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600920B RID: 37387 RVA: 0x003487A4 File Offset: 0x003469A4
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

		// Token: 0x0600920C RID: 37388 RVA: 0x003487F9 File Offset: 0x003469F9
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x0600920D RID: 37389 RVA: 0x0034881A File Offset: 0x00346A1A
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x0600920E RID: 37390 RVA: 0x00348837 File Offset: 0x00346A37
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x0600920F RID: 37391 RVA: 0x00348857 File Offset: 0x00346A57
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x06009210 RID: 37392 RVA: 0x00348879 File Offset: 0x00346A79
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

		// Token: 0x06009211 RID: 37393 RVA: 0x003488B0 File Offset: 0x00346AB0
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

		// Token: 0x06009212 RID: 37394 RVA: 0x003489FC File Offset: 0x00346BFC
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

		// Token: 0x04005C3F RID: 23615
		public byte[] InputBuffer;

		// Token: 0x04005C40 RID: 23616
		public int NextIn;

		// Token: 0x04005C41 RID: 23617
		public int AvailableBytesIn;

		// Token: 0x04005C42 RID: 23618
		public long TotalBytesIn;

		// Token: 0x04005C43 RID: 23619
		public byte[] OutputBuffer;

		// Token: 0x04005C44 RID: 23620
		public int NextOut;

		// Token: 0x04005C45 RID: 23621
		public int AvailableBytesOut;

		// Token: 0x04005C46 RID: 23622
		public long TotalBytesOut;

		// Token: 0x04005C47 RID: 23623
		public string Message;

		// Token: 0x04005C48 RID: 23624
		internal DeflateManager dstate;

		// Token: 0x04005C49 RID: 23625
		internal InflateManager istate;

		// Token: 0x04005C4A RID: 23626
		internal uint _Adler32;

		// Token: 0x04005C4B RID: 23627
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x04005C4C RID: 23628
		public int WindowBits = 15;

		// Token: 0x04005C4D RID: 23629
		public CompressionStrategy Strategy;
	}
}
