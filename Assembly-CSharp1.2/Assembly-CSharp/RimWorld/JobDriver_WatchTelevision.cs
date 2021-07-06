using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BAA RID: 2986
	public class JobDriver_WatchTelevision : JobDriver_WatchBuilding
	{
		// Token: 0x0600462A RID: 17962 RVA: 0x00194854 File Offset: 0x00192A54
		protected override void WatchTickAction()
		{
			if (!((Building)base.TargetA.Thing).TryGetComp<CompPowerTrader>().PowerOn)
			{
				base.EndJobWith(JobCondition.Incompletable);
				return;
			}
			base.WatchTickAction();
		}
	}
}
