using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FE7 RID: 8167
	public class QuestNode_ResolveQuestDescription : QuestNode
	{
		// Token: 0x0600AD34 RID: 44340 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AD35 RID: 44341 RVA: 0x00070D30 File Offset: 0x0006EF30
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestDescription.Resolve();
		}

		// Token: 0x0600AD36 RID: 44342 RVA: 0x00326D94 File Offset: 0x00324F94
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestDescription", out text, false))
			{
				text = QuestGenUtility.ResolveAbsoluteText(QuestGen.QuestDescriptionRulesReadOnly, QuestGen.QuestDescriptionConstantsReadOnly, "questDescription", true);
				QuestGen.slate.Set<string>("resolvedQuestDescription", text, false);
			}
			QuestGen.quest.description = text;
		}

		// Token: 0x040076B7 RID: 30391
		public SlateRef<RulePack> rules;

		// Token: 0x040076B8 RID: 30392
		public const string TextRoot = "questDescription";
	}
}
