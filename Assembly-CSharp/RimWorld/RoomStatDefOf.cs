using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C70 RID: 7280
	[DefOf]
	public static class RoomStatDefOf
	{
		// Token: 0x06009F73 RID: 40819 RVA: 0x0006A410 File Offset: 0x00068610
		static RoomStatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomStatDefOf));
		}

		// Token: 0x04006A87 RID: 27271
		public static RoomStatDef Cleanliness;

		// Token: 0x04006A88 RID: 27272
		public static RoomStatDef Wealth;

		// Token: 0x04006A89 RID: 27273
		public static RoomStatDef Space;

		// Token: 0x04006A8A RID: 27274
		public static RoomStatDef Beauty;

		// Token: 0x04006A8B RID: 27275
		public static RoomStatDef Impressiveness;

		// Token: 0x04006A8C RID: 27276
		public static RoomStatDef InfectionChanceFactor;

		// Token: 0x04006A8D RID: 27277
		public static RoomStatDef ResearchSpeedFactor;

		// Token: 0x04006A8E RID: 27278
		public static RoomStatDef GraveVisitingJoyGainFactor;

		// Token: 0x04006A8F RID: 27279
		public static RoomStatDef FoodPoisonChance;
	}
}
