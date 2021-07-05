using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A39 RID: 2617
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x06003E65 RID: 15973 RVA: 0x0002EE17 File Offset: 0x0002D017
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
