using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D02 RID: 3330
	public class TemperatureMemory : IExposable
	{
		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06004DD0 RID: 19920 RVA: 0x001A1CB2 File Offset: 0x0019FEB2
		public bool GrowthSeasonOutdoorsNow
		{
			get
			{
				return Find.TickManager.TicksGame < this.growthSeasonUntilTick;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x001A1CC6 File Offset: 0x0019FEC6
		public bool GrowthSeasonOutdoorsNowForSowing
		{
			get
			{
				return (this.noSowUntilTick <= 0 || Find.TickManager.TicksGame >= this.noSowUntilTick) && this.GrowthSeasonOutdoorsNow;
			}
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x001A1CEB File Offset: 0x0019FEEB
		public TemperatureMemory(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x001A1D08 File Offset: 0x0019FF08
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

		// Token: 0x06004DD4 RID: 19924 RVA: 0x001A1D8E File Offset: 0x0019FF8E
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.growthSeasonUntilTick, "growthSeasonUntilTick", 0, true);
			Scribe_Values.Look<int>(ref this.noSowUntilTick, "noSowUntilTick", 0, true);
		}

		// Token: 0x04002EFB RID: 12027
		private Map map;

		// Token: 0x04002EFC RID: 12028
		private int growthSeasonUntilTick = -1;

		// Token: 0x04002EFD RID: 12029
		private int noSowUntilTick = -1;

		// Token: 0x04002EFE RID: 12030
		private const int TicksBuffer = 30000;
	}
}
