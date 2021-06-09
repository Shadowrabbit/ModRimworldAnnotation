using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000016 RID: 22
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x000072BC File Offset: 0x000054BC
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000072DE File Offset: 0x000054DE
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000072FD File Offset: 0x000054FD
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000732F File Offset: 0x0000552F
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000736A File Offset: 0x0000556A
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000073A5 File Offset: 0x000055A5
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000073E0 File Offset: 0x000055E0
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007407 File Offset: 0x00005607
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007436 File Offset: 0x00005636
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000745D File Offset: 0x0000565D
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000748C File Offset: 0x0000568C
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000074C8 File Offset: 0x000056C8
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007507 File Offset: 0x00005707
		public override bool Equals(object o)
		{
			return o is ColorInt && this.Equals((ColorInt)o);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000751F File Offset: 0x0000571F
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0007C33C File Offset: 0x0007A53C
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0007C388 File Offset: 0x0007A588
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x0007C3D8 File Offset: 0x0007A5D8
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0007C440 File Offset: 0x0007A640
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

		// Token: 0x04000063 RID: 99
		public int r;

		// Token: 0x04000064 RID: 100
		public int g;

		// Token: 0x04000065 RID: 101
		public int b;

		// Token: 0x04000066 RID: 102
		public int a;
	}
}
