using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CA0 RID: 3232
	public class JobGiver_AITrashBuildingsDistant : ThinkNode_JobGiver
	{
		// Token: 0x06004B3D RID: 19261 RVA: 0x00035AF7 File Offset: 0x00033CF7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AITrashBuildingsDistant jobGiver_AITrashBuildingsDistant = (JobGiver_AITrashBuildingsDistant)base.DeepCopy(resolve);
			jobGiver_AITrashBuildingsDistant.attackAllInert = this.attackAllInert;
			return jobGiver_AITrashBuildingsDistant;
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x001A4B50 File Offset: 0x001A2D50
		protected override Job TryGiveJob(Pawn pawn)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			if (allBuildingsColonist.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < 75; i++)
			{
				Building building = allBuildingsColonist.RandomElement<Building>();
				if (TrashUtility.ShouldTrashBuilding(pawn, building, this.attackAllInert))
				{
					Job job = TrashUtility.TrashJob(pawn, building, this.attackAllInert);
					if (job != null)
					{
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x040031C6 RID: 12742
		public bool attackAllInert;
	}
}
