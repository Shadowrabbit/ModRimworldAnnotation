using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000045 RID: 69
	public static class GenColor
	{
		// Token: 0x060002ED RID: 749 RVA: 0x00081BD8 File Offset: 0x0007FDD8
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

		// Token: 0x060002EE RID: 750 RVA: 0x00081C40 File Offset: 0x0007FE40
		public static bool IndistinguishableFrom(this Color colA, Color colB)
		{
			if (GenColor.Colors32Equal(colA, colB))
			{
				return true;
			}
			Color color = colA - colB;
			return Mathf.Abs(color.r) + Mathf.Abs(color.g) + Mathf.Abs(color.b) + Mathf.Abs(color.a) < 0.001f;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00081C98 File Offset: 0x0007FE98
		public static bool Colors32Equal(Color a, Color b)
		{
			Color32 color = a;
			Color32 color2 = b;
			return color.r == color2.r && color.g == color2.g && color.b == color2.b && color.a == color2.a;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00008DF5 File Offset: 0x00006FF5
		public static Color RandomColorOpaque()
		{
			return new Color(Rand.Value, Rand.Value, Rand.Value, 1f);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00081CF0 File Offset: 0x0007FEF0
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

		// Token: 0x060002F2 RID: 754 RVA: 0x00081D44 File Offset: 0x0007FF44
		public static Color FromHex(string hex)
		{
			if (hex.StartsWith("#"))
			{
				hex = hex.Substring(1);
			}
			if (hex.Length != 6 && hex.Length != 8)
			{
				Log.Error(hex + " is not a valid hex color.", false);
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
	}
}
