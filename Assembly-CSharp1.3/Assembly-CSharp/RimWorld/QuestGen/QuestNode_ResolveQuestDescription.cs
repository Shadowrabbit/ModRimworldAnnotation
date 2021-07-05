using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001715 RID: 5909
	public class QuestNode_ResolveQuestDescription : QuestNode
	{
		// Token: 0x06008865 RID: 34917 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008866 RID: 34918 RVA: 0x00310134 File Offset: 0x0030E334
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestDescription.Resolve();
		}

		// Token: 0x06008867 RID: 34919 RVA: 0x00310164 File Offset: 0x0030E364
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestDescription", out text, false) && !QuestGen.quest.hidden)
			{
				text = QuestGenUtility.ResolveAbsoluteText(QuestGen.QuestDescriptionRulesReadOnly, QuestGen.QuestDescriptionConstantsReadOnly, "questDescription", true);
				QuestGen.slate.Set<string>("resolvedQuestDescription", text, false);
			}
			QuestGen.quest.description = text;
		}

		// Token: 0x0400564A RID: 22090
		public SlateRef<RulePack> rules;

		// Token: 0x0400564B RID: 22091
		public const string TextRoot = "questDescription";
	}
}
