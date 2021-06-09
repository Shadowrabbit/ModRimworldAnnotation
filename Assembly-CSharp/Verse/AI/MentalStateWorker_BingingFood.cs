using System;

namespace Verse.AI
{
	// Token: 0x02000A3F RID: 2623
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x06003E81 RID: 16001 RVA: 0x0002EF77 File Offset: 0x0002D177
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
