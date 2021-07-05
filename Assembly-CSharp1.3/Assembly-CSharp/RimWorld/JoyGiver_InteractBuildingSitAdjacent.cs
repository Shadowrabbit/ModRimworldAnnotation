using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D7 RID: 2007
	public class JoyGiver_InteractBuildingSitAdjacent : JoyGiver_InteractBuilding
	{
		// Token: 0x060035F1 RID: 13809 RVA: 0x001316C8 File Offset: 0x0012F8C8
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Clear();
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.AddRange(GenAdjFast.AdjacentCellsCardinal(t));
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Shuffle<IntVec3>();
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < JoyGiver_InteractBuildingSitAdjacent.tmpCells.Count; j++)
				{
					IntVec3 intVec = JoyGiver_InteractBuildingSitAdjacent.tmpCells[j];
					if (!intVec.IsForbidden(pawn) && pawn.CanReserveSittableOrSpot(intVec, false))
					{
						if (i == 0)
						{
							Building edifice = intVec.GetEdifice(pawn.Map);
							if (edifice == null || !edifice.def.building.isSittable)
							{
								goto IL_95;
							}
						}
						return JobMaker.MakeJob(this.def.jobDef, t, intVec);
					}
					IL_95:;
				}
				if (this.def.requireChair)
				{
					break;
				}
			}
			return null;
		}

		// Token: 0x04001EC9 RID: 7881
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
