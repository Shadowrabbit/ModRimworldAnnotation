using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C2D RID: 3117
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		// Token: 0x0600493B RID: 18747 RVA: 0x00034D66 File Offset: 0x00032F66
		protected override void Init()
		{
			if (base.Plant.def.plant.harvestedThingDef != null && base.Plant.CanYieldNow())
			{
				this.xpPerTick = 0.085f;
				return;
			}
			this.xpPerTick = 0f;
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x00034DA3 File Offset: 0x00032FA3
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_Interact.DestroyThing(TargetIndex.A);
		}
	}
}
