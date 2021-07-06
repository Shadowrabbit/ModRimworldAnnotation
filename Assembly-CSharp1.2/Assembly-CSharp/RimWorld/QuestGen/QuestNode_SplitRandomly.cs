using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F69 RID: 8041
	public class QuestNode_SplitRandomly : QuestNode
	{
		// Token: 0x0600AB68 RID: 43880 RVA: 0x000701C6 File Offset: 0x0006E3C6
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600AB69 RID: 43881 RVA: 0x000701D0 File Offset: 0x0006E3D0
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AB6A RID: 43882 RVA: 0x0031F4B4 File Offset: 0x0031D6B4
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

		// Token: 0x040074BA RID: 29882
		[NoTranslate]
		public SlateRef<string> storeAsFormat;

		// Token: 0x040074BB RID: 29883
		[NoTranslate]
		public SlateRef<string> storeAs1;

		// Token: 0x040074BC RID: 29884
		[NoTranslate]
		public SlateRef<string> storeAs2;

		// Token: 0x040074BD RID: 29885
		[NoTranslate]
		public SlateRef<string> storeAs3;

		// Token: 0x040074BE RID: 29886
		[NoTranslate]
		public SlateRef<string> storeAs4;

		// Token: 0x040074BF RID: 29887
		[NoTranslate]
		public SlateRef<string> storeAs5;

		// Token: 0x040074C0 RID: 29888
		public SlateRef<float?> valueToSplit;

		// Token: 0x040074C1 RID: 29889
		public SlateRef<int> countToSplit;

		// Token: 0x040074C2 RID: 29890
		public SlateRef<float> zeroIfFractionBelow1;

		// Token: 0x040074C3 RID: 29891
		public SlateRef<float> zeroIfFractionBelow2;

		// Token: 0x040074C4 RID: 29892
		public SlateRef<float> zeroIfFractionBelow3;

		// Token: 0x040074C5 RID: 29893
		public SlateRef<float> zeroIfFractionBelow4;

		// Token: 0x040074C6 RID: 29894
		public SlateRef<float> zeroIfFractionBelow5;

		// Token: 0x040074C7 RID: 29895
		public SlateRef<float> minFraction1;

		// Token: 0x040074C8 RID: 29896
		public SlateRef<float> minFraction2;

		// Token: 0x040074C9 RID: 29897
		public SlateRef<float> minFraction3;

		// Token: 0x040074CA RID: 29898
		public SlateRef<float> minFraction4;

		// Token: 0x040074CB RID: 29899
		public SlateRef<float> minFraction5;

		// Token: 0x040074CC RID: 29900
		private static List<float> tmpValues = new List<float>();

		// Token: 0x040074CD RID: 29901
		private static List<float> tmpZeroIfFractionBelow = new List<float>();

		// Token: 0x040074CE RID: 29902
		private static List<float> tmpMinFractions = new List<float>();
	}
}
