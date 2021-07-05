using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000746 RID: 1862
	public class JobDriver_PlantHarvest_Designated : JobDriver_PlantHarvest
	{
		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06003392 RID: 13202 RVA: 0x00125793 File Offset: 0x00123993
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.HarvestPlant;
			}
		}
	}
}
