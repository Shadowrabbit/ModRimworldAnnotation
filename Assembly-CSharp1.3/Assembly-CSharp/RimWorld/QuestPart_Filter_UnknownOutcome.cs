using System;

namespace RimWorld
{
	// Token: 0x02000B3E RID: 2878
	public class QuestPart_Filter_UnknownOutcome : QuestPart_Filter
	{
		// Token: 0x06004350 RID: 17232 RVA: 0x0016726C File Offset: 0x0016546C
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return !args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) || questEndOutcome == QuestEndOutcome.Unknown;
		}
	}
}
