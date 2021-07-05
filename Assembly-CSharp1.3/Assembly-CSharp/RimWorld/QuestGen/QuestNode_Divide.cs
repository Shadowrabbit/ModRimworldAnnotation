using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001620 RID: 5664
	public class QuestNode_Divide : QuestNode
	{
		// Token: 0x060084C6 RID: 33990 RVA: 0x002FB897 File Offset: 0x002F9A97
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060084C7 RID: 33991 RVA: 0x002FB8B0 File Offset: 0x002F9AB0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) / this.value2.GetValue(slate), false);
		}

		// Token: 0x04005278 RID: 21112
		public SlateRef<double> value1;

		// Token: 0x04005279 RID: 21113
		public SlateRef<double> value2;

		// Token: 0x0400527A RID: 21114
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
