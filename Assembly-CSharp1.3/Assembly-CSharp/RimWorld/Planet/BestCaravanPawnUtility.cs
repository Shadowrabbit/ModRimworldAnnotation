using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200179A RID: 6042
	public static class BestCaravanPawnUtility
	{
		// Token: 0x06008BB7 RID: 35767 RVA: 0x003225CF File Offset: 0x003207CF
		public static Pawn FindBestDiplomat(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.NegotiationAbility, null);
		}

		// Token: 0x06008BB8 RID: 35768 RVA: 0x003225E0 File Offset: 0x003207E0
		public static Pawn FindBestNegotiator(Caravan caravan, Faction negotiatingWith = null, TraderKindDef trader = null)
		{
			Predicate<Pawn> pawnValidator = null;
			if (negotiatingWith != null)
			{
				pawnValidator = ((Pawn p) => p.CanTradeWith(negotiatingWith, trader).Accepted);
			}
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.TradePriceImprovement, pawnValidator);
		}

		// Token: 0x06008BB9 RID: 35769 RVA: 0x00322624 File Offset: 0x00320824
		public static Pawn FindBestEntertainingPawnFor(Caravan caravan, Pawn forPawn)
		{
			Pawn pawn = null;
			float num = -1f;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn2 = caravan.pawns[i];
				if (pawn2 != forPawn && pawn2.RaceProps.Humanlike && !pawn2.Dead && !pawn2.Downed && !pawn2.InMentalState && pawn2.IsPrisoner == forPawn.IsPrisoner && !StatDefOf.SocialImpact.Worker.IsDisabledFor(pawn2))
				{
					float statValue = pawn2.GetStatValue(StatDefOf.SocialImpact, true);
					if (pawn == null || statValue > num)
					{
						pawn = pawn2;
						num = statValue;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06008BBA RID: 35770 RVA: 0x003226C4 File Offset: 0x003208C4
		public static Pawn FindPawnWithBestStat(Caravan caravan, StatDef stat, Predicate<Pawn> pawnValidator = null)
		{
			Pawn pawn = null;
			float num = -1f;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (BestCaravanPawnUtility.IsConsciousOwner(pawn2, caravan) && !stat.Worker.IsDisabledFor(pawn2) && (pawnValidator == null || pawnValidator(pawn2)))
				{
					float statValue = pawn2.GetStatValue(stat, true);
					if (pawn == null || statValue > num)
					{
						pawn = pawn2;
						num = statValue;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06008BBB RID: 35771 RVA: 0x0032273A File Offset: 0x0032093A
		private static bool IsConsciousOwner(Pawn pawn, Caravan caravan)
		{
			return !pawn.Dead && !pawn.Downed && !pawn.InMentalState && caravan.IsOwner(pawn);
		}
	}
}
