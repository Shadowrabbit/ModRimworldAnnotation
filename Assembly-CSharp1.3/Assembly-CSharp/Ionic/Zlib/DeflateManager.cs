using System;

namespace Ionic.Zlib
{
	// Token: 0x02001828 RID: 6184
	internal sealed class DeflateManager
	{
		// Token: 0x06009124 RID: 37156 RVA: 0x00340300 File Offset: 0x0033E500
		internal DeflateManager()
		{
			this.dyn_ltree = new short[DeflateManager.HEAP_SIZE * 2];
			this.dyn_dtree = new short[(2 * InternalConstants.D_CODES + 1) * 2];
			this.bl_tree = new short[(2 * InternalConstants.BL_CODES + 1) * 2];
		}

		// Token: 0x06009125 RID: 37157 RVA: 0x003403B4 File Offset: 0x0033E5B4
		private void _InitializeLazyMatch()
		{
			this.window_size = 2 * this.w_size;
			Array.Clear(this.head, 0, this.hash_size);
			this.config = DeflateManager.Config.Lookup(this.compressionLevel);
			this.SetDeflater();
			this.strstart = 0;
			this.block_start = 0;
			this.lookahead = 0;
			this.match_length = (this.prev_length = DeflateManager.MIN_MATCH - 1);
			this.match_available = 0;
			this.ins_h = 0;
		}

		// Token: 0x06009126 RID: 37158 RVA: 0x00340434 File Offset: 0x0033E634
		private void _InitializeTreeData()
		{
			this.treeLiterals.dyn_tree = this.dyn_ltree;
			this.treeLiterals.staticTree = StaticTree.Literals;
			this.treeDistances.dyn_tree = this.dyn_dtree;
			this.treeDistances.staticTree = StaticTree.Distances;
			this.treeBitLengths.dyn_tree = this.bl_tree;
			this.treeBitLengths.staticTree = StaticTree.BitLengths;
			this.bi_buf = 0;
			this.bi_valid = 0;
			this.last_eob_len = 8;
			this._InitializeBlocks();
		}

		// Token: 0x06009127 RID: 37159 RVA: 0x003404C0 File Offset: 0x0033E6C0
		internal void _InitializeBlocks()
		{
			for (int i = 0; i < InternalConstants.L_CODES; i++)
			{
				this.dyn_ltree[i * 2] = 0;
			}
			for (int j = 0; j < InternalConstants.D_CODES; j++)
			{
				this.dyn_dtree[j * 2] = 0;
			}
			for (int k = 0; k < InternalConstants.BL_CODES; k++)
			{
				this.bl_tree[k * 2] = 0;
			}
			this.dyn_ltree[DeflateManager.END_BLOCK * 2] = 1;
			this.opt_len = (this.static_len = 0);
			this.last_lit = (this.matches = 0);
		}

		// Token: 0x06009128 RID: 37160 RVA: 0x00340550 File Offset: 0x0033E750
		internal void pqdownheap(short[] tree, int k)
		{
			int num = this.heap[k];
			for (int i = k << 1; i <= this.heap_len; i <<= 1)
			{
				if (i < this.heap_len && DeflateManager._IsSmaller(tree, this.heap[i + 1], this.heap[i], this.depth))
				{
					i++;
				}
				if (DeflateManager._IsSmaller(tree, num, this.heap[i], this.depth))
				{
					break;
				}
				this.heap[k] = this.heap[i];
				k = i;
			}
			this.heap[k] = num;
		}

		// Token: 0x06009129 RID: 37161 RVA: 0x003405DC File Offset: 0x0033E7DC
		internal static bool _IsSmaller(short[] tree, int n, int m, sbyte[] depth)
		{
			short num = tree[n * 2];
			short num2 = tree[m * 2];
			return num < num2 || (num == num2 && depth[n] <= depth[m]);
		}

