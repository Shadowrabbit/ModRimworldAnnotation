using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001669 RID: 5737
	public class QuestNode_GetBodySize : QuestNode
	{
		// Token: 0x060085A9 RID: 34217 RVA: 0x002FF2B2 File Offset: 0x002FD4B2
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
			return true;
		}

		// Token: 0x060085AA RID: 34218 RVA: 0x002FF2E0 File Offset: 0x002FD4E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
		}

		// Token: 0x0400537D RID: 21373
		public SlateRef<PawnKindDef> pawnKind;

		// Token: 0x0400537E RID: 21374
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
