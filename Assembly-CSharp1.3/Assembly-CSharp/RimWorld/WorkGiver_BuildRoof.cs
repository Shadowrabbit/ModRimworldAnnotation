using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000810 RID: 2064
	public class WorkGiver_BuildRoof : WorkGiver_Scanner
	{
		// Token: 0x060036FD RID: 14077 RVA: 0x0013756B File Offset: 0x0013576B
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.BuildRoof.ActiveCells;
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x00137582 File Offset: 0x00135782
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.BuildRoof.TrueCount == 0;
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060036FF RID: 14079 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x0013759C File Offset: 0x0013579C
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
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn) && this.BuildingToTouchToBeAbleToBuildRoof(c, pawn) == null)
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

		// Token: 0x06003702 RID: 14082 RVA: 0x00137658 File Offset: 0x00135858
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
			if (!pawn.CanReach(edifice, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return edifice;
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x001376A4 File Offset: 0x001358A4
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			LocalTargetInfo targetB = c;
			Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
			if (thing != null)
			{
				return RoofUtility.HandleBlockingThingJob(thing, pawn, forced);
			}
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				targetB = this.BuildingToTouchToBeAbleToBuildRoof(c, pawn);
			}
			return JobMaker.MakeJob(JobDefOf.BuildRoof, c, targetB);
		}
	}
}
