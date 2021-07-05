using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2B RID: 3115
	public class IncidentWorker_GiveQuest : IncidentWorker
	{
		// Token: 0x06004925 RID: 18725 RVA: 0x0018379C File Offset: 0x0018199C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			if (this.def.questScriptDef != null)
			{
				if (!this.def.questScriptDef.CanRun(parms.points))
				{
					return false;
				}
			}
			else if (parms.questScriptDef != null && !parms.questScriptDef.CanRun(parms.points))
			{
				return false;
			}
			return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.Any<Pawn>();
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x00183804 File Offset: 0x00181A04
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			QuestScriptDef questScriptDef;
			if ((questScriptDef = this.def.questScriptDef) == null)
			{
				questScriptDef = (parms.questScriptDef ?? NaturalRandomQuestChooser.ChooseNaturalRandomQuest(parms.points, parms.target));
			}
			QuestScriptDef questDef = questScriptDef;
			this.GiveQuest(parms, questDef);
			return true;
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x00183848 File Offset: 0x00181A48
		protected virtual void GiveQuest(IncidentParms parms, QuestScriptDef questDef)
		{
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questDef, parms.points);
			if (!quest.hidden && questDef.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest);
			}
		}
	}
}
