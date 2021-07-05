using System;

namespace RimWorld
{
	// Token: 0x02000FC1 RID: 4033
	public class CompProperties_RitualEffectIntervalSpawnDividedCircle : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x06005F0C RID: 24332 RVA: 0x002084CA File Offset: 0x002066CA
		public CompProperties_RitualEffectIntervalSpawnDividedCircle()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnDividedCircleEffecter);
		}

		// Token: 0x040036C6 RID: 14022
		public float radius = 5f;

		// Token: 0x040036C7 RID: 14023
		public int numCopies = 5;
	}
}
