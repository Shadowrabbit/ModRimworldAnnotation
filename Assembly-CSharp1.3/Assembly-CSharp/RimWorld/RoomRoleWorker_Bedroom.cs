using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDF RID: 3295
	public class RoomRoleWorker_Bedroom : RoomRoleWorker
	{
		// Token: 0x06004CEA RID: 19690 RVA: 0x0019A950 File Offset: 0x00198B50
		public override float GetScore(Room room)
		{
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			List<Pawn> list = null;
			bool flag = false;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed;
				if ((building_Bed = (containedAndAdjacentThings[i] as Building_Bed)) != null && building_Bed.def.building.bed_humanlike)
				{
					if (building_Bed.Medical || building_Bed.ForPrisoners)
					{
						return 0f;
					}
					flag = true;
					List<Pawn> assignedPawnsForReading = building_Bed.CompAssignableToPawn.AssignedPawnsForReading;
					if (!assignedPawnsForReading.NullOrEmpty<Pawn>())
					{
						if (list == null)
						{
							list = assignedPawnsForReading[0].GetLoveCluster();
						}
						foreach (Pawn item in assignedPawnsForReading)
						{
							if (!list.Contains(item))
							{
								return 0f;
							}
						}
					}
				}
			}
			if (!flag)
			{
				return 0f;
			}
			return 100000f;
		}
	}
}
