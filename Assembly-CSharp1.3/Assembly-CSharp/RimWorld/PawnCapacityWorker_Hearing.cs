using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA9 RID: 3497
	public class PawnCapacityWorker_Hearing : PawnCapacityWorker
	{
		// Token: 0x06005108 RID: 20744 RVA: 0x001B1E6C File Offset: 0x001B006C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.HearingSource, float.MaxValue, default(FloatRange), impactors, 0.75f);
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x001B1E98 File Offset: 0x001B0098
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.HearingSource);
		}
	}
}
