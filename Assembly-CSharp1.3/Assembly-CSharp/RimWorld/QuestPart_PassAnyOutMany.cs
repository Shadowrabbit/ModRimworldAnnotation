using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1D RID: 2845
	public class QuestPart_PassAnyOutMany : QuestPart
	{
		// Token: 0x060042E0 RID: 17120 RVA: 0x00165C08 File Offset: 0x00163E08
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				for (int i = 0; i < this.outSignals.Count; i++)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignals[i], signal.args));
				}
			}
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x00165C66 File Offset: 0x00163E66
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x00165C9C File Offset: 0x00163E9C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			this.outSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x040028B5 RID: 10421
		public List<string> inSignals = new List<string>();

		// Token: 0x040028B6 RID: 10422
		public List<string> outSignals = new List<string>();
	}
}
