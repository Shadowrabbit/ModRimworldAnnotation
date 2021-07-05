using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001444 RID: 5188
	public static class PawnSkinColors
	{
		// Token: 0x06006FFA RID: 28666 RVA: 0x00224DC8 File Offset: 0x00222FC8
		public static bool IsDarkSkin(Color color)
		{
			Color skinColor = PawnSkinColors.GetSkinColor(0.5f);
			return color.r + color.g + color.b <= skinColor.r + skinColor.g + skinColor.b + 0.01f;
		}

		// Token: 0x06006FFB RID: 28667 RVA: 0x00224E14 File Offset: 0x00223014
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

		// Token: 0x06006FFC RID: 28668 RVA: 0x00224E98 File Offset: 0x00223098
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

		// Token: 0x06006FFD RID: 28669 RVA: 0x00224F7C File Offset: 0x0022317C
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

		// Token: 0x06006FFE RID: 28670 RVA: 0x0004B868 File Offset: 0x00049A68
		public static float GetRandomMelaninSimilarTo(float value, float clampMin = 0f, float clampMax = 1f)
		{
			return Mathf.Clamp01(Mathf.Clamp(Rand.Gaussian(value, 0.05f), clampMin, clampMax));
		}

		// Token: 0x06006FFF RID: 28671 RVA: 0x00224FE0 File Offset: 0x002231E0
		private static float GetSkinDataCommonalityFactor(int skinDataIndex)
		{
			float num = 0f;
			for (int i = 0; i < PawnSkinColors.SkinColors.Length; i++)
			{
				num = Mathf.Max(num, PawnSkinColors.GetTotalAreaWhereClosestToSelector(i));
			}
			return PawnSkinColors.GetTotalAreaWhereClosestToSelector(skinDataIndex) / num;
		}

		// Token: 0x06007000 RID: 28672 RVA: 0x0022501C File Offset: 0x0022321C
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

		// Token: 0x06007001 RID: 28673 RVA: 0x002250DC File Offset: 0x002232DC
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

		// Token: 0x040049FB RID: 18939
		private static readonly PawnSkinColors.SkinColorData[] SkinColors = new PawnSkinColors.SkinColorData[]
		{
			new PawnSkinColors.SkinColorData(0f, 0f, new Color(0.9490196f, 0.92941177f, 0.8784314f)),
			new PawnSkinColors.SkinColorData(0.25f, 0.2f, new Color(1f, 0.9372549f, 0.8352941f)),
			new PawnSkinColors.SkinColorData(0.5f, 0.7f, new Color(1f, 0.9372549f, 0.7411765f)),
			new PawnSkinColors.SkinColorData(0.75f, 0.8f, new Color(0.89411765f, 0.61960787f, 0.3529412f)),
			new PawnSkinColors.SkinColorData(0.9f, 0.9f, new Color(0.50980395f, 0.35686275f, 0.1882353f)),
			new PawnSkinColors.SkinColorData(1f, 1f, new Color(0.3882353f, 0.27450982f, 0.14117648f))
		};

		// Token: 0x02001445 RID: 5189
		private struct SkinColorData
		{
			// Token: 0x06007003 RID: 28675 RVA: 0x0004B881 File Offset: 0x00049A81
			public SkinColorData(float melanin, float selector, Color color)
			{
				this.melanin = melanin;
				this.selector = selector;
				this.color = color;
			}

			// Token: 0x040049FC RID: 18940
			public float melanin;

			// Token: 0x040049FD RID: 18941
			public float selector;

			// Token: 0x040049FE RID: 18942
			public Color color;
		}
	}
}
