using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020015B6 RID: 5558
	public class SitePartWorker_ConditionCauser_ClimateAdjuster : SitePartWorker_ConditionCauser
	{
		// Token: 0x060078B5 RID: 30901 RVA: 0x0024AC04 File Offset: 0x00248E04
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string output = part.conditionCauser.TryGetComp<CompCauseGameCondition_TemperatureOffset>().temperatureOffset.ToStringTemperatureOffset("F1");
			outExtraDescriptionRules.Add(new Rule_String("temperatureOffset", output));
		}
	}
}
