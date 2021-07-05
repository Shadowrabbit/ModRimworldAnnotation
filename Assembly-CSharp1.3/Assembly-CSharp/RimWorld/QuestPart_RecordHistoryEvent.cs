using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B91 RID: 2961
	public class QuestPart_RecordHistoryEvent : QuestPart
	{
		// Token: 0x0600453A RID: 17722 RVA: 0x0016EF82 File Offset: 0x0016D182
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(this.historyEvent), true);
			}
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x0016EFB4 File Offset: 0x0016D1B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
		}

		// Token: 0x04002A1A RID: 10778
		public string inSignal;

		// Token: 0x04002A1B RID: 10779
		public HistoryEventDef historyEvent;
	}
}
