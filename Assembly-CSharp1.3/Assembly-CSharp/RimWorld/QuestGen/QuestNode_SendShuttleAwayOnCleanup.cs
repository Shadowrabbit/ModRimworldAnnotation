using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016DF RID: 5855
	public class QuestNode_SendShuttleAwayOnCleanup : QuestNode
	{
		// Token: 0x0600875B RID: 34651 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600875C RID: 34652 RVA: 0x00307044 File Offset: 0x00305244
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

		// Token: 0x04005573 RID: 21875
		public SlateRef<Thing> shuttle;

		// Token: 0x04005574 RID: 21876
		public SlateRef<bool> dropEverything;
	}
}
