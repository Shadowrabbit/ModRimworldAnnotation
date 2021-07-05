using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC2 RID: 3010
	public class QuestPart_GiveTechprints : QuestPart
	{
		// Token: 0x06004678 RID: 18040 RVA: 0x00174BE0 File Offset: 0x00172DE0
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

		// Token: 0x06004679 RID: 18041 RVA: 0x00174C2C File Offset: 0x00172E2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalWasGiven, "outSignalWasGiven", null, false);
		}

		// Token: 0x04002AFA RID: 11002
		public const string WasGivenSignal = "AddedTechprints";

		// Token: 0x04002AFB RID: 11003
		public string inSignal;

		// Token: 0x04002AFC RID: 11004
		public string outSignalWasGiven;

		// Token: 0x04002AFD RID: 11005
		public ResearchProjectDef project;

		// Token: 0x04002AFE RID: 11006
		public int amount;
	}
}
