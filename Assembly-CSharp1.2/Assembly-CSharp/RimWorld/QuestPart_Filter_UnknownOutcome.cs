using System;

namespace RimWorld
{
	// Token: 0x02001069 RID: 4201
	public class QuestPart_Filter_UnknownOutcome : QuestPart_Filter
	{
		// Token: 0x06005B59 RID: 23385 RVA: 0x001D7E70 File Offset: 0x001D6070
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return !args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) || questEndOutcome == QuestEndOutcome.Unknown;
		}
	}
}
