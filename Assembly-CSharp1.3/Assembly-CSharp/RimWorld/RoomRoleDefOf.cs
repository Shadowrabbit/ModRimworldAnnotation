using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001431 RID: 5169
	[DefOf]
	public static class RoomRoleDefOf
	{
		// Token: 0x06007D24 RID: 32036 RVA: 0x002C4880 File Offset: 0x002C2A80
		static RoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf));
		}

		// Token: 0x04004B5A RID: 19290
		public static RoomRoleDef None;

		// Token: 0x04004B5B RID: 19291
		public static RoomRoleDef Bedroom;

		// Token: 0x04004B5C RID: 19292
		public static RoomRoleDef Barracks;

		// Token: 0x04004B5D RID: 19293
		public static RoomRoleDef PrisonCell;

		// Token: 0x04004B5E RID: 19294
		public static RoomRoleDef PrisonBarracks;

		// Token: 0x04004B5F RID: 19295
		public static RoomRoleDef DiningRoom;

		// Token: 0x04004B60 RID: 19296
		public static RoomRoleDef RecRoom;

		// Token: 0x04004B61 RID: 19297
		public static RoomRoleDef Hospital;

		// Token: 0x04004B62 RID: 19298
		public static RoomRoleDef Laboratory;

		// Token: 0x04004B63 RID: 19299
		[MayRequireRoyalty]
		public static RoomRoleDef ThroneRoom;

		// Token: 0x04004B64 RID: 19300
		[MayRequireIdeology]
		public static RoomRoleDef WorshipRoom;
	}
}
