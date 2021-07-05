using System;

namespace RimWorld
{
	// Token: 0x02000FB6 RID: 4022
	public class CompProperties_RitualEffectConstantCircle : CompProperties_RitualVisualEffect
	{
		// Token: 0x06005EEF RID: 24303 RVA: 0x00207EAC File Offset: 0x002060AC
		public CompProperties_RitualEffectConstantCircle()
		{
			this.compClass = typeof(CompRitualEffect_ConstantCircle);
		}

		// Token: 0x040036B6 RID: 14006
		public float radius = 5f;

		// Token: 0x040036B7 RID: 14007
		public int numCopies = 5;
	}
}
