using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185C RID: 6236
	public class CompProperties_SpawnerPawn : CompProperties
	{
		// Token: 0x06008A58 RID: 35416 RVA: 0x0028676C File Offset: 0x0028496C
		public CompProperties_SpawnerPawn()
		{
			this.compClass = typeof(CompSpawnerPawn);
		}

		// Token: 0x040058B9 RID: 22713
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x040058BA RID: 22714
		public SoundDef spawnSound;

		// Token: 0x040058BB RID: 22715
		public string spawnMessageKey;

		// Token: 0x040058BC RID: 22716
		public string noPawnsLeftToSpawnKey;

		// Token: 0x040058BD RID: 22717
		public string pawnsLeftToSpawnKey;

		// Token: 0x040058BE RID: 22718
		public bool showNextSpawnInInspect;

		// Token: 0x040058BF RID: 22719
		public bool shouldJoinParentLord;

		// Token: 0x040058C0 RID: 22720
		public Type lordJob;

		// Token: 0x040058C1 RID: 22721
		public float defendRadius = 21f;

		// Token: 0x040058C2 RID: 22722
		public int initialPawnsCount;

		// Token: 0x040058C3 RID: 22723
		public float initialPawnsPoints;

		// Token: 0x040058C4 RID: 22724
		public float maxSpawnedPawnsPoints = -1f;

		// Token: 0x040058C5 RID: 22725
		public FloatRange pawnSpawnIntervalDays = new FloatRange(0.85f, 1.15f);

		// Token: 0x040058C6 RID: 22726
		public int pawnSpawnRadius = 2;

		// Token: 0x040058C7 RID: 22727
		public IntRange maxPawnsToSpawn = IntRange.zero;

		// Token: 0x040058C8 RID: 22728
		public bool chooseSingleTypeToSpawn;

		// Token: 0x040058C9 RID: 22729
		public string nextSpawnInspectStringKey;

		// Token: 0x040058CA RID: 22730
		public string nextSpawnInspectStringKeyDormant;
	}
}
