using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001689 RID: 5769
	public class QuestNode_GetRandomInRangeFloat : QuestNode
	{
		// Token: 0x06008632 RID: 34354 RVA: 0x003020D4 File Offset: 0x003002D4
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x06008633 RID: 34355 RVA: 0x0030210C File Offset: 0x0030030C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x04005401 RID: 21505
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005402 RID: 21506
		public SlateRef<FloatRange> range;
	}
}
