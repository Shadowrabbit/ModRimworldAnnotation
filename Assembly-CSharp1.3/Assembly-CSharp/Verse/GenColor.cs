using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000026 RID: 38
	public static class GenColor
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000B1A8 File Offset: 0x000093A8
		public static Color SaturationChanged(this Color col, float change)
		{
			float num = col.r;
			float num2 = col.g;
			float num3 = col.b;
			float num4 = Mathf.Sqrt(num * num * 0.299f + num2 * num2 * 0.587f + num3 * num3 * 0.114f);
			num = num4 + (num - num4) * change;
			num2 = num4 + (num2 - num4) * change;
			num3 = num4 + (num3 - num4) * change;
			return new Color(num, num2, num3);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B210 File Offset: 0x00009410
		public static bool IndistinguishableFromFast(this Color colA, Color colB)
		{
			return Mathf.Abs(colA.r - colB.r) + Mathf.Abs(colA.g - colB.g) + Mathf.Abs(colA.b - colB.b) + Mathf.Abs(colA.a - colB.a) < 0.001f;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B270 File Offset: 0x00009470
		public static bool IndistinguishableFrom(this Color colA, Color colB)
		{
			if (GenColor.Colors32Equal(colA, colB))
			{
				return true;
			}
			Color color = colA - colB;
			return Mathf.Abs(color.r) + Mathf.Abs(color.g) + Mathf.Abs(color.b) + Mathf.Abs(color.a) < 0.001f;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B2C8 File Offset: 0x000094C8
		public static bool Colors32Equal(Color a, Color b)
		{
			Color32 color = a;
			Color32 color2 = b;
			return color.r == color2.r && color.g == color2.g && color.b == color2.b && color.a == color2.a;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B31D File Offset: 0x0000951D
		public static Color RandomColorOpaque()
		{
			return new Color(Rand.Value, Rand.Value, Rand.Value, 1f);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000B338 File Offset: 0x00009538
		public static Color FromBytes(int r, int g, int b, int a = 255)
		{
			return new Color
			{
				r = (float)r / 255f,
				g = (float)g / 255f,
				b = (float)b / 255f,
				a = (float)a / 255f
			};
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000B38C File Offset: 0x0000958C
		public static Color FromHex(string hex)
		{
			if (hex.StartsWith("#"))
			{
				hex = hex.Substring(1);
			}
			if (hex.Length != 6 && hex.Length != 8)
			{
				Log.Error(hex + " is not a valid hex color.");
				return Color.white;
			}
			int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			int a = 255;
			if (hex.Length == 8)
			{
				a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
			}
			return GenColor.FromBytes(r, g, b, a);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B43C File Offset: 0x0000963C
		public static Color GetDominantColor(this Texture2D texture, int buckets = 25, float minBrightness = 0f)
		{
			if (texture == BaseContent.BadTex)
			{
				return Color.white;
			}
			if (GenColor.tmpBuckets == null || GenColor.tmpBuckets.GetLength(0) != buckets)
			{
				GenColor.tmpBuckets = new float[buckets, buckets, buckets];
			}
			for (int i = 0; i < texture.width; i++)
			{
				for (int j = 0; j < texture.height; j++)
				{
					Color pixel = texture.GetPixel(i, j);
					if ((pixel.r + pixel.g + pixel.b) / 3f >= minBrightness)
					{
						GenColor.tmpBuckets[Mathf.Clamp((int)(pixel.r * (float)buckets), 0, buckets - 1), Mathf.Clamp((int)(pixel.g * (float)buckets), 0, buckets - 1), Mathf.Clamp((int)(pixel.b * (float)buckets), 0, buckets - 1)] += pixel.a;
					}
				}
			}
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int k = 0; k < buckets; k++)
			{
				for (int l = 0; l < buckets; l++)
				{
					for (int m = 0; m < buckets; m++)
					{
						if (GenColor.tmpBuckets[k, l, m] > num)
						{
							num = GenColor.tmpBuckets[k, l, m];
							num2 = k;
							num3 = l;
							num4 = m;
						}
					}
				}
			}
			return new Color(((float)num2 + 0.5f) / (float)buckets, ((float)num3 + 0.5f) / (float)buckets, ((float)num4 + 0.5f) / (float)buckets);
		}

		// Token: 0x04000062 RID: 98
		private static float[,,] tmpBuckets;
	}
}
