using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FE9 RID: 4073
	public class SitePartWorker_ConditionCauser_ClimateAdjuster : SitePartWorker_ConditionCauser
	{
		// Token: 0x06005FF6 RID: 24566 RVA: 0x0020BFA4 File Offset: 0x0020A1A4
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string output = part.conditionCauser.TryGetComp<CompCauseGameCondition_TemperatureOffset>().temperatureOffset.ToStringTemperatureOffset("F1");
			outExtraDescriptionRules.Add(new Rule_String("temperatureOffset", output));
		}
	}
}