		// Token: 0x0600912A RID: 37162 RVA: 0x0034060C File Offset: 0x0033E80C
		internal void scan_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(max_code + 1) * 2 + 1] = short.MaxValue;
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						this.bl_tree[num6 * 2] = (short)((int)this.bl_tree[num6 * 2] + num3);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							short[] array = this.bl_tree;
							int num7 = num6 * 2;
							array[num7] += 1;
						}
						short[] array2 = this.bl_tree;
						int num8 = InternalConstants.REP_3_6 * 2;
						array2[num8] += 1;
					}
					else if (num3 <= 10)
					{
						short[] array3 = this.bl_tree;
						int num9 = InternalConstants.REPZ_3_10 * 2;
						array3[num9] += 1;
					}
					else
					{
						short[] array4 = this.bl_tree;
						int num10 = InternalConstants.REPZ_11_138 * 2;
						array4[num10] += 1;
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x0600912B RID: 37163 RVA: 0x00340728 File Offset: 0x0033E928
		internal int build_bl_tree()
		{
			this.scan_tree(this.dyn_ltree, this.treeLiterals.max_code);
			this.scan_tree(this.dyn_dtree, this.treeDistances.max_code);
			this.treeBitLengths.build_tree(this);
			int num = InternalConstants.BL_CODES - 1;
			while (num >= 3 && this.bl_tree[(int)(Tree.bl_order[num] * 2 + 1)] == 0)
			{
				num--;
			}
			this.opt_len += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		// Token: 0x0600912C RID: 37164 RVA: 0x003407B0 File Offset: 0x0033E9B0
		internal void send_all_trees(int lcodes, int dcodes, int blcodes)
		{
			this.send_bits(lcodes - 257, 5);
			this.send_bits(dcodes - 1, 5);
			this.send_bits(blcodes - 4, 4);
			for (int i = 0; i < blcodes; i++)
			{
				this.send_bits((int)this.bl_tree[(int)(Tree.bl_order[i] * 2 + 1)], 3);
			}
			this.send_tree(this.dyn_ltree, lcodes - 1);
			this.send_tree(this.dyn_dtree, dcodes - 1);
		}

		// Token: 0x0600912D RID: 37165 RVA: 0x00340824 File Offset: 0x0033EA24
		internal void send_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						do
						{
							this.send_code(num6, this.bl_tree);
						}
						while (--num3 != 0);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							this.send_code(num6, this.bl_tree);
							num3--;
						}
						this.send_code(InternalConstants.REP_3_6, this.bl_tree);
						this.send_bits(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.send_code(InternalConstants.REPZ_3_10, this.bl_tree);
						this.send_bits(num3 - 3, 3);
					}
					else
					{
						this.send_code(InternalConstants.REPZ_11_138, this.bl_tree);
						this.send_bits(num3 - 11, 7);
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x0600912E RID: 37166 RVA: 0x0034093A File Offset: 0x0033EB3A
		private void put_bytes(byte[] p, int start, int len)
		{
			Array.Copy(p, start, this.pending, this.pendingCount, len);
			this.pendingCount += len;
		}

		// Token: 0x0600912F RID: 37167 RVA: 0x00340960 File Offset: 0x0033EB60
		internal void send_code(int c, short[] tree)
		{
			int num = c * 2;
			this.send_bits((int)tree[num] & 65535, (int)tree[num + 1] & 65535);
		}

		// Token: 0x06009130 RID: 37168 RVA: 0x0034098C File Offset: 0x0033EB8C
		internal void send_bits(int value, int length)
		{
			if (this.bi_valid > DeflateManager.Buf_size - length)
			{
				this.bi_buf |= (short)(value << this.bi_valid & 65535);
				byte[] array = this.pending;
				int num = this.pendingCount;
				this.pendingCount = num + 1;
				array[num] = (byte)this.bi_buf;
				byte[] array2 = this.pending;
				num = this.pendingCount;
				this.pendingCount = num + 1;
				array2[num] = (byte)(this.bi_buf >> 8);
				this.bi_buf = (short)((uint)value >> DeflateManager.Buf_size - this.bi_valid);
				this.bi_valid += length - DeflateManager.Buf_size;
				return;
			}
			this.bi_buf |= (short)(value << this.bi_valid & 65535);
			this.bi_valid += length;
		}

		// Token: 0x06009131 RID: 37169 RVA: 0x00340A68 File Offset: 0x0033EC68
		internal void _tr_align()
		{
			this.send_bits(DeflateManager.STATIC_TREES << 1, 3);
			this.send_code(DeflateManager.END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
			this.bi_flush();
			if (1 + this.last_eob_len + 10 - this.bi_valid < 9)
			{
				this.send_bits(DeflateManager.STATIC_TREES << 1, 3);
				this.send_code(DeflateManager.END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
				this.bi_flush();
			}
			this.last_eob_len = 7;
		}

		// Token: 0x06009132 RID: 37170 RVA: 0x00340ADC File Offset: 0x0033ECDC
		internal bool _tr_tally(int dist, int lc)
		{
			this.pending[this._distanceOffset + this.last_lit * 2] = (byte)((uint)dist >> 8);
			this.pending[this._distanceOffset + this.last_lit * 2 + 1] = (byte)dist;
			this.pending[this._lengthOffset + this.last_lit] = (byte)lc;
			this.last_lit++;
			if (dist == 0)
			{
				short[] array = this.dyn_ltree;
				int num = lc * 2;
				array[num] += 1;
			}
			else
			{
				this.matches++;
				dist--;
				short[] array2 = this.dyn_ltree;
				int num2 = ((int)Tree.LengthCode[lc] + InternalConstants.LITERALS + 1) * 2;
				array2[num2] += 1;
				short[] array3 = this.dyn_dtree;
				int num3 = Tree.DistanceCode(dist) * 2;
				array3[num3] += 1;
			}
			if ((this.last_lit & 8191) == 0 && this.compressionLevel > CompressionLevel.Level2)
			{
				int num4 = this.last_lit << 3;
				int num5 = this.strstart - this.block_start;
				for (int i = 0; i < InternalConstants.D_CODES; i++)
				{
					num4 = (int)((long)num4 + (long)this.dyn_dtree[i * 2] * (5L + (long)Tree.ExtraDistanceBits[i]));
				}
				num4 >>= 3;
				if (this.matches < this.last_lit / 2 && num4 < num5 / 2)
				{
					return true;
				}
			}
			return this.last_lit == this.lit_bufsize - 1 || this.last_lit == this.lit_bufsize;
		}

		// Token: 0x06009133 RID: 37171 RVA: 0x00340C40 File Offset: 0x0033EE40
		internal void send_compressed_block(short[] ltree, short[] dtree)
		{
			int num = 0;
			if (this.last_lit != 0)
			{
				do
				{
					int num2 = this._distanceOffset + num * 2;
					int num3 = ((int)this.pending[num2] << 8 & 65280) | (int)(this.pending[num2 + 1] & byte.MaxValue);
					int num4 = (int)(this.pending[this._lengthOffset + num] & byte.MaxValue);
					num++;
					if (num3 == 0)
					{
						this.send_code(num4, ltree);
					}
					else
					{
						int num5 = (int)Tree.LengthCode[num4];
						this.send_code(num5 + InternalConstants.LITERALS + 1, ltree);
						int num6 = Tree.ExtraLengthBits[num5];
						if (num6 != 0)
						{
							num4 -= Tree.LengthBase[num5];
							this.send_bits(num4, num6);
						}
						num3--;
						num5 = Tree.DistanceCode(num3);
						this.send_code(num5, dtree);
						num6 = Tree.ExtraDistanceBits[num5];
						if (num6 != 0)
						{
							num3 -= Tree.DistanceBase[num5];
							this.send_bits(num3, num6);
						}
					}
				}
				while (num < this.last_lit);
			}
			this.send_code(DeflateManager.END_BLOCK, ltree);
			this.last_eob_len = (int)ltree[DeflateManager.END_BLOCK * 2 + 1];
		}

		// Token: 0x06009134 RID: 37172 RVA: 0x00340D48 File Offset: 0x0033EF48
		internal void set_data_type()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < 7)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 128)
			{
				num += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < InternalConstants.LITERALS)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			this.data_type = (sbyte)((num2 > num >> 2) ? DeflateManager.Z_BINARY : DeflateManager.Z_ASCII);
		}

		// Token: 0x06009135 RID: 37173 RVA: 0x00340DC4 File Offset: 0x0033EFC4
		internal void bi_flush()
		{
			if (this.bi_valid == 16)
			{
				byte[] array = this.pending;
				int num = this.pendingCount;
				this.pendingCount = num + 1;
				array[num] = (byte)this.bi_buf;
				byte[] array2 = this.pending;
				num = this.pendingCount;
				this.pendingCount = num + 1;
				array2[num] = (byte)(this.bi_buf >> 8);
				this.bi_buf = 0;
				this.bi_valid = 0;
				return;
			}
			if (this.bi_valid >= 8)
			{
				byte[] array3 = this.pending;
				int num = this.pendingCount;
				this.pendingCount = num + 1;
				array3[num] = (byte)this.bi_buf;
				this.bi_buf = (short)(this.bi_buf >> 8);
				this.bi_valid -= 8;
			}
		}

		// Token: 0x06009136 RID: 37174 RVA: 0x00340E70 File Offset: 0x0033F070
		internal void bi_windup()
		{
			if (this.bi_valid > 8)
			{
				byte[] array = this.pending;
				int num = this.pendingCount;
				this.pendingCount = num + 1;
				array[num] = (byte)this.bi_buf;
				byte[] array2 = this.pending;
				num = this.pendingCount;
				this.pendingCount = num + 1;
				array2[num] = (byte)(this.bi_buf >> 8);
			}
			else if (this.bi_valid > 0)
			{
				byte[] array3 = this.pending;
				int num = this.pendingCount;
				this.pendingCount = num + 1;
				array3[num] = (byte)this.bi_buf;
			}
			this.bi_buf = 0;
			this.bi_valid = 0;
		}

		// Token: 0x06009137 RID: 37175 RVA: 0x00340F00 File Offset: 0x0033F100
		internal void copy_block(int buf, int len, bool header)
		{
			this.bi_windup();
			this.last_eob_len = 8;
			if (header)
			{
				byte[] array = this.pending;
				int num = this.pendingCount;
				this.pendingCount = num + 1;
				array[num] = (byte)len;
				byte[] array2 = this.pending;
				num = this.pendingCount;
				this.pendingCount = num + 1;
				array2[num] = (byte)(len >> 8);
				byte[] array3 = this.pending;
				num = this.pendingCount;
				this.pendingCount = num + 1;
				array3[num] = (byte)(~(byte)len);
				byte[] array4 = this.pending;
				num = this.pendingCount;
				this.pendingCount = num + 1;
				array4[num] = (byte)(~len >> 8);
			}
			this.put_bytes(this.window, buf, len);
		}

		// Token: 0x06009138 RID: 37176 RVA: 0x00340F99 File Offset: 0x0033F199
		internal void flush_block_only(bool eof)
		{
			this._tr_flush_block((this.block_start >= 0) ? this.block_start : -1, this.strstart - this.block_start, eof);
			this.block_start = this.strstart;
			this._codec.flush_pending();
		}

		// Token: 0x06009139 RID: 37177 RVA: 0x00340FD8 File Offset: 0x0033F1D8
		internal BlockState DeflateNone(FlushType flush)
		{
			int num = 65535;
			if (num > this.pending.Length - 5)
			{
				num = this.pending.Length - 5;
			}
			for (;;)
			{
				if (this.lookahead <= 1)
				{
					this._fillWindow();
					if (this.lookahead == 0 && flush == FlushType.None)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_DB;
					}
				}
				this.strstart += this.lookahead;
				this.lookahead = 0;
				int num2 = this.block_start + num;
				if (this.strstart == 0 || this.strstart >= num2)
				{
					this.lookahead = this.strstart - num2;
					this.strstart = num2;
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
				if (this.strstart - this.block_start >= this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
			}
			return BlockState.NeedMore;
			IL_DB:
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.NeedMore;
				}
				return BlockState.FinishStarted;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
		}

		// Token: 0x0600913A RID: 37178 RVA: 0x003410E6 File Offset: 0x0033F2E6
		internal void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			this.send_bits((DeflateManager.STORED_BLOCK << 1) + (eof ? 1 : 0), 3);
			this.copy_block(buf, stored_len, true);
		}

		// Token: 0x0600913B RID: 37179 RVA: 0x00341108 File Offset: 0x0033F308
		internal void _tr_flush_block(int buf, int stored_len, bool eof)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.compressionLevel > CompressionLevel.None)
			{
				if ((int)this.data_type == DeflateManager.Z_UNKNOWN)
				{
					this.set_data_type();
				}
				this.treeLiterals.build_tree(this);
				this.treeDistances.build_tree(this);
				num = this.build_bl_tree();
				num2 = this.opt_len + 3 + 7 >> 3;
				num3 = this.static_len + 3 + 7 >> 3;
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num3 = (num2 = stored_len + 5);
			}
			if (stored_len + 4 <= num2 && buf != -1)
			{
				this._tr_stored_block(buf, stored_len, eof);
			}
			else if (num3 == num2)
			{
				this.send_bits((DeflateManager.STATIC_TREES << 1) + (eof ? 1 : 0), 3);
				this.send_compressed_block(StaticTree.lengthAndLiteralsTreeCodes, StaticTree.distTreeCodes);
			}
			else
			{
				this.send_bits((DeflateManager.DYN_TREES << 1) + (eof ? 1 : 0), 3);
				this.send_all_trees(this.treeLiterals.max_code + 1, this.treeDistances.max_code + 1, num + 1);
				this.send_compressed_block(this.dyn_ltree, this.dyn_dtree);
			}
			this._InitializeBlocks();
			if (eof)
			{
				this.bi_windup();
			}
		}

		// Token: 0x0600913C RID: 37180 RVA: 0x00341218 File Offset: 0x0033F418
		private void _fillWindow()
		{
			for (;;)
			{
				int num = this.window_size - this.lookahead - this.strstart;
				int num2;
				if (num == 0 && this.strstart == 0 && this.lookahead == 0)
				{
					num = this.w_size;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.strstart >= this.w_size + this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
					this.match_start -= this.w_size;
					this.strstart -= this.w_size;
					this.block_start -= this.w_size;
					num2 = this.hash_size;
					int num3 = num2;
					do
					{
						int num4 = (int)this.head[--num3] & 65535;
						this.head[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num2 = this.w_size;
					num3 = num2;
					do
					{
						int num4 = (int)this.prev[--num3] & 65535;
						this.prev[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num += this.w_size;
				}
				if (this._codec.AvailableBytesIn == 0)
				{
					break;
				}
				num2 = this._codec.read_buf(this.window, this.strstart + this.lookahead, num);
				this.lookahead += num2;
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
				}
				if (this.lookahead >= DeflateManager.MIN_LOOKAHEAD || this._codec.AvailableBytesIn == 0)
				{
					return;
				}
			}
		}

		// Token: 0x0600913D RID: 37181 RVA: 0x00341418 File Offset: 0x0033F618
		internal BlockState DeflateFast(FlushType flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < DeflateManager.MIN_LOOKAHEAD)
				{
					this._fillWindow();
					if (this.lookahead < DeflateManager.MIN_LOOKAHEAD && flush == FlushType.None)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_2E8;
					}
				}
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				if ((long)num != 0L && (this.strstart - num & 65535) <= this.w_size - DeflateManager.MIN_LOOKAHEAD && this.compressionStrategy != CompressionStrategy.HuffmanOnly)
				{
					this.match_length = this.longest_match(num);
				}
				bool flag;
				if (this.match_length >= DeflateManager.MIN_MATCH)
				{
					flag = this._tr_tally(this.strstart - this.match_start, this.match_length - DeflateManager.MIN_MATCH);
					this.lookahead -= this.match_length;
					if (this.match_length <= this.config.MaxLazy && this.lookahead >= DeflateManager.MIN_MATCH)
					{
						this.match_length--;
						int num2;
						do
						{
							this.strstart++;
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
							num2 = this.match_length - 1;
							this.match_length = num2;
						}
						while (num2 != 0);
						this.strstart++;
					}
					else
					{
						this.strstart += this.match_length;
						this.match_length = 0;
						this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
						this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
					}
				}
				else
				{
					flag = this._tr_tally(0, (int)(this.window[this.strstart] & byte.MaxValue));
					this.lookahead--;
					this.strstart++;
				}
				if (flag)
				{
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
			}
			return BlockState.NeedMore;
			IL_2E8:
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush == FlushType.Finish)
				{
					return BlockState.FinishStarted;
				}
				return BlockState.NeedMore;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
		}

		// Token: 0x0600913E RID: 37182 RVA: 0x00341734 File Offset: 0x0033F934
		internal BlockState DeflateSlow(FlushType flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < DeflateManager.MIN_LOOKAHEAD)
				{
					this._fillWindow();
					if (this.lookahead < DeflateManager.MIN_LOOKAHEAD && flush == FlushType.None)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_363;
					}
				}
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				this.prev_length = this.match_length;
				this.prev_match = this.match_start;
				this.match_length = DeflateManager.MIN_MATCH - 1;
				if (num != 0 && this.prev_length < this.config.MaxLazy && (this.strstart - num & 65535) <= this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					if (this.compressionStrategy != CompressionStrategy.HuffmanOnly)
					{
						this.match_length = this.longest_match(num);
					}
					if (this.match_length <= 5 && (this.compressionStrategy == CompressionStrategy.Filtered || (this.match_length == DeflateManager.MIN_MATCH && this.strstart - this.match_start > 4096)))
					{
						this.match_length = DeflateManager.MIN_MATCH - 1;
					}
				}
				if (this.prev_length >= DeflateManager.MIN_MATCH && this.match_length <= this.prev_length)
				{
					int num2 = this.strstart + this.lookahead - DeflateManager.MIN_MATCH;
					bool flag = this._tr_tally(this.strstart - 1 - this.prev_match, this.prev_length - DeflateManager.MIN_MATCH);
					this.lookahead -= this.prev_length - 1;
					this.prev_length -= 2;
					int num3;
					do
					{
						num3 = this.strstart + 1;
						this.strstart = num3;
						if (num3 <= num2)
						{
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
						num3 = this.prev_length - 1;
						this.prev_length = num3;
					}
					while (num3 != 0);
					this.match_available = 0;
					this.match_length = DeflateManager.MIN_MATCH - 1;
					this.strstart++;
					if (flag)
					{
						this.flush_block_only(false);
						if (this._codec.AvailableBytesOut == 0)
						{
							return BlockState.NeedMore;
						}
					}
				}
				else if (this.match_available != 0)
				{
					bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & byte.MaxValue));
					if (flag)
					{
						this.flush_block_only(false);
					}
					this.strstart++;
					this.lookahead--;
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
				else
				{
					this.match_available = 1;
					this.strstart++;
					this.lookahead--;
				}
			}
			return BlockState.NeedMore;
			IL_363:
			if (this.match_available != 0)
			{
				bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & byte.MaxValue));
				this.match_available = 0;
			}
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush == FlushType.Finish)
				{
					return BlockState.FinishStarted;
				}
				return BlockState.NeedMore;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
		}

		// Token: 0x0600913F RID: 37183 RVA: 0x00341AF8 File Offset: 0x0033FCF8
		internal int longest_match(int cur_match)
		{
			int num = this.config.MaxChainLength;
			int num2 = this.strstart;
			int num3 = this.prev_length;
			int num4 = (this.strstart > this.w_size - DeflateManager.MIN_LOOKAHEAD) ? (this.strstart - (this.w_size - DeflateManager.MIN_LOOKAHEAD)) : 0;
			int niceLength = this.config.NiceLength;
			int num5 = this.w_mask;
			int num6 = this.strstart + DeflateManager.MAX_MATCH;
			byte b = this.window[num2 + num3 - 1];
			byte b2 = this.window[num2 + num3];
			if (this.prev_length >= this.config.GoodLength)
			{
				num >>= 2;
			}
			if (niceLength > this.lookahead)
			{
				niceLength = this.lookahead;
			}
			do
			{
				int num7 = cur_match;
				if (this.window[num7 + num3] == b2 && this.window[num7 + num3 - 1] == b && this.window[num7] == this.window[num2] && this.window[++num7] == this.window[num2 + 1])
				{
					num2 += 2;
					num7++;
					while (this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && num2 < num6)
					{
					}
					int num8 = DeflateManager.MAX_MATCH - (num6 - num2);
					num2 = num6 - DeflateManager.MAX_MATCH;
					if (num8 > num3)
					{
						this.match_start = cur_match;
						num3 = num8;
						if (num8 >= niceLength)
						{
							break;
						}
						b = this.window[num2 + num3 - 1];
						b2 = this.window[num2 + num3];
					}
				}
			}
			while ((cur_match = ((int)this.prev[cur_match & num5] & 65535)) > num4 && --num != 0);
			if (num3 <= this.lookahead)
			{
				return num3;
			}
			return this.lookahead;
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x06009140 RID: 37184 RVA: 0x00341D6E File Offset: 0x0033FF6E
		// (set) Token: 0x06009141 RID: 37185 RVA: 0x00341D76 File Offset: 0x0033FF76
		internal bool WantRfc1950HeaderBytes
		{
			get
			{
				return this._WantRfc1950HeaderBytes;
			}
			set
			{
				this._WantRfc1950HeaderBytes = value;
			}
		}

		// Token: 0x06009142 RID: 37186 RVA: 0x00341D7F File Offset: 0x0033FF7F
		internal int Initialize(ZlibCodec codec, CompressionLevel level)
		{
			return this.Initialize(codec, level, 15);
		}

		// Token: 0x06009143 RID: 37187 RVA: 0x00341D8B File Offset: 0x0033FF8B
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, CompressionStrategy.Default);
		}

		// Token: 0x06009144 RID: 37188 RVA: 0x00341D9C File Offset: 0x0033FF9C
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits, CompressionStrategy compressionStrategy)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, compressionStrategy);
		}

		// Token: 0x06009145 RID: 37189 RVA: 0x00341DB0 File Offset: 0x0033FFB0
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy)
		{
			this._codec = codec;
			this._codec.Message = null;
			if (windowBits < 9 || windowBits > 15)
			{
				throw new ZlibException("windowBits must be in the range 9..15.");
			}
			if (memLevel < 1 || memLevel > DeflateManager.MEM_LEVEL_MAX)
			{
				throw new ZlibException(string.Format("memLevel must be in the range 1.. {0}", DeflateManager.MEM_LEVEL_MAX));
			}
			this._codec.dstate = this;
			this.w_bits = windowBits;
			this.w_size = 1 << this.w_bits;
			this.w_mask = this.w_size - 1;
			this.hash_bits = memLevel + 7;
			this.hash_size = 1 << this.hash_bits;
			this.hash_mask = this.hash_size - 1;
			this.hash_shift = (this.hash_bits + DeflateManager.MIN_MATCH - 1) / DeflateManager.MIN_MATCH;
			this.window = new byte[this.w_size * 2];
			this.prev = new short[this.w_size];
			this.head = new short[this.hash_size];
			this.lit_bufsize = 1 << memLevel + 6;
			this.pending = new byte[this.lit_bufsize * 4];
			this._distanceOffset = this.lit_bufsize;
			this._lengthOffset = 3 * this.lit_bufsize;
			this.compressionLevel = level;
			this.compressionStrategy = strategy;
			this.Reset();
			return 0;
		}

		// Token: 0x06009146 RID: 37190 RVA: 0x00341F0C File Offset: 0x0034010C
		internal void Reset()
		{
			this._codec.TotalBytesIn = (this._codec.TotalBytesOut = 0L);
			this._codec.Message = null;
			this.pendingCount = 0;
			this.nextPending = 0;
			this.Rfc1950BytesEmitted = false;
			this.status = (this.WantRfc1950HeaderBytes ? DeflateManager.INIT_STATE : DeflateManager.BUSY_STATE);
			this._codec._Adler32 = Adler.Adler32(0U, null, 0, 0);
			this.last_flush = 0;
			this._InitializeTreeData();
			this._InitializeLazyMatch();
		}

		// Token: 0x06009147 RID: 37191 RVA: 0x00341F98 File Offset: 0x00340198
		internal int End()
		{
			if (this.status != DeflateManager.INIT_STATE && this.status != DeflateManager.BUSY_STATE && this.status != DeflateManager.FINISH_STATE)
			{
				return -2;
			}
			this.pending = null;
			this.head = null;
			this.prev = null;
			this.window = null;
			if (this.status != DeflateManager.BUSY_STATE)
			{
				return 0;
			}
			return -3;
		}

		// Token: 0x06009148 RID: 37192 RVA: 0x00341FFC File Offset: 0x003401FC
		private void SetDeflater()
		{
			switch (this.config.Flavor)
			{
			case DeflateFlavor.Store:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateNone);
				return;
			case DeflateFlavor.Fast:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateFast);
				return;
			case DeflateFlavor.Slow:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateSlow);
				return;
			default:
				return;
			}
		}

		// Token: 0x06009149 RID: 37193 RVA: 0x00342060 File Offset: 0x00340260
		internal int SetParams(CompressionLevel level, CompressionStrategy strategy)
		{
			int result = 0;
			if (this.compressionLevel != level)
			{
				DeflateManager.Config config = DeflateManager.Config.Lookup(level);
				if (config.Flavor != this.config.Flavor && this._codec.TotalBytesIn != 0L)
				{
					result = this._codec.Deflate(FlushType.Partial);
				}
				this.compressionLevel = level;
				this.config = config;
				this.SetDeflater();
			}
			this.compressionStrategy = strategy;
			return result;
		}

		// Token: 0x0600914A RID: 37194 RVA: 0x003420C8 File Offset: 0x003402C8
		internal int SetDictionary(byte[] dictionary)
		{
			int num = dictionary.Length;
			int sourceIndex = 0;
			if (dictionary == null || this.status != DeflateManager.INIT_STATE)
			{
				throw new ZlibException("Stream error.");
			}
			this._codec._Adler32 = Adler.Adler32(this._codec._Adler32, dictionary, 0, dictionary.Length);
			if (num < DeflateManager.MIN_MATCH)
			{
				return 0;
			}
			if (num > this.w_size - DeflateManager.MIN_LOOKAHEAD)
			{
				num = this.w_size - DeflateManager.MIN_LOOKAHEAD;
				sourceIndex = dictionary.Length - num;
			}
			Array.Copy(dictionary, sourceIndex, this.window, 0, num);
			this.strstart = num;
			this.block_start = num;
			this.ins_h = (int)(this.window[0] & byte.MaxValue);
			this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[1] & byte.MaxValue)) & this.hash_mask);
			for (int i = 0; i <= num - DeflateManager.MIN_MATCH; i++)
			{
				this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[i + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
				this.prev[i & this.w_mask] = this.head[this.ins_h];
				this.head[this.ins_h] = (short)i;
			}
			return 0;
		}

		// Token: 0x0600914B RID: 37195 RVA: 0x00342214 File Offset: 0x00340414
		internal int Deflate(FlushType flush)
		{
			if (this._codec.OutputBuffer == null || (this._codec.InputBuffer == null && this._codec.AvailableBytesIn != 0) || (this.status == DeflateManager.FINISH_STATE && flush != FlushType.Finish))
			{
				this._codec.Message = DeflateManager._ErrorMessage[4];
				throw new ZlibException(string.Format("Something is fishy. [{0}]", this._codec.Message));
			}
			if (this._codec.AvailableBytesOut == 0)
			{
				this._codec.Message = DeflateManager._ErrorMessage[7];
				throw new ZlibException("OutputBuffer is full (AvailableBytesOut == 0)");
			}
			int num = this.last_flush;
			this.last_flush = (int)flush;
			int num4;
			if (this.status == DeflateManager.INIT_STATE)
			{
				int num2 = DeflateManager.Z_DEFLATED + (this.w_bits - 8 << 4) << 8;
				int num3 = (this.compressionLevel - CompressionLevel.BestSpeed & 255) >> 1;
				if (num3 > 3)
				{
					num3 = 3;
				}
				num2 |= num3 << 6;
				if (this.strstart != 0)
				{
					num2 |= DeflateManager.PRESET_DICT;
				}
				num2 += 31 - num2 % 31;
				this.status = DeflateManager.BUSY_STATE;
				byte[] array = this.pending;
				num4 = this.pendingCount;
				this.pendingCount = num4 + 1;
				array[num4] = (byte)(num2 >> 8);
				byte[] array2 = this.pending;
				num4 = this.pendingCount;
				this.pendingCount = num4 + 1;
				array2[num4] = (byte)num2;
				if (this.strstart != 0)
				{
					byte[] array3 = this.pending;
					num4 = this.pendingCount;
					this.pendingCount = num4 + 1;
					array3[num4] = (byte)((this._codec._Adler32 & 4278190080U) >> 24);
					byte[] array4 = this.pending;
					num4 = this.pendingCount;
					this.pendingCount = num4 + 1;
					array4[num4] = (byte)((this._codec._Adler32 & 16711680U) >> 16);
					byte[] array5 = this.pending;
					num4 = this.pendingCount;
					this.pendingCount = num4 + 1;
					array5[num4] = (byte)((this._codec._Adler32 & 65280U) >> 8);
					byte[] array6 = this.pending;
					num4 = this.pendingCount;
					this.pendingCount = num4 + 1;
					array6[num4] = (byte)(this._codec._Adler32 & 255U);
				}
				this._codec._Adler32 = Adler.Adler32(0U, null, 0, 0);
			}
			if (this.pendingCount != 0)
			{
				this._codec.flush_pending();
				if (this._codec.AvailableBytesOut == 0)
				{
					this.last_flush = -1;
					return 0;
				}
			}
			else if (this._codec.AvailableBytesIn == 0 && flush <= (FlushType)num && flush != FlushType.Finish)
			{
				return 0;
			}
			if (this.status == DeflateManager.FINISH_STATE && this._codec.AvailableBytesIn != 0)
			{
				this._codec.Message = DeflateManager._ErrorMessage[7];
				throw new ZlibException("status == FINISH_STATE && _codec.AvailableBytesIn != 0");
			}
			if (this._codec.AvailableBytesIn != 0 || this.lookahead != 0 || (flush != FlushType.None && this.status != DeflateManager.FINISH_STATE))
			{
				BlockState blockState = this.DeflateFunction(flush);
				if (blockState == BlockState.FinishStarted || blockState == BlockState.FinishDone)
				{
					this.status = DeflateManager.FINISH_STATE;
				}
				if (blockState == BlockState.NeedMore || blockState == BlockState.FinishStarted)
				{
					if (this._codec.AvailableBytesOut == 0)
					{
						this.last_flush = -1;
					}
					return 0;
				}
				if (blockState == BlockState.BlockDone)
				{
					if (flush == FlushType.Partial)
					{
						this._tr_align();
					}
					else
					{
						this._tr_stored_block(0, 0, false);
						if (flush == FlushType.Full)
						{
							for (int i = 0; i < this.hash_size; i++)
							{
								this.head[i] = 0;
							}
						}
					}
					this._codec.flush_pending();
					if (this._codec.AvailableBytesOut == 0)
					{
						this.last_flush = -1;
						return 0;
					}
				}
			}
			if (flush != FlushType.Finish)
			{
				return 0;
			}
			if (!this.WantRfc1950HeaderBytes || this.Rfc1950BytesEmitted)
			{
				return 1;
			}
			byte[] array7 = this.pending;
			num4 = this.pendingCount;
			this.pendingCount = num4 + 1;
			array7[num4] = (byte)((this._codec._Adler32 & 4278190080U) >> 24);
			byte[] array8 = this.pending;
			num4 = this.pendingCount;
			this.pendingCount = num4 + 1;
			array8[num4] = (byte)((this._codec._Adler32 & 16711680U) >> 16);
			byte[] array9 = this.pending;
			num4 = this.pendingCount;
			this.pendingCount = num4 + 1;
			array9[num4] = (byte)((this._codec._Adler32 & 65280U) >> 8);
			byte[] array10 = this.pending;
			num4 = this.pendingCount;
			this.pendingCount = num4 + 1;
			array10[num4] = (byte)(this._codec._Adler32 & 255U);
			this._codec.flush_pending();
			this.Rfc1950BytesEmitted = true;
			if (this.pendingCount == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x04005B1B RID: 23323
		private static readonly int MEM_LEVEL_MAX = 9;

		// Token: 0x04005B1C RID: 23324
		private static readonly int MEM_LEVEL_DEFAULT = 8;

		// Token: 0x04005B1D RID: 23325
		private DeflateManager.CompressFunc DeflateFunction;

		// Token: 0x04005B1E RID: 23326
		private static readonly string[] _ErrorMessage = new string[]
		{
			"need dictionary",
			"stream end",
			"",
			"file error",
			"stream error",
			"data error",
			"insufficient memory",
			"buffer error",
			"incompatible version",
			""
		};

		// Token: 0x04005B1F RID: 23327
		private static readonly int PRESET_DICT = 32;

		// Token: 0x04005B20 RID: 23328
		private static readonly int INIT_STATE = 42;

		// Token: 0x04005B21 RID: 23329
		private static readonly int BUSY_STATE = 113;

		// Token: 0x04005B22 RID: 23330
		private static readonly int FINISH_STATE = 666;

		// Token: 0x04005B23 RID: 23331
		private static readonly int Z_DEFLATED = 8;

		// Token: 0x04005B24 RID: 23332
		private static readonly int STORED_BLOCK = 0;

		// Token: 0x04005B25 RID: 23333
		private static readonly int STATIC_TREES = 1;

		// Token: 0x04005B26 RID: 23334
		private static readonly int DYN_TREES = 2;

		// Token: 0x04005B27 RID: 23335
		private static readonly int Z_BINARY = 0;

		// Token: 0x04005B28 RID: 23336
		private static readonly int Z_ASCII = 1;

		// Token: 0x04005B29 RID: 23337
		private static readonly int Z_UNKNOWN = 2;

		// Token: 0x04005B2A RID: 23338
		private static readonly int Buf_size = 16;

		// Token: 0x04005B2B RID: 23339
		private static readonly int MIN_MATCH = 3;

		// Token: 0x04005B2C RID: 23340
		private static readonly int MAX_MATCH = 258;

		// Token: 0x04005B2D RID: 23341
		private static readonly int MIN_LOOKAHEAD = DeflateManager.MAX_MATCH + DeflateManager.MIN_MATCH + 1;

		// Token: 0x04005B2E RID: 23342
		private static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

		// Token: 0x04005B2F RID: 23343
		private static readonly int END_BLOCK = 256;

		// Token: 0x04005B30 RID: 23344
		internal ZlibCodec _codec;

		// Token: 0x04005B31 RID: 23345
		internal int status;

		// Token: 0x04005B32 RID: 23346
		internal byte[] pending;

		// Token: 0x04005B33 RID: 23347
		internal int nextPending;

		// Token: 0x04005B34 RID: 23348
		internal int pendingCount;

		// Token: 0x04005B35 RID: 23349
		internal sbyte data_type;

		// Token: 0x04005B36 RID: 23350
		internal int last_flush;

		// Token: 0x04005B37 RID: 23351
		internal int w_size;

		// Token: 0x04005B38 RID: 23352
		internal int w_bits;

		// Token: 0x04005B39 RID: 23353
		internal int w_mask;

		// Token: 0x04005B3A RID: 23354
		internal byte[] window;

		// Token: 0x04005B3B RID: 23355
		internal int window_size;

		// Token: 0x04005B3C RID: 23356
		internal short[] prev;

		// Token: 0x04005B3D RID: 23357
		internal short[] head;

		// Token: 0x04005B3E RID: 23358
		internal int ins_h;

		// Token: 0x04005B3F RID: 23359
		internal int hash_size;

		// Token: 0x04005B40 RID: 23360
		internal int hash_bits;

		// Token: 0x04005B41 RID: 23361
		internal int hash_mask;

		// Token: 0x04005B42 RID: 23362
		internal int hash_shift;

		// Token: 0x04005B43 RID: 23363
		internal int block_start;

		// Token: 0x04005B44 RID: 23364
		private DeflateManager.Config config;

		// Token: 0x04005B45 RID: 23365
		internal int match_length;

		// Token: 0x04005B46 RID: 23366
		internal int prev_match;

		// Token: 0x04005B47 RID: 23367
		internal int match_available;

		// Token: 0x04005B48 RID: 23368
		internal int strstart;

		// Token: 0x04005B49 RID: 23369
		internal int match_start;

		// Token: 0x04005B4A RID: 23370
		internal int lookahead;

		// Token: 0x04005B4B RID: 23371
		internal int prev_length;

		// Token: 0x04005B4C RID: 23372
		internal CompressionLevel compressionLevel;

		// Token: 0x04005B4D RID: 23373
		internal CompressionStrategy compressionStrategy;

		// Token: 0x04005B4E RID: 23374
		internal short[] dyn_ltree;

		// Token: 0x04005B4F RID: 23375
		internal short[] dyn_dtree;

		// Token: 0x04005B50 RID: 23376
		internal short[] bl_tree;

		// Token: 0x04005B51 RID: 23377
		internal Tree treeLiterals = new Tree();

		// Token: 0x04005B52 RID: 23378
		internal Tree treeDistances = new Tree();

		// Token: 0x04005B53 RID: 23379
		internal Tree treeBitLengths = new Tree();

		// Token: 0x04005B54 RID: 23380
		internal short[] bl_count = new short[InternalConstants.MAX_BITS + 1];

		// Token: 0x04005B55 RID: 23381
		internal int[] heap = new int[2 * InternalConstants.L_CODES + 1];

		// Token: 0x04005B56 RID: 23382
		internal int heap_len;

		// Token: 0x04005B57 RID: 23383
		internal int heap_max;

		// Token: 0x04005B58 RID: 23384
		internal sbyte[] depth = new sbyte[2 * InternalConstants.L_CODES + 1];

		// Token: 0x04005B59 RID: 23385
		internal int _lengthOffset;

		// Token: 0x04005B5A RID: 23386
		internal int lit_bufsize;

		// Token: 0x04005B5B RID: 23387
		internal int last_lit;

		// Token: 0x04005B5C RID: 23388
		internal int _distanceOffset;

		// Token: 0x04005B5D RID: 23389
		internal int opt_len;

		// Token: 0x04005B5E RID: 23390
		internal int static_len;

		// Token: 0x04005B5F RID: 23391
		internal int matches;

		// Token: 0x04005B60 RID: 23392
		internal int last_eob_len;

		// Token: 0x04005B61 RID: 23393
		internal short bi_buf;

		// Token: 0x04005B62 RID: 23394
		internal int bi_valid;

		// Token: 0x04005B63 RID: 23395
		private bool Rfc1950BytesEmitted;

		// Token: 0x04005B64 RID: 23396
		private bool _WantRfc1950HeaderBytes = true;

		// Token: 0x02002A9B RID: 10907
		// (Invoke) Token: 0x0600E6D9 RID: 59097
		internal delegate BlockState CompressFunc(FlushType flush);

		// Token: 0x02002A9C RID: 10908
		internal class Config
		{
			// Token: 0x0600E6DC RID: 59100 RVA: 0x0043252F File Offset: 0x0043072F
			private Config(int goodLength, int maxLazy, int niceLength, int maxChainLength, DeflateFlavor flavor)
			{
				this.GoodLength = goodLength;
				this.MaxLazy = maxLazy;
				this.NiceLength = niceLength;
				this.MaxChainLength = maxChainLength;
				this.Flavor = flavor;
			}

			// Token: 0x0600E6DD RID: 59101 RVA: 0x0043255C File Offset: 0x0043075C
			public static DeflateManager.Config Lookup(CompressionLevel level)
			{
				return DeflateManager.Config.Table[(int)level];
			}

			// Token: 0x0400A06B RID: 41067
			internal int GoodLength;

			// Token: 0x0400A06C RID: 41068
			internal int MaxLazy;

			// Token: 0x0400A06D RID: 41069
			internal int NiceLength;

			// Token: 0x0400A06E RID: 41070
			internal int MaxChainLength;

			// Token: 0x0400A06F RID: 41071
			internal DeflateFlavor Flavor;

			// Token: 0x0400A070 RID: 41072
			private static readonly DeflateManager.Config[] Table = new DeflateManager.Config[]
			{
				new DeflateManager.Config(0, 0, 0, 0, DeflateFlavor.Store),
				new DeflateManager.Config(4, 4, 8, 4, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 5, 16, 8, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 6, 32, 32, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 4, 16, 16, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 16, 32, 32, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 16, 128, 128, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 32, 128, 256, DeflateFlavor.Slow),
				new DeflateManager.Config(32, 128, 258, 1024, DeflateFlavor.Slow),
				new DeflateManager.Config(32, 258, 258, 4096, DeflateFlavor.Slow)
			};
		}
	}
}
