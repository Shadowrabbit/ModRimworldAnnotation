using System;

namespace Verse.AI
{
	// Token: 0x020009EC RID: 2540
	internal class Toils_Interact
	{
		// Token: 0x06003D22 RID: 15650 RVA: 0x0017469C File Offset: 0x0017289C
		public static Toil DestroyThing(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				if (!thing.Destroyed)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			};
			return toil;
		}
	}
}
