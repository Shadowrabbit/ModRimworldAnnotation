using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076E RID: 1902
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		// Token: 0x06003475 RID: 13429 RVA: 0x00129731 File Offset: 0x00127931
		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x00129738 File Offset: 0x00127938
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing result;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out result, out thingDef, false, true, true, true, true, false, false, false, false, FoodPreferability.RawTasty))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04001E53 RID: 7763
		private const int BaseIngestInterval = 1100;
	}
}
