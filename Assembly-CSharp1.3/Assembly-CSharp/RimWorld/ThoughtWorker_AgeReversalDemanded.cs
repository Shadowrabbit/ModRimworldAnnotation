using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000978 RID: 2424
	public class ThoughtWorker_AgeReversalDemanded : ThoughtWorker_Precept
	{
		// Token: 0x06003D6F RID: 15727 RVA: 0x00152224 File Offset: 0x00150424
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ThoughtWorker_AgeReversalDemanded.CanHaveThoughtImpl(p))
			{
				return false;
			}
			long ageReversalDemandedDeadlineTicks = p.ageTracker.AgeReversalDemandedDeadlineTicks;
			if (ageReversalDemandedDeadlineTicks > 0L)
			{
				return false;
			}
			long num = -ageReversalDemandedDeadlineTicks / 60000L;
			int stageIndex;
			if (num <= 15L)
			{
				stageIndex = 0;
			}
			else if (num <= 30L)
			{
				stageIndex = 1;
			}
			else
			{
				stageIndex = 2;
			}
			return ThoughtState.ActiveAtStage(stageIndex);
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x0015227E File Offset: 0x0015047E
		public static bool CanHaveThought(Pawn pawn)
		{
			return ModLister.CheckIdeology("Age Reversal") && ThoughtWorker_Precept.CanHaveThought((ThoughtWorker_AgeReversalDemanded)ThoughtDefOf.AgeReversalDemanded.Worker, pawn) && ThoughtWorker_AgeReversalDemanded.CanHaveThoughtImpl(pawn);
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x001522AD File Offset: 0x001504AD
		private static bool CanHaveThoughtImpl(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && pawn.ageTracker != null && pawn.ageTracker.AgeBiologicalYears >= 20;
		}
	}
}
