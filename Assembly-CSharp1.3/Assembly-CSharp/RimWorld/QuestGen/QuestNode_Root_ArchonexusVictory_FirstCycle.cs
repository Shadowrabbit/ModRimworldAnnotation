using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F4 RID: 5876
	public class QuestNode_Root_ArchonexusVictory_FirstCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x060087A8 RID: 34728 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override int ArchonexusCycle
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060087A9 RID: 34729 RVA: 0x00308528 File Offset: 0x00306728
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Faction faction = slate.Get<Faction>("civilOutlander", null, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally);
			}
			base.PickNewColony(faction, WorldObjectDefOf.Settlement_SecondArchonexusCycle, SoundDefOf.GameStartSting_FirstArchonexusCycle, 1);
			slate.Set<bool>("factionless", faction == null, false);
		}
	}
}
