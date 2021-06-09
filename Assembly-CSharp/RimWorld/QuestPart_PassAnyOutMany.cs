using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104F RID: 4175
	public class QuestPart_PassAnyOutMany : QuestPart
	{
		// Token: 0x06005AFE RID: 23294 RVA: 0x001D7114 File Offset: 0x001D5314
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

		// Token: 0x06005AFF RID: 23295 RVA: 0x0003F221 File Offset: 0x0003D421
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06005B00 RID: 23296 RVA: 0x001D7174 File Offset: 0x001D5374
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

		// Token: 0x04003D1F RID: 15647
		public List<string> inSignals = new List<string>();

		// Token: 0x04003D20 RID: 15648
		public List<string> outSignals = new List<string>();
	}
}
