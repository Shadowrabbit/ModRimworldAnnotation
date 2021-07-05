using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B14 RID: 2836
	public class QuestPart_Pass : QuestPart
	{
		// Token: 0x060042B8 RID: 17080 RVA: 0x0016523C File Offset: 0x0016343C
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

		// Token: 0x060042B9 RID: 17081 RVA: 0x001652B4 File Offset: 0x001634B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outSignalOutcomeArg, "outSignalOutcomeArg", null, false);
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x00165305 File Offset: 0x00163505
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x040028A1 RID: 10401
		public string inSignal;

		// Token: 0x040028A2 RID: 10402
		public string outSignal;

		// Token: 0x040028A3 RID: 10403
		public QuestEndOutcome? outSignalOutcomeArg;
	}
}
