using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A7 RID: 4519
	public class CompProperties_SpawnerPawn : CompProperties
	{
		// Token: 0x06006CD0 RID: 27856 RVA: 0x0024863C File Offset: 0x0024683C
		public CompProperties_SpawnerPawn()
		{
			this.compClass = typeof(CompSpawnerPawn);
		}

		// Token: 0x04003C81 RID: 15489
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x04003C82 RID: 15490
		public SoundDef spawnSound;

		// Token: 0x04003C83 RID: 15491
		public string spawnMessageKey;

		// Token: 0x04003C84 RID: 15492
		public string noPawnsLeftToSpawnKey;

		// Token: 0x04003C85 RID: 15493
		public string pawnsLeftToSpawnKey;

		// Token: 0x04003C86 RID: 15494
		public bool showNextSpawnInInspect;

		// Token: 0x04003C87 RID: 15495
		public bool shouldJoinParentLord;

		// Token: 0x04003C88 RID: 15496
		public Type lordJob;

		// Token: 0x04003C89 RID: 15497
		public float defendRadius = 21f;

		// Token: 0x04003C8A RID: 15498
		public int initialPawnsCount;

		// Token: 0x04003C8B RID: 15499
		public float initialPawnsPoints;

		// Token: 0x04003C8C RID: 15500
		public float maxSpawnedPawnsPoints = -1f;

		// Token: 0x04003C8D RID: 15501
		public FloatRange pawnSpawnIntervalDays = new FloatRange(0.85f, 1.15f);

		// Token: 0x04003C8E RID: 15502
		public int pawnSpawnRadius = 2;

		// Token: 0x04003C8F RID: 15503
		public IntRange maxPawnsToSpawn = IntRange.zero;

		// Token: 0x04003C90 RID: 15504
		public bool chooseSingleTypeToSpawn;

		// Token: 0x04003C91 RID: 15505
		public string nextSpawnInspectStringKey;

		// Token: 0x04003C92 RID: 15506
		public string nextSpawnInspectStringKeyDormant;
	}
}
