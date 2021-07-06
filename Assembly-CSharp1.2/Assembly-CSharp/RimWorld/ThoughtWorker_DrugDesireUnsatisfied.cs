using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBC RID: 3772
	public class ThoughtWorker_DrugDesireUnsatisfied : ThoughtWorker
	{
		// Token: 0x060053C9 RID: 21449 RVA: 0x001C1BA0 File Offset: 0x001BFDA0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Chemical_Any drugsDesire = p.needs.drugsDesire;
			if (drugsDesire == null)
			{
				return false;
			}
			int moodBuffForCurrentLevel = (int)drugsDesire.MoodBuffForCurrentLevel;
			if (moodBuffForCurrentLevel < 3)
			{
				return ThoughtState.ActiveAtStage(3 - moodBuffForCurrentLevel - 1);
			}
			return false;
		}

		// Token: 0x0400350F RID: 13583
		private const int Neutral = 3;
	}
}
