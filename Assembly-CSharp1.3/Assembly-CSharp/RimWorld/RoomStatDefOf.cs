using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001430 RID: 5168
	[DefOf]
	public static class RoomStatDefOf
	{
		// Token: 0x06007D23 RID: 32035 RVA: 0x002C486F File Offset: 0x002C2A6F
		static RoomStatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomStatDefOf));
		}

		// Token: 0x04004B50 RID: 19280
		public static RoomStatDef Cleanliness;

		// Token: 0x04004B51 RID: 19281
		public static RoomStatDef Wealth;

		// Token: 0x04004B52 RID: 19282
		public static RoomStatDef Space;

		// Token: 0x04004B53 RID: 19283
		public static RoomStatDef Beauty;

		// Token: 0x04004B54 RID: 19284
		public static RoomStatDef Impressiveness;

		// Token: 0x04004B55 RID: 19285
		public static RoomStatDef InfectionChanceFactor;

		// Token: 0x04004B56 RID: 19286
		public static RoomStatDef ResearchSpeedFactor;

		// Token: 0x04004B57 RID: 19287
		public static RoomStatDef GraveVisitingJoyGainFactor;

		// Token: 0x04004B58 RID: 19288
		public static RoomStatDef FoodPoisonChance;

		// Token: 0x04004B59 RID: 19289
		[MayRequireIdeology]
		public static RoomStatDef BiosculpterPodSpeedFactorOffset;
	}
}
