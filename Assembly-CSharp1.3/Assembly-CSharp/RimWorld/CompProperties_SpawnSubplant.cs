using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119B RID: 4507
	public class CompProperties_SpawnSubplant : CompProperties
	{
		// Token: 0x04003C51 RID: 15441
		public ThingDef subplant;

		// Token: 0x04003C52 RID: 15442
		public SoundDef spawnSound;

		// Token: 0x04003C53 RID: 15443
		public float maxRadius;

		// Token: 0x04003C54 RID: 15444
		public float subplantSpawnDays;

		// Token: 0x04003C55 RID: 15445
		public float minGrowthForSpawn;

		// Token: 0x04003C56 RID: 15446
		public FloatRange? initialGrowthRange;

		// Token: 0x04003C57 RID: 15447
		public bool canSpawnOverPlayerSownPlants = true;
	}
}
