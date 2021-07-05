using System;

namespace RimWorld
{
	// Token: 0x02000C4A RID: 3146
	public class StorytellerComp_RandomQuest : StorytellerComp_OnOffCycle
	{
		// Token: 0x060049AB RID: 18859 RVA: 0x001857F6 File Offset: 0x001839F6
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = base.GenerateParms(incCat, target);
			incidentParms.questScriptDef = NaturalRandomQuestChooser.ChooseNaturalRandomQuest(incidentParms.points, target);
			return incidentParms;
		}
	}
}
