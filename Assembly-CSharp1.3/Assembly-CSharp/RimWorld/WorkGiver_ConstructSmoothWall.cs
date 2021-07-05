using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000819 RID: 2073
	public class WorkGiver_ConstructSmoothWall : WorkGiver_Scanner
	{
		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x0600372E RID: 14126 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x001382AC File Offset: 0x001364AC
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.SmoothWall))
			{
				yield return designation.target.Cell;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x001382BC File Offset: 0x001364BC
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.SmoothWall);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x001382D8 File Offset: 0x001364D8
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (c.IsForbidden(pawn) || pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) == null)
			{
				return false;
			}
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice == null || !edifice.def.IsSmoothable)
			{
				Log.ErrorOnce("Failed to find valid edifice when trying to smooth a wall", 58988176);
				pawn.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.SmoothWall);
				return false;
			}
			return pawn.CanReserve(edifice, 1, -1, null, forced) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x00138373 File Offset: 0x00136573
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothWall, c.GetEdifice(pawn.Map));
		}
	}
}
