using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBF RID: 4031
	public class CompProperties_RitualEffectIntervalSpawnCircle : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x06005F08 RID: 24328 RVA: 0x002083EC File Offset: 0x002065EC
		public CompProperties_RitualEffectIntervalSpawnCircle()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnCircle);
		}

		// Token: 0x040036C3 RID: 14019
		public IntVec2 area;

		// Token: 0x040036C4 RID: 14020
		public float radius = 5f;

		// Token: 0x040036C5 RID: 14021
		public float concentration = 1f;
	}
}
