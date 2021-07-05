using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A3 RID: 4515
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x06006CC1 RID: 27841 RVA: 0x00248364 File Offset: 0x00246564
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}

		// Token: 0x04003C74 RID: 15476
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x04003C75 RID: 15477
		public float HiveSpawnRadius = 10f;

		// Token: 0x04003C76 RID: 15478
		public FloatRange HiveSpawnIntervalDays = new FloatRange(2f, 3f);

		// Token: 0x04003C77 RID: 15479
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
