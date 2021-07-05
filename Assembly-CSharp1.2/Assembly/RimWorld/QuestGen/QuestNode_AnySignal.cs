using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF3 RID: 7923
	public class QuestNode_AnySignal : QuestNode
	{
		// Token: 0x0600A9D2 RID: 43474 RVA: 0x0006F71B File Offset: 0x0006D91B
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600A9D3 RID: 43475 RVA: 0x003197F0 File Offset: 0x003179F0
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
				QuestPart_PassAny questPart_PassAny = new QuestPart_PassAny();
				foreach (string signal in this.inSignals.GetValue(slate))
				{
					questPart_PassAny.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
				if (this.node != null)
				{
					questPart_PassAny.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInnerNode(this.node, questPart_PassAny.outSignal);
				}
				else
				{
					questPart_PassAny.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignals.GetValue(slate).First<string>());
				}
				questPart_PassAny.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
				QuestGen.quest.AddPart(questPart_PassAny);
				return;
			}
			QuestPart_PassAnyOutMany questPart_PassAnyOutMany = new QuestPart_PassAnyOutMany();
			foreach (string signal2 in this.inSignals.GetValue(slate))
			{
				questPart_PassAnyOutMany.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
			}
			if (this.node != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassAnyOutMany.outSignals.Add(text);
				QuestGenUtility.RunInnerNode(this.node, text);
			}
			foreach (string signal3 in this.outSignals.GetValue(slate))
			{
				questPart_PassAnyOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal3));
			}
			questPart_PassAnyOutMany.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.quest.AddPart(questPart_PassAnyOutMany);
		}

		// Token: 0x04007318 RID: 29464
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x04007319 RID: 29465
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x0400731A RID: 29466
		public QuestNode node;

		// Token: 0x0400731B RID: 29467
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x0400731C RID: 29468
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
