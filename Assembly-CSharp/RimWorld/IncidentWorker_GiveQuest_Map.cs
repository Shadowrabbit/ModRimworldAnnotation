using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001209 RID: 4617
	public class IncidentWorker_GiveQuest_Map : IncidentWorker_GiveQuest
	{
		// Token: 0x060064F0 RID: 25840 RVA: 0x001F4CE8 File Offset: 0x001F2EE8
		protected override void GiveQuest(IncidentParms parms, QuestScriptDef questDef)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", parms.points, false);
			slate.Set<Map>("map", (Map)parms.target, false);
			QuestUtility.SendLetterQuestAvailable(QuestUtility.GenerateQuestAndMakeAvailable(questDef, slate));
		}
	}
}
