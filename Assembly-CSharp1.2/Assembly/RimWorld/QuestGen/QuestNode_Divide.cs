using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EDC RID: 7900
	public class QuestNode_Divide : QuestNode
	{
		// Token: 0x0600A98F RID: 43407 RVA: 0x0006F5A8 File Offset: 0x0006D7A8
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600A990 RID: 43408 RVA: 0x00318B18 File Offset: 0x00316D18
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) / this.value2.GetValue(slate), false);
		}

		// Token: 0x040072CF RID: 29391
		public SlateRef<double> value1;

		// Token: 0x040072D0 RID: 29392
		public SlateRef<double> value2;

		// Token: 0x040072D1 RID: 29393
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
