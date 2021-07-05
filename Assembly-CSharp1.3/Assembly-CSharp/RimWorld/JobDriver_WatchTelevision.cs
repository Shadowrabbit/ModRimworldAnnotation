using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000706 RID: 1798
	public class JobDriver_WatchTelevision : JobDriver_WatchBuilding
	{
		// Token: 0x060031F1 RID: 12785 RVA: 0x00121994 File Offset: 0x0011FB94
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
