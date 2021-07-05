using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B1 RID: 2481
	public class ThoughtWorker_DrugDesireSatisfied : ThoughtWorker
	{
		// Token: 0x06003DF1 RID: 15857 RVA: 0x00153CF8 File Offset: 0x00151EF8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Chemical_Any drugsDesire = p.needs.drugsDesire;
			if (drugsDesire == null)
			{
				return false;
			}
			int moodBuffForCurrentLevel = (int)drugsDesire.MoodBuffForCurrentLevel;
			if (moodBuffForCurrentLevel > 3)
			{
				return ThoughtState.ActiveAtStage(moodBuffForCurrentLevel - 3 - 1);
			}
			return false;
		}

		// Token: 0x040020E5 RID: 8421
		private const int Neutral = 3;
	}
}
