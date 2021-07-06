using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF3 RID: 3315
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		// Token: 0x06004C34 RID: 19508 RVA: 0x001A8F78 File Offset: 0x001A7178
		protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!base.CanInteractWith(pawn, t, inBed))
			{
				return false;
			}
			if (inBed)
			{
				Building_Bed bed = pawn.CurrentBed();
				return WatchBuildingUtility.CanWatchFromBed(pawn, bed, t);
			}
			return true;
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x001A8FA8 File Offset: 0x001A71A8
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			IntVec3 c;
			Building t2;
			if (!WatchBuildingUtility.TryFindBestWatchCell(t, pawn, this.def.desireSit, out c, out t2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t, c, t2);
		}
	}
}
