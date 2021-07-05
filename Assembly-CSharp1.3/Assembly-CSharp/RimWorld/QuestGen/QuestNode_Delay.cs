using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200163C RID: 5692
	public class QuestNode_Delay : QuestNode
	{
		// Token: 0x06008519 RID: 34073 RVA: 0x002FCE50 File Offset: 0x002FB050
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600851A RID: 34074 RVA: 0x002FCE68 File Offset: 0x002FB068
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Delay questPart_Delay;
			if (this.delayTicksRange.GetValue(slate) != null)
			{
				questPart_Delay = new QuestPart_DelayRandom();
				((QuestPart_DelayRandom)questPart_Delay).delayTicksRange = this.delayTicksRange.GetValue(slate).Value;
			}
			else
			{
				questPart_Delay = this.MakeDelayQuestPart();
				questPart_Delay.delayTicks = this.delayTicks.GetValue(slate);
			}
			questPart_Delay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Delay.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_Delay.reactivatable = this.reactivatable.GetValue(slate);
			if (!this.inspectStringTargets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				questPart_Delay.inspectString = this.inspectString.GetValue(slate);
				questPart_Delay.inspectStringTargets = new List<ISelectable>();
				questPart_Delay.inspectStringTargets.AddRange(this.inspectStringTargets.GetValue(slate));
			}
			if (this.isQuestTimeout.GetValue(slate))
			{
				questPart_Delay.isBad = true;
				questPart_Delay.expiryInfoPart = "QuestExpiresIn".Translate();
				questPart_Delay.expiryInfoPartTip = "QuestExpiresOn".Translate();
			}
			else
			{
				questPart_Delay.expiryInfoPart = this.expiryInfoPart.GetValue(slate);
				questPart_Delay.expiryInfoPartTip = this.expiryInfoPartTip.GetValue(slate);
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_Delay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_Delay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_Delay);
		}

		// Token: 0x0600851B RID: 34075 RVA: 0x002FD020 File Offset: 0x002FB220
		protected virtual QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_Delay();
		}

		// Token: 0x040052D9 RID: 21209
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040052DA RID: 21210
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040052DB RID: 21211
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x040052DC RID: 21212
		public SlateRef<string> expiryInfoPart;

		// Token: 0x040052DD RID: 21213
		public SlateRef<string> expiryInfoPartTip;

		// Token: 0x040052DE RID: 21214
		public SlateRef<string> inspectString;

		// Token: 0x040052DF RID: 21215
		public SlateRef<IEnumerable<ISelectable>> inspectStringTargets;

		// Token: 0x040052E0 RID: 21216
		public SlateRef<int> delayTicks;

		// Token: 0x040052E1 RID: 21217
		public SlateRef<IntRange?> delayTicksRange;

		// Token: 0x040052E2 RID: 21218
		public SlateRef<bool> isQuestTimeout;

		// Token: 0x040052E3 RID: 21219
		public SlateRef<bool> reactivatable;

		// Token: 0x040052E4 RID: 21220
		public QuestNode node;
	}
}
