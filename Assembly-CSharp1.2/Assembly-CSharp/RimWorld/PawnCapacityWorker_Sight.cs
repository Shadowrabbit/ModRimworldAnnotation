using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E8 RID: 5096
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x06006E40 RID: 28224 RVA: 0x0021BD34 File Offset: 0x00219F34
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.SightSource, float.MaxValue, default(FloatRange), impactors, 0.75f);
		}

		// Token: 0x06006E41 RID: 28225 RVA: 0x0004ABED File Offset: 0x00048DED
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}
	}
}
