using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBB RID: 3771
	public class ThoughtWorker_DrugDesireSatisfied : ThoughtWorker
	{
		// Token: 0x060053C7 RID: 21447 RVA: 0x001C1B60 File Offset: 0x001BFD60
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

		// Token: 0x0400350E RID: 13582
		private const int Neutral = 3;
	}
}
