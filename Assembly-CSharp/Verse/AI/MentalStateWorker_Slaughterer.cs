using System;

namespace Verse.AI
{
	// Token: 0x02000A4A RID: 2634
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06003E9D RID: 16029 RVA: 0x0002F0A0 File Offset: 0x0002D2A0
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
