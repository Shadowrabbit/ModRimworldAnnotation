using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108B RID: 4235
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x060064DC RID: 25820 RVA: 0x0021FAB3 File Offset: 0x0021DCB3
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (Find.TickManager.TicksGame % 30 == 4)
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}

		// Token: 0x040038BE RID: 14526
		private const int HeatPushInterval = 30;
	}
}
