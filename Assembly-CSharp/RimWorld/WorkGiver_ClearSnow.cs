using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D68 RID: 3432
	public class WorkGiver_ClearSnow : WorkGiver_Scanner
	{
		// Token: 0x06004E57 RID: 20055 RVA: 0x0003747B File Offset: 0x0003567B
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowClear.ActiveCells;
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06004E58 RID: 20056 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x00037492 File Offset: 0x00035692
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.SnowClear.TrueCount == 0;
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x000374AC File Offset: 0x000356AC
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.snowGrid.GetDepth(c) >= 0.2f && !c.IsForbidden(pawn) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x000374E8 File Offset: 0x000356E8
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ClearSnow, c);
		}
	}
}
