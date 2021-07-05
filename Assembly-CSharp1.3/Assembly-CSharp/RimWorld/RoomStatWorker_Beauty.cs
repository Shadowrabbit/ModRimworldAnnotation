using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CED RID: 3309
	public class RoomStatWorker_Beauty : RoomStatWorker
	{
		// Token: 0x06004D08 RID: 19720 RVA: 0x0019AFC4 File Offset: 0x001991C4
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
				if (thing.GetRoom(RegionType.Set_All) != room && !RoomStatWorker_Beauty.countedAdjCells.Contains(thing.Position))
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

		// Token: 0x04002E8B RID: 11915
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

		// Token: 0x04002E8C RID: 11916
		private static List<Thing> countedThings = new List<Thing>();

		// Token: 0x04002E8D RID: 11917
		private static List<IntVec3> countedAdjCells = new List<IntVec3>();
	}
}
