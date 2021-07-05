using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E8 RID: 4584
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		// Token: 0x06006E7B RID: 28283 RVA: 0x00250425 File Offset: 0x0024E625
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}

		// Token: 0x04003D42 RID: 15682
		public int goodwillImpact = -200;
	}
}
