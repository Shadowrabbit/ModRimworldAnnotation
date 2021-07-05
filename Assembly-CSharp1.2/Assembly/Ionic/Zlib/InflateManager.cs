using System;

namespace Ionic.Zlib
{
	// Token: 0x02002210 RID: 8720
	internal sealed class InflateManager
	{
		// Token: 0x17001BF4 RID: 7156
		// (get) Token: 0x0600BB4F RID: 47951 RVA: 0x00079457 File Offset: 0x00077657
		// (set) Token: 0x0600BB50 RID: 47952 RVA: 0x0007945F File Offset: 0x0007765F
		internal bool HandleRfc1950HeaderBytes
		{
			get
			{
				return this._handleRfc1950HeaderBytes;
			}
			set
			{
				this._handleRfc1950HeaderBytes = value;
			}
		}

		// Token: 0x0600BB51 RID: 47953 RVA: 0x00079468 File Offset: 0x00077668
		public InflateManager()
		{
		}

		// Token: 0x0600BB52 RID: 47954 RVA: 0x00079477 File Offset: 0x00077677
		public InflateManager(bool expectRfc1950HeaderBytes)
		{
			this._handleRfc1950HeaderBytes = expectRfc1950HeaderBytes;
		}

		// Token: 0x0600BB53 RID: 47955 RVA: 0x0035F66C File Offset: 0x0035D86C
		internal int Reset()
		{
			this._codec.TotalBytesIn = (this._codec.TotalBytesOut = 0L);
			this._codec.Message = null;
			this.mode = (this.HandleRfc1950HeaderBytes ? InflateManager.InflateManagerMode.METHOD : InflateManager.InflateManagerMode.BLOCKS);
			this.blocks.Reset();
			return 0;
		}

		// Token: 0x0600BB54 RID: 47956 RVA: 0x0007948D File Offset: 0x0007768D
		internal int End()
		{
			if (this.blocks != null)
			{
				this.blocks.Free();
			}
			this.blocks = null;
			return 0;
		}

		// Token: 0x0600BB55 RID: 47957 RVA: 0x0035F6C0 File Offset: 0x0035D8C0
		internal int Initialize(ZlibCodec codec, int w)
		{
			this._codec = codec;
			this._codec.Message = null;
			this.blocks = null;
			if (w < 8 || w > 15)
			{
				this.End();
				throw new ZlibException("Bad window size.");
			}
			this.wbits = w;
			this.blocks = new InflateBlocks(codec, this.HandleRfc1950HeaderBytes ? this : null, 1 << w);
			this.Reset();
			return 0;
		}

