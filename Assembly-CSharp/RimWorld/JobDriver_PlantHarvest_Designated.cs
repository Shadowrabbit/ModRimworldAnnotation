using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2E RID: 3118
	public class JobDriver_PlantHarvest_Designated : JobDriver_PlantHarvest
	{
		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x0600493E RID: 18750 RVA: 0x00034DAB File Offset: 0x00032FAB
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.HarvestPlant;
			}
		}
	}
}
