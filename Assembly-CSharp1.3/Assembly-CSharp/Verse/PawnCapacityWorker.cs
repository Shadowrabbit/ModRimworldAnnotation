using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000E5 RID: 229
	public class PawnCapacityWorker
	{
		// Token: 0x06000642 RID: 1602 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001F168 File Offset: 0x0001D368
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
