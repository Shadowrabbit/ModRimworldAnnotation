using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104D RID: 4173
	public class QuestPart_PassAny : QuestPart
	{
		// Token: 0x06005AF6 RID: 23286 RVA: 0x0003F154 File Offset: 0x0003D354
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args));
			}
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x0003F18B File Offset: 0x0003D38B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x001D7064 File Offset: 0x001D5264
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

		// Token: 0x04003D1C RID: 15644
		public List<string> inSignals = new List<string>();

		// Token: 0x04003D1D RID: 15645
		public string outSignal;
	}
}
