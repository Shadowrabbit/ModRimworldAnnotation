using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001301 RID: 4865
	public class RoomRoleWorker_Barn : RoomRoleWorker
	{
		// Token: 0x06006997 RID: 27031 RVA: 0x00208538 File Offset: 0x00206738
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && !building_Bed.def.building.bed_humanlike)
				{
					num++;
				}
			}
			return (float)num * 7.6f;
		}
	}
}
