using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007A6 RID: 1958
	public class JobGiver_SpectateInCircleDuty : JobGiver_SpectateDutySpectateRect
	{
		// Token: 0x06003553 RID: 13651 RVA: 0x0012DB30 File Offset: 0x0012BD30
		protected override bool TryFindSpot(Pawn pawn, PawnDuty duty, out IntVec3 spot)
		{
			if (!SpectatorCellFinder.TryFindCircleSpectatorCellFor(pawn, duty.spectateRect, (float)duty.spectateDistance.min, (float)duty.spectateDistance.max, pawn.Map, out spot, null, new Func<IntVec3, Pawn, Map, bool>(RitualUility.GoodSpectateCellForRitual)))
			{
				return base.TryFindSpot(pawn, duty, out spot);
			}
			return spot.IsValid;
		}
	}
}
