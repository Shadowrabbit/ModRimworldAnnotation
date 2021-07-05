using System;

namespace Verse.AI
{
	// Token: 0x020005D6 RID: 1494
	public class MentalStateWorker_RitualBlinding : MentalStateWorker
	{
		// Token: 0x06002B46 RID: 11078 RVA: 0x00102E0F File Offset: 0x0010100F
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && RitualBlindingMentalStateUtility.FindPawnToBlind(pawn) != null;
		}
	}
}
