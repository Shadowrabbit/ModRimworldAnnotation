using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001718 RID: 5912
	public class QuestNode_ResolveTextRequests : QuestNode
	{
		// Token: 0x06008871 RID: 34929 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008872 RID: 34930 RVA: 0x003103A4 File Offset: 0x0030E5A4
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
				QuestGen.AddQuestContentRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveTextRequests.Resolve();
		}

		// Token: 0x06008873 RID: 34931 RVA: 0x003103F4 File Offset: 0x0030E5F4
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
					Log.Error("Error while resolving text request: " + arg);
				}
			}
			textRequestsReadOnly.Clear();
		}

		// Token: 0x04005652 RID: 22098
		public SlateRef<RulePack> rules;
	}
}
