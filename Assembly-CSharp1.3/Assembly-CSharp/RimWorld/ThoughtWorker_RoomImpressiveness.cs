using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097A RID: 2426
	public abstract class ThoughtWorker_RoomImpressiveness : ThoughtWorker
	{
		// Token: 0x06003D77 RID: 15735 RVA: 0x00152360 File Offset: 0x00150560
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return ThoughtState.Inactive;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(scoreStageIndex);
		}
	}
}
