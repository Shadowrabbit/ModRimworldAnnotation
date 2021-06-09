using System;

namespace RimWorld
{
	// Token: 0x02001C7C RID: 7292
	[DefOf]
	public static class WorldObjectDefOf
	{
		// Token: 0x06009F7F RID: 40831 RVA: 0x0006A4DC File Offset: 0x000686DC
		static WorldObjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorldObjectDefOf));
		}

		// Token: 0x04006B96 RID: 27542
		public static WorldObjectDef Caravan;

		// Token: 0x04006B97 RID: 27543
		public static WorldObjectDef Settlement;

		// Token: 0x04006B98 RID: 27544
		public static WorldObjectDef AbandonedSettlement;

		// Token: 0x04006B99 RID: 27545
		public static WorldObjectDef EscapeShip;

		// Token: 0x04006B9A RID: 27546
		public static WorldObjectDef Ambush;

		// Token: 0x04006B9B RID: 27547
		public static WorldObjectDef DestroyedSettlement;

		// Token: 0x04006B9C RID: 27548
		public static WorldObjectDef AttackedNonPlayerCaravan;

		// Token: 0x04006B9D RID: 27549
		public static WorldObjectDef TravelingTransportPods;

		// Token: 0x04006B9E RID: 27550
		public static WorldObjectDef RoutePlannerWaypoint;

		// Token: 0x04006B9F RID: 27551
		public static WorldObjectDef Site;

		// Token: 0x04006BA0 RID: 27552
		public static WorldObjectDef PeaceTalks;

		// Token: 0x04006BA1 RID: 27553
		[MayRequireRoyalty]
		public static WorldObjectDef TravelingShuttle;

		// Token: 0x04006BA2 RID: 27554
		public static WorldObjectDef Debug_Arena;
	}
}
