using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001618 RID: 5656
	public static class QuestGen_Requirements
	{
		// Token: 0x06008499 RID: 33945 RVA: 0x002F9698 File Offset: 0x002F7898
		public static QuestPart_RequirementsToAcceptFactionRelation RequirementsToAcceptFactionRelation(this Quest quest, Faction faction, FactionRelationKind relationKind)
		{
			QuestPart_RequirementsToAcceptFactionRelation questPart_RequirementsToAcceptFactionRelation = new QuestPart_RequirementsToAcceptFactionRelation();
			questPart_RequirementsToAcceptFactionRelation.otherFaction = faction;
			questPart_RequirementsToAcceptFactionRelation.relationKind = relationKind;
			quest.AddPart(questPart_RequirementsToAcceptFactionRelation);
			return questPart_RequirementsToAcceptFactionRelation;
		}
	}
}
