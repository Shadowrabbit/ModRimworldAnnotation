using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001645 RID: 5701
	public class QuestNode_Signal : QuestNode
	{
		// Token: 0x06008536 RID: 34102 RVA: 0x002FD708 File Offset: 0x002FB908
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008537 RID: 34103 RVA: 0x002FD720 File Offset: 0x002FB920
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int num = ((this.outSignals.GetValue(slate) != null) ? this.outSignals.GetValue(slate).Count<string>() : 0) + ((this.node != null) ? 1 : 0);
			if (num == 0)
			{
				return;
			}
			if (num == 1)
			{
				QuestPart_Pass questPart_Pass = new QuestPart_Pass();
				questPart_Pass.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
				if (this.node != null)
				{
					questPart_Pass.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInnerNode(this.node, questPart_Pass.outSignal);
				}
				else
				{
					questPart_Pass.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignals.GetValue(slate).First<string>());
				}
				questPart_Pass.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
				QuestGen.quest.AddPart(questPart_Pass);
				return;
			}
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			if (this.node != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassOutMany.outSignals.Add(text);
				QuestGenUtility.RunInnerNode(this.node, text);
			}
			foreach (string signal in this.outSignals.GetValue(slate))
			{
				questPart_PassOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			questPart_PassOutMany.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x040052FE RID: 21246
		[NoTranslate]
		[TranslationHandle(Priority = 100)]
		public SlateRef<string> inSignal;

		// Token: 0x040052FF RID: 21247
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x04005300 RID: 21248
		public QuestNode node;

		// Token: 0x04005301 RID: 21249
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04005302 RID: 21250
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
