using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001311 RID: 4881
	public class RoomStatWorker_Cleanliness : RoomStatWorker
	{
		// Token: 0x060069B9 RID: 27065 RVA: 0x00208C7C File Offset: 0x00206E7C
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Filth || thing.def.category == ThingCategory.Plant)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Cleanliness, true);
				}
			}
			foreach (IntVec3 c in room.Cells)
			{
				num += c.GetTerrain(room.Map).GetStatValueAbstract(StatDefOf.Cleanliness, null);
			}
			return num / (float)room.CellCount;
		}
	}
}
