using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001855 RID: 6229
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x06008A36 RID: 35382 RVA: 0x002863CC File Offset: 0x002845CC
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}

		// Token: 0x040058A1 RID: 22689
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x040058A2 RID: 22690
		public float HiveSpawnRadius = 10f;

		// Token: 0x040058A3 RID: 22691
		public FloatRange HiveSpawnIntervalDays = new FloatRange(2f, 3f);

		// Token: 0x040058A4 RID: 22692
		public SimpleCurve ReproduceRateFactorFromNearbyHiveCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(7f, 0.35f),
				true
			}
		};
	}
}
