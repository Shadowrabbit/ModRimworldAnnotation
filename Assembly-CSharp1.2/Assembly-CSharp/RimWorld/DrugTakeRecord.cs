using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CA RID: 5322
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x060072A6 RID: 29350 RVA: 0x0004D1BF File Offset: 0x0004B3BF
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x060072A7 RID: 29351 RVA: 0x0004D1CC File Offset: 0x0004B3CC
		// (set) Token: 0x060072A8 RID: 29352 RVA: 0x0004D1E3 File Offset: 0x0004B3E3
		public int TimesTakenThisDay
		{
			get
			{
				if (this.thisDay != GenDate.DaysPassed)
				{
					return 0;
				}
				return this.timesTakenThisDayInt;
			}
			set
			{
				this.timesTakenThisDayInt = value;
				this.thisDay = GenDate.DaysPassed;
			}
		}

		// Token: 0x060072A9 RID: 29353 RVA: 0x002303CC File Offset: 0x0022E5CC
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}

		// Token: 0x04004B82 RID: 19330
		public ThingDef drug;

		// Token: 0x04004B83 RID: 19331
		public int lastTakenTicks;

		// Token: 0x04004B84 RID: 19332
		private int timesTakenThisDayInt;

		// Token: 0x04004B85 RID: 19333
		private int thisDay;
	}
}
