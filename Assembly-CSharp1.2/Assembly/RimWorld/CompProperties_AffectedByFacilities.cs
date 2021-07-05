using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFD RID: 3837
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x06005504 RID: 21764 RVA: 0x0003AED0 File Offset: 0x000390D0
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}

		// Token: 0x04003603 RID: 13827
		public List<ThingDef> linkableFacilities;
	}
}
