using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B20 RID: 2848
	public class QuestPart_PassOutRandom : QuestPart
	{
		// Token: 0x060042ED RID: 17133 RVA: 0x00165FF0 File Offset: 0x001641F0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			string tag;
			if (signal.tag == this.inSignal && this.outSignals.TryRandomElement(out tag))
			{
				Find.SignalManager.SendSignal(new Signal(tag, signal.args));
			}
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x0016603C File Offset: 0x0016423C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x0016606C File Offset: 0x0016426C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x040028BD RID: 10429
		public string inSignal;

		// Token: 0x040028BE RID: 10430
		public List<string> outSignals = new List<string>();
	}
}
