using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000414 RID: 1044
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x06001947 RID: 6471 RVA: 0x000E1C90 File Offset: 0x000DFE90
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float x = (float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy;
			if (Rand.MTBEventOccurs(this.ageFractionMtbDaysCurve.Evaluate(x), 60000f, 60f))
			{
				if (this.minPlayerPopulation > 0 && pawn.Faction == Faction.OfPlayer && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.Count<Pawn>() < this.minPlayerPopulation)
				{
					return;
				}
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}

		// Token: 0x040012EC RID: 4844
		public SimpleCurve ageFractionMtbDaysCurve;

		// Token: 0x040012ED RID: 4845
		public int minPlayerPopulation;
	}
}
