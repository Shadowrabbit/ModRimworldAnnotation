using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000A6C RID: 2668
	public class GatheringWorker_Party : GatheringWorker
	{
		// Token: 0x06004012 RID: 16402 RVA: 0x0015B25C File Offset: 0x0015945C
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Party(spot, organizer, this.def);
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0015B26B File Offset: 0x0015946B
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			return RCellFinder.TryFindGatheringSpot(organizer, this.def, false, out spot);
		}
	}
}
