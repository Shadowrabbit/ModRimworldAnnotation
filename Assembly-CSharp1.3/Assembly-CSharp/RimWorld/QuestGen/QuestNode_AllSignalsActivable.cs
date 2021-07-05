using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001637 RID: 5687
	public class QuestNode_AllSignalsActivable : QuestNode
	{
		// Token: 0x06008509 RID: 34057 RVA: 0x002FC704 File Offset: 0x002FA904
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600850A RID: 34058 RVA: 0x002FC71C File Offset: 0x002FA91C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if ((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + this.node == null)
			{
				return;
			}
			QuestPart_PassAllActivable questPart_PassAllActivable = new QuestPart_PassAllActivable();
			QuestGen.quest.AddPart(questPart_PassAllActivable);
			questPart_PassAllActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassAllActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_PassAllActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassAllActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal2 in value)
				{
					questPart_PassAllActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
				}
			}
			questPart_PassAllActivable.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x040052C1 RID: 21185
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040052C2 RID: 21186
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040052C3 RID: 21187
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x040052C4 RID: 21188
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x040052C5 RID: 21189
		public QuestNode node;

		// Token: 0x040052C6 RID: 21190
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
