using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F5 RID: 5877
	public class QuestNode_Root_ArchonexusVictory_SecondCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x060087AB RID: 34731 RVA: 0x0009007E File Offset: 0x0008E27E
		protected override int ArchonexusCycle
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060087AC RID: 34732 RVA: 0x0030858C File Offset: 0x0030678C
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			base.TryAddstudyRequirement(quest, 2100, slate);
			quest.Dialog("[resolvedQuestDescription]", null, quest.AddedSignal, null, null, QuestPart.SignalListenMode.NotYetAcceptedOnly);
			Faction faction = slate.Get<Faction>("roughOutlander", null, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally);
			}
			base.PickNewColony(faction, WorldObjectDefOf.Settlement_ThirdArchonexusCycle, SoundDefOf.GameStartSting_SecondArchonexusCycle, 2);
			slate.Set<bool>("factionless", faction == null, false);
		}

		// Token: 0x040055B9 RID: 21945
		private const int ArchonexusSuperstructureResearchCost = 2100;

		// Token: 0x040055BA RID: 21946
		private const int MaxAllowedRelicsToTake = 2;
	}
}
