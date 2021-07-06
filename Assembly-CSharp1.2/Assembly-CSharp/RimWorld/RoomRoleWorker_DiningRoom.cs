using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001304 RID: 4868
	public class RoomRoleWorker_DiningRoom : RoomRoleWorker
	{
		// Token: 0x0600699D RID: 27037 RVA: 0x00208684 File Offset: 0x00206884
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
