using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA5 RID: 3493
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x060050FC RID: 20732 RVA: 0x001B1C54 File Offset: 0x001AFE54
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodPumpingSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x001B1C80 File Offset: 0x001AFE80
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
