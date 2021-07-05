using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200081C RID: 2076
	public class WorkGiver_RemoveRoof : WorkGiver_Scanner
	{
		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06003740 RID: 14144 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x0013844E File Offset: 0x0013664E
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.NoRoof.ActiveCells;
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x00138465 File Offset: 0x00136665
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof.TrueCount == 0;
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06003743 RID: 14147 RVA: 0x00034716 File Offset: 0x00032916
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x00138480 File Offset: 0x00136680
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof[c] && c.Roofed(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Ceiling, forced);
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x001384DB File Offset: 0x001366DB
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveRoof, c, c);
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x001384F4 File Offset: 0x001366F4
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
