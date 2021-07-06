using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001046 RID: 4166
	public class QuestPart_Pass : QuestPart
	{
		// Token: 0x06005AD8 RID: 23256 RVA: 0x001D6AD0 File Offset: 0x001D4CD0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				SignalArgs args = new SignalArgs(signal.args);
				if (this.outSignalOutcomeArg != null)
				{
					args.Add(this.outSignalOutcomeArg.Value.Named("OUTCOME"));
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignal, args));
			}
		}

		// Token: 0x06005AD9 RID: 23257 RVA: 0x001D6B48 File Offset: 0x001D4D48
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outSignalOutcomeArg, "outSignalOutcomeArg", null, false);
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x0003EFA5 File Offset: 0x0003D1A5
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003D0D RID: 15629
		public string inSignal;

		// Token: 0x04003D0E RID: 15630
		public string outSignal;

		// Token: 0x04003D0F RID: 15631
		public QuestEndOutcome? outSignalOutcomeArg;
	}
}
