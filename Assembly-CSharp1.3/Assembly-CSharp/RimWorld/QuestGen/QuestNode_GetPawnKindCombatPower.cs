using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C1 RID: 5825
	public class QuestNode_GetPawnKindCombatPower : QuestNode
	{
		// Token: 0x060086F6 RID: 34550 RVA: 0x003053BD File Offset: 0x003035BD
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
			return true;
		}

		// Token: 0x060086F7 RID: 34551 RVA: 0x003053E4 File Offset: 0x003035E4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
		}

		// Token: 0x040054D5 RID: 21717
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x040054D6 RID: 21718
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
