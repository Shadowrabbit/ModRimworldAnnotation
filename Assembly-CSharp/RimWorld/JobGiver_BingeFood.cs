using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C87 RID: 3207
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		// Token: 0x06004AE1 RID: 19169 RVA: 0x000357E3 File Offset: 0x000339E3
		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x001A374C File Offset: 0x001A194C
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing result;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out result, out thingDef, false, true, true, true, true, false, false, false, FoodPreferability.Undefined))
			{
				return result;
			}
			return null;
		}

		// Token: 0x040031A6 RID: 12710
		private const int BaseIngestInterval = 1100;
	}
}
