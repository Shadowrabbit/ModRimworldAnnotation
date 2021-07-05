using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE0 RID: 3296
	public class RoomRoleWorker_DiningRoom : RoomRoleWorker
	{
		// Token: 0x06004CEC RID: 19692 RVA: 0x0019AA4C File Offset: 0x00198C4C
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building && thing.def.surfaceType == SurfaceType.Eat)
				{
					num++;
				}
			}
			return (float)num * 8f;
		}
	}
}
