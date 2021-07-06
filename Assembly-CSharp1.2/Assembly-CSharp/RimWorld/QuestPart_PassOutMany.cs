using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001050 RID: 4176
	public class QuestPart_PassOutMany : QuestPart
	{
		// Token: 0x06005B02 RID: 23298 RVA: 0x001D71E8 File Offset: 0x001D53E8
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

		// Token: 0x06005B03 RID: 23299 RVA: 0x0003F273 File Offset: 0x0003D473
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06005B04 RID: 23300 RVA: 0x001D7248 File Offset: 0x001D5448
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x04003D21 RID: 15649
		public string inSignal;

		// Token: 0x04003D22 RID: 15650
		public List<string> outSignals = new List<string>();
	}
}
