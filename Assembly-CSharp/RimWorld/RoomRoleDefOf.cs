using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C71 RID: 7281
	[DefOf]
	public static class RoomRoleDefOf
	{
		// Token: 0x06009F74 RID: 40820 RVA: 0x0006A421 File Offset: 0x00068621
		static RoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf));
		}

		// Token: 0x04006A90 RID: 27280
		public static RoomRoleDef None;

		// Token: 0x04006A91 RID: 27281
		public static RoomRoleDef Bedroom;

		// Token: 0x04006A92 RID: 27282
		public static RoomRoleDef Barracks;

		// Token: 0x04006A93 RID: 27283
		public static RoomRoleDef PrisonCell;

		// Token: 0x04006A94 RID: 27284
		public static RoomRoleDef PrisonBarracks;

		// Token: 0x04006A95 RID: 27285
		public static RoomRoleDef DiningRoom;

		// Token: 0x04006A96 RID: 27286
		public static RoomRoleDef RecRoom;

		// Token: 0x04006A97 RID: 27287
		public static RoomRoleDef Hospital;

		// Token: 0x04006A98 RID: 27288
		public static RoomRoleDef Laboratory;

		// Token: 0x04006A99 RID: 27289
		[MayRequireRoyalty]
		public static RoomRoleDef ThroneRoom;
	}
}
