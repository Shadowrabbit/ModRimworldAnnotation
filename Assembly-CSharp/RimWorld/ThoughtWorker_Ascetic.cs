using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC4 RID: 3780
	public class ThoughtWorker_Ascetic : ThoughtWorker
	{
		// Token: 0x060053DA RID: 21466 RVA: 0x001C1DE4 File Offset: 0x001BFFE4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return false;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] != null)
			{
				return ThoughtState.ActiveAtStage(scoreStageIndex);
			}
			return ThoughtState.Inactive;
		}
	}
}
