using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000156 RID: 342
	public class PawnCapacityWorker
	{
		// Token: 0x060008B5 RID: 2229 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00097004 File Offset: 0x00095204
		protected float CalculateCapacityAndRecord(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			float level = diffSet.pawn.health.capacities.GetLevel(capacity);
			if (impactors != null && level != 1f)
			{
				impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
				{
					capacity = capacity
				});
			}
			return level;
		}
	}
}
