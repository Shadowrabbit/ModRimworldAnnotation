using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C2C RID: 3116
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		// Token: 0x06004938 RID: 18744 RVA: 0x00034D44 File Offset: 0x00032F44
		protected override void Init()
		{
			this.xpPerTick = 0.085f;
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x00034D51 File Offset: 0x00032F51
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
