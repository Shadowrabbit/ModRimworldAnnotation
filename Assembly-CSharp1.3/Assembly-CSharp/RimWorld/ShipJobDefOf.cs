using System;

namespace RimWorld
{
	// Token: 0x0200146D RID: 5229
	[DefOf]
	public static class ShipJobDefOf
	{
		// Token: 0x06007D5F RID: 32095 RVA: 0x002C4C6B File Offset: 0x002C2E6B
		static ShipJobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShipJobDefOf));
		}

		// Token: 0x04004E05 RID: 19973
		[MayRequireRoyalty]
		public static ShipJobDef Arrive;

		// Token: 0x04004E06 RID: 19974
		[MayRequireRoyalty]
		public static ShipJobDef FlyAway;

		// Token: 0x04004E07 RID: 19975
		[MayRequireRoyalty]
		public static ShipJobDef WaitTime;

		// Token: 0x04004E08 RID: 19976
		[MayRequireRoyalty]
		public static ShipJobDef WaitForever;

		// Token: 0x04004E09 RID: 19977
		[MayRequireRoyalty]
		public static ShipJobDef Unload;

		// Token: 0x04004E0A RID: 19978
		[MayRequireRoyalty]
		public static ShipJobDef WaitSendable;
	}
}
