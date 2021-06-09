using System;

namespace RimWorld
{
	// Token: 0x02001236 RID: 4662
	public class StorytellerComp_RandomQuest : StorytellerComp_OnOffCycle
	{
		// Token: 0x060065D1 RID: 26065 RVA: 0x000459CE File Offset: 0x00043BCE
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = base.GenerateParms(incCat, target);
			incidentParms.questScriptDef = NaturalRandomQuestChooser.ChooseNaturalRandomQuest(incidentParms.points, target);
			return incidentParms;
		}
	}
}
