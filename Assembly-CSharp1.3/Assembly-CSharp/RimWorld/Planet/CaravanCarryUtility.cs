using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200179F RID: 6047
	public static class CaravanCarryUtility
	{
		// Token: 0x06008C25 RID: 35877 RVA: 0x003243C4 File Offset: 0x003225C4
		public static bool CarriedByCaravan(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			return caravan != null && caravan.carryTracker.IsCarried(p);
		}

		// Token: 0x06008C26 RID: 35878 RVA: 0x003243E9 File Offset: 0x003225E9
		public static bool WouldBenefitFromBeingCarried(Pawn p)
		{
			return CaravanBedUtility.WouldBenefitFromRestingInBed(p);
		}
	}
}
