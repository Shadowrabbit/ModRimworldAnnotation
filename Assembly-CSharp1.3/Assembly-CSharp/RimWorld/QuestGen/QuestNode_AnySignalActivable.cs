using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001639 RID: 5689
	public class QuestNode_AnySignalActivable : QuestNode
	{
		// Token: 0x0600850F RID: 34063 RVA: 0x002FCADC File Offset: 0x002FACDC
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008510 RID: 34064 RVA: 0x002FCAF4 File Offset: 0x002FACF4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if ((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + this.node == null)
			{
				return;
			}
			QuestPart_PassAnyActivable questPart_PassAnyActivable = new QuestPart_PassAnyActivable();
			QuestGen.quest.AddPart(questPart_PassAnyActivable);
			questPart_PassAnyActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassAnyActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_PassAnyActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassAnyActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal2 in value)
				{
					questPart_PassAnyActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
				}
			}
			questPart_PassAnyActivable.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x040052CC RID: 21196
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040052CD RID: 21197
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040052CE RID: 21198
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x040052CF RID: 21199
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x040052D0 RID: 21200
		public QuestNode node;

		// Token: 0x040052D1 RID: 21201
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
