using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B24 RID: 2852
	public abstract class QuestPart_Filter : QuestPart
	{
		// Token: 0x060042FC RID: 17148
		protected abstract bool Pass(SignalArgs args);

		// Token: 0x060042FD RID: 17149 RVA: 0x001662D8 File Offset: 0x001644D8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				if (this.Pass(signal.args))
				{
					if (!this.outSignal.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args));
						return;
					}
				}
				else if (!this.outSignalElse.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalElse, signal.args));
				}
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x0016635E File Offset: 0x0016455E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalElse, "outSignalElse", null, false);
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x0016639C File Offset: 0x0016459C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x040028C5 RID: 10437
		public string inSignal;

		// Token: 0x040028C6 RID: 10438
		public string outSignal;

		// Token: 0x040028C7 RID: 10439
		public string outSignalElse;
	}
}
