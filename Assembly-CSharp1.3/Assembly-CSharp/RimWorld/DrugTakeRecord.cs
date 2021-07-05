using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2D RID: 3629
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x060053E8 RID: 21480 RVA: 0x001C64ED File Offset: 0x001C46ED
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x060053E9 RID: 21481 RVA: 0x001C64FA File Offset: 0x001C46FA
		// (set) Token: 0x060053EA RID: 21482 RVA: 0x001C6511 File Offset: 0x001C4711
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

		// Token: 0x060053EB RID: 21483 RVA: 0x001C6528 File Offset: 0x001C4728
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}

		// Token: 0x04003164 RID: 12644
		public ThingDef drug;

		// Token: 0x04003165 RID: 12645
		public int lastTakenTicks;

		// Token: 0x04003166 RID: 12646
		private int timesTakenThisDayInt;

		// Token: 0x04003167 RID: 12647
		private int thisDay;
	}
}
