using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E41 RID: 3649
	public static class BeautyUtility
	{
		// Token: 0x0600548B RID: 21643 RVA: 0x001CA768 File Offset: 0x001C8968
		public static float AverageBeautyPerceptible(IntVec3 root, Map map)
		{
			if (!root.IsValid || !root.InBounds(map))
			{
				return 0f;
			}
			BeautyUtility.tempCountedThings.Clear();
			float num = 0f;
			int num2 = 0;
			BeautyUtility.FillBeautyRelevantCells(root, map);
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				num += BeautyUtility.CellBeauty(BeautyUtility.beautyRelevantCells[i], map, BeautyUtility.tempCountedThings);
				num2++;
			}
			BeautyUtility.tempCountedThings.Clear();
			if (num2 == 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x001CA7F0 File Offset: 0x001C89F0
		public static void FillBeautyRelevantCells(IntVec3 root, Map map)
		{
			BeautyUtility.beautyRelevantCells.Clear();
			Room room = root.GetRoom(map);
			if (room == null)
			{
				return;
			}
			BeautyUtility.visibleRooms.Clear();
			BeautyUtility.visibleRooms.Add(room);
			if (room.IsDoorway)
			{
				foreach (Region region in room.FirstRegion.Neighbors)
				{
					if (!BeautyUtility.visibleRooms.Contains(region.Room))
					{
						BeautyUtility.visibleRooms.Add(region.Room);
					}
				}
			}
			for (int i = 0; i < BeautyUtility.SampleNumCells_Beauty; i++)
			{
				IntVec3 intVec = root + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && !intVec.Fogged(map))
				{
					Room room2 = intVec.GetRoom(map);
					if (!BeautyUtility.visibleRooms.Contains(room2))
					{
						bool flag = false;
						for (int j = 0; j < 8; j++)
						{
							IntVec3 loc = intVec + GenAdj.AdjacentCells[j];
							if (BeautyUtility.visibleRooms.Contains(loc.GetRoom(map)))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							goto IL_11A;
						}
					}
					BeautyUtility.beautyRelevantCells.Add(intVec);
				}
				IL_11A:;
			}
			BeautyUtility.visibleRooms.Clear();
		}

		// Token: 0x0600548D RID: 21645 RVA: 0x001CA940 File Offset: 0x001C8B40
		public static float CellBeauty(IntVec3 c, Map map, List<Thing> countedThings = null)
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (BeautyUtility.BeautyRelevant(thing.def.category))
				{
					if (countedThings != null)
					{
						if (countedThings.Contains(thing))
						{
							goto IL_CB;
						}
						countedThings.Add(thing);
					}
					SlotGroup slotGroup = thing.GetSlotGroup();
					if (slotGroup == null || slotGroup.parent == thing || !slotGroup.parent.IgnoreStoredThingsBeauty)
					{
						float num3 = thing.GetStatValue(StatDefOf.Beauty, true);
						if (thing is Filth && !map.roofGrid.Roofed(c))
						{
							num3 *= 0.3f;
						}
						if (thing.def.Fillage == FillCategory.Full)
						{
							flag = true;
							num2 += num3;
						}
						else
						{
							num += num3;
						}
					}
				}
				IL_CB:;
			}
			if (flag)
			{
				return num2;
			}
			return num + map.terrainGrid.TerrainAt(c).GetStatValueAbstract(StatDefOf.Beauty, null);
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x001CAA4B File Offset: 0x001C8C4B
		public static bool BeautyRelevant(ThingCategory cat)
		{
			return cat == ThingCategory.Building || cat == ThingCategory.Item || cat == ThingCategory.Plant || cat == ThingCategory.Filth;
		}

		// Token: 0x040031DA RID: 12762
		public static List<IntVec3> beautyRelevantCells = new List<IntVec3>();

		// Token: 0x040031DB RID: 12763
		private static List<Room> visibleRooms = new List<Room>();

		// Token: 0x040031DC RID: 12764
		public static readonly int SampleNumCells_Beauty = GenRadial.NumCellsInRadius(8.9f);

		// Token: 0x040031DD RID: 12765
		private static List<Thing> tempCountedThings = new List<Thing>();
	}
}
