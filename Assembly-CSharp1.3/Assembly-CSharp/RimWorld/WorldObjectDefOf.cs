using System;

namespace RimWorld
{
	// Token: 0x0200143C RID: 5180
	[DefOf]
	public static class WorldObjectDefOf
	{
		// Token: 0x06007D2F RID: 32047 RVA: 0x002C493B File Offset: 0x002C2B3B
		static WorldObjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorldObjectDefOf));
		}

		// Token: 0x04004C76 RID: 19574
		public static WorldObjectDef Caravan;

		// Token: 0x04004C77 RID: 19575
		public static WorldObjectDef Settlement;

		// Token: 0x04004C78 RID: 19576
		public static WorldObjectDef AbandonedSettlement;

		// Token: 0x04004C79 RID: 19577
		public static WorldObjectDef EscapeShip;

		// Token: 0x04004C7A RID: 19578
		public static WorldObjectDef Ambush;

		// Token: 0x04004C7B RID: 19579
		public static WorldObjectDef DestroyedSettlement;

		// Token: 0x04004C7C RID: 19580
		public static WorldObjectDef AttackedNonPlayerCaravan;

		// Token: 0x04004C7D RID: 19581
		public static WorldObjectDef TravelingTransportPods;

		// Token: 0x04004C7E RID: 19582
		public static WorldObjectDef RoutePlannerWaypoint;

		// Token: 0x04004C7F RID: 19583
		public static WorldObjectDef Site;

		// Token: 0x04004C80 RID: 19584
		public static WorldObjectDef PeaceTalks;

		// Token: 0x04004C81 RID: 19585
		[MayRequireRoyalty]
		public static WorldObjectDef TravelingShuttle;

		// Token: 0x04004C82 RID: 19586
		public static WorldObjectDef Debug_Arena;

		// Token: 0x04004C83 RID: 19587
		[MayRequireIdeology]
		public static WorldObjectDef Settlement_SecondArchonexusCycle;

		// Token: 0x04004C84 RID: 19588
		[MayRequireIdeology]
		public static WorldObjectDef Settlement_ThirdArchonexusCycle;
	}
}
