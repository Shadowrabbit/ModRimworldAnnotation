using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F02 RID: 7938
	public class QuestNode_SignalActivable : QuestNode
	{
		// Token: 0x0600AA06 RID: 43526 RVA: 0x0006F841 File Offset: 0x0006DA41
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600AA07 RID: 43527 RVA: 0x0031A6A4 File Offset: 0x003188A4
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

		// Token: 0x04007355 RID: 29525
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04007356 RID: 29526
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04007357 RID: 29527
		[NoTranslate]
		[TranslationHandle(Priority = 100)]
		public SlateRef<string> inSignal;

		// Token: 0x04007358 RID: 29528
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x04007359 RID: 29529
		public QuestNode node;

		// Token: 0x0400735A RID: 29530
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
