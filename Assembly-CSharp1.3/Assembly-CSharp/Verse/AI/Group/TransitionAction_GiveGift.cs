using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000684 RID: 1668
	public class TransitionAction_GiveGift : TransitionAction
	{
		// Token: 0x06002F08 RID: 12040 RVA: 0x00117FB0 File Offset: 0x001161B0
		public override void DoAction(Transition trans)
		{
			if (this.gifts.NullOrEmpty<Thing>())
			{
				return;
			}
			VisitorGiftForPlayerUtility.GiveGift(trans.target.lord.ownedPawns, trans.target.lord.faction, this.gifts);
			this.gifts.Clear();
		}

		// Token: 0x04001CBA RID: 7354
		public List<Thing> gifts;
	}
}
