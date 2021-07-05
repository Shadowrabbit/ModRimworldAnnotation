using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001717 RID: 5911
	public class QuestNode_ResolveTextNow : QuestNode
	{
		// Token: 0x0600886E RID: 34926 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600886F RID: 34927 RVA: 0x0031035C File Offset: 0x0030E55C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string var = QuestGenUtility.ResolveLocalTextWithDescriptionRules(this.rules.GetValue(slate), this.root.GetValue(slate));
			slate.Set<string>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0400564F RID: 22095
		[NoTranslate]
		public SlateRef<string> root;

		// Token: 0x04005650 RID: 22096
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005651 RID: 22097
		public SlateRef<RulePack> rules;
	}
}
