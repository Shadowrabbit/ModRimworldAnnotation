using System;

namespace Verse.AI
{
	// Token: 0x020005D4 RID: 1492
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06002B42 RID: 11074 RVA: 0x00102DE3 File Offset: 0x00100FE3
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
