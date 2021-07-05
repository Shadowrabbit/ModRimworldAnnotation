using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000747 RID: 1863
	public class JobDriver_PlantCut_Designated : JobDriver_PlantCut
	{
		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06003394 RID: 13204 RVA: 0x001257A2 File Offset: 0x001239A2
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.CutPlant;
			}
		}
	}
}
