using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025A RID: 602
	public static class PawnNameColorUtility
	{
		// Token: 0x06001122 RID: 4386 RVA: 0x00061128 File Offset: 0x0005F328
		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0006134C File Offset: 0x0005F54C
		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x000613B8 File Offset: 0x0005F5B8
		public static Color PawnNameColorOf(Pawn pawn)
		{
			if (pawn.MentalStateDef != null)
			{
				return pawn.MentalStateDef.nameColor;
			}
			int index;
			if (pawn.Faction == null)
			{
				index = 0;
			}
			else
			{
				index = pawn.Faction.randomKey % 10;
			}
			if (pawn.IsPrisoner)
			{
				return PawnNameColorUtility.ColorsPrisoner[index];
			}
			if (pawn.IsSlave && SlaveRebellionUtility.IsRebelling(pawn))
			{
				return PawnNameColorUtility.ColorBaseHostile;
			}
			if (pawn.IsSlave)
			{
				return PawnNameColorUtility.ColorSlave;
			}
			if (pawn.IsWildMan())
			{
				return PawnNameColorUtility.ColorWildMan;
			}
			if (pawn.Faction == null)
			{
				return PawnNameColorUtility.ColorsNeutral[index];
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return PawnNameColorUtility.ColorColony;
			}
			if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return PawnNameColorUtility.ColorsHostile[index];
			}
			return PawnNameColorUtility.ColorsNeutral[index];
		}

		// Token: 0x04000D04 RID: 3332
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		// Token: 0x04000D05 RID: 3333
		private static readonly List<Color> ColorsHostile = new List<Color>();

		// Token: 0x04000D06 RID: 3334
		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		// Token: 0x04000D07 RID: 3335
		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		// Token: 0x04000D08 RID: 3336
		private static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04000D09 RID: 3337
		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		// Token: 0x04000D0A RID: 3338
		private static readonly Color ColorSlave = new Color32(222, 192, 22, byte.MaxValue);

		// Token: 0x04000D0B RID: 3339
		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04000D0C RID: 3340
		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		// Token: 0x04000D0D RID: 3341
		private const int ColorShiftCount = 10;

		// Token: 0x04000D0E RID: 3342
		private static readonly List<Color> ColorShifts = new List<Color>
		{
			new Color(1f, 1f, 1f),
			new Color(0.8f, 1f, 1f),
			new Color(0.8f, 0.8f, 1f),
			new Color(0.8f, 0.8f, 0.8f),
			new Color(1.2f, 1f, 1f),
			new Color(0.8f, 1.2f, 1f),
			new Color(0.8f, 1.2f, 1.2f),
			new Color(1.2f, 1.2f, 1.2f),
			new Color(1f, 1.2f, 1f),
			new Color(1.2f, 1f, 0.8f)
		};
	}
}
