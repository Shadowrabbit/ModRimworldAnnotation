using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E0 RID: 5088
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x06006E28 RID: 28200 RVA: 0x0021B9E8 File Offset: 0x00219BE8
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodPumpingSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x06006E29 RID: 28201 RVA: 0x0004AB85 File Offset: 0x00048D85
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
