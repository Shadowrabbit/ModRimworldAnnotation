using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F97 RID: 8087
	public class QuestNode_GetPawnKindCombatPower : QuestNode
	{
		// Token: 0x0600ABFE RID: 44030 RVA: 0x0007076B File Offset: 0x0006E96B
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
			return true;
		}

		// Token: 0x0600ABFF RID: 44031 RVA: 0x00320B00 File Offset: 0x0031ED00
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
		}

		// Token: 0x04007555 RID: 30037
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x04007556 RID: 30038
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
