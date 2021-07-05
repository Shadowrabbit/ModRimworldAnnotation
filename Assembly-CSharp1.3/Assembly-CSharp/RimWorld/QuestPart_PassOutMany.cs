using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1F RID: 2847
	public class QuestPart_PassOutMany : QuestPart
	{
		// Token: 0x060042E9 RID: 17129 RVA: 0x00165EF4 File Offset: 0x001640F4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.outSignals.Count; i++)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignals[i], signal.args));
				}
			}
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x00165F52 File Offset: 0x00164152
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x00165F84 File Offset: 0x00164184
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x040028BB RID: 10427
		public string inSignal;

		// Token: 0x040028BC RID: 10428
		public List<string> outSignals = new List<string>();
	}
}
