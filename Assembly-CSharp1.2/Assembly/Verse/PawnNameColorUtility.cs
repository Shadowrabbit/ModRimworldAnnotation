using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037E RID: 894
	public static class PawnNameColorUtility
	{
		// Token: 0x06001675 RID: 5749 RVA: 0x000D65D0 File Offset: 0x000D47D0
		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x000D67D4 File Offset: 0x000D49D4
		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000D6840 File Offset: 0x000D4A40
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

		// Token: 0x0400113F RID: 4415
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		// Token: 0x04001140 RID: 4416
		private static readonly List<Color> ColorsHostile = new List<Color>();

		// Token: 0x04001141 RID: 4417
		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		// Token: 0x04001142 RID: 4418
		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		// Token: 0x04001143 RID: 4419
		private static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04001144 RID: 4420
		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		// Token: 0x04001145 RID: 4421
		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04001146 RID: 4422
		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		// Token: 0x04001147 RID: 4423
		private const int ColorShiftCount = 10;

		// Token: 0x04001148 RID: 4424
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
