using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E1 RID: 5089
	public class PawnCapacityWorker_Breathing : PawnCapacityWorker
	{
		// Token: 0x06006E2B RID: 28203 RVA: 0x0021BA14 File Offset: 0x00219C14
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingPathway, 1f, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSourceCage, 1f, default(FloatRange), impactors, -1f);
		}

		// Token: 0x06006E2C RID: 28204 RVA: 0x0004AB92 File Offset: 0x00048D92
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BreathingSource);
		}
	}
}
