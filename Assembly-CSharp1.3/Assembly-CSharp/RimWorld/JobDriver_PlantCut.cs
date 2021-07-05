using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000745 RID: 1861
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		// Token: 0x0600338F RID: 13199 RVA: 0x0012574E File Offset: 0x0012394E
		protected override void Init()
		{
			if (base.Plant.def.plant.harvestedThingDef != null && base.Plant.CanYieldNow())
			{
				this.xpPerTick = 0.085f;
				return;
			}
			this.xpPerTick = 0f;
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x0012578B File Offset: 0x0012398B
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_Interact.DestroyThing(TargetIndex.A);
		}
	}
}
