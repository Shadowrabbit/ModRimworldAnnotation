using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAE RID: 3502
	public class PawnCapacityWorker_Talking : PawnCapacityWorker
	{
		// Token: 0x06005117 RID: 20759 RVA: 0x001B2050 File Offset: 0x001B0250
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.TalkingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.TalkingPathway, 1f, default(FloatRange), impactors, -1f) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x001B20AA File Offset: 0x001B02AA
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.TalkingSource);
		}
	}
}
