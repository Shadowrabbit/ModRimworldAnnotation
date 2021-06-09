using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FEC RID: 8172
	public class QuestNode_ResolveTextRequests : QuestNode
	{
		// Token: 0x0600AD45 RID: 44357 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AD46 RID: 44358 RVA: 0x00326FE4 File Offset: 0x003251E4
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
				QuestGen.AddQuestContentRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveTextRequests.Resolve();
		}

		// Token: 0x0600AD47 RID: 44359 RVA: 0x00327034 File Offset: 0x00325234
		public static void Resolve()
		{
			List<QuestTextRequest> textRequestsReadOnly = QuestGen.TextRequestsReadOnly;
			for (int i = 0; i < textRequestsReadOnly.Count; i++)
			{
				try
				{
					List<Rule> list = new List<Rule>();
					list.AddRange(QuestGen.QuestDescriptionRulesReadOnly);
					list.AddRange(QuestGen.QuestContentRulesReadOnly);
					if (textRequestsReadOnly[i].extraRules != null)
					{
						list.AddRange(textRequestsReadOnly[i].extraRules);
					}
					string obj = QuestGenUtility.ResolveAbsoluteText(list, QuestGen.QuestDescriptionConstantsReadOnly, textRequestsReadOnly[i].keyword, true);
					textRequestsReadOnly[i].setter(obj);
				}
				catch (Exception arg)
				{
					Log.Error("Error while resolving text request: " + arg, false);
				}
			}
			textRequestsReadOnly.Clear();
		}

		// Token: 0x040076C2 RID: 30402
		public SlateRef<RulePack> rules;
	}
}
