using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000744 RID: 1860
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		// Token: 0x0600338C RID: 13196 RVA: 0x0012572C File Offset: 0x0012392C
		protected override void Init()
		{
			this.xpPerTick = 0.085f;
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x00125739 File Offset: 0x00123939
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
