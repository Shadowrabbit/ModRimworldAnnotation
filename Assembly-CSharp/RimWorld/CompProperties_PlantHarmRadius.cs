using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001805 RID: 6149
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x0600880E RID: 34830 RVA: 0x0005B4EE File Offset: 0x000596EE
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}

		// Token: 0x04005749 RID: 22345
		public float harmFrequencyPerArea = 0.011f;

		// Token: 0x0400574A RID: 22346
		public float leaflessPlantKillChance = 0.05f;

		// Token: 0x0400574B RID: 22347
		public SimpleCurve radiusPerDayCurve;
	}
}
