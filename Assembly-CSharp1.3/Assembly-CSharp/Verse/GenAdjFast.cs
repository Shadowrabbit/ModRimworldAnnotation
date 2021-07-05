using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000023 RID: 35
	public static class GenAdjFast
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x00009518 File Offset: 0x00007718
		public static List<IntVec3> AdjacentCells8Way(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCells8Way((Thing)pack);
			}
			return GenAdjFast.AdjacentCells8Way((IntVec3)pack);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000953C File Offset: 0x0000773C
		public static List<IntVec3> AdjacentCells8Way(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 8; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.AdjacentCells[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000959D File Offset: 0x0000779D
		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000095BC File Offset: 0x000077BC
		public static List<IntVec3> AdjacentCells8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCells8Way(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num - 1, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2);
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4);
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num);
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000096ED File Offset: 0x000078ED
		public static List<IntVec3> AdjacentCellsCardinal(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCellsCardinal((Thing)pack);
			}
			return GenAdjFast.AdjacentCellsCardinal((IntVec3)pack);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009710 File Offset: 0x00007910
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 4; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.CardinalDirections[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009771 File Offset: 0x00007971
		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009790 File Offset: 0x00007990
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCellsCardinal(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2 - 1);
			intVec.x++;
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4 - 1);
			intVec.z++;
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num + 1);
			intVec.x--;
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000098EC File Offset: 0x00007AEC
		public static void AdjacentThings8Way(Thing thing, List<Thing> outThings)
		{
			outThings.Clear();
			if (!thing.Spawned)
			{
				return;
			}
			Map map = thing.Map;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(thing);
			for (int i = 0; i < list.Count; i++)
			{
				List<Thing> thingList = list[i].GetThingList(map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (!outThings.Contains(thingList[j]))
					{
						outThings.Add(thingList[j]);
					}
				}
			}
		}

		// Token: 0x04000060 RID: 96
		private static List<IntVec3> resultList = new List<IntVec3>();

		// Token: 0x04000061 RID: 97
		private static bool working = false;
	}
}
