using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1A RID: 2842
	public class QuestPart_PassAllSequence : QuestPart
	{
		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x060042D3 RID: 17107 RVA: 0x00165949 File Offset: 0x00163B49
		private bool AllSignalsReceived
		{
			get
			{
				return this.ptr >= this.inSignals.Count - 1;
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x00165964 File Offset: 0x00163B64
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

		// Token: 0x060042D5 RID: 17109 RVA: 0x001659C0 File Offset: 0x00163BC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<int>(ref this.ptr, "ptr", 0, false);
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x00165A10 File Offset: 0x00163C10
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

		// Token: 0x040028AF RID: 10415
		public List<string> inSignals = new List<string>();

		// Token: 0x040028B0 RID: 10416
		public string outSignal;

		// Token: 0x040028B1 RID: 10417
		private int ptr = -1;
	}
}
