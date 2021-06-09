using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F51 RID: 8017
	public class QuestNode_GetRandomInRangeInt : QuestNode
	{
		// Token: 0x0600AB18 RID: 43800 RVA: 0x0031E198 File Offset: 0x0031C398
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x0600AB19 RID: 43801 RVA: 0x0031E1D0 File Offset: 0x0031C3D0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x0400746F RID: 29807
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007470 RID: 29808
		public SlateRef<IntRange> range;
	}
}
