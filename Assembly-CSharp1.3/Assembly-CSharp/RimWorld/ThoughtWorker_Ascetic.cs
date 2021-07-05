using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BA RID: 2490
	public class ThoughtWorker_Ascetic : ThoughtWorker
	{
		// Token: 0x06003E04 RID: 15876 RVA: 0x0015400C File Offset: 0x0015220C
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
