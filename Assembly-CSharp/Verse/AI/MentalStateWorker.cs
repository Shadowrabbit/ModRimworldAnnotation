using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A3D RID: 2621
	public class MentalStateWorker
	{
		// Token: 0x06003E7D RID: 15997 RVA: 0x00178864 File Offset: 0x00176A64
		public virtual bool StateCanOccur(Pawn pawn)
		{
			if (!this.def.unspawnedCanDo && !pawn.Spawned)
			{
				return false;
			}
			if (!this.def.prisonersCanDo && pawn.HostFaction != null)
			{
				return false;
			}
			if (this.def.colonistsOnly && pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002B06 RID: 11014
		public MentalStateDef def;
	}
}
