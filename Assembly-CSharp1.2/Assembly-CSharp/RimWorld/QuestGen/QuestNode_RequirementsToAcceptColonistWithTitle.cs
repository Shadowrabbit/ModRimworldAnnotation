using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB5 RID: 8117
	public class QuestNode_RequirementsToAcceptColonistWithTitle : QuestNode
	{
		// Token: 0x0600AC61 RID: 44129 RVA: 0x00322080 File Offset: 0x00320280
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptColonistWithTitle
			{
				minimumTitle = this.minimumTitle.GetValue(slate),
				faction = this.faction.GetValue(slate)
			});
		}

		// Token: 0x0600AC62 RID: 44130 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040075DA RID: 30170
		public SlateRef<RoyalTitleDef> minimumTitle;

		// Token: 0x040075DB RID: 30171
		public SlateRef<Faction> faction;
	}
}
