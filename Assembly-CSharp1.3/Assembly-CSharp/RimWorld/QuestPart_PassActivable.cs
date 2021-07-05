using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B15 RID: 2837
	public class QuestPart_PassActivable : QuestPartActivable
	{
		// Token: 0x060042BC RID: 17084 RVA: 0x00165341 File Offset: 0x00163541
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (signal.tag == this.inSignal)
			{
				this.Complete(signal.args);
			}
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x00165369 File Offset: 0x00163569
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x00165383 File Offset: 0x00163583
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x040028A4 RID: 10404
		public string inSignal;
	}
}
