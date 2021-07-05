using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200168B RID: 5771
	public class QuestNode_GetRandomInRangeInt : QuestNode
	{
		// Token: 0x06008639 RID: 34361 RVA: 0x00302200 File Offset: 0x00300400
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x0600863A RID: 34362 RVA: 0x00302238 File Offset: 0x00300438
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x04005408 RID: 21512
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005409 RID: 21513
		public SlateRef<IntRange> range;
	}
}
