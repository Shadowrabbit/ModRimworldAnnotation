using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CD RID: 1997
	public class JobGiver_PrisonerWaitInsteadOfEscaping : JobGiver_Wander
	{
		// Token: 0x060035CB RID: 13771 RVA: 0x00130C38 File Offset: 0x0012EE38
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.guest == null || !pawn.guest.ShouldWaitInsteadOfEscaping)
			{
				return null;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room != null && room.IsPrisonCell)
			{
				return null;
			}
			IntVec3 spotToWaitInsteadOfEscaping = pawn.guest.spotToWaitInsteadOfEscaping;
			if (!spotToWaitInsteadOfEscaping.IsValid || !pawn.CanReach(spotToWaitInsteadOfEscaping, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out spotToWaitInsteadOfEscaping))
				{
					return null;
				}
				pawn.guest.spotToWaitInsteadOfEscaping = spotToWaitInsteadOfEscaping;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x00130CB9 File Offset: 0x0012EEB9
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.guest.spotToWaitInsteadOfEscaping;
		}
	}
}
