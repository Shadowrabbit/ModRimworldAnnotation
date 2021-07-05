using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184A RID: 6218
	public class CompProperties_Spawner : CompProperties
	{
		// Token: 0x060089F3 RID: 35315 RVA: 0x0005C94F File Offset: 0x0005AB4F
		public CompProperties_Spawner()
		{
			this.compClass = typeof(CompSpawner);
		}

		// Token: 0x04005879 RID: 22649
		public ThingDef thingToSpawn;

		// Token: 0x0400587A RID: 22650
		public int spawnCount = 1;

		// Token: 0x0400587B RID: 22651
		public IntRange spawnIntervalRange = new IntRange(100, 100);

		// Token: 0x0400587C RID: 22652
		public int spawnMaxAdjacent = -1;

		// Token: 0x0400587D RID: 22653
		public bool spawnForbidden;

		// Token: 0x0400587E RID: 22654
		public bool requiresPower;

		// Token: 0x0400587F RID: 22655
		public bool writeTimeLeftToSpawn;

		// Token: 0x04005880 RID: 22656
		public bool showMessageIfOwned;

		// Token: 0x04005881 RID: 22657
		public string saveKeysPrefix;

		// Token: 0x04005882 RID: 22658
		public bool inheritFaction;
	}
}
