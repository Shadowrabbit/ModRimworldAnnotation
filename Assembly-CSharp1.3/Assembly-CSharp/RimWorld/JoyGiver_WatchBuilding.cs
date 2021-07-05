using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DA RID: 2010
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		// Token: 0x060035FA RID: 13818 RVA: 0x001318C8 File Offset: 0x0012FAC8
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

		// Token: 0x060035FB RID: 13819 RVA: 0x001318F8 File Offset: 0x0012FAF8
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
