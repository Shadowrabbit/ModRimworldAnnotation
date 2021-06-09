using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FBF RID: 8127
	public class QuestNode_SetTicksUntilAcceptanceExpiry : QuestNode
	{
		// Token: 0x0600AC86 RID: 44166 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC87 RID: 44167 RVA: 0x003225D4 File Offset: 0x003207D4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.ticksUntilAcceptanceExpiry = this.ticks.GetValue(slate);
		}

		// Token: 0x040075FA RID: 30202
		public SlateRef<int> ticks;
	}
}
