using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001695 RID: 5781
	public class QuestNode_SplitRandomly : QuestNode
	{
		// Token: 0x06008665 RID: 34405 RVA: 0x003034BD File Offset: 0x003016BD
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x06008666 RID: 34406 RVA: 0x003034C7 File Offset: 0x003016C7
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x06008667 RID: 34407 RVA: 0x003034D4 File Offset: 0x003016D4
		private void DoWork(Slate slate)
		{
			float num = this.valueToSplit.GetValue(slate) ?? 1f;
			QuestNode_SplitRandomly.tmpMinFractions.Clear();
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction1.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction2.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction3.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction4.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction5.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Clear();
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow1.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow2.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow3.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow4.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow5.GetValue(slate));
			Rand.SplitRandomly(num, this.countToSplit.GetValue(slate), QuestNode_SplitRandomly.tmpValues, QuestNode_SplitRandomly.tmpZeroIfFractionBelow, QuestNode_SplitRandomly.tmpMinFractions);
			for (int i = 0; i < QuestNode_SplitRandomly.tmpValues.Count; i++)
			{
				if (this.storeAsFormat.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAsFormat.GetValue(slate).Formatted(i.Named("INDEX")), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				if (i == 0 && this.storeAs1.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs1.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 1 && this.storeAs2.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs2.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 2 && this.storeAs3.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs3.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 3 && this.storeAs4.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs4.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 4 && this.storeAs5.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs5.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
			}
		}

		// Token: 0x04005431 RID: 21553
		[NoTranslate]
		public SlateRef<string> storeAsFormat;

		// Token: 0x04005432 RID: 21554
		[NoTranslate]
		public SlateRef<string> storeAs1;

		// Token: 0x04005433 RID: 21555
		[NoTranslate]
		public SlateRef<string> storeAs2;

		// Token: 0x04005434 RID: 21556
		[NoTranslate]
		public SlateRef<string> storeAs3;

		// Token: 0x04005435 RID: 21557
		[NoTranslate]
		public SlateRef<string> storeAs4;

		// Token: 0x04005436 RID: 21558
		[NoTranslate]
		public SlateRef<string> storeAs5;

		// Token: 0x04005437 RID: 21559
		public SlateRef<float?> valueToSplit;

		// Token: 0x04005438 RID: 21560
		public SlateRef<int> countToSplit;

		// Token: 0x04005439 RID: 21561
		public SlateRef<float> zeroIfFractionBelow1;

		// Token: 0x0400543A RID: 21562
		public SlateRef<float> zeroIfFractionBelow2;

		// Token: 0x0400543B RID: 21563
		public SlateRef<float> zeroIfFractionBelow3;

		// Token: 0x0400543C RID: 21564
		public SlateRef<float> zeroIfFractionBelow4;

		// Token: 0x0400543D RID: 21565
		public SlateRef<float> zeroIfFractionBelow5;

		// Token: 0x0400543E RID: 21566
		public SlateRef<float> minFraction1;

		// Token: 0x0400543F RID: 21567
		public SlateRef<float> minFraction2;

		// Token: 0x04005440 RID: 21568
		public SlateRef<float> minFraction3;

		// Token: 0x04005441 RID: 21569
		public SlateRef<float> minFraction4;

		// Token: 0x04005442 RID: 21570
		public SlateRef<float> minFraction5;

		// Token: 0x04005443 RID: 21571
		private static List<float> tmpValues = new List<float>();

		// Token: 0x04005444 RID: 21572
		private static List<float> tmpZeroIfFractionBelow = new List<float>();

		// Token: 0x04005445 RID: 21573
		private static List<float> tmpMinFractions = new List<float>();
	}
}
