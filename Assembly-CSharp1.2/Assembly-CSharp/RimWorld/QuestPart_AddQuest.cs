using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200109E RID: 4254
	public abstract class QuestPart_AddQuest : QuestPart
	{
		// Token: 0x06005CC6 RID: 23750 RVA: 0x000405C6 File Offset: 0x0003E7C6
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestUtility.GenerateQuestAndMakeAvailable(this.questDef, this.GetSlate()).Accept(this.acceptee);
			}
		}

		// Token: 0x06005CC7 RID: 23751
		public abstract Slate GetSlate();

		// Token: 0x06005CC8 RID: 23752 RVA: 0x000405FE File Offset: 0x0003E7FE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.acceptee, "acceptee", false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.questDef, "questToAdd");
		}

		// Token: 0x04003E1D RID: 15901
		public string inSignal;

		// Token: 0x04003E1E RID: 15902
		public Pawn acceptee;

		// Token: 0x04003E1F RID: 15903
		public QuestScriptDef questDef;
	}
}
