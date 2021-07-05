using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B21 RID: 2849
	public class QuestPart_PassWhileActive : QuestPartActivable
	{
		// Token: 0x060042F1 RID: 17137 RVA: 0x001660D7 File Offset: 0x001642D7
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (signal.tag == this.inSignal)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args));
			}
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x0016610E File Offset: 0x0016430E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x040028BF RID: 10431
		public string inSignal;

		// Token: 0x040028C0 RID: 10432
		public string outSignal;
	}
}
