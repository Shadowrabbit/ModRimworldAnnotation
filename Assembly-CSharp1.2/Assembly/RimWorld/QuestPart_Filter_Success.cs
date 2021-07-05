using System;

namespace RimWorld
{
	// Token: 0x02001068 RID: 4200
	public class QuestPart_Filter_Success : QuestPart_Filter
	{
		// Token: 0x06005B57 RID: 23383 RVA: 0x001D7E4C File Offset: 0x001D604C
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Success;
		}
	}
}
