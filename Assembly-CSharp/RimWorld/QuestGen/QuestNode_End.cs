using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF7 RID: 7927
	public class QuestNode_End : QuestNode
	{
		// Token: 0x0600A9E0 RID: 43488 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600A9E1 RID: 43489 RVA: 0x00319D9C File Offset: 0x00317F9C
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

		// Token: 0x04007330 RID: 29488
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007331 RID: 29489
		public SlateRef<QuestEndOutcome> outcome;

		// Token: 0x04007332 RID: 29490
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04007333 RID: 29491
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04007334 RID: 29492
		public SlateRef<int> goodwillChangeAmount;

		// Token: 0x04007335 RID: 29493
		public SlateRef<Thing> goodwillChangeFactionOf;
	}
}
