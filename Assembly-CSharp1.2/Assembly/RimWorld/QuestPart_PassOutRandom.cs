using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001051 RID: 4177
	public class QuestPart_PassOutRandom : QuestPart
	{
		// Token: 0x06005B06 RID: 23302 RVA: 0x001D72A0 File Offset: 0x001D54A0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			string tag;
			if (signal.tag == this.inSignal && this.outSignals.TryRandomElement(out tag))
			{
				Find.SignalManager.SendSignal(new Signal(tag, signal.args));
			}
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x0003F2B6 File Offset: 0x0003D4B6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x001D72EC File Offset: 0x001D54EC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x04003D23 RID: 15651
		public string inSignal;

		// Token: 0x04003D24 RID: 15652
		public List<string> outSignals = new List<string>();
	}
}
