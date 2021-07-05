using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001622 RID: 5666
	public class QuestNode_MultiplyRange : QuestNode
	{
		// Token: 0x060084CC RID: 33996 RVA: 0x002FB947 File Offset: 0x002F9B47
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060084CD RID: 33997 RVA: 0x002FB960 File Offset: 0x002F9B60
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<FloatRange>(this.storeAs.GetValue(slate), this.range.GetValue(slate) * this.value.GetValue(slate), false);
		}

		// Token: 0x0400527E RID: 21118
		public SlateRef<FloatRange> range;

		// Token: 0x0400527F RID: 21119
		public SlateRef<float> value;

		// Token: 0x04005280 RID: 21120
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
