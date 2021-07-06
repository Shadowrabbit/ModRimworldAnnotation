using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C6 RID: 6342
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		// Token: 0x06008CB1 RID: 36017 RVA: 0x0005E4BB File Offset: 0x0005C6BB
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}

		// Token: 0x040059FB RID: 23035
		public int goodwillImpact = -200;
	}
}
