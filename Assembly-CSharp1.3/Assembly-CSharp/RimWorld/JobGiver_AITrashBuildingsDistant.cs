using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000784 RID: 1924
	public class JobGiver_AITrashBuildingsDistant : ThinkNode_JobGiver
	{
		// Token: 0x060034E3 RID: 13539 RVA: 0x0012B84F File Offset: 0x00129A4F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AITrashBuildingsDistant jobGiver_AITrashBuildingsDistant = (JobGiver_AITrashBuildingsDistant)base.DeepCopy(resolve);
			jobGiver_AITrashBuildingsDistant.attackAllInert = this.attackAllInert;
			return jobGiver_AITrashBuildingsDistant;
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x0012B86C File Offset: 0x00129A6C
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
					Job job = TrashUtility.TrashJob(pawn, building, this.attackAllInert, false);
					if (job != null)
					{
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x04001E71 RID: 7793
		public bool attackAllInert;
	}
}
