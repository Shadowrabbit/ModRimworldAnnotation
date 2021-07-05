using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000802 RID: 2050
	public abstract class WorkGiver
	{
		// Token: 0x060036B9 RID: 14009 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return false;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Job NonScanJob(Pawn pawn)
		{
			return null;
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x00136B0C File Offset: 0x00134D0C
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x04001EFC RID: 7932
		public WorkGiverDef def;
	}
}
