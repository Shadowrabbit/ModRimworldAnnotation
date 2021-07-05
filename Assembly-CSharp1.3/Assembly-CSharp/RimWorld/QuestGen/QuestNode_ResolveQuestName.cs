using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001716 RID: 5910
	public class QuestNode_ResolveQuestName : QuestNode
	{
		// Token: 0x06008869 RID: 34921 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600886A RID: 34922 RVA: 0x003101C8 File Offset: 0x0030E3C8
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestNameRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestName.Resolve();
		}

		// Token: 0x0600886B RID: 34923 RVA: 0x003101F8 File Offset: 0x0030E3F8
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestName", out text, false))
			{
				text = (QuestGen.quest.hidden ? QuestGen.quest.root.defName : QuestNode_ResolveQuestName.GenerateName().StripTags());
				QuestGen.slate.Set<string>("resolvedQuestName", text, false);
			}
			QuestGen.quest.name = text;
		}

		// Token: 0x0600886C RID: 34924 RVA: 0x00310260 File Offset: 0x0030E460
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
			for (int i = 0; i < 20; i++)
			{
				text = NameGenerator.GenerateName(request, null, false, "questName", null);
				if (predicate(text) || QuestGen.Root.defaultHidden)
				{
					break;
				}
			}
			return text;
		}

		// Token: 0x0400564C RID: 22092
		public SlateRef<RulePack> rules;

		// Token: 0x0400564D RID: 22093
		public const string TextRoot = "questName";

		// Token: 0x0400564E RID: 22094
		private const int MaxTriesTryAvoidDuplicateName = 20;
	}
}
