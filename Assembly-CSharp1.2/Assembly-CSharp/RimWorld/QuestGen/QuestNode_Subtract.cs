using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EDF RID: 7903
	public class QuestNode_Subtract : QuestNode
	{
		// Token: 0x0600A998 RID: 43416 RVA: 0x0006F5EA File Offset: 0x0006D7EA
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600A999 RID: 43417 RVA: 0x00318BDC File Offset: 0x00316DDC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) - this.value2.GetValue(slate), false);
		}

		// Token: 0x040072D8 RID: 29400
		public SlateRef<double> value1;

		// Token: 0x040072D9 RID: 29401
		public SlateRef<double> value2;

		// Token: 0x040072DA RID: 29402
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
