using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E5 RID: 5093
	public class PawnCapacityWorker_Manipulation : PawnCapacityWorker
	{
		// Token: 0x06006E37 RID: 28215 RVA: 0x0021BBF4 File Offset: 0x00219DF4
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			return PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.ManipulationLimbCore, BodyPartTagDefOf.ManipulationLimbSegment, BodyPartTagDefOf.ManipulationLimbDigit, 0.8f, out num, impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06006E38 RID: 28216 RVA: 0x0004ABC6 File Offset: 0x00048DC6
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ManipulationLimbCore);
		}
	}
}
