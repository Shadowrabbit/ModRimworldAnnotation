using System;
using UnityEngine;

namespace RimWorld.QuestGen
{
	// Token: 0x02001610 RID: 5648
	public static class QuestGen_End
	{
		// Token: 0x06008454 RID: 33876 RVA: 0x002F6F14 File Offset: 0x002F5114
		public static QuestPart_QuestEnd End(this Quest quest, QuestEndOutcome outcome, int goodwillChangeAmount = 0, Faction goodwillChangeFactionOf = null, string inSignal = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly, bool sendStandardLetter = false)
		{
			Slate slate = QuestGen.slate;
			if (goodwillChangeAmount != 0 && goodwillChangeFactionOf != null && goodwillChangeFactionOf != null)
			{
				QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
				questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
				questPart_FactionGoodwillChange.faction = goodwillChangeFactionOf;
				questPart_FactionGoodwillChange.change = goodwillChangeAmount;
				slate.Set<string>("goodwillPenalty", Mathf.Abs(goodwillChangeAmount).ToString(), false);
				QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
			}
			QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
			questPart_QuestEnd.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_QuestEnd.outcome = new QuestEndOutcome?(outcome);
			questPart_QuestEnd.signalListenMode = signalListenMode;
			questPart_QuestEnd.sendLetter = sendStandardLetter;
			QuestGen.quest.AddPart(questPart_QuestEnd);
			return questPart_QuestEnd;
		}
	}
}
