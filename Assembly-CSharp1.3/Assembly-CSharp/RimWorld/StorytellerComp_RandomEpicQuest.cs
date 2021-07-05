using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C45 RID: 3141
	public class StorytellerComp_RandomEpicQuest : StorytellerComp_OnOffCycle
	{
		// Token: 0x0600499E RID: 18846 RVA: 0x001855D1 File Offset: 0x001837D1
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].root.IsEpic && (questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing))
				{
					yield break;
				}
			}
			foreach (FiringIncident firingIncident in base.MakeIntervalIncidents(target))
			{
				yield return firingIncident;
			}
			IEnumerator<FiringIncident> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x001855E8 File Offset: 0x001837E8
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms parms = base.GenerateParms(incCat, target);
			QuestScriptDef questScriptDef;
			if ((from x in DefDatabase<QuestScriptDef>.AllDefs
			where x.IsEpic && x.CanRun(parms.points)
			select x).TryRandomElement(out questScriptDef))
			{
				parms.questScriptDef = questScriptDef;
			}
			return parms;
		}
	}
}
