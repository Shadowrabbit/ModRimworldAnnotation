using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F15 RID: 7957
	public class QuestNode_GetBodySize : QuestNode
	{
		// Token: 0x0600AA43 RID: 43587 RVA: 0x0006F985 File Offset: 0x0006DB85
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
			return true;
		}

		// Token: 0x0600AA44 RID: 43588 RVA: 0x0031B338 File Offset: 0x00319538
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
		}

		// Token: 0x040073A4 RID: 29604
		public SlateRef<PawnKindDef> pawnKind;

		// Token: 0x040073A5 RID: 29605
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
