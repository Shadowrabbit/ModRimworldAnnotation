using System;

namespace Verse.AI
{
	// Token: 0x020005D5 RID: 1493
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x06002B44 RID: 11076 RVA: 0x00102DF9 File Offset: 0x00100FF9
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
