using System;

namespace RimWorld
{
	// Token: 0x02000FC3 RID: 4035
	public class CompProperties_RitualEffectLightball : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x06005F11 RID: 24337 RVA: 0x0020862E File Offset: 0x0020682E
		public CompProperties_RitualEffectLightball()
		{
			this.compClass = typeof(CompRitualEffect_Lightball);
		}

		// Token: 0x040036C8 RID: 14024
		public float radius;
	}
}
