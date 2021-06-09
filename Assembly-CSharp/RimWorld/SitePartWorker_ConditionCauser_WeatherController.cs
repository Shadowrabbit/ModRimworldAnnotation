using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020015B5 RID: 5557
	public class SitePartWorker_ConditionCauser_WeatherController : SitePartWorker_ConditionCauser
	{
		// Token: 0x060078B3 RID: 30899 RVA: 0x0024ABC8 File Offset: 0x00248DC8
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			WeatherDef weather = part.conditionCauser.TryGetComp<CompCauseGameCondition_ForceWeather>().weather;
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("weather", weather));
		}
	}
}
