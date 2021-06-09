using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EDB RID: 7899
	public class QuestNode_Add : QuestNode
	{
		// Token: 0x0600A98C RID: 43404 RVA: 0x0006F58A File Offset: 0x0006D78A
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600A98D RID: 43405 RVA: 0x00318AD8 File Offset: 0x00316CD8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) + this.value2.GetValue(slate), false);
		}

		// Token: 0x040072CC RID: 29388
		public SlateRef<double> value1;

		// Token: 0x040072CD RID: 29389
		public SlateRef<double> value2;

		// Token: 0x040072CE RID: 29390
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
