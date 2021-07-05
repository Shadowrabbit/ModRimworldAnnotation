using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000012 RID: 18
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x000057EB File Offset: 0x000039EB
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000580D File Offset: 0x00003A0D
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000582C File Offset: 0x00003A2C
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000585E File Offset: 0x00003A5E
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005899 File Offset: 0x00003A99
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000058D4 File Offset: 0x00003AD4
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000590F File Offset: 0x00003B0F
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005936 File Offset: 0x00003B36
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005965 File Offset: 0x00003B65
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000598C File Offset: 0x00003B8C
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000059BB File Offset: 0x00003BBB
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000059F7 File Offset: 0x00003BF7
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005A36 File Offset: 0x00003C36
		public override bool Equals(object o)
		{
			return o is ColorInt && this.Equals((ColorInt)o);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005A4E File Offset: 0x00003C4E
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005A5C File Offset: 0x00003C5C
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005AA8 File Offset: 0x00003CA8
		public void ClampToNonNegative()
		{
			if (this.r < 0)
			{
				this.r = 0;
			}
			if (this.g < 0)
			{
				this.g = 0;
			}
			if (this.b < 0)
			{
				this.b = 0;
			}
			if (this.a < 0)
			{
				this.a = 0;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00005AF8 File Offset: 0x00003CF8
		public Color ToColor
		{
			get
			{
				return new Color
				{
					r = (float)this.r / 255f,
					g = (float)this.g / 255f,
					b = (float)this.b / 255f,
					a = (float)this.a / 255f
				};
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005B60 File Offset: 0x00003D60
		public Color32 ToColor32
		{
			get
			{
				Color32 result = default(Color32);
				if (this.a > 255)
				{
					result.a = byte.MaxValue;
				}
				else
				{
					result.a = (byte)this.a;
				}
				if (this.r > 255)
				{
					result.r = byte.MaxValue;
				}
				else
				{
					result.r = (byte)this.r;
				}
				if (this.g > 255)
				{
					result.g = byte.MaxValue;
				}
				else
				{
					result.g = (byte)this.g;
				}
				if (this.b > 255)
				{
					result.b = byte.MaxValue;
				}
				else
				{
					result.b = (byte)this.b;
				}
				return result;
			}
		}

		// Token: 0x04000037 RID: 55
		public int r;

		// Token: 0x04000038 RID: 56
		public int g;

		// Token: 0x04000039 RID: 57
		public int b;

		// Token: 0x0400003A RID: 58
		public int a;
	}
}
