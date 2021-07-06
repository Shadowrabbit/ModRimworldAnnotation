using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FE8 RID: 8168
	public class QuestNode_ResolveQuestName : QuestNode
	{
		// Token: 0x0600AD38 RID: 44344 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AD39 RID: 44345 RVA: 0x00070D5E File Offset: 0x0006EF5E
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestNameRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestName.Resolve();
		}

		// Token: 0x0600AD3A RID: 44346 RVA: 0x00326DEC File Offset: 0x00324FEC
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestName", out text, false))
			{
				text = QuestNode_ResolveQuestName.GenerateName().StripTags();
				QuestGen.slate.Set<string>("resolvedQuestName", text, false);
			}
			QuestGen.quest.name = text;
		}

		// Token: 0x0600AD3B RID: 44347 RVA: 0x00326E34 File Offset: 0x00325034
		private static string GenerateName()
		{
			GrammarRequest request = default(GrammarRequest);
			request.Rules.AddRange(QuestGen.QuestNameRulesReadOnly);
			foreach (KeyValuePair<string, string> keyValuePair in QuestGen.QuestNameConstantsReadOnly)
			{
				request.Constants.Add(keyValuePair.Key, keyValuePair.Value);
			}
			QuestGenUtility.AddSlateVars(ref request);
			Predicate<string> predicate = (string x) => !Find.QuestManager.QuestsListForReading.Any((Quest y) => y.name == x);
			if (QuestGen.Root.nameMustBeUnique)
			{
				return NameGenerator.GenerateName(request, predicate, false, "questName", null);
			}
			string text = null;
			int i;
			for (i = 0; i < 20; i++)
			{
				text = NameGenerator.GenerateName(request, null, false, "questName", null);
				if (predicate(text) || QuestGen.Root.defaultHidden)
				{
					break;
				}
			}
			if (i == 20)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Generated duplicate quest name. QuestScriptDef: ",
					QuestGen.Root,
					". Quest name: ",
					text
				}), false);
			}
			return text;
		}

		// Token: 0x040076B9 RID: 30393
		public SlateRef<RulePack> rules;

		// Token: 0x040076BA RID: 30394
		public const string TextRoot = "questName";

		// Token: 0x040076BB RID: 30395
		private const int MaxTriesTryAvoidDuplicateName = 20;
	}
}
