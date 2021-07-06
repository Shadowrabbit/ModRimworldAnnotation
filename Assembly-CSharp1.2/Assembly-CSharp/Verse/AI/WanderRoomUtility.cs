using System;

namespace Verse.AI
{
	// Token: 0x02000AA2 RID: 2722
	public static class WanderRoomUtility
	{
		// Token: 0x06004089 RID: 16521 RVA: 0x00182DF8 File Offset: 0x00180FF8
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map, RegionType.Set_Passable);
			return room == null || room.RegionType == RegionType.Portal || WanderUtility.InSameRoom(root, loc, pawn.Map);
		}
	}
}