		// Token: 0x0600BB56 RID: 47958 RVA: 0x0035F730 File Offset: 0x0035D930
		internal int Inflate(FlushType flush)
		{
			if (this._codec.InputBuffer == null)
			{
				throw new ZlibException("InputBuffer is null. ");
			}
			int num = 0;
			int num2 = -5;
			int nextIn;
			for (;;)
			{
				switch (this.mode)
				{
				case InflateManager.InflateManagerMode.METHOD:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					byte[] inputBuffer = this._codec.InputBuffer;
					ZlibCodec codec = this._codec;
					nextIn = codec.NextIn;
					codec.NextIn = nextIn + 1;
					if (((this.method = inputBuffer[nextIn]) & 15) != 8)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = string.Format("unknown compression method (0x{0:X2})", this.method);
						this.marker = 5;
						continue;
					}
					if ((this.method >> 4) + 8 > this.wbits)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = string.Format("invalid window size ({0})", (this.method >> 4) + 8);
						this.marker = 5;
						continue;
					}
					this.mode = InflateManager.InflateManagerMode.FLAG;
					continue;
				}
				case InflateManager.InflateManagerMode.FLAG:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					byte[] inputBuffer2 = this._codec.InputBuffer;
					ZlibCodec codec2 = this._codec;
					nextIn = codec2.NextIn;
					codec2.NextIn = nextIn + 1;
					int num3 = inputBuffer2[nextIn] & 255;
					if (((this.method << 8) + num3) % 31 != 0)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = "incorrect header check";
						this.marker = 5;
						continue;
					}
					this.mode = (((num3 & 32) == 0) ? InflateManager.InflateManagerMode.BLOCKS : InflateManager.InflateManagerMode.DICT4);
					continue;
				}
				case InflateManager.InflateManagerMode.DICT4:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					byte[] inputBuffer3 = this._codec.InputBuffer;
					ZlibCodec codec3 = this._codec;
					nextIn = codec3.NextIn;
					codec3.NextIn = nextIn + 1;
					this.expectedCheck = (uint)(inputBuffer3[nextIn] << 24 & (long)((ulong)-16777216));
					this.mode = InflateManager.InflateManagerMode.DICT3;
					continue;
				}
				case InflateManager.InflateManagerMode.DICT3:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num4 = this.expectedCheck;
					byte[] inputBuffer4 = this._codec.InputBuffer;
					ZlibCodec codec4 = this._codec;
					nextIn = codec4.NextIn;
					codec4.NextIn = nextIn + 1;
					this.expectedCheck = num4 + (inputBuffer4[nextIn] << 16 & 16711680U);
					this.mode = InflateManager.InflateManagerMode.DICT2;
					continue;
				}
				case InflateManager.InflateManagerMode.DICT2:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num5 = this.expectedCheck;
					byte[] inputBuffer5 = this._codec.InputBuffer;
					ZlibCodec codec5 = this._codec;
					nextIn = codec5.NextIn;
					codec5.NextIn = nextIn + 1;
					this.expectedCheck = num5 + (inputBuffer5[nextIn] << 8 & 65280U);
					this.mode = InflateManager.InflateManagerMode.DICT1;
					continue;
				}
				case InflateManager.InflateManagerMode.DICT1:
					goto IL_383;
				case InflateManager.InflateManagerMode.DICT0:
					goto IL_40D;
				case InflateManager.InflateManagerMode.BLOCKS:
					num2 = this.blocks.Process(num2);
					if (num2 == -3)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this.marker = 0;
						continue;
					}
					if (num2 == 0)
					{
						num2 = num;
					}
					if (num2 != 1)
					{
						return num2;
					}
					num2 = num;
					this.computedCheck = this.blocks.Reset();
					if (!this.HandleRfc1950HeaderBytes)
					{
						goto Block_16;
					}
					this.mode = InflateManager.InflateManagerMode.CHECK4;
					continue;
				case InflateManager.InflateManagerMode.CHECK4:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					byte[] inputBuffer6 = this._codec.InputBuffer;
					ZlibCodec codec6 = this._codec;
					nextIn = codec6.NextIn;
					codec6.NextIn = nextIn + 1;
					this.expectedCheck = (uint)(inputBuffer6[nextIn] << 24 & (long)((ulong)-16777216));
					this.mode = InflateManager.InflateManagerMode.CHECK3;
					continue;
				}
				case InflateManager.InflateManagerMode.CHECK3:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num6 = this.expectedCheck;
					byte[] inputBuffer7 = this._codec.InputBuffer;
					ZlibCodec codec7 = this._codec;
					nextIn = codec7.NextIn;
					codec7.NextIn = nextIn + 1;
					this.expectedCheck = num6 + (inputBuffer7[nextIn] << 16 & 16711680U);
					this.mode = InflateManager.InflateManagerMode.CHECK2;
					continue;
				}
				case InflateManager.InflateManagerMode.CHECK2:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num7 = this.expectedCheck;
					byte[] inputBuffer8 = this._codec.InputBuffer;
					ZlibCodec codec8 = this._codec;
					nextIn = codec8.NextIn;
					codec8.NextIn = nextIn + 1;
					this.expectedCheck = num7 + (inputBuffer8[nextIn] << 8 & 65280U);
					this.mode = InflateManager.InflateManagerMode.CHECK1;
					continue;
				}
				case InflateManager.InflateManagerMode.CHECK1:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num8 = this.expectedCheck;
					byte[] inputBuffer9 = this._codec.InputBuffer;
					ZlibCodec codec9 = this._codec;
					nextIn = codec9.NextIn;
					codec9.NextIn = nextIn + 1;
					this.expectedCheck = num8 + (inputBuffer9[nextIn] & 255U);
					if (this.computedCheck != this.expectedCheck)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = "incorrect data check";
						this.marker = 5;
						continue;
					}
					goto IL_6AE;
				}
				case InflateManager.InflateManagerMode.DONE:
					return 1;
				case InflateManager.InflateManagerMode.BAD:
					goto IL_6BA;
				}
				break;
			}
			throw new ZlibException("Stream error.");
			IL_383:
			if (this._codec.AvailableBytesIn == 0)
			{
				return num2;
			}
			this._codec.AvailableBytesIn--;
			this._codec.TotalBytesIn += 1L;
			uint num9 = this.expectedCheck;
			byte[] inputBuffer10 = this._codec.InputBuffer;
			ZlibCodec codec10 = this._codec;
			nextIn = codec10.NextIn;
			codec10.NextIn = nextIn + 1;
			this.expectedCheck = num9 + (inputBuffer10[nextIn] & 255U);
			this._codec._Adler32 = this.expectedCheck;
			this.mode = InflateManager.InflateManagerMode.DICT0;
			return 2;
			IL_40D:
			this.mode = InflateManager.InflateManagerMode.BAD;
			this._codec.Message = "need dictionary";
			this.marker = 0;
			return -2;
			Block_16:
			this.mode = InflateManager.InflateManagerMode.DONE;
			return 1;
			IL_6AE:
			this.mode = InflateManager.InflateManagerMode.DONE;
			return 1;
			IL_6BA:
			throw new ZlibException(string.Format("Bad state ({0})", this._codec.Message));
		}

		// Token: 0x0600BB57 RID: 47959 RVA: 0x0035FE1C File Offset: 0x0035E01C
		internal int SetDictionary(byte[] dictionary)
		{
			int start = 0;
			int num = dictionary.Length;
			if (this.mode != InflateManager.InflateManagerMode.DICT0)
			{
				throw new ZlibException("Stream error.");
			}
			if (Adler.Adler32(1U, dictionary, 0, dictionary.Length) != this._codec._Adler32)
			{
				return -3;
			}
			this._codec._Adler32 = Adler.Adler32(0U, null, 0, 0);
			if (num >= 1 << this.wbits)
			{
				num = (1 << this.wbits) - 1;
				start = dictionary.Length - num;
			}
			this.blocks.SetDictionary(dictionary, start, num);
			this.mode = InflateManager.InflateManagerMode.BLOCKS;
			return 0;
		}

		// Token: 0x0600BB58 RID: 47960 RVA: 0x0035FEAC File Offset: 0x0035E0AC
		internal int Sync()
		{
			if (this.mode != InflateManager.InflateManagerMode.BAD)
			{
				this.mode = InflateManager.InflateManagerMode.BAD;
				this.marker = 0;
			}
			int num;
			if ((num = this._codec.AvailableBytesIn) == 0)
			{
				return -5;
			}
			int num2 = this._codec.NextIn;
			int num3 = this.marker;
			while (num != 0 && num3 < 4)
			{
				if (this._codec.InputBuffer[num2] == InflateManager.mark[num3])
				{
					num3++;
				}
				else if (this._codec.InputBuffer[num2] != 0)
				{
					num3 = 0;
				}
				else
				{
					num3 = 4 - num3;
				}
				num2++;
				num--;
			}
			this._codec.TotalBytesIn += (long)(num2 - this._codec.NextIn);
			this._codec.NextIn = num2;
			this._codec.AvailableBytesIn = num;
			this.marker = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long totalBytesIn = this._codec.TotalBytesIn;
			long totalBytesOut = this._codec.TotalBytesOut;
			this.Reset();
			this._codec.TotalBytesIn = totalBytesIn;
			this._codec.TotalBytesOut = totalBytesOut;
			this.mode = InflateManager.InflateManagerMode.BLOCKS;
			return 0;
		}

		// Token: 0x0600BB59 RID: 47961 RVA: 0x000794AA File Offset: 0x000776AA
		internal int SyncPoint(ZlibCodec z)
		{
			return this.blocks.SyncPoint();
		}

		// Token: 0x0400801B RID: 32795
		private const int PRESET_DICT = 32;

		// Token: 0x0400801C RID: 32796
		private const int Z_DEFLATED = 8;

		// Token: 0x0400801D RID: 32797
		private InflateManager.InflateManagerMode mode;

		// Token: 0x0400801E RID: 32798
		internal ZlibCodec _codec;

		// Token: 0x0400801F RID: 32799
		internal int method;

		// Token: 0x04008020 RID: 32800
		internal uint computedCheck;

		// Token: 0x04008021 RID: 32801
		internal uint expectedCheck;

		// Token: 0x04008022 RID: 32802
		internal int marker;

		// Token: 0x04008023 RID: 32803
		private bool _handleRfc1950HeaderBytes = true;

		// Token: 0x04008024 RID: 32804
		internal int wbits;

		// Token: 0x04008025 RID: 32805
		internal InflateBlocks blocks;

		// Token: 0x04008026 RID: 32806
		private static readonly byte[] mark = new byte[]
		{
			0,
			0,
			byte.MaxValue,
			byte.MaxValue
		};

		// Token: 0x02002211 RID: 8721
		private enum InflateManagerMode
		{
			// Token: 0x04008028 RID: 32808
			METHOD,
			// Token: 0x04008029 RID: 32809
			FLAG,
			// Token: 0x0400802A RID: 32810
			DICT4,
			// Token: 0x0400802B RID: 32811
			DICT3,
			// Token: 0x0400802C RID: 32812
			DICT2,
			// Token: 0x0400802D RID: 32813
			DICT1,
			// Token: 0x0400802E RID: 32814
			DICT0,
			// Token: 0x0400802F RID: 32815
			BLOCKS,
			// Token: 0x04008030 RID: 32816
			CHECK4,
			// Token: 0x04008031 RID: 32817
			CHECK3,
			// Token: 0x04008032 RID: 32818
			CHECK2,
			// Token: 0x04008033 RID: 32819
			CHECK1,
			// Token: 0x04008034 RID: 32820
			DONE,
			// Token: 0x04008035 RID: 32821
			BAD
		}
	}
}
