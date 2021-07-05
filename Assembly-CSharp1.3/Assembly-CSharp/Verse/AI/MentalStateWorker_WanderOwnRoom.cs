using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005CB RID: 1483
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x06002B2A RID: 11050 RVA: 0x00102B24 File Offset: 0x00100D24
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			return ownedBed != null && ownedBed.GetRoom(RegionType.Set_All) != null && !ownedBed.GetRoom(RegionType.Set_All).PsychologicallyOutdoors;
		}
	}
}
