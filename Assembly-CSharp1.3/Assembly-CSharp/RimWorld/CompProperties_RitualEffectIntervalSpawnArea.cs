using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBB RID: 4027
	public class CompProperties_RitualEffectIntervalSpawnArea : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x06005EFE RID: 24318 RVA: 0x00208191 File Offset: 0x00206391
		public CompProperties_RitualEffectIntervalSpawnArea()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnArea);
		}

		// Token: 0x040036BF RID: 14015
		public IntVec2 area;

		// Token: 0x040036C0 RID: 14016
		public bool smoothEdges = true;
	}
}
