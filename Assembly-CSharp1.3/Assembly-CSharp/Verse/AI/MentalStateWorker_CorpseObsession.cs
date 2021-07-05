using System;

namespace Verse.AI
{
	// Token: 0x020005D2 RID: 1490
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06002B3E RID: 11070 RVA: 0x00102D9E File Offset: 0x00100F9E
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
