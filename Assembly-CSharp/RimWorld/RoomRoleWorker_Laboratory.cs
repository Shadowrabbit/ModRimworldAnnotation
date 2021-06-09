using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001307 RID: 4871
	public class RoomRoleWorker_Laboratory : RoomRoleWorker
	{
		// Token: 0x060069A3 RID: 27043 RVA: 0x0020881C File Offset: 0x00206A1C
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_ResearchBench)
				{
					num++;
				}
			}
			return 30f * (float)num;
		}
	}
}
