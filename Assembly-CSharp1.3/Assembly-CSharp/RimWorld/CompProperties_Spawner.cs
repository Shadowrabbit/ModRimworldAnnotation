using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119E RID: 4510
	public class CompProperties_Spawner : CompProperties
	{
		// Token: 0x06006C99 RID: 27801 RVA: 0x0024751C File Offset: 0x0024571C
		public CompProperties_Spawner()
		{
			this.compClass = typeof(CompSpawner);
		}

		// Token: 0x04003C5E RID: 15454
		public ThingDef thingToSpawn;

		// Token: 0x04003C5F RID: 15455
		public int spawnCount = 1;

		// Token: 0x04003C60 RID: 15456
		public IntRange spawnIntervalRange = new IntRange(100, 100);

		// Token: 0x04003C61 RID: 15457
		public int spawnMaxAdjacent = -1;

		// Token: 0x04003C62 RID: 15458
		public bool spawnForbidden;

		// Token: 0x04003C63 RID: 15459
		public bool requiresPower;

		// Token: 0x04003C64 RID: 15460
		public bool writeTimeLeftToSpawn;

		// Token: 0x04003C65 RID: 15461
		public bool showMessageIfOwned;

		// Token: 0x04003C66 RID: 15462
		public string saveKeysPrefix;

		// Token: 0x04003C67 RID: 15463
		public bool inheritFaction;
	}
}
