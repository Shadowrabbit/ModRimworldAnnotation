using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E0 RID: 6112
	public static class SiteTuning
	{
		// Token: 0x040059D9 RID: 23001
		public static readonly IntRange QuestSiteTimeoutDaysRange = new IntRange(12, 28);

		// Token: 0x040059DA RID: 23002
		public static readonly FloatRange SitePointRandomFactorRange = new FloatRange(0.7f, 1.3f);

		// Token: 0x040059DB RID: 23003
		public static readonly SimpleCurve ThreatPointsToSiteThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 120f),
				true
			},
			{
				new CurvePoint(1000f, 300f),
				true
			},
			{
				new CurvePoint(2000f, 600f),
				true
			},
			{
				new CurvePoint(3000f, 800f),
				true
			},
			{
				new CurvePoint(4000f, 900f),
				true
			},
			{
				new CurvePoint(5000f, 1000f),
				true
			}
		};
	}
}
