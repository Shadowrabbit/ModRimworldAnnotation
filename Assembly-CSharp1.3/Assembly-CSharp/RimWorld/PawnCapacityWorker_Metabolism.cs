using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAB RID: 3499
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		// Token: 0x0600510E RID: 20750 RVA: 0x001B1EF4 File Offset: 0x001B00F4
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.MetabolismSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x001B1F20 File Offset: 0x001B0120
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
