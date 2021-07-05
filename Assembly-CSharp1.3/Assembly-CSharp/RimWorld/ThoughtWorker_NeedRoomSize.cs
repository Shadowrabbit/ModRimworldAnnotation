using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E1 RID: 2529
	public class ThoughtWorker_NeedRoomSize : ThoughtWorker
	{
		// Token: 0x06003E70 RID: 15984 RVA: 0x00155494 File Offset: 0x00153694
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.roomsize == null)
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room == null || room.PsychologicallyOutdoors)
			{
				return ThoughtState.Inactive;
			}
			RoomSizeCategory curCategory = p.needs.roomsize.CurCategory;
			if (p.Ideo != null && curCategory < RoomSizeCategory.Normal && p.Ideo.IdeoDisablesCrampedRoomThoughts())
			{
				return ThoughtState.Inactive;
			}
			switch (curCategory)
			{
			case RoomSizeCategory.VeryCramped:
				return ThoughtState.ActiveAtStage(0);
			case RoomSizeCategory.Cramped:
				return ThoughtState.ActiveAtStage(1);
			case RoomSizeCategory.Normal:
				return ThoughtState.Inactive;
			case RoomSizeCategory.Spacious:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new InvalidOperationException("Unknown RoomSizeCategory");
			}
		}
	}
}
