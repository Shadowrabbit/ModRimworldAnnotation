using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC2 RID: 3778
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		// Token: 0x060053D6 RID: 21462 RVA: 0x001C1C74 File Offset: 0x001BFE74
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			int num = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness)) + 1;
			if (this.def.stages[num] != null)
			{
				return ThoughtState.ActiveAtStage(num);
			}
			return ThoughtState.Inactive;
		}
	}
}
