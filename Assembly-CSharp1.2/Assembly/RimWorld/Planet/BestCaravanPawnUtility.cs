using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020CD RID: 8397
	public static class BestCaravanPawnUtility
	{
		// Token: 0x0600B208 RID: 45576 RVA: 0x00073B1B File Offset: 0x00071D1B
		public static Pawn FindBestDiplomat(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.NegotiationAbility, null);
		}

		// Token: 0x0600B209 RID: 45577 RVA: 0x00339B0C File Offset: 0x00337D0C
		public static Pawn FindBestNegotiator(Caravan caravan, Faction negotiatingWith = null, TraderKindDef trader = null)
		{
			Predicate<Pawn> pawnValidator = null;
			if (negotiatingWith != null)
			{
				pawnValidator = ((Pawn p) => p.CanTradeWith(negotiatingWith, trader));
			}
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.TradePriceImprovement, pawnValidator);
		}

		// Token: 0x0600B20A RID: 45578 RVA: 0x00339B50 File Offset: 0x00337D50
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

		// Token: 0x0600B20B RID: 45579 RVA: 0x00339BF0 File Offset: 0x00337DF0
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

		// Token: 0x0600B20C RID: 45580 RVA: 0x00073B29 File Offset: 0x00071D29
		private static bool IsConsciousOwner(Pawn pawn, Caravan caravan)
		{
			return !pawn.Dead && !pawn.Downed && !pawn.InMentalState && caravan.IsOwner(pawn);
		}
	}
}
