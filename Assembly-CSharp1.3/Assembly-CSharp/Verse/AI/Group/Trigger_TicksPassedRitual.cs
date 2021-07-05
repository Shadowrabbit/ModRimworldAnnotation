using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200068E RID: 1678
	public class Trigger_TicksPassedRitual : Trigger
	{
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002F22 RID: 12066 RVA: 0x00118394 File Offset: 0x00116594
		protected TriggerData_TicksPassedRitual Data
		{
			get
			{
				return (TriggerData_TicksPassedRitual)this.data;
			}
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x001183A1 File Offset: 0x001165A1
		public Trigger_TicksPassedRitual(int tickLimit, RitualStage stage)
		{
			this.data = new TriggerData_TicksPassedRitual();
			this.duration = tickLimit;
			this.stage = stage;
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x001183CC File Offset: 0x001165CC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				LordJob_Ritual ritual = lord.LordJob as LordJob_Ritual;
				TriggerData_TicksPassedRitual data = this.Data;
				data.ticksPassed += this.stage.ProgressPerTick(ritual);
				return data.ticksPassed > (float)this.duration;
			}
			return false;
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x0011841D File Offset: 0x0011661D
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0f;
			}
		}

		// Token: 0x04001CD6 RID: 7382
		private int duration = 100;

		// Token: 0x04001CD7 RID: 7383
		private RitualStage stage;
	}
}
