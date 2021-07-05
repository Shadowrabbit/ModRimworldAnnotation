using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B11 RID: 2833
	public class QuestPart_SetQuestNotYetAccepted : QuestPart
	{
		// Token: 0x0600429F RID: 17055 RVA: 0x00164D6E File Offset: 0x00162F6E
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.quest.SetNotYetAccepted();
			}
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x00164D98 File Offset: 0x00162F98
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.revertOngoingQuestAfterLoad, "revertOngoingQuestAfterLoad", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.revertOngoingQuestAfterLoad && this.quest.State == QuestState.Ongoing)
			{
				this.quest.SetNotYetAccepted();
			}
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x00164DF8 File Offset: 0x00162FF8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002894 RID: 10388
		public string inSignal;

		// Token: 0x04002895 RID: 10389
		public bool revertOngoingQuestAfterLoad;
	}
}
