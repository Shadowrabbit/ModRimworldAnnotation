using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D8 RID: 5848
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x0600807C RID: 32892 RVA: 0x00056405 File Offset: 0x00054605
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (Find.TickManager.TicksGame % 30 == 4)
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}

		// Token: 0x0400532F RID: 21295
		private const int HeatPushInterval = 30;
	}
}
