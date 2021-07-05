using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020016DD RID: 5853
	public class QuestNode_RequirementsToAcceptColonistWithTitle : QuestNode
	{
		// Token: 0x06008755 RID: 34645 RVA: 0x00306F78 File Offset: 0x00305178
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptColonistWithTitle
			{
				minimumTitle = this.minimumTitle.GetValue(slate),
				faction = this.faction.GetValue(slate)
			});
		}

		// Token: 0x06008756 RID: 34646 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400556E RID: 21870
		public SlateRef<RoyalTitleDef> minimumTitle;

		// Token: 0x0400556F RID: 21871
		public SlateRef<Faction> faction;
	}
}
