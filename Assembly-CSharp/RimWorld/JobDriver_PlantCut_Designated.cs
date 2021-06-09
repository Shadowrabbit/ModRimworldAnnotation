using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2F RID: 3119
	public class JobDriver_PlantCut_Designated : JobDriver_PlantCut
	{
		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004940 RID: 18752 RVA: 0x00034DBA File Offset: 0x00032FBA
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.CutPlant;
			}
		}
	}
}
