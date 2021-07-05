using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020002DB RID: 731
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x060013A2 RID: 5026 RVA: 0x0006F6FC File Offset: 0x0006D8FC
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

		// Token: 0x04000E75 RID: 3701
		public SimpleCurve ageFractionMtbDaysCurve;

		// Token: 0x04000E76 RID: 3702
		public int minPlayerPopulation;
	}
}
