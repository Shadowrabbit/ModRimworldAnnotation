using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2C RID: 3116
	public class IncidentWorker_GiveQuest_Map : IncidentWorker_GiveQuest
	{
		// Token: 0x06004929 RID: 18729 RVA: 0x00183878 File Offset: 0x00181A78
		protected override void GiveQuest(IncidentParms parms, QuestScriptDef questDef)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", parms.points, false);
			slate.Set<Map>("map", (Map)parms.target, false);
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questDef, slate);
			if (quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest);
			}
		}
	}
}
