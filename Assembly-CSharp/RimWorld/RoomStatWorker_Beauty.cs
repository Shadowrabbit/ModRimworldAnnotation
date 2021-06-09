using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001310 RID: 4880
	public class RoomStatWorker_Beauty : RoomStatWorker
	{
		// Token: 0x060069B6 RID: 27062 RVA: 0x00208B00 File Offset: 0x00206D00
		public override float GetScore(Room room)
		{
			float num = 0f;
			int num2 = 0;
			RoomStatWorker_Beauty.countedThings.Clear();
			foreach (IntVec3 c in room.Cells)
			{
				num += BeautyUtility.CellBeauty(c, room.Map, RoomStatWorker_Beauty.countedThings);
				num2++;
			}
			RoomStatWorker_Beauty.countedAdjCells.Clear();
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.GetRoom(RegionType.Set_Passable) != room && !RoomStatWorker_Beauty.countedAdjCells.Contains(thing.Position))
				{
					num += BeautyUtility.CellBeauty(thing.Position, room.Map, RoomStatWorker_Beauty.countedThings);
					RoomStatWorker_Beauty.countedAdjCells.Add(thing.Position);
				}
			}
			RoomStatWorker_Beauty.countedThings.Clear();
			if (num2 == 0)
			{
				return 0f;
			}
			return num / RoomStatWorker_Beauty.CellCountCurve.Evaluate((float)num2);
		}

		// Token: 0x04004655 RID: 18005
		private static readonly SimpleCurve CellCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(40f, 40f),
				true
			},
			{
				new CurvePoint(100000f, 100000f),
				true
			}
		};

		// Token: 0x04004656 RID: 18006
		private static List<Thing> countedThings = new List<Thing>();

		// Token: 0x04004657 RID: 18007
		private static List<IntVec3> countedAdjCells = new List<IntVec3>();
	}
}
