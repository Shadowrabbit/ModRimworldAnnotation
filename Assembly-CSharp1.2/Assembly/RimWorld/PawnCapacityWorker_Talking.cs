using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E9 RID: 5097
	public class PawnCapacityWorker_Talking : PawnCapacityWorker
	{
		// Token: 0x06006E43 RID: 28227 RVA: 0x0021BD60 File Offset: 0x00219F60
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.TalkingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.TalkingPathway, 1f, default(FloatRange), impactors, -1f) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06006E44 RID: 28228 RVA: 0x0004ABFA File Offset: 0x00048DFA
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.TalkingSource);
		}
	}
}
