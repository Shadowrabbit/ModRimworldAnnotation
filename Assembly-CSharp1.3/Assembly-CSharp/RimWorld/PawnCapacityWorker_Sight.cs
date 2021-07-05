using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAD RID: 3501
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x06005114 RID: 20756 RVA: 0x001B2014 File Offset: 0x001B0214
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.SightSource, float.MaxValue, default(FloatRange), impactors, 0.75f);
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x001B2040 File Offset: 0x001B0240
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}
	}
}
