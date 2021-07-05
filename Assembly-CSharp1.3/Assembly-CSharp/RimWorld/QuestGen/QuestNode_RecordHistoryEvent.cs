using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D8 RID: 5848
	public class QuestNode_RecordHistoryEvent : QuestNode
	{
		// Token: 0x06008746 RID: 34630 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008747 RID: 34631 RVA: 0x00306CD4 File Offset: 0x00304ED4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			HistoryEventDef value = this.historyDef.GetValue(slate);
			if (value == null)
			{
				return;
			}
			quest.AddPart(new QuestPart_RecordHistoryEvent
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false)),
				historyEvent = value
			});
		}

		// Token: 0x04005561 RID: 21857
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005562 RID: 21858
		public SlateRef<HistoryEventDef> historyDef;
	}
}
