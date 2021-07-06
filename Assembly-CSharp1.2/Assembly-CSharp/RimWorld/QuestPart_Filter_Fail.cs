using System;

namespace RimWorld
{
	// Token: 0x02001067 RID: 4199
	public class QuestPart_Filter_Fail : QuestPart_Filter
	{
		// Token: 0x06005B55 RID: 23381 RVA: 0x001D7E28 File Offset: 0x001D6028
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Fail;
		}
	}
}
