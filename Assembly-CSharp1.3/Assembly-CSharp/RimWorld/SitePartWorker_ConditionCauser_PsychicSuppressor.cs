using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FEB RID: 4075
	public class SitePartWorker_ConditionCauser_PsychicSuppressor : SitePartWorker_ConditionCauser
	{
		// Token: 0x06005FFA RID: 24570 RVA: 0x0020C028 File Offset: 0x0020A228
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string label = part.conditionCauser.TryGetComp<CompCauseGameCondition_PsychicSuppression>().gender.GetLabel(false);
			outExtraDescriptionRules.Add(new Rule_String("affectedGender", label));
		}
	}
}
