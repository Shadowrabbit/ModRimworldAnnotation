using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB7 RID: 8119
	public class QuestNode_SendShuttleAwayOnCleanup : QuestNode
	{
		// Token: 0x0600AC67 RID: 44135 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC68 RID: 44136 RVA: 0x0032214C File Offset: 0x0032034C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAwayOnCleanup questPart_SendShuttleAwayOnCleanup = new QuestPart_SendShuttleAwayOnCleanup();
			questPart_SendShuttleAwayOnCleanup.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAwayOnCleanup.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAwayOnCleanup);
		}

		// Token: 0x040075DF RID: 30175
		public SlateRef<Thing> shuttle;

		// Token: 0x040075E0 RID: 30176
		public SlateRef<bool> dropEverything;
	}
}
