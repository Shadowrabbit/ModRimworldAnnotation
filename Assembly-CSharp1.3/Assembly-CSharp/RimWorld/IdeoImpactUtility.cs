using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED5 RID: 3797
	[StaticConstructorOnStartup]
	public static class IdeoImpactUtility
	{
		// Token: 0x060059F7 RID: 23031 RVA: 0x001ED8F8 File Offset: 0x001EBAF8
		public static string MemeImpactLabel(int impact)
		{
			impact = Mathf.Clamp(impact, 1, 3);
			return string.Format("IdeoMemeImpactLabel_{0}", impact).Translate();
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x001ED91E File Offset: 0x001EBB1E
		public static string OverallImpactLabel(int impact)
		{
			impact = Mathf.Clamp(impact, 1, 9);
			return string.Format("IdeoImpactLabel_{0}", impact).Translate();
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x001ED945 File Offset: 0x001EBB45
		public static void DrawImpactIcon(Rect rect, int impact)
		{
			impact = Mathf.Clamp(impact, 1, 3);
			Color color = GUI.color;
			GUI.color = IdeoImpactUtility.IconTint;
			GUI.DrawTexture(rect, IdeoImpactUtility.ImpactIcons[impact]);
			GUI.color = color;
		}

		// Token: 0x040034AF RID: 13487
		public const int MaxMemeImpact = 3;

		// Token: 0x040034B0 RID: 13488
		public const int MaxCombinedImpact = 9;

		// Token: 0x040034B1 RID: 13489
		private static readonly Color IconTint = Color.Lerp(ColoredText.ImpactColor, new Color(0.3f, 0.3f, 0.3f, 1f), 0.9f);

		// Token: 0x040034B2 RID: 13490
		private static readonly Texture2D[] ImpactIcons = new Texture2D[]
		{
			null,
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact1", true),
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact2", true),
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact3", true)
		};
	}
}
