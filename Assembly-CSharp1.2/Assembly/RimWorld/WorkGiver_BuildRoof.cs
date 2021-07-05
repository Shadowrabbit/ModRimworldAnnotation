using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D49 RID: 3401
	public class WorkGiver_BuildRoof : WorkGiver_Scanner
	{
		// Token: 0x06004DBD RID: 19901 RVA: 0x00036ECE File Offset: 0x000350CE
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.BuildRoof.ActiveCells;
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x00036EE5 File Offset: 0x000350E5
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.BuildRoof.TrueCount == 0;
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06004DBF RID: 19903 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x001AF67C File Offset: 0x001AD87C
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (!pawn.Map.areaManager.BuildRoof[c])
			{
				return false;
			}
			if (c.Roofed(pawn.Map))
			{
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				return false;
			}
			if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Ceiling, forced))
			{
				return false;
			}
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn) && this.BuildingToTouchToBeAbleToBuildRoof(c, pawn) == null)
			{
				return false;
			}
			if (!RoofCollapseUtility.WithinRangeOfRoofHolder(c, pawn.Map, false))
			{
				return false;
			}
			if (!RoofCollapseUtility.ConnectedToRoofHolder(c, pawn.Map, true))
			{
				return false;
			}
			Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
			return thing == null || RoofUtility.CanHandleBlockingThing(thing, pawn, forced);
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x001AF738 File Offset: 0x001AD938
		private Building BuildingToTouchToBeAbleToBuildRoof(IntVec3 c, Pawn pawn)
		{
			if (c.Standable(pawn.Map))
			{
				return null;
			}
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice == null)
			{
				return null;
			}
			if (!pawn.CanReach(edifice, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
			{
				return null;
			}
			return edifice;
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x001AF784 File Offset: 0x001AD984
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			LocalTargetInfo targetB = c;
			Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
			if (thing != null)
			{
				return RoofUtility.HandleBlockingThingJob(thing, pawn, forced);
			}
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
			{
				targetB = this.BuildingToTouchToBeAbleToBuildRoof(c, pawn);
			}
			return JobMaker.MakeJob(JobDefOf.BuildRoof, c, targetB);
		}
	}
}
