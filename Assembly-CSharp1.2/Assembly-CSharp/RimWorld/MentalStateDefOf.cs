using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5C RID: 7260
	[DefOf]
	public static class MentalStateDefOf
	{
		// Token: 0x06009F5F RID: 40799 RVA: 0x0006A2BC File Offset: 0x000684BC
		static MentalStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
		}

		// Token: 0x040068D1 RID: 26833
		public static MentalStateDef Berserk;

		// Token: 0x040068D2 RID: 26834
		public static MentalStateDef Binging_DrugExtreme;

		// Token: 0x040068D3 RID: 26835
		[MayRequireRoyalty]
		public static MentalStateDef BerserkMechanoid;

		// Token: 0x040068D4 RID: 26836
		public static MentalStateDef Wander_Psychotic;

		// Token: 0x040068D5 RID: 26837
		public static MentalStateDef Binging_DrugMajor;

		// Token: 0x040068D6 RID: 26838
		public static MentalStateDef Wander_Sad;

		// Token: 0x040068D7 RID: 26839
		public static MentalStateDef Wander_OwnRoom;

		// Token: 0x040068D8 RID: 26840
		public static MentalStateDef PanicFlee;

		// Token: 0x040068D9 RID: 26841
		public static MentalStateDef Manhunter;

		// Token: 0x040068DA RID: 26842
		public static MentalStateDef ManhunterPermanent;

		// Token: 0x040068DB RID: 26843
		public static MentalStateDef SocialFighting;
	}
}
