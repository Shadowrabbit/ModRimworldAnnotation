using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E5 RID: 5861
	public class QuestNode_SetTicksUntilAcceptanceExpiry : QuestNode
	{
		// Token: 0x0600876F RID: 34671 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008770 RID: 34672 RVA: 0x00307258 File Offset: 0x00305458
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.ticksUntilAcceptanceExpiry = this.ticks.GetValue(slate);
		}

		// Token: 0x04005581 RID: 21889
		public SlateRef<int> ticks;
	}
}
