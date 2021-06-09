using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E6 RID: 5094
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		// Token: 0x06006E3A RID: 28218 RVA: 0x0021BC34 File Offset: 0x00219E34
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.MetabolismSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x06006E3B RID: 28219 RVA: 0x0004ABD3 File Offset: 0x00048DD3
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
