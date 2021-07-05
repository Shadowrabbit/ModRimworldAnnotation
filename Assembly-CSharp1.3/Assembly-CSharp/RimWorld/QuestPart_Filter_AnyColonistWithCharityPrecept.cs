using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2D RID: 2861
	public class QuestPart_Filter_AnyColonistWithCharityPrecept : QuestPart_Filter
	{
		// Token: 0x0600431C RID: 17180 RVA: 0x001669BD File Offset: 0x00164BBD
		protected override bool Pass(SignalArgs args)
		{
			return IdeoUtility.AllColonistsWithCharityPrecept().Any<Pawn>();
		}
	}
}
