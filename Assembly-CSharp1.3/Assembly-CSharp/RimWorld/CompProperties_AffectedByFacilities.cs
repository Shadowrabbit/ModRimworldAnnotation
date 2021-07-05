using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F2 RID: 2546
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x06003EC6 RID: 16070 RVA: 0x001573DC File Offset: 0x001555DC
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}

		// Token: 0x04002190 RID: 8592
		public List<ThingDef> linkableFacilities;
	}
}
