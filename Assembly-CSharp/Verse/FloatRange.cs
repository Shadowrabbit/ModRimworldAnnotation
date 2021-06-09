using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000019 RID: 25
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00007589 File Offset: 0x00005789
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000EC RID: 236 RVA: 0x0000759A File Offset: 0x0000579A
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000075AB File Offset: 0x000057AB
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000075BC File Offset: 0x000057BC
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000EF RID: 239 RVA: 0x000075D1 File Offset: 0x000057D1
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000075E4 File Offset: 0x000057E4
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x000075F7 File Offset: 0x000057F7
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000760A File Offset: 0x0000580A
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00007619 File Offset: 0x00005819
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007629 File Offset: 0x00005829
		public float ClampToRange(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000763D File Offset: 0x0000583D
		public float RandomInRangeSeeded(int seed)
		{
			return Rand.RangeSeeded(this.min, this.max, seed);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007651 File Offset: 0x00005851
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007665 File Offset: 0x00005865
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007679 File Offset: 0x00005879
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00007692 File Offset: 0x00005892
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000076B7 File Offset: 0x000058B7
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000076CE File Offset: 0x000058CE
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000076EE File Offset: 0x000058EE
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007711 File Offset: 0x00005911
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007728 File Offset: 0x00005928
		public static FloatRange operator *(float val, FloatRange r)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0007C4FC File Offset: 0x0007A6FC
		public static FloatRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split(new char[]
			{
				'~'
			});
			if (array.Length == 1)
			{
				float num = Convert.ToSingle(array[0], invariantCulture);
				return new FloatRange(num, num);
			}
			return new FloatRange(Convert.ToSingle(array[0], invariantCulture), Convert.ToSingle(array[1], invariantCulture));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000773F File Offset: 0x0000593F
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007761 File Offset: 0x00005961
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007779 File Offset: 0x00005979
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007791 File Offset: 0x00005991
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04000068 RID: 104
		public float min;

		// Token: 0x04000069 RID: 105
		public float max;
	}
}
