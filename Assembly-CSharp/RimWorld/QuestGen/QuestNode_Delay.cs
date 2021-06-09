using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF6 RID: 7926
	public class QuestNode_Delay : QuestNode
	{
		// Token: 0x0600A9DC RID: 43484 RVA: 0x0006F74A File Offset: 0x0006D94A
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600A9DD RID: 43485 RVA: 0x00319BE4 File Offset: 0x00317DE4
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

		// Token: 0x0600A9DE RID: 43486 RVA: 0x0006F762 File Offset: 0x0006D962
		protected virtual QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_Delay();
		}

		// Token: 0x04007324 RID: 29476
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04007325 RID: 29477
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04007326 RID: 29478
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04007327 RID: 29479
		public SlateRef<string> expiryInfoPart;

		// Token: 0x04007328 RID: 29480
		public SlateRef<string> expiryInfoPartTip;

		// Token: 0x04007329 RID: 29481
		public SlateRef<string> inspectString;

		// Token: 0x0400732A RID: 29482
		public SlateRef<IEnumerable<ISelectable>> inspectStringTargets;

		// Token: 0x0400732B RID: 29483
		public SlateRef<int> delayTicks;

		// Token: 0x0400732C RID: 29484
		public SlateRef<IntRange?> delayTicksRange;

		// Token: 0x0400732D RID: 29485
		public SlateRef<bool> isQuestTimeout;

		// Token: 0x0400732E RID: 29486
		public SlateRef<bool> reactivatable;

		// Token: 0x0400732F RID: 29487
		public QuestNode node;
	}
}
