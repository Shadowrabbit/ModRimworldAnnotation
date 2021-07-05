using System;

namespace RimWorld
{
	// Token: 0x02000B3A RID: 2874
	public class QuestPart_Filter_Fail : QuestPart_Filter
	{
		// Token: 0x06004347 RID: 17223 RVA: 0x001671C4 File Offset: 0x001653C4
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Fail;
		}
	}
}
