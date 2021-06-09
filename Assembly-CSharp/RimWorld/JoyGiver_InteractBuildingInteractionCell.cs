using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF1 RID: 3313
	public class JoyGiver_InteractBuildingInteractionCell : JoyGiver_InteractBuilding
	{
		// Token: 0x06004C2E RID: 19502 RVA: 0x001A8E88 File Offset: 0x001A7088
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (t.InteractionCell.Standable(t.Map) && !t.IsForbidden(pawn) && !t.InteractionCell.IsForbidden(pawn) && !pawn.Map.pawnDestinationReservationManager.IsReserved(t.InteractionCell))
			{
				return JobMaker.MakeJob(this.def.jobDef, t, t.InteractionCell);
			}
			return null;
		}
	}
}
