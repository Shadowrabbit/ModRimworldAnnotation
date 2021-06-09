using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104C RID: 4172
	public class QuestPart_PassAllSequence : QuestPart
	{
		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06005AF1 RID: 23281 RVA: 0x0003F120 File Offset: 0x0003D320
		private bool AllSignalsReceived
		{
			get
			{
				return this.ptr >= this.inSignals.Count - 1;
			}
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x001D6F54 File Offset: 0x001D5154
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!this.AllSignalsReceived && this.inSignals.IndexOf(signal.tag) == this.ptr + 1)
			{
				this.ptr++;
				if (this.AllSignalsReceived)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal));
				}
			}
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x001D6FB0 File Offset: 0x001D51B0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<int>(ref this.ptr, "ptr", 0, false);
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x001D7000 File Offset: 0x001D5200
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003D19 RID: 15641
		public List<string> inSignals = new List<string>();

		// Token: 0x04003D1A RID: 15642
		public string outSignal;

		// Token: 0x04003D1B RID: 15643
		private int ptr = -1;
	}
}
