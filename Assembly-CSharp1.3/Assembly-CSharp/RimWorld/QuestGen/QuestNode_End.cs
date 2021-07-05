using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200163D RID: 5693
	public class QuestNode_End : QuestNode
	{
		// Token: 0x0600851D RID: 34077 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600851E RID: 34078 RVA: 0x002FD028 File Offset: 0x002FB228
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int value = this.goodwillChangeAmount.GetValue(slate);
			Thing value2 = this.goodwillChangeFactionOf.GetValue(slate);
			if (value != 0 && value2 != null && value2.Faction != null)
			{
				QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
				questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
				questPart_FactionGoodwillChange.faction = value2.Faction;
				questPart_FactionGoodwillChange.change = value;
				questPart_FactionGoodwillChange.historyEvent = this.goodwillChangeReason.GetValue(slate);
				slate.Set<string>("goodwillPenalty", Mathf.Abs(value).ToString(), false);
				QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
			}
			QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
			questPart_QuestEnd.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_QuestEnd.outcome = new QuestEndOutcome?(this.outcome.GetValue(slate));
			questPart_QuestEnd.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			questPart_QuestEnd.sendLetter = (this.sendStandardLetter.GetValue(slate) ?? false);
			QuestGen.quest.AddPart(questPart_QuestEnd);
		}

		// Token: 0x040052E5 RID: 21221
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040052E6 RID: 21222
		public SlateRef<QuestEndOutcome> outcome;

		// Token: 0x040052E7 RID: 21223
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x040052E8 RID: 21224
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x040052E9 RID: 21225
		public SlateRef<int> goodwillChangeAmount;

		// Token: 0x040052EA RID: 21226
		public SlateRef<Thing> goodwillChangeFactionOf;

		// Token: 0x040052EB RID: 21227
		public SlateRef<HistoryEventDef> goodwillChangeReason;
	}
}
