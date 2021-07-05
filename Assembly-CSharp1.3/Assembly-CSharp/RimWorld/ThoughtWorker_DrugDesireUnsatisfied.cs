using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B2 RID: 2482
	public class ThoughtWorker_DrugDesireUnsatisfied : ThoughtWorker
	{
		// Token: 0x06003DF3 RID: 15859 RVA: 0x00153D38 File Offset: 0x00151F38
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

		// Token: 0x040020E6 RID: 8422
		private const int Neutral = 3;
	}
}
