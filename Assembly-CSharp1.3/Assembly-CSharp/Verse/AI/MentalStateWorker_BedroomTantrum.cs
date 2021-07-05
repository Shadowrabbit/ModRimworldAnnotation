using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005CF RID: 1487
	public class MentalStateWorker_BedroomTantrum : MentalStateWorker
	{
		// Token: 0x06002B35 RID: 11061 RVA: 0x00102C40 File Offset: 0x00100E40
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			if (ownedBed == null || ownedBed.GetRoom(RegionType.Set_All) == null || ownedBed.GetRoom(RegionType.Set_All).PsychologicallyOutdoors)
			{
				return false;
			}
			MentalStateWorker_BedroomTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_All), pawn, MentalStateWorker_BedroomTantrum.tmpThings, null, 0);
			bool result = MentalStateWorker_BedroomTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_BedroomTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04001A70 RID: 6768
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
