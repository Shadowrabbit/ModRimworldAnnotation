using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1B RID: 2843
	public class QuestPart_PassAny : QuestPart
	{
		// Token: 0x060042D8 RID: 17112 RVA: 0x00165A8D File Offset: 0x00163C8D
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args));
			}
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x00165AC4 File Offset: 0x00163CC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x00165AF4 File Offset: 0x00163CF4
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

		// Token: 0x040028B2 RID: 10418
		public List<string> inSignals = new List<string>();

		// Token: 0x040028B3 RID: 10419
		public string outSignal;
	}
}
