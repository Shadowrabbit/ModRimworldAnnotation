using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC8 RID: 3528
	public static class PawnSkinColors
	{
		// Token: 0x060051A4 RID: 20900 RVA: 0x001B87FC File Offset: 0x001B69FC
		public static bool IsDarkSkin(Color color)
		{
			Color skinColor = PawnSkinColors.GetSkinColor(0.5f);
			return color.r + color.g + color.b <= skinColor.r + skinColor.g + skinColor.b + 0.01f;
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x001B8848 File Offset: 0x001B6A48
		public static Color GetSkinColor(float melanin)
		{
			int skinDataIndexOfMelanin = PawnSkinColors.GetSkinDataIndexOfMelanin(melanin);
			if (skinDataIndexOfMelanin == PawnSkinColors.SkinColors.Length - 1)
			{
				return PawnSkinColors.SkinColors[skinDataIndexOfMelanin].color;
			}
			float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].melanin, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].melanin, melanin);
			return Color.Lerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].color, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].color, t);
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x001B88CC File Offset: 0x001B6ACC
		public static float RandomMelanin(Faction fac)
		{
			float num;
			if (fac == null)
			{
				num = Rand.Value;
			}
			else
			{
				num = Rand.Range(Mathf.Clamp01(fac.centralMelanin - fac.def.geneticVariance), Mathf.Clamp01(fac.centralMelanin + fac.def.geneticVariance));
			}
			int num2 = 0;
			int num3 = 0;
			while (num3 < PawnSkinColors.SkinColors.Length && num >= PawnSkinColors.SkinColors[num3].selector)
			{
				num2 = num3;
				num3++;
			}
			if (num2 == PawnSkinColors.SkinColors.Length - 1)
			{
				return PawnSkinColors.SkinColors[num2].melanin;
			}
			float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[num2].selector, PawnSkinColors.SkinColors[num2 + 1].selector, num);
			return Mathf.Lerp(PawnSkinColors.SkinColors[num2].melanin, PawnSkinColors.SkinColors[num2 + 1].melanin, t);
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x001B89B0 File Offset: 0x001B6BB0
		public static float GetMelaninCommonalityFactor(float melanin)
		{
			int skinDataIndexOfMelanin = PawnSkinColors.GetSkinDataIndexOfMelanin(melanin);
			if (skinDataIndexOfMelanin == PawnSkinColors.SkinColors.Length - 1)
			{
				return PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin);
			}
			float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].melanin, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].melanin, melanin);
			return Mathf.Lerp(PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin), PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin + 1), t);
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x001B8A14 File Offset: 0x001B6C14
		public static float GetRandomMelaninSimilarTo(float value, float clampMin = 0f, float clampMax = 1f)
		{
			return Mathf.Clamp01(Mathf.Clamp(Rand.Gaussian(value, 0.05f), clampMin, clampMax));
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x001B8A30 File Offset: 0x001B6C30
		private static float GetSkinDataCommonalityFactor(int skinDataIndex)
		{
			float num = 0f;
			for (int i = 0; i < PawnSkinColors.SkinColors.Length; i++)
			{
				num = Mathf.Max(num, PawnSkinColors.GetTotalAreaWhereClosestToSelector(i));
			}
			return PawnSkinColors.GetTotalAreaWhereClosestToSelector(skinDataIndex) / num;
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x001B8A6C File Offset: 0x001B6C6C
		private static float GetTotalAreaWhereClosestToSelector(int skinDataIndex)
		{
			float num = 0f;
			if (skinDataIndex == 0)
			{
				num += PawnSkinColors.SkinColors[skinDataIndex].selector;
			}
			else if (PawnSkinColors.SkinColors.Length > 1)
			{
				num += (PawnSkinColors.SkinColors[skinDataIndex].selector - PawnSkinColors.SkinColors[skinDataIndex - 1].selector) / 2f;
			}
			if (skinDataIndex == PawnSkinColors.SkinColors.Length - 1)
			{
				num += 1f - PawnSkinColors.SkinColors[skinDataIndex].selector;
			}
			else if (PawnSkinColors.SkinColors.Length > 1)
			{
				num += (PawnSkinColors.SkinColors[skinDataIndex + 1].selector - PawnSkinColors.SkinColors[skinDataIndex].selector) / 2f;
			}
			return num;
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001B8B2C File Offset: 0x001B6D2C
		private static int GetSkinDataIndexOfMelanin(float melanin)
		{
			int result = 0;
			int num = 0;
			while (num < PawnSkinColors.SkinColors.Length && melanin >= PawnSkinColors.SkinColors[num].melanin)
			{
				result = num;
				num++;
			}
			return result;
		}

		// Token: 0x0400306A RID: 12394
		private static readonly PawnSkinColors.SkinColorData[] SkinColors = new PawnSkinColors.SkinColorData[]
		{
			new PawnSkinColors.SkinColorData(0f, 0f, new Color32(242, 237, 224, byte.MaxValue)),
			new PawnSkinColors.SkinColorData(0.25f, 0.2f, new Color32(byte.MaxValue, 239, 213, byte.MaxValue)),
			new PawnSkinColors.SkinColorData(0.5f, 0.7f, new Color32(byte.MaxValue, 239, 189, byte.MaxValue)),
			new PawnSkinColors.SkinColorData(0.75f, 0.8f, new Color32(228, 158, 90, byte.MaxValue)),
			new PawnSkinColors.SkinColorData(0.9f, 0.9f, new Color32(130, 91, 48, byte.MaxValue)),
			new PawnSkinColors.SkinColorData(1f, 1f, new Color32(99, 70, 36, byte.MaxValue))
		};

		// Token: 0x0200226B RID: 8811
		private struct SkinColorData
		{
			// Token: 0x0600C2D0 RID: 49872 RVA: 0x003D6F54 File Offset: 0x003D5154
			public SkinColorData(float melanin, float selector, Color color)
			{
				this.melanin = melanin;
				this.selector = selector;
				this.color = color;
			}

			// Token: 0x0400833A RID: 33594
			public float melanin;

			// Token: 0x0400833B RID: 33595
			public float selector;

			// Token: 0x0400833C RID: 33596
			public Color color;
		}
	}
}
