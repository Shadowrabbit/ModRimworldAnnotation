using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001338 RID: 4920
	public class TemperatureMemory : IExposable
	{
		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06006AB9 RID: 27321 RVA: 0x00048931 File Offset: 0x00046B31
		public bool GrowthSeasonOutdoorsNow
		{
			get
			{
				return Find.TickManager.TicksGame < this.growthSeasonUntilTick;
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06006ABA RID: 27322 RVA: 0x00048945 File Offset: 0x00046B45
		public bool GrowthSeasonOutdoorsNowForSowing
		{
			get
			{
				return (this.noSowUntilTick <= 0 || Find.TickManager.TicksGame >= this.noSowUntilTick) && this.GrowthSeasonOutdoorsNow;
			}
		}

		// Token: 0x06006ABB RID: 27323 RVA: 0x0004896A File Offset: 0x00046B6A
		public TemperatureMemory(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x0020F73C File Offset: 0x0020D93C
		public void GrowthSeasonMemoryTick()
		{
			if (this.map.mapTemperature.OutdoorTemp > 0f && this.map.mapTemperature.OutdoorTemp < 58f)
			{
				this.growthSeasonUntilTick = Find.TickManager.TicksGame + 30000;
				return;
			}
			if (this.map.mapTemperature.OutdoorTemp < -2f)
			{
				this.growthSeasonUntilTick = -1;
				this.noSowUntilTick = Find.TickManager.TicksGame + 30000;
			}
		}

		// Token: 0x06006ABD RID: 27325 RVA: 0x00048987 File Offset: 0x00046B87
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.growthSeasonUntilTick, "growthSeasonUntilTick", 0, true);
			Scribe_Values.Look<int>(ref this.noSowUntilTick, "noSowUntilTick", 0, true);
		}

		// Token: 0x04004707 RID: 18183
		private Map map;

		// Token: 0x04004708 RID: 18184
		private int growthSeasonUntilTick = -1;

		// Token: 0x04004709 RID: 18185
		private int noSowUntilTick = -1;

		// Token: 0x0400470A RID: 18186
		private const int TicksBuffer = 30000;
	}
}
