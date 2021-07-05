using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF0 RID: 3312
	public class JoyGiver_InteractBuildingSitAdjacent : JoyGiver_InteractBuilding
	{
		// Token: 0x06004C2B RID: 19499 RVA: 0x001A8D9C File Offset: 0x001A6F9C
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Clear();
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.AddRange(GenAdjFast.AdjacentCellsCardinal(t));
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Shuffle<IntVec3>();
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < JoyGiver_InteractBuildingSitAdjacent.tmpCells.Count; j++)
				{
					IntVec3 c = JoyGiver_InteractBuildingSitAdjacent.tmpCells[j];
					if (!c.IsForbidden(pawn) && pawn.CanReserve(c, 1, -1, null, false))
					{
						if (i == 0)
						{
							Building edifice = c.GetEdifice(pawn.Map);
							if (edifice == null || !edifice.def.building.isSittable || !pawn.CanReserve(edifice, 1, -1, null, false))
							{
								goto IL_AF;
							}
						}
						return JobMaker.MakeJob(this.def.jobDef, t, c);
					}
					IL_AF:;
				}
				if (this.def.requireChair)
				{
					break;
				}
			}
			return null;
		}

		// Token: 0x04003231 RID: 12849
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
