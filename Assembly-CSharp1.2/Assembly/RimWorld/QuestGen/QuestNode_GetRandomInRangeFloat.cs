using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F4F RID: 8015
	public class QuestNode_GetRandomInRangeFloat : QuestNode
	{
		// Token: 0x0600AB11 RID: 43793 RVA: 0x0031E084 File Offset: 0x0031C284
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x0600AB12 RID: 43794 RVA: 0x0031E0BC File Offset: 0x0031C2BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x04007468 RID: 29800
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007469 RID: 29801
		public SlateRef<FloatRange> range;
	}
}
