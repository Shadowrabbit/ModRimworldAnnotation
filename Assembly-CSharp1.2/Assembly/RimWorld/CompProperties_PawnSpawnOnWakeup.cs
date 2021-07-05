using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FF RID: 6143
	public class CompProperties_PawnSpawnOnWakeup : CompProperties
	{
		// Token: 0x060087F2 RID: 34802 RVA: 0x0005B3FF File Offset: 0x000595FF
		public CompProperties_PawnSpawnOnWakeup()
		{
			this.compClass = typeof(CompPawnSpawnOnWakeup);
		}

		// Token: 0x04005731 RID: 22321
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x04005732 RID: 22322
		public SoundDef spawnSound;

		// Token: 0x04005733 RID: 22323
		public EffecterDef spawnEffecter;

		// Token: 0x04005734 RID: 22324
		public Type lordJob;

		// Token: 0x04005735 RID: 22325
		public bool shouldJoinParentLord;

		// Token: 0x04005736 RID: 22326
		public string activatedMessageKey;

		// Token: 0x04005737 RID: 22327
		public FloatRange points;

		// Token: 0x04005738 RID: 22328
		public IntRange pawnSpawnRadius = new IntRange(2, 2);

		// Token: 0x04005739 RID: 22329
		public bool aggressive = true;

		// Token: 0x0400573A RID: 22330
		public bool dropInPods;

		// Token: 0x0400573B RID: 22331
		public float defendRadius = 21f;
	}
}
