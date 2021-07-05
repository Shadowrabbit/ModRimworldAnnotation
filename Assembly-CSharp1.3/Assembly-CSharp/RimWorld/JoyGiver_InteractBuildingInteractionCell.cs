using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D8 RID: 2008
	public class JoyGiver_InteractBuildingInteractionCell : JoyGiver_InteractBuilding
	{
		// Token: 0x060035F4 RID: 13812 RVA: 0x001317A8 File Offset: 0x0012F9A8
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (t.InteractionCell.Standable(t.Map) && !t.IsForbidden(pawn) && !t.InteractionCell.IsForbidden(pawn) && !pawn.Map.pawnDestinationReservationManager.IsReserved(t.InteractionCell) && pawn.CanReserveSittableOrSpot(t.InteractionCell, false))
			{
				return JobMaker.MakeJob(this.def.jobDef, t, t.InteractionCell);
			}
			return null;
		}
	}
}
