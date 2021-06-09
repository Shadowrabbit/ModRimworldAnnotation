﻿using System;

namespace Ionic.Zlib
{
	// Token: 0x02002216 RID: 8726
	internal sealed class Tree
	{
		// Token: 0x0600BB87 RID: 48007 RVA: 0x0007960F File Offset: 0x0007780F
		internal static int DistanceCode(int dist)
		{
			if (dist >= 256)
			{
				return (int)Tree._dist_code[256 + SharedUtils.URShift(dist, 7)];
			}
			return (int)Tree._dist_code[dist];
		}

		// Token: 0x0600BB88 RID: 48008 RVA: 0x00360FD0 File Offset: 0x0035F1D0
		internal void gen_bitlen(DeflateManager s)
		{
			short[] array = this.dyn_tree;
			short[] treeCodes = this.staticTree.treeCodes;
			int[] extraBits = this.staticTree.extraBits;
			int extraBase = this.staticTree.extraBase;
			int maxLength = this.staticTree.maxLength;
			int num = 0;
			for (int i = 0; i <= InternalConstants.MAX_BITS; i++)
			{
				s.bl_count[i] = 0;
			}
			array[s.heap[s.heap_max] * 2 + 1] = 0;
			int j;
			for (j = s.heap_max + 1; j < Tree.HEAP_SIZE; j++)
			{
				int num2 = s.heap[j];
				int i = (int)(array[(int)(array[num2 * 2 + 1] * 2 + 1)] + 1);
				if (i > maxLength)
				{
					i = maxLength;
					num++;
				}
				array[num2 * 2 + 1] = (short)i;
				if (num2 <= this.max_code)
				{
					short[] bl_count = s.bl_count;
					int num3 = i;
					bl_count[num3] += 1;
					int num4 = 0;
					if (num2 >= extraBase)
					{
						num4 = extraBits[num2 - extraBase];
					}
					short num5 = array[num2 * 2];
					s.opt_len += (int)num5 * (i + num4);
					if (treeCodes != null)
					{
						s.static_len += (int)num5 * ((int)treeCodes[num2 * 2 + 1] + num4);
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			do
			{
				int i = maxLength - 1;
				while (s.bl_count[i] == 0)
				{
					i--;
				}
				short[] bl_count2 = s.bl_count;
				int num6 = i;
				bl_count2[num6] -= 1;
				s.bl_count[i + 1] = s.bl_count[i + 1] + 2;
				short[] bl_count3 = s.bl_count;
				int num7 = maxLength;
				bl_count3[num7] -= 1;
				num -= 2;
			}
			while (num > 0);
			for (int i = maxLength; i != 0; i--)
			{
				int num2 = (int)s.bl_count[i];
				while (num2 != 0)
				{
					int num8 = s.heap[--j];
					if (num8 <= this.max_code)
					{
						if ((int)array[num8 * 2 + 1] != i)
						{
							s.opt_len = (int)((long)s.opt_len + ((long)i - (long)array[num8 * 2 + 1]) * (long)array[num8 * 2]);
							array[num8 * 2 + 1] = (short)i;
						}
						num2--;
					}
				}
			}
		}

		// Token: 0x0600BB89 RID: 48009 RVA: 0x003611F0 File Offset: 0x0035F3F0
		internal void build_tree(DeflateManager s)
		{
			short[] array = this.dyn_tree;
			short[] treeCodes = this.staticTree.treeCodes;
			int elems = this.staticTree.elems;
			int num = -1;
			s.heap_len = 0;
			s.heap_max = Tree.HEAP_SIZE;
			int num2;
			for (int i = 0; i < elems; i++)
			{
				if (array[i * 2] != 0)
				{
					int[] heap = s.heap;
					num2 = s.heap_len + 1;
					s.heap_len = num2;
					num = (heap[num2] = i);
					s.depth[i] = 0;
				}
				else
				{
					array[i * 2 + 1] = 0;
				}
			}
			int num3;
			while (s.heap_len < 2)
			{
				int[] heap2 = s.heap;
				num2 = s.heap_len + 1;
				s.heap_len = num2;
				num3 = (heap2[num2] = ((num < 2) ? (++num) : 0));
				array[num3 * 2] = 1;
				s.depth[num3] = 0;
				s.opt_len--;
				if (treeCodes != null)
				{
					s.static_len -= (int)treeCodes[num3 * 2 + 1];
				}
			}
			this.max_code = num;
			for (int i = s.heap_len / 2; i >= 1; i--)
			{
				s.pqdownheap(array, i);
			}
			num3 = elems;
			do
			{
				int i = s.heap[1];
				int[] heap3 = s.heap;
				int num4 = 1;
				int[] heap4 = s.heap;
				num2 = s.heap_len;
				s.heap_len = num2 - 1;
				heap3[num4] = heap4[num2];
				s.pqdownheap(array, 1);
				int num5 = s.heap[1];
				int[] heap5 = s.heap;
				num2 = s.heap_max - 1;
				s.heap_max = num2;
				heap5[num2] = i;
				int[] heap6 = s.heap;
				num2 = s.heap_max - 1;
				s.heap_max = num2;
				heap6[num2] = num5;
				array[num3 * 2] = array[i * 2] + array[num5 * 2];
				s.depth[num3] = (sbyte)(Math.Max((byte)s.depth[i], (byte)s.depth[num5]) + 1);
				array[i * 2 + 1] = (array[num5 * 2 + 1] = (short)num3);
				s.heap[1] = num3++;
				s.pqdownheap(array, 1);
			}
			while (s.heap_len >= 2);
			int[] heap7 = s.heap;
			num2 = s.heap_max - 1;
			s.heap_max = num2;
			heap7[num2] = s.heap[1];
			this.gen_bitlen(s);
			Tree.gen_codes(array, num, s.bl_count);
		}

		// Token: 0x0600BB8A RID: 48010 RVA: 0x00361430 File Offset: 0x0035F630
		internal static void gen_codes(short[] tree, int max_code, short[] bl_count)
		{
			short[] array = new short[InternalConstants.MAX_BITS + 1];
			short num = 0;
			for (int i = 1; i <= InternalConstants.MAX_BITS; i++)
			{
				num = (array[i] = (short)(num + bl_count[i - 1] << 1));
			}
			for (int j = 0; j <= max_code; j++)
			{
				int num2 = (int)tree[j * 2 + 1];
				if (num2 != 0)
				{
					int num3 = j * 2;
					short[] array2 = array;
					int num4 = num2;
					short num5 = array2[num4];
					array2[num4] = num5 + 1;
					tree[num3] = (short)Tree.bi_reverse((int)num5, num2);
				}
			}
		}

		// Token: 0x0600BB8B RID: 48011 RVA: 0x003614A8 File Offset: 0x0035F6A8
		internal static int bi_reverse(int code, int len)
		{
			int num = 0;
			do
			{
				num |= (code & 1);
				code >>= 1;
				num <<= 1;
			}
			while (--len > 0);
			return num >> 1;
		}

		// Token: 0x04008086 RID: 32902
		private static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

		// Token: 0x04008087 RID: 32903
		internal static readonly int[] ExtraLengthBits = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			2,
			2,
			2,
			2,
			3,
			3,
			3,
			3,
			4,
			4,
			4,
			4,
			5,
			5,
			5,
			5,
			0
		};

		// Token: 0x04008088 RID: 32904
		internal static readonly int[] ExtraDistanceBits = new int[]
		{
			0,
			0,
			0,
			0,
			1,
			1,
			2,
			2,
			3,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			7,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			13,
			13
		};

		// Token: 0x04008089 RID: 32905
		internal static readonly int[] extra_blbits = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			2,
			3,
			7
		};

		// Token: 0x0400808A RID: 32906
		internal static readonly sbyte[] bl_order = new sbyte[]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		// Token: 0x0400808B RID: 32907
		internal const int Buf_size = 16;

		// Token: 0x0400808C RID: 32908
		private static readonly sbyte[] _dist_code = new sbyte[]
		{
			0,
			1,
			2,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			6,
			6,
			7,
			7,
			7,
			7,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			0,
			0,
			16,
			17,
			18,
			18,
			19,
			19,
			20,
			20,
			20,
			20,
			21,
			21,
			21,
			21,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29
		};

		// Token: 0x0400808D RID: 32909
		internal static readonly sbyte[] LengthCode = new sbyte[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			12,
			12,
			13,
			13,
			13,
			13,
			14,
			14,
			14,
			14,
			15,
			15,
			15,
			15,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			17,
			17,
			17,
			17,
			17,
			17,
			17,
			17,
			18,
			18,
			18,
			18,
			18,
			18,
			18,
			18,
			19,
			19,
			19,
			19,
			19,
			19,
			19,
			19,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			28
		};

		// Token: 0x0400808E RID: 32910
		internal static readonly int[] LengthBase = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			10,
			12,
			14,
			16,
			20,
			24,
			28,
			32,
			40,
			48,
			56,
			64,
			80,
			96,
			112,
			128,
			160,
			192,
			224,
			0
		};

		// Token: 0x0400808F RID: 32911
		internal static readonly int[] DistanceBase = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			6,
			8,
			12,
			16,
			24,
			32,
			48,
			64,
			96,
			128,
			192,
			256,
			384,
			512,
			768,
			1024,
			1536,
			2048,
			3072,
			4096,
			6144,
			8192,
			12288,
			16384,
			24576
		};

		// Token: 0x04008090 RID: 32912
		internal short[] dyn_tree;

		// Token: 0x04008091 RID: 32913
		internal int max_code;

		// Token: 0x04008092 RID: 32914
		internal StaticTree staticTree;
	}
}
