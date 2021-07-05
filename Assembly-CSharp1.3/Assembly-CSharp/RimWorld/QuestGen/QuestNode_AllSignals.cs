using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001636 RID: 5686
	public class QuestNode_AllSignals : QuestNode
	{
		// Token: 0x06008506 RID: 34054 RVA: 0x002FC4BD File Offset: 0x002FA6BD
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008507 RID: 34055 RVA: 0x002FC4D8 File Offset: 0x002FA6D8
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
				QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
				foreach (string signal in this.inSignals.GetValue(slate))
				{
					questPart_PassAll.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
				if (this.node != null)
				{
					questPart_PassAll.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInnerNode(this.node, questPart_PassAll.outSignal);
				}
				else
				{
					questPart_PassAll.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignals.GetValue(slate).First<string>());
				}
				questPart_PassAll.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
				QuestGen.quest.AddPart(questPart_PassAll);
				return;
			}
			QuestPart_PassAllOutMany questPart_PassAllOutMany = new QuestPart_PassAllOutMany();
			foreach (string signal2 in this.inSignals.GetValue(slate))
			{
				questPart_PassAllOutMany.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
			}
			if (this.node != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassAllOutMany.outSignals.Add(text);
				QuestGenUtility.RunInnerNode(this.node, text);
			}
			foreach (string signal3 in this.outSignals.GetValue(slate))
			{
				questPart_PassAllOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal3));
			}
			questPart_PassAllOutMany.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.quest.AddPart(questPart_PassAllOutMany);
		}

		// Token: 0x040052BC RID: 21180
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x040052BD RID: 21181
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x040052BE RID: 21182
		public QuestNode node;

		// Token: 0x040052BF RID: 21183
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x040052C0 RID: 21184
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
