using System;

namespace Ionic.Zlib
{
	// Token: 0x02002207 RID: 8711
	internal sealed class DeflateManager
	{
		// Token: 0x0600BAD5 RID: 47829 RVA: 0x0035A7BC File Offset: 0x003589BC
		internal DeflateManager()
		{
			this.dyn_ltree = new short[DeflateManager.HEAP_SIZE * 2];
			this.dyn_dtree = new short[(2 * InternalConstants.D_CODES + 1) * 2];
			this.bl_tree = new short[(2 * InternalConstants.BL_CODES + 1) * 2];
		}

		// Token: 0x0600BAD6 RID: 47830 RVA: 0x0035A870 File Offset: 0x00358A70
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

		// Token: 0x0600BAD7 RID: 47831 RVA: 0x0035A8F0 File Offset: 0x00358AF0
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

		// Token: 0x0600BAD8 RID: 47832 RVA: 0x0035A97C File Offset: 0x00358B7C
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

		// Token: 0x0600BAD9 RID: 47833 RVA: 0x0035AA0C File Offset: 0x00358C0C
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

		// Token: 0x0600BADA RID: 47834 RVA: 0x0035AA98 File Offset: 0x00358C98
		internal static bool _IsSmaller(short[] tree, int n, int m, sbyte[] depth)
		{
			short num = tree[n * 2];
			short num2 = tree[m * 2];
			return num < num2 || (num == num2 && depth[n] <= depth[m]);
		}

		// Token: 0x0600BADB RID: 47835 RVA: 0x0035AAC8 File Offset: 0x00358CC8
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

		// Token: 0x0600BADC RID: 47836 RVA: 0x0035ABE4 File Offset: 0x00358DE4
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

		// Token: 0x0600BADD RID: 47837 RVA: 0x0035AC6C File Offset: 0x00358E6C
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

		// Token: 0x0600BADE RID: 47838 RVA: 0x0035ACE0 File Offset: 0x00358EE0
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

		// Token: 0x0600BADF RID: 47839 RVA: 0x00078FDE File Offset: 0x000771DE
		private void put_bytes(byte[] p, int start, int len)
		{
			Array.Copy(p, start, this.pending, this.pendingCount, len);
			this.pendingCount += len;
		}

		// Token: 0x0600BAE0 RID: 47840 RVA: 0x0035ADF8 File Offset: 0x00358FF8
		internal void send_code(int c, short[] tree)
		{
			int num = c * 2;
			this.send_bits((int)tree[num] & 65535, (int)tree[num + 1] & 65535);
		}

		// Token: 0x0600BAE1 RID: 47841 RVA: 0x0035AE24 File Offset: 0x00359024
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

		// Token: 0x0600BAE2 RID: 47842 RVA: 0x0035AF00 File Offset: 0x00359100
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

		// Token: 0x0600BAE3 RID: 47843 RVA: 0x0035AF74 File Offset: 0x00359174
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

		// Token: 0x0600BAE4 RID: 47844 RVA: 0x0035B0D8 File Offset: 0x003592D8
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

		// Token: 0x0600BAE5 RID: 47845 RVA: 0x0035B1E0 File Offset: 0x003593E0
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

		// Token: 0x0600BAE6 RID: 47846 RVA: 0x0035B25C File Offset: 0x0035945C
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

		// Token: 0x0600BAE7 RID: 47847 RVA: 0x0035B308 File Offset: 0x00359508
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

		// Token: 0x0600BAE8 RID: 47848 RVA: 0x0035B398 File Offset: 0x00359598
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

		// Token: 0x0600BAE9 RID: 47849 RVA: 0x00079002 File Offset: 0x00077202
		internal void flush_block_only(bool eof)
		{
			this._tr_flush_block((this.block_start >= 0) ? this.block_start : -1, this.strstart - this.block_start, eof);
			this.block_start = this.strstart;
			this._codec.flush_pending();
		}

		// Token: 0x0600BAEA RID: 47850 RVA: 0x0035B434 File Offset: 0x00359634
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

		// Token: 0x0600BAEB RID: 47851 RVA: 0x00079041 File Offset: 0x00077241
		internal void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			this.send_bits((DeflateManager.STORED_BLOCK << 1) + (eof ? 1 : 0), 3);
			this.copy_block(buf, stored_len, true);
		}

		// Token: 0x0600BAEC RID: 47852 RVA: 0x0035B544 File Offset: 0x00359744
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

		// Token: 0x0600BAED RID: 47853 RVA: 0x0035B654 File Offset: 0x00359854
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

		// Token: 0x0600BAEE RID: 47854 RVA: 0x0035B854 File Offset: 0x00359A54
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

		// Token: 0x0600BAEF RID: 47855 RVA: 0x0035BB70 File Offset: 0x00359D70
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

		// Token: 0x0600BAF0 RID: 47856 RVA: 0x0035BF34 File Offset: 0x0035A134
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

