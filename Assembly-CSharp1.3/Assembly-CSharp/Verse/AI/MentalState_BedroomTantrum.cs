using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005F1 RID: 1521
	public class MentalState_BedroomTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06002BCA RID: 11210 RVA: 0x001049E0 File Offset: 0x00102BE0
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			outThings.Clear();
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return;
			}
			if (ownedBed.GetRoom(RegionType.Set_All) != null && !ownedBed.GetRoom(RegionType.Set_All).PsychologicallyOutdoors)
			{
				TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_All), this.pawn, outThings, this.GetCustomValidator(), 0);
				return;
			}
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, ownedBed.Position, outThings, this.GetCustomValidator(), 0, 8);
		}
	}
}
