using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA6 RID: 3494
	public class PawnCapacityWorker_Breathing : PawnCapacityWorker
	{
		// Token: 0x060050FF RID: 20735 RVA: 0x001B1C90 File Offset: 0x001AFE90
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingPathway, 1f, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSourceCage, 1f, default(FloatRange), impactors, -1f);
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x001B1CFC File Offset: 0x001AFEFC
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BreathingSource);
		}
	}
}
