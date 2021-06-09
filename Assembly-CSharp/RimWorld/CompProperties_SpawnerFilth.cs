using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184F RID: 6223
	public class CompProperties_SpawnerFilth : CompProperties
	{
		// Token: 0x06008A18 RID: 35352 RVA: 0x0005CAA2 File Offset: 0x0005ACA2
		public CompProperties_SpawnerFilth()
		{
			this.compClass = typeof(CompSpawnerFilth);
		}

		// Token: 0x0400588B RID: 22667
		public ThingDef filthDef;

		// Token: 0x0400588C RID: 22668
		public int spawnCountOnSpawn = 5;

		// Token: 0x0400588D RID: 22669
		public float spawnMtbHours = 12f;

		// Token: 0x0400588E RID: 22670
		public float spawnRadius = 3f;

		// Token: 0x0400588F RID: 22671
		public float spawnEveryDays = -1f;

		// Token: 0x04005890 RID: 22672
		public RotStage? requiredRotStage;
	}
}
