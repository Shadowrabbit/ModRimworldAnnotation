using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000936 RID: 2358
	public abstract class ThoughtWorker_Precept : ThoughtWorker
	{
		// Token: 0x06003CB4 RID: 15540
		protected abstract ThoughtState ShouldHaveThought(Pawn p);

		// Token: 0x06003CB5 RID: 15541 RVA: 0x0014FF77 File Offset: 0x0014E177
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ThoughtWorker_Precept.CanHaveThought(this, p))
			{
				return false;
			}
			return this.ShouldHaveThought(p);
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x0014FF90 File Offset: 0x0014E190
		protected static bool CanHaveThought(ThoughtWorker_Precept worker, Pawn p)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return false;
			}
			if (worker.def.gender != Gender.None && p.gender != worker.def.gender)
			{
				return false;
			}
			if (p.IsQuestLodger())
			{
				return false;
			}
			if (!ideo.cachedPossibleSituationalThoughts.Contains(worker.def))
			{
				return false;
			}
			if (worker.def.minExpectationForNegativeThought != null)
			{
				if (!p.Spawned)
				{
					return false;
				}
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
				if (expectationDef != null && expectationDef.order < worker.def.minExpectationForNegativeThought.order)
				{
					return false;
				}
			}
			return true;
		}
	}
}
