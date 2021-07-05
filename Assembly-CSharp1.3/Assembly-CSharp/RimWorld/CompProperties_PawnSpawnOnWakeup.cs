using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116A RID: 4458
	public class CompProperties_PawnSpawnOnWakeup : CompProperties
	{
		// Token: 0x06006B10 RID: 27408 RVA: 0x0023EE5A File Offset: 0x0023D05A
		public CompProperties_PawnSpawnOnWakeup()
		{
			this.compClass = typeof(CompPawnSpawnOnWakeup);
		}

		// Token: 0x04003B88 RID: 15240
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x04003B89 RID: 15241
		public SoundDef spawnSound;

		// Token: 0x04003B8A RID: 15242
		public EffecterDef spawnEffecter;

		// Token: 0x04003B8B RID: 15243
		public Type lordJob;

		// Token: 0x04003B8C RID: 15244
		public bool shouldJoinParentLord;

		// Token: 0x04003B8D RID: 15245
		public string activatedMessageKey;

		// Token: 0x04003B8E RID: 15246
		public FloatRange points;

		// Token: 0x04003B8F RID: 15247
		public IntRange pawnSpawnRadius = new IntRange(2, 2);

		// Token: 0x04003B90 RID: 15248
		public bool aggressive = true;

		// Token: 0x04003B91 RID: 15249
		public bool dropInPods;

		// Token: 0x04003B92 RID: 15250
		public float defendRadius = 21f;
	}
}
