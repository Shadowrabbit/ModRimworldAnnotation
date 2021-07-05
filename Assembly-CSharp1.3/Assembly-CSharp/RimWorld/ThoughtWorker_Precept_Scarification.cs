using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093C RID: 2364
	public class ThoughtWorker_Precept_Scarification : ThoughtWorker_Precept
	{
		// Token: 0x06003CC6 RID: 15558 RVA: 0x0015027C File Offset: 0x0014E47C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			int hediffCount = p.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
			int requiredScars = p.ideo.Ideo.RequiredScars;
			if (hediffCount >= requiredScars)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (hediffCount == 0)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (hediffCount < requiredScars)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return false;
		}
	}
}
