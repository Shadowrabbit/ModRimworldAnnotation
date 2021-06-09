using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F98 RID: 3992
	public class GatheringWorker_Party : GatheringWorker
	{
		// Token: 0x0600578C RID: 22412 RVA: 0x0003CB53 File Offset: 0x0003AD53
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Party(spot, organizer, this.def);
		}

		// Token: 0x0600578D RID: 22413 RVA: 0x0003CB62 File Offset: 0x0003AD62
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			return RCellFinder.TryFindGatheringSpot_NewTemp(organizer, this.def, false, out spot);
		}
	}
}
