using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001142 RID: 4418
	public class QuestPart_GiveTechprints : QuestPart
	{
		// Token: 0x060060E4 RID: 24804 RVA: 0x001E60D8 File Offset: 0x001E42D8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.amount; i++)
				{
					Find.ResearchManager.ApplyTechprint(this.project, null);
				}
			}
		}

		// Token: 0x060060E5 RID: 24805 RVA: 0x001E6124 File Offset: 0x001E4324
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalWasGiven, "outSignalWasGiven", null, false);
		}

		// Token: 0x040040BE RID: 16574
		public const string WasGivenSignal = "AddedTechprints";

		// Token: 0x040040BF RID: 16575
		public string inSignal;

		// Token: 0x040040C0 RID: 16576
		public string outSignalWasGiven;

		// Token: 0x040040C1 RID: 16577
		public ResearchProjectDef project;

		// Token: 0x040040C2 RID: 16578
		public int amount;
	}
}
