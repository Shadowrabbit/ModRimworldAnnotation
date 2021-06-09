using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E4 RID: 5092
	public class PawnCapacityWorker_Hearing : PawnCapacityWorker
	{
		// Token: 0x06006E34 RID: 28212 RVA: 0x0021BBC8 File Offset: 0x00219DC8
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.HearingSource, float.MaxValue, default(FloatRange), impactors, 0.75f);
		}

		// Token: 0x06006E35 RID: 28213 RVA: 0x0004ABB9 File Offset: 0x00048DB9
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.HearingSource);
		}
	}
}
