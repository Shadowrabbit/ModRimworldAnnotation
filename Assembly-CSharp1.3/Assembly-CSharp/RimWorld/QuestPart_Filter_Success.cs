using System;

namespace RimWorld
{
	// Token: 0x02000B3D RID: 2877
	public class QuestPart_Filter_Success : QuestPart_Filter
	{
		// Token: 0x0600434E RID: 17230 RVA: 0x00167248 File Offset: 0x00165448
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Success;
		}
	}
}
