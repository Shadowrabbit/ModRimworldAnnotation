using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3D RID: 3389
	public abstract class WorkGiver
	{
		// Token: 0x06004D6E RID: 19822 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return false;
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Job NonScanJob(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x001AEE4C File Offset: 0x001AD04C
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

		// Token: 0x040032E1 RID: 13025
		public WorkGiverDef def;
	}
}
