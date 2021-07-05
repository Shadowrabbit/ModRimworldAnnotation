using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B8 RID: 2488
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		// Token: 0x06003E00 RID: 15872 RVA: 0x00153E9C File Offset: 0x0015209C
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
