using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016DC RID: 5852
	public class QuestNode_RequirementsToAcceptBedroom : QuestNode
	{
		// Token: 0x06008752 RID: 34642 RVA: 0x00306ED8 File Offset: 0x003050D8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			QuestPart_RequirementsToAcceptBedroom questPart_RequirementsToAcceptBedroom = new QuestPart_RequirementsToAcceptBedroom();
			questPart_RequirementsToAcceptBedroom.targetPawns = (from p in this.pawns.GetValue(QuestGen.slate)
			where p.royalty != null && p.royalty.HighestTitleWithBedroomRequirements() != null
			orderby p.royalty.HighestTitleWithBedroomRequirements().def.seniority descending
			select p).ToList<Pawn>();
			questPart_RequirementsToAcceptBedroom.mapParent = slate.Get<Map>("map", null, false).Parent;
			quest.AddPart(questPart_RequirementsToAcceptBedroom);
		}

		// Token: 0x06008753 RID: 34643 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400556D RID: 21869
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
