using System;

namespace Verse.AI
{
	// Token: 0x02000623 RID: 1571
	public class ThinkNode_ConditionalCanPickupOpportunisticWeapon : ThinkNode_Conditional
	{
		// Token: 0x06002D34 RID: 11572 RVA: 0x0010F38B File Offset: 0x0010D58B
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.pickupOpportunisticWeapon;
		}
	}
}
