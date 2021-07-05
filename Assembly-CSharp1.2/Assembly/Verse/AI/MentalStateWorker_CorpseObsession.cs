using System;

namespace Verse.AI
{
	// Token: 0x02000A48 RID: 2632
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06003E99 RID: 16025 RVA: 0x0002F05B File Offset: 0x0002D25B
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
