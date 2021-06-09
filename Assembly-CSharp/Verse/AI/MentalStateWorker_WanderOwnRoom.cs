using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A40 RID: 2624
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x06003E83 RID: 16003 RVA: 0x00178974 File Offset: 0x00176B74
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			return ownedBed != null && ownedBed.GetRoom(RegionType.Set_Passable) != null && !ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors;
		}
	}
}
