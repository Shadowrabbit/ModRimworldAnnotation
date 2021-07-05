using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116C RID: 4460
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x06006B1C RID: 27420 RVA: 0x0023F47C File Offset: 0x0023D67C
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}

		// Token: 0x04003B95 RID: 15253
		public float harmFrequencyPerArea = 0.011f;

		// Token: 0x04003B96 RID: 15254
		public float leaflessPlantKillChance = 0.05f;

		// Token: 0x04003B97 RID: 15255
		public SimpleCurve radiusPerDayCurve;
	}
}
