using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EDD RID: 7901
	public class QuestNode_Multiply : QuestNode
	{
		// Token: 0x0600A992 RID: 43410 RVA: 0x0006F5BE File Offset: 0x0006D7BE
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600A993 RID: 43411 RVA: 0x00318B58 File Offset: 0x00316D58
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) * this.value2.GetValue(slate), false);
		}

		// Token: 0x040072D2 RID: 29394
		public SlateRef<double> value1;

		// Token: 0x040072D3 RID: 29395
		public SlateRef<double> value2;

		// Token: 0x040072D4 RID: 29396
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
