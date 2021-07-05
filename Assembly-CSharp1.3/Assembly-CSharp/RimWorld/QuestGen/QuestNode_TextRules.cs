using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200171A RID: 5914
	public class QuestNode_TextRules : QuestNode
	{
		// Token: 0x06008875 RID: 34933 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008876 RID: 34934 RVA: 0x003104B4 File Offset: 0x0030E6B4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			switch (this.target.GetValue(slate))
			{
			case TextRulesTarget.Description:
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(slate));
				return;
			case TextRulesTarget.Name:
				QuestGen.AddQuestNameRules(this.rules.GetValue(slate));
				return;
			case TextRulesTarget.DecriptionAndName:
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(slate));
				QuestGen.AddQuestNameRules(this.rules.GetValue(slate));
				return;
			default:
				return;
			}
		}

		// Token: 0x04005657 RID: 22103
		public SlateRef<RulePack> rules;

		// Token: 0x04005658 RID: 22104
		public SlateRef<TextRulesTarget> target;
	}
}
