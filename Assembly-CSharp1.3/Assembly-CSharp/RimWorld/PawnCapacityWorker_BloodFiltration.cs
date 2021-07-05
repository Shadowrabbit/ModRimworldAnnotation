using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA4 RID: 3492
	public class PawnCapacityWorker_BloodFiltration : PawnCapacityWorker
	{
		// Token: 0x060050F9 RID: 20729 RVA: 0x001B1B98 File Offset: 0x001AFD98
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			if (diffSet.pawn.RaceProps.body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney))
			{
				return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationKidney, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationLiver, float.MaxValue, default(FloatRange), impactors, -1f);
			}
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x001B1C20 File Offset: 0x001AFE20
		public override bool CanHaveCapacity(BodyDef body)
		{
			return (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney) && body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationLiver)) || body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationSource);
		}
	}
}
