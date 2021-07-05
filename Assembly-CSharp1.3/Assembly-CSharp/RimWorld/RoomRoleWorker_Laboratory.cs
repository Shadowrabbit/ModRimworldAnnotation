using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE3 RID: 3299
	public class RoomRoleWorker_Laboratory : RoomRoleWorker
	{
		// Token: 0x06004CF2 RID: 19698 RVA: 0x0019ABE4 File Offset: 0x00198DE4
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
