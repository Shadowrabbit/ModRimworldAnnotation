using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001054 RID: 4180
	public abstract class QuestPart_Filter : QuestPart
	{
		// Token: 0x06005B12 RID: 23314
		protected abstract bool Pass(SignalArgs args);

		// Token: 0x06005B13 RID: 23315 RVA: 0x001D748C File Offset: 0x001D568C
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

		// Token: 0x06005B14 RID: 23316 RVA: 0x0003F34E File Offset: 0x0003D54E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalElse, "outSignalElse", null, false);
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x0003F38C File Offset: 0x0003D58C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003D29 RID: 15657
		public string inSignal;

		// Token: 0x04003D2A RID: 15658
		public string outSignal;

		// Token: 0x04003D2B RID: 15659
		public string outSignalElse;
	}
}
