using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017CA RID: 6090
	public static class MapParentTuning
	{
		// Token: 0x0400598C RID: 22924
		[Obsolete]
		public const float DefaultForceExitAndRemoveMapCountdownDays = 1f;

		// Token: 0x0400598D RID: 22925
		[Obsolete]
		public const float DefaultSiteForceExitAndRemoveMapCountdownDays = 3f;

		// Token: 0x0400598E RID: 22926
		public const float ShortDetectionCountdownDays = 4f;

		// Token: 0x0400598F RID: 22927
		public const float DefaultDetectionCountdownDays = 4f;

		// Token: 0x04005990 RID: 22928
		public static readonly FloatRange SiteDetectionCountdownMultiplier = new FloatRange(0.85f, 1.15f);
	}
}
