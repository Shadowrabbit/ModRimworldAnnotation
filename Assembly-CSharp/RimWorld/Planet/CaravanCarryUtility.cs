using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020DF RID: 8415
	public static class CaravanCarryUtility
	{
		// Token: 0x0600B2D2 RID: 45778 RVA: 0x0033D114 File Offset: 0x0033B314
		public static bool CarriedByCaravan(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			return caravan != null && caravan.carryTracker.IsCarried(p);
		}

		// Token: 0x0600B2D3 RID: 45779 RVA: 0x00074342 File Offset: 0x00072542
		public static bool WouldBenefitFromBeingCarried(Pawn p)
		{
			return CaravanBedUtility.WouldBenefitFromRestingInBed(p);
		}
	}
}