		// Token: 0x17001BDD RID: 7133
		// (get) Token: 0x0600BAF1 RID: 47857 RVA: 0x00079062 File Offset: 0x00077262
		// (set) Token: 0x0600BAF2 RID: 47858 RVA: 0x0007906A File Offset: 0x0007726A
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

		// Token: 0x0600BAF3 RID: 47859 RVA: 0x00079073 File Offset: 0x00077273
		internal int Initialize(ZlibCodec codec, CompressionLevel level)
		{
			return this.Initialize(codec, level, 15);
		}

		// Token: 0x0600BAF4 RID: 47860 RVA: 0x0007907F File Offset: 0x0007727F
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, CompressionStrategy.Default);
		}

		// Token: 0x0600BAF5 RID: 47861 RVA: 0x00079090 File Offset: 0x00077290
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits, CompressionStrategy compressionStrategy)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, compressionStrategy);
		}

		// Token: 0x0600BAF6 RID: 47862 RVA: 0x0035C1AC File Offset: 0x0035A3AC
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

		// Token: 0x0600BAF7 RID: 47863 RVA: 0x0035C308 File Offset: 0x0035A508
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

		// Token: 0x0600BAF8 RID: 47864 RVA: 0x0035C394 File Offset: 0x0035A594
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

		// Token: 0x0600BAF9 RID: 47865 RVA: 0x0035C3F8 File Offset: 0x0035A5F8
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

		// Token: 0x0600BAFA RID: 47866 RVA: 0x0035C45C File Offset: 0x0035A65C
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

		// Token: 0x0600BAFB RID: 47867 RVA: 0x0035C4C4 File Offset: 0x0035A6C4
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

		// Token: 0x0600BAFC RID: 47868 RVA: 0x0035C610 File Offset: 0x0035A810
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

		// Token: 0x04007F84 RID: 32644
		private static readonly int MEM_LEVEL_MAX = 9;

		// Token: 0x04007F85 RID: 32645
		private static readonly int MEM_LEVEL_DEFAULT = 8;

		// Token: 0x04007F86 RID: 32646
		private DeflateManager.CompressFunc DeflateFunction;

		// Token: 0x04007F87 RID: 32647
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

		// Token: 0x04007F88 RID: 32648
		private static readonly int PRESET_DICT = 32;

		// Token: 0x04007F89 RID: 32649
		private static readonly int INIT_STATE = 42;

		// Token: 0x04007F8A RID: 32650
		private static readonly int BUSY_STATE = 113;

		// Token: 0x04007F8B RID: 32651
		private static readonly int FINISH_STATE = 666;

		// Token: 0x04007F8C RID: 32652
		private static readonly int Z_DEFLATED = 8;

		// Token: 0x04007F8D RID: 32653
		private static readonly int STORED_BLOCK = 0;

		// Token: 0x04007F8E RID: 32654
		private static readonly int STATIC_TREES = 1;

		// Token: 0x04007F8F RID: 32655
		private static readonly int DYN_TREES = 2;

		// Token: 0x04007F90 RID: 32656
		private static readonly int Z_BINARY = 0;

		// Token: 0x04007F91 RID: 32657
		private static readonly int Z_ASCII = 1;

		// Token: 0x04007F92 RID: 32658
		private static readonly int Z_UNKNOWN = 2;

		// Token: 0x04007F93 RID: 32659
		private static readonly int Buf_size = 16;

		// Token: 0x04007F94 RID: 32660
		private static readonly int MIN_MATCH = 3;

		// Token: 0x04007F95 RID: 32661
		private static readonly int MAX_MATCH = 258;

		// Token: 0x04007F96 RID: 32662
		private static readonly int MIN_LOOKAHEAD = DeflateManager.MAX_MATCH + DeflateManager.MIN_MATCH + 1;

		// Token: 0x04007F97 RID: 32663
		private static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

		// Token: 0x04007F98 RID: 32664
		private static readonly int END_BLOCK = 256;

		// Token: 0x04007F99 RID: 32665
		internal ZlibCodec _codec;

		// Token: 0x04007F9A RID: 32666
		internal int status;

		// Token: 0x04007F9B RID: 32667
		internal byte[] pending;

		// Token: 0x04007F9C RID: 32668
		internal int nextPending;

		// Token: 0x04007F9D RID: 32669
		internal int pendingCount;

		// Token: 0x04007F9E RID: 32670
		internal sbyte data_type;

		// Token: 0x04007F9F RID: 32671
		internal int last_flush;

		// Token: 0x04007FA0 RID: 32672
		internal int w_size;

		// Token: 0x04007FA1 RID: 32673
		internal int w_bits;

		// Token: 0x04007FA2 RID: 32674
		internal int w_mask;

		// Token: 0x04007FA3 RID: 32675
		internal byte[] window;

		// Token: 0x04007FA4 RID: 32676
		internal int window_size;

		// Token: 0x04007FA5 RID: 32677
		internal short[] prev;

		// Token: 0x04007FA6 RID: 32678
		internal short[] head;

		// Token: 0x04007FA7 RID: 32679
		internal int ins_h;

		// Token: 0x04007FA8 RID: 32680
		internal int hash_size;

		// Token: 0x04007FA9 RID: 32681
		internal int hash_bits;

		// Token: 0x04007FAA RID: 32682
		internal int hash_mask;

		// Token: 0x04007FAB RID: 32683
		internal int hash_shift;

		// Token: 0x04007FAC RID: 32684
		internal int block_start;

		// Token: 0x04007FAD RID: 32685
		private DeflateManager.Config config;

		// Token: 0x04007FAE RID: 32686
		internal int match_length;

		// Token: 0x04007FAF RID: 32687
		internal int prev_match;

		// Token: 0x04007FB0 RID: 32688
		internal int match_available;

		// Token: 0x04007FB1 RID: 32689
		internal int strstart;

		// Token: 0x04007FB2 RID: 32690
		internal int match_start;

		// Token: 0x04007FB3 RID: 32691
		internal int lookahead;

		// Token: 0x04007FB4 RID: 32692
		internal int prev_length;

		// Token: 0x04007FB5 RID: 32693
		internal CompressionLevel compressionLevel;

		// Token: 0x04007FB6 RID: 32694
		internal CompressionStrategy compressionStrategy;

		// Token: 0x04007FB7 RID: 32695
		internal short[] dyn_ltree;

		// Token: 0x04007FB8 RID: 32696
		internal short[] dyn_dtree;

		// Token: 0x04007FB9 RID: 32697
		internal short[] bl_tree;

		// Token: 0x04007FBA RID: 32698
		internal Tree treeLiterals = new Tree();

		// Token: 0x04007FBB RID: 32699
		internal Tree treeDistances = new Tree();

		// Token: 0x04007FBC RID: 32700
		internal Tree treeBitLengths = new Tree();

		// Token: 0x04007FBD RID: 32701
		internal short[] bl_count = new short[InternalConstants.MAX_BITS + 1];

		// Token: 0x04007FBE RID: 32702
		internal int[] heap = new int[2 * InternalConstants.L_CODES + 1];

		// Token: 0x04007FBF RID: 32703
		internal int heap_len;

		// Token: 0x04007FC0 RID: 32704
		internal int heap_max;

		// Token: 0x04007FC1 RID: 32705
		internal sbyte[] depth = new sbyte[2 * InternalConstants.L_CODES + 1];

		// Token: 0x04007FC2 RID: 32706
		internal int _lengthOffset;

		// Token: 0x04007FC3 RID: 32707
		internal int lit_bufsize;

		// Token: 0x04007FC4 RID: 32708
		internal int last_lit;

		// Token: 0x04007FC5 RID: 32709
		internal int _distanceOffset;

		// Token: 0x04007FC6 RID: 32710
		internal int opt_len;

		// Token: 0x04007FC7 RID: 32711
		internal int static_len;

		// Token: 0x04007FC8 RID: 32712
		internal int matches;

		// Token: 0x04007FC9 RID: 32713
		internal int last_eob_len;

		// Token: 0x04007FCA RID: 32714
		internal short bi_buf;

		// Token: 0x04007FCB RID: 32715
		internal int bi_valid;

		// Token: 0x04007FCC RID: 32716
		private bool Rfc1950BytesEmitted;

		// Token: 0x04007FCD RID: 32717
		private bool _WantRfc1950HeaderBytes = true;

		// Token: 0x02002208 RID: 8712
		// (Invoke) Token: 0x0600BAFF RID: 47871
		internal delegate BlockState CompressFunc(FlushType flush);

		// Token: 0x02002209 RID: 8713
		internal class Config
		{
			// Token: 0x0600BB02 RID: 47874 RVA: 0x000790A2 File Offset: 0x000772A2
			private Config(int goodLength, int maxLazy, int niceLength, int maxChainLength, DeflateFlavor flavor)
			{
				this.GoodLength = goodLength;
				this.MaxLazy = maxLazy;
				this.NiceLength = niceLength;
				this.MaxChainLength = maxChainLength;
				this.Flavor = flavor;
			}

			// Token: 0x0600BB03 RID: 47875 RVA: 0x000790CF File Offset: 0x000772CF
			public static DeflateManager.Config Lookup(CompressionLevel level)
			{
				return DeflateManager.Config.Table[(int)level];
			}

			// Token: 0x04007FCE RID: 32718
			internal int GoodLength;

			// Token: 0x04007FCF RID: 32719
			internal int MaxLazy;

			// Token: 0x04007FD0 RID: 32720
			internal int NiceLength;

			// Token: 0x04007FD1 RID: 32721
			internal int MaxChainLength;

			// Token: 0x04007FD2 RID: 32722
			internal DeflateFlavor Flavor;

			// Token: 0x04007FD3 RID: 32723
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
