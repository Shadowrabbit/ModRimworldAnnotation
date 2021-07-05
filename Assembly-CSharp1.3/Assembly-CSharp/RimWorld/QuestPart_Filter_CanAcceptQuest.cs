using System;

namespace RimWorld
{
	// Token: 0x02000B37 RID: 2871
	public class QuestPart_Filter_CanAcceptQuest : QuestPart_Filter
	{
		// Token: 0x06004340 RID: 17216 RVA: 0x0016710C File Offset: 0x0016530C
		protected override bool Pass(SignalArgs args)
		{
			return QuestUtility.CanAcceptQuest(this.quest);
		}
	}
}
