using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005F4 RID: 1524
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x06002BD6 RID: 11222 RVA: 0x00104A79 File Offset: 0x00102C79
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
