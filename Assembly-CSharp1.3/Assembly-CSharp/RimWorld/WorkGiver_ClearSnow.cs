using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000836 RID: 2102
	public class WorkGiver_ClearSnow : WorkGiver_Scanner
	{
		// Token: 0x060037A7 RID: 14247 RVA: 0x00139A58 File Offset: 0x00137C58
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowClear.ActiveCells;
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060037A8 RID: 14248 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x00139A6F File Offset: 0x00137C6F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.SnowClear.TrueCount == 0;
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x00139A89 File Offset: 0x00137C89
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.snowGrid.GetDepth(c) >= 0.2f && !c.IsForbidden(pawn) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x00139AC5 File Offset: 0x00137CC5
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ClearSnow, c);
		}
	}
}
