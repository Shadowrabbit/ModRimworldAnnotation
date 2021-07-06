using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CC RID: 6348
	public class CompProperties_TargetEffect_MoteOnTarget : CompProperties
	{
		// Token: 0x06008CBD RID: 36029 RVA: 0x0005E547 File Offset: 0x0005C747
		public CompProperties_TargetEffect_MoteOnTarget()
		{
			this.compClass = typeof(ComTargetEffect_MoteOnTarget);
		}

		// Token: 0x040059FD RID: 23037
		public ThingDef moteDef;
	}
}
