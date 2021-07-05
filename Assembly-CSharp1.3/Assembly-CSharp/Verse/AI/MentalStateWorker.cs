using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005C8 RID: 1480
	public class MentalStateWorker
	{
		// Token: 0x06002B24 RID: 11044 RVA: 0x001029B0 File Offset: 0x00100BB0
		public virtual bool StateCanOccur(Pawn pawn)
		{
			if (!this.def.unspawnedCanDo && !pawn.Spawned)
			{
				return false;
			}
			if (!this.def.prisonersCanDo && pawn.IsPrisoner)
			{
				return false;
			}
			if (!this.def.slavesCanDo && pawn.IsSlave)
			{
				return false;
			}
			if (this.def.colonistsOnly && pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			if (this.def.slavesOnly && !pawn.IsSlave)
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

		// Token: 0x04001A6C RID: 6764
		public MentalStateDef def;
	}
}
