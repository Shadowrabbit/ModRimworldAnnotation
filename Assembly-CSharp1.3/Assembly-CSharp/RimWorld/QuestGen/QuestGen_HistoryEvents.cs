using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001614 RID: 5652
	public static class QuestGen_HistoryEvents
	{
		// Token: 0x0600846A RID: 33898 RVA: 0x002F7AA8 File Offset: 0x002F5CA8
		public static QuestPart_RecordHistoryEvent RecordHistoryEvent(this Quest quest, HistoryEventDef def, string inSignal = null)
		{
			QuestPart_RecordHistoryEvent questPart_RecordHistoryEvent = new QuestPart_RecordHistoryEvent();
			questPart_RecordHistoryEvent.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RecordHistoryEvent.historyEvent = def;
			quest.AddPart(questPart_RecordHistoryEvent);
			return questPart_RecordHistoryEvent;
		}
	}
}
