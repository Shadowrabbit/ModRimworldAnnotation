using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEB RID: 3307
	public class RoomRoleWorker_Workshop : RoomRoleWorker
	{
		// Token: 0x06004D03 RID: 19715 RVA: 0x0019AE90 File Offset: 0x00199090
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_WorkTable && containedAndAdjacentThings[i].def.designationCategory == DesignationCategoryDefOf.Production)
				{
					num++;
				}
			}
			return 13.5f * (float)num;
		}
	}
}
