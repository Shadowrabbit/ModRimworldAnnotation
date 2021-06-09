using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FEB RID: 8171
	public class QuestNode_ResolveTextNow : QuestNode
	{
		// Token: 0x0600AD42 RID: 44354 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AD43 RID: 44355 RVA: 0x00326F9C File Offset: 0x0032519C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string var = QuestGenUtility.ResolveLocalTextWithDescriptionRules(this.rules.GetValue(slate), this.root.GetValue(slate));
			slate.Set<string>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x040076BF RID: 30399
		[NoTranslate]
		public SlateRef<string> root;

		// Token: 0x040076C0 RID: 30400
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040076C1 RID: 30401
		public SlateRef<RulePack> rules;
	}
}
