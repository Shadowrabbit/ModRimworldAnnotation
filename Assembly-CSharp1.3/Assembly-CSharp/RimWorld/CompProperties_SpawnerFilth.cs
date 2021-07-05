using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A1 RID: 4513
	public class CompProperties_SpawnerFilth : CompProperties
	{
		// Token: 0x06006CB3 RID: 27827 RVA: 0x00247CCD File Offset: 0x00245ECD
		public CompProperties_SpawnerFilth()
		{
			this.compClass = typeof(CompSpawnerFilth);
		}

		// Token: 0x04003C6A RID: 15466
		public ThingDef filthDef;

		// Token: 0x04003C6B RID: 15467
		public int spawnCountOnSpawn = 5;

		// Token: 0x04003C6C RID: 15468
		public float spawnMtbHours = 12f;

		// Token: 0x04003C6D RID: 15469
		public float spawnRadius = 3f;

		// Token: 0x04003C6E RID: 15470
		public float spawnEveryDays = -1f;

		// Token: 0x04003C6F RID: 15471
		public RotStage? requiredRotStage;
	}
}
