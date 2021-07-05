using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011EE RID: 4590
	public class CompProperties_TargetEffect_MoteOnTarget : CompProperties
	{
		// Token: 0x06006E87 RID: 28295 RVA: 0x002505C4 File Offset: 0x0024E7C4
		public CompProperties_TargetEffect_MoteOnTarget()
		{
			this.compClass = typeof(ComTargetEffect_MoteOnTarget);
		}

		// Token: 0x04003D44 RID: 15684
		public ThingDef moteDef;
	}
}
