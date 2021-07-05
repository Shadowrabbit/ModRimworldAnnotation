using System;

namespace RimWorld
{
	// Token: 0x02000FC5 RID: 4037
	public class CompProperties_RitualEffectDrum : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x06005F16 RID: 24342 RVA: 0x002087CC File Offset: 0x002069CC
		public CompProperties_RitualEffectDrum()
		{
			this.compClass = typeof(CompRitualEffect_Drum);
		}

		// Token: 0x040036C9 RID: 14025
		public int maxDistance;

		// Token: 0x040036CA RID: 14026
		public float maxOffset;
	}
}
