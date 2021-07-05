using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141C RID: 5148
	[DefOf]
	public static class MentalStateDefOf
	{
		// Token: 0x06007D0F RID: 32015 RVA: 0x002C471B File Offset: 0x002C291B
		static MentalStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
		}

		// Token: 0x04004945 RID: 18757
		public static MentalStateDef Berserk;

		// Token: 0x04004946 RID: 18758
		public static MentalStateDef Binging_DrugExtreme;

		// Token: 0x04004947 RID: 18759
		[MayRequireRoyalty]
		public static MentalStateDef BerserkMechanoid;

		// Token: 0x04004948 RID: 18760
		public static MentalStateDef Wander_Psychotic;

		// Token: 0x04004949 RID: 18761
		public static MentalStateDef Binging_DrugMajor;

		// Token: 0x0400494A RID: 18762
		public static MentalStateDef Wander_Sad;

		// Token: 0x0400494B RID: 18763
		public static MentalStateDef Wander_OwnRoom;

		// Token: 0x0400494C RID: 18764
		public static MentalStateDef PanicFlee;

		// Token: 0x0400494D RID: 18765
		public static MentalStateDef Manhunter;

		// Token: 0x0400494E RID: 18766
		public static MentalStateDef ManhunterPermanent;

		// Token: 0x0400494F RID: 18767
		public static MentalStateDef SocialFighting;

		// Token: 0x04004950 RID: 18768
		public static MentalStateDef Roaming;
	}
}
