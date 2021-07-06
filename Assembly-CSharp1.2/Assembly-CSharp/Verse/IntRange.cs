using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001A RID: 26
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000077B1 File Offset: 0x000059B1
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000077BA File Offset: 0x000059BA
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000077C3 File Offset: 0x000059C3
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000077D6 File Offset: 0x000059D6
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000077E9 File Offset: 0x000059E9
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00007800 File Offset: 0x00005A00
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007813 File Offset: 0x00005A13
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007823 File Offset: 0x00005A23
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0007C550 File Offset: 0x0007A750
		public static IntRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split(new char[]
			{
				'~'
			});
			if (array.Length == 1)
			{
				int num = Convert.ToInt32(array[0], invariantCulture);
				return new IntRange(num, num);
			}
			return new IntRange(Convert.ToInt32(array[0], invariantCulture), Convert.ToInt32(array[1], invariantCulture));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007841 File Offset: 0x00005A41
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007863 File Offset: 0x00005A63
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007876 File Offset: 0x00005A76
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000788E File Offset: 0x00005A8E
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000078AE File Offset: 0x00005AAE
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000078B8 File Offset: 0x00005AB8
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000078C4 File Offset: 0x00005AC4
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		// Token: 0x0400006A RID: 106
		public int min;

		// Token: 0x0400006B RID: 107
		public int max;
	}
}
