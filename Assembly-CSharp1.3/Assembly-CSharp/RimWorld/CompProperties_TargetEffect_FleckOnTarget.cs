using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E6 RID: 4582
	public class CompProperties_TargetEffect_FleckOnTarget : CompProperties
	{
		// Token: 0x06006E77 RID: 28279 RVA: 0x002503D1 File Offset: 0x0024E5D1
		public CompProperties_TargetEffect_FleckOnTarget()
		{
			this.compClass = typeof(ComTargetEffect_FleckOnTarget);
		}

		// Token: 0x04003D41 RID: 15681
		public FleckDef fleckDef;
	}
}
