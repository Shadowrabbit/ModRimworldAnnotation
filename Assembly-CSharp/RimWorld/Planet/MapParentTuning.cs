using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002137 RID: 8503
	public static class MapParentTuning
	{
		// Token: 0x04007C11 RID: 31761
		[Obsolete]
		public const float DefaultForceExitAndRemoveMapCountdownDays = 1f;

		// Token: 0x04007C12 RID: 31762
		[Obsolete]
		public const float DefaultSiteForceExitAndRemoveMapCountdownDays = 3f;

		// Token: 0x04007C13 RID: 31763
		public const float ShortDetectionCountdownDays = 4f;

		// Token: 0x04007C14 RID: 31764
		public const float DefaultDetectionCountdownDays = 4f;

		// Token: 0x04007C15 RID: 31765
		public static readonly FloatRange SiteDetectionCountdownMultiplier = new FloatRange(0.85f, 1.15f);
	}
}
