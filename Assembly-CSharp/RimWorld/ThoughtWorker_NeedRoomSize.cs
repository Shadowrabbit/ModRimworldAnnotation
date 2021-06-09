using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E82 RID: 3714
	public class ThoughtWorker_NeedRoomSize : ThoughtWorker
	{
		// Token: 0x0600534D RID: 21325 RVA: 0x001C04BC File Offset: 0x001BE6BC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.roomsize == null)
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_Passable);
			if (room == null || room.PsychologicallyOutdoors)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.roomsize.CurCategory)
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
