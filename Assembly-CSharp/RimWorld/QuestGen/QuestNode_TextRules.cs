using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FEE RID: 8174
	public class QuestNode_TextRules : QuestNode
	{
		// Token: 0x0600AD49 RID: 44361 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AD4A RID: 44362 RVA: 0x003270F4 File Offset: 0x003252F4
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

		// Token: 0x040076C7 RID: 30407
		public SlateRef<RulePack> rules;

		// Token: 0x040076C8 RID: 30408
		public SlateRef<TextRulesTarget> target;
	}
}
