using System;

namespace Verse.AI
{
	// Token: 0x02000641 RID: 1601
	public static class WanderRoomUtility
	{
		// Token: 0x06002D94 RID: 11668 RVA: 0x001105D4 File Offset: 0x0010E7D4
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map);
			return room == null || room.IsDoorway || WanderUtility.InSameRoom(root, loc, pawn.Map);
		}
	}
}
