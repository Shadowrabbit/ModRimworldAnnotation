using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EDE RID: 7902
	public class QuestNode_MultiplyRange : QuestNode
	{
		// Token: 0x0600A995 RID: 43413 RVA: 0x0006F5D4 File Offset: 0x0006D7D4
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600A996 RID: 43414 RVA: 0x00318B98 File Offset: 0x00316D98
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<FloatRange>(this.storeAs.GetValue(slate), this.range.GetValue(slate) * this.value.GetValue(slate), false);
		}

		// Token: 0x040072D5 RID: 29397
		public SlateRef<FloatRange> range;

		// Token: 0x040072D6 RID: 29398
		public SlateRef<float> value;

		// Token: 0x040072D7 RID: 29399
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
