using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5A RID: 3418
	public class WorkGiver_RemoveRoof : WorkGiver_Scanner
	{
		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06004E1F RID: 19999 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x000372E4 File Offset: 0x000354E4
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.NoRoof.ActiveCells;
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x000372FB File Offset: 0x000354FB
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof.TrueCount == 0;
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004E22 RID: 20002 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x001B060C File Offset: 0x001AE80C
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof[c] && c.Roofed(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Ceiling, forced);
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x00037315 File Offset: 0x00035515
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveRoof, c, c);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x001B0668 File Offset: 0x001AE868
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			IntVec3 cell = t.Cell;
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c = cell + GenAdj.AdjacentCells[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.holdsRoof)
					{
						return -60f;
					}
					if (c.Roofed(pawn.Map))
					{
						num++;
					}
				}
			}
			return (float)(-(float)Mathf.Min(num, 3));
		}
	}
}
