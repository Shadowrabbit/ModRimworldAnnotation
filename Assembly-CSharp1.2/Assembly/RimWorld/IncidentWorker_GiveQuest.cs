using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001208 RID: 4616
	public class IncidentWorker_GiveQuest : IncidentWorker
	{
		// Token: 0x060064EC RID: 25836 RVA: 0x001F4C2C File Offset: 0x001F2E2C
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

		// Token: 0x060064ED RID: 25837 RVA: 0x001F4C94 File Offset: 0x001F2E94
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			QuestScriptDef root;
			if ((root = this.def.questScriptDef) == null)
			{
				root = (parms.questScriptDef ?? NaturalRandomQuestChooser.ChooseNaturalRandomQuest(parms.points, parms.target));
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(root, parms.points);
			if (!quest.hidden)
			{
				QuestUtility.SendLetterQuestAvailable(quest);
			}
			return true;
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x00045217 File Offset: 0x00043417
		protected virtual void GiveQuest(IncidentParms parms, QuestScriptDef questDef)
		{
			QuestUtility.SendLetterQuestAvailable(QuestUtility.GenerateQuestAndMakeAvailable(questDef, parms.points));
		}
	}
}
