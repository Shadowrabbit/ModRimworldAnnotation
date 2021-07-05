using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000016 RID: 22
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005EF0 File Offset: 0x000040F0
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005EF9 File Offset: 0x000040F9
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005F02 File Offset: 0x00004102
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005F15 File Offset: 0x00004115
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005F28 File Offset: 0x00004128
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005F3F File Offset: 0x0000413F
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005F52 File Offset: 0x00004152
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005F62 File Offset: 0x00004162
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005F80 File Offset: 0x00004180
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

		// Token: 0x060000DD RID: 221 RVA: 0x00005FD2 File Offset: 0x000041D2
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005FF4 File Offset: 0x000041F4
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006007 File Offset: 0x00004207
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000601F File Offset: 0x0000421F
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000603F File Offset: 0x0000423F
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006049 File Offset: 0x00004249
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006055 File Offset: 0x00004255
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		// Token: 0x0400003E RID: 62
		public int min;

		// Token: 0x0400003F RID: 63
		public int max;
	}
}
