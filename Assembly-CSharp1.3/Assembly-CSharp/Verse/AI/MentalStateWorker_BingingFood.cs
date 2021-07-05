using System;

namespace Verse.AI
{
	// Token: 0x020005CA RID: 1482
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x06002B28 RID: 11048 RVA: 0x00102AF6 File Offset: 0x00100CF6
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
