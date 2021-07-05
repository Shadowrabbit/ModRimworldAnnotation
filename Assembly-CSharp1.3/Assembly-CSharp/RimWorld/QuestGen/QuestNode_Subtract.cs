using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001623 RID: 5667
	public class QuestNode_Subtract : QuestNode
	{
		// Token: 0x060084CF RID: 33999 RVA: 0x002FB9A3 File Offset: 0x002F9BA3
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060084D0 RID: 34000 RVA: 0x002FB9BC File Offset: 0x002F9BBC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) - this.value2.GetValue(slate), false);
		}

		// Token: 0x04005281 RID: 21121
		public SlateRef<double> value1;

		// Token: 0x04005282 RID: 21122
		public SlateRef<double> value2;

		// Token: 0x04005283 RID: 21123
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
