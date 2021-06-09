using System;

namespace Verse.AI
{
	// Token: 0x02000A4B RID: 2635
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x06003E9F RID: 16031 RVA: 0x0002F0B6 File Offset: 0x0002D2B6
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
