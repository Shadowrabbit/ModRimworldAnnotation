using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CE3 RID: 3299
	public class JobGiver_PrisonerWaitInsteadOfEscaping : JobGiver_Wander
	{
		// Token: 0x06004C01 RID: 19457 RVA: 0x001A85E4 File Offset: 0x001A67E4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.guest == null || !pawn.guest.ShouldWaitInsteadOfEscaping)
			{
				return null;
			}
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room != null && room.isPrisonCell)
			{
				return null;
			}
			IntVec3 spotToWaitInsteadOfEscaping = pawn.guest.spotToWaitInsteadOfEscaping;
			if (!spotToWaitInsteadOfEscaping.IsValid || !pawn.CanReach(spotToWaitInsteadOfEscaping, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out spotToWaitInsteadOfEscaping))
				{
					return null;
				}
				pawn.guest.spotToWaitInsteadOfEscaping = spotToWaitInsteadOfEscaping;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x00036168 File Offset: 0x00034368
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.guest.spotToWaitInsteadOfEscaping;
		}
	}
}
