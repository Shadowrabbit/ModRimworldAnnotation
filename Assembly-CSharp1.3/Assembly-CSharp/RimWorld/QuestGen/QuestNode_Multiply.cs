using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001621 RID: 5665
	public class QuestNode_Multiply : QuestNode
	{
		// Token: 0x060084C9 RID: 33993 RVA: 0x002FB8EF File Offset: 0x002F9AEF
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x060084CA RID: 33994 RVA: 0x002FB908 File Offset: 0x002F9B08
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) * this.value2.GetValue(slate), false);
		}

		// Token: 0x0400527B RID: 21115
		public SlateRef<double> value1;

		// Token: 0x0400527C RID: 21116
		public SlateRef<double> value2;

		// Token: 0x0400527D RID: 21117
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
