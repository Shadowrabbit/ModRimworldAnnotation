using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB3 RID: 8115
	public class QuestNode_RequirementsToAcceptBedroom : QuestNode
	{
		// Token: 0x0600AC5A RID: 44122 RVA: 0x00321FE0 File Offset: 0x003201E0
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

		// Token: 0x0600AC5B RID: 44123 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040075D6 RID: 30166
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
