using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000015 RID: 21
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00005C76 File Offset: 0x00003E76
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00005C87 File Offset: 0x00003E87
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005C98 File Offset: 0x00003E98
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00005CA9 File Offset: 0x00003EA9
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005CBE File Offset: 0x00003EBE
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005CD1 File Offset: 0x00003ED1
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005CE4 File Offset: 0x00003EE4
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00005CF7 File Offset: 0x00003EF7
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005D06 File Offset: 0x00003F06
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005D16 File Offset: 0x00003F16
		public float ClampToRange(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005D2A File Offset: 0x00003F2A
		public float RandomInRangeSeeded(int seed)
		{
			return Rand.RangeSeeded(this.min, this.max, seed);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005D3E File Offset: 0x00003F3E
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005D52 File Offset: 0x00003F52
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005D66 File Offset: 0x00003F66
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005D7F File Offset: 0x00003F7F
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005DA4 File Offset: 0x00003FA4
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005DBB File Offset: 0x00003FBB
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005DDB File Offset: 0x00003FDB
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005DFE File Offset: 0x00003FFE
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005E15 File Offset: 0x00004015
		public static FloatRange operator *(float val, FloatRange r)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005E2C File Offset: 0x0000402C
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

		// Token: 0x060000D0 RID: 208 RVA: 0x00005E7E File Offset: 0x0000407E
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005EA0 File Offset: 0x000040A0
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005EB8 File Offset: 0x000040B8
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005ED0 File Offset: 0x000040D0
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x0400003C RID: 60
		public float min;

		// Token: 0x0400003D RID: 61
		public float max;
	}
}
