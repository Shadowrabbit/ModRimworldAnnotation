using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001646 RID: 5702
	public class QuestNode_SignalActivable : QuestNode
	{
		// Token: 0x06008539 RID: 34105 RVA: 0x002FD8E0 File Offset: 0x002FBAE0
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600853A RID: 34106 RVA: 0x002FD8F8 File Offset: 0x002FBAF8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if ((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + this.node == null)
			{
				return;
			}
			QuestPart_PassActivable questPart_PassActivable = new QuestPart_PassActivable();
			QuestGen.quest.AddPart(questPart_PassActivable);
			questPart_PassActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_PassActivable.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal in value)
				{
					questPart_PassActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
			}
			questPart_PassActivable.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x04005303 RID: 21251
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005304 RID: 21252
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04005305 RID: 21253
		[NoTranslate]
		[TranslationHandle(Priority = 100)]
		public SlateRef<string> inSignal;

		// Token: 0x04005306 RID: 21254
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x04005307 RID: 21255
		public QuestNode node;

		// Token: 0x04005308 RID: 21256
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
