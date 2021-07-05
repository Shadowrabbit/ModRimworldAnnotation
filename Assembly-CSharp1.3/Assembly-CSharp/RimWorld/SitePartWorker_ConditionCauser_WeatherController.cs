using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FE8 RID: 4072
	public class SitePartWorker_ConditionCauser_WeatherController : SitePartWorker_ConditionCauser
	{
		// Token: 0x06005FF4 RID: 24564 RVA: 0x0020BF60 File Offset: 0x0020A160
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			WeatherDef weather = part.conditionCauser.TryGetComp<CompCauseGameCondition_ForceWeather>().weather;
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("weather", weather));
		}
	}
}
