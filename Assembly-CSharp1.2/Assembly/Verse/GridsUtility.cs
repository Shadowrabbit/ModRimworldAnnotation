using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200026A RID: 618
	public static class GridsUtility
	{
		// Token: 0x06000FBC RID: 4028 RVA: 0x00011D18 File Offset: 0x0000FF18
		public static float GetTemperature(this IntVec3 loc, Map map)
		{
			return GenTemperature.GetTemperatureForCell(loc, map);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00011D21 File Offset: 0x0000FF21
		public static Region GetRegion(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00011D2B File Offset: 0x0000FF2B
		public static Room GetRoom(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00011D35 File Offset: 0x0000FF35
		public static RoomGroup GetRoomGroup(this IntVec3 loc, Map map)
		{
			return RegionAndRoomQuery.RoomGroupAt(loc, map);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00011D3E File Offset: 0x0000FF3E
		public static Room GetRoomOrAdjacent(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAtOrAdjacent(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00011D48 File Offset: 0x0000FF48
		public static List<Thing> GetThingList(this IntVec3 c, Map map)
		{
			return map.thingGrid.ThingsListAt(c);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00011D56 File Offset: 0x0000FF56
		public static float GetSnowDepth(this IntVec3 c, Map map)
		{
			return map.snowGrid.GetDepth(c);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00011D64 File Offset: 0x0000FF64
		public static bool Fogged(this Thing t)
		{
			return t.Map.fogGrid.IsFogged(t.Position);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00011D7C File Offset: 0x0000FF7C
		public static bool Fogged(this IntVec3 c, Map map)
		{
			return map.fogGrid.IsFogged(c);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00011D8A File Offset: 0x0000FF8A
		public static RoofDef GetRoof(this IntVec3 c, Map map)
		{
			return map.roofGrid.RoofAt(c);
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00011D98 File Offset: 0x0000FF98
		public static bool Roofed(this IntVec3 c, Map map)
		{
			return map.roofGrid.Roofed(c);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x000B7658 File Offset: 0x000B5858
		public static bool Filled(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00011DA6 File Offset: 0x0000FFA6
		public static TerrainDef GetTerrain(this IntVec3 c, Map map)
		{
			return map.terrainGrid.TerrainAt(c);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00011DB4 File Offset: 0x0000FFB4
		public static Zone GetZone(this IntVec3 c, Map map)
		{
			return map.zoneManager.ZoneAt(c);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x000B7680 File Offset: 0x000B5880
		public static Plant GetPlant(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Plant)
				{
					return (Plant)list[i];
				}
			}
			return null;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x000B76D0 File Offset: 0x000B58D0
		public static Thing GetRoofHolderOrImpassable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.holdsRoof || thingList[i].def.passability == Traversability.Impassable)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x000B7728 File Offset: 0x000B5928
		public static Thing GetFirstThing(this IntVec3 c, Map map, ThingDef def)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == def)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x000B7768 File Offset: 0x000B5968
		public static ThingWithComps GetFirstThingWithComp<TComp>(this IntVec3 c, Map map) where TComp : ThingComp
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].TryGetComp<TComp>() != null)
				{
					return (ThingWithComps)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x000B77B0 File Offset: 0x000B59B0
		public static T GetFirstThing<T>(this IntVec3 c, Map map) where T : Thing
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				T t = thingList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x000B77FC File Offset: 0x000B59FC
		public static Thing GetFirstHaulable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.designateHaulable)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x000B7844 File Offset: 0x000B5A44
		public static Thing GetFirstItem(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x000B788C File Offset: 0x000B5A8C
		public static Building GetFirstBuilding(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Building building = list[i] as Building;
				if (building != null)
				{
					return building;
				}
			}
			return null;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x000B78CC File Offset: 0x000B5ACC
		public static Pawn GetFirstPawn(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x000B7908 File Offset: 0x000B5B08
		public static Mineable GetFirstMineable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Mineable mineable = thingList[i] as Mineable;
				if (mineable != null)
				{
					return mineable;
				}
			}
			return null;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x000B7944 File Offset: 0x000B5B44
		public static Blight GetFirstBlight(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Blight blight = thingList[i] as Blight;
				if (blight != null)
				{
					return blight;
				}
			}
			return null;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x000B7980 File Offset: 0x000B5B80
		public static Skyfaller GetFirstSkyfaller(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Skyfaller skyfaller = thingList[i] as Skyfaller;
				if (skyfaller != null)
				{
					return skyfaller;
				}
			}
			return null;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x000B79BC File Offset: 0x000B5BBC
		public static IPlantToGrowSettable GetPlantToGrowSettable(this IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetEdifice(map) as IPlantToGrowSettable;
			if (plantToGrowSettable == null)
			{
				plantToGrowSettable = (c.GetZone(map) as IPlantToGrowSettable);
			}
			return plantToGrowSettable;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000B79E8 File Offset: 0x000B5BE8
		public static Building GetTransmitter(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.EverTransmitsPower)
				{
					return (Building)list[i];
				}
			}
			return null;
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x000B7A34 File Offset: 0x000B5C34
		public static Building_Door GetDoor(this IntVec3 c, Map map)
		{
			Building_Door result;
			if ((result = (c.GetEdifice(map) as Building_Door)) != null)
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00011DC2 File Offset: 0x0000FFC2
		public static Building GetEdifice(this IntVec3 c, Map map)
		{
			return map.edificeGrid[c];
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00011DD0 File Offset: 0x0000FFD0
		public static Thing GetCover(this IntVec3 c, Map map)
		{
			return map.coverGrid[c];
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x000B7A54 File Offset: 0x000B5C54
		public static Gas GetGas(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Gas)
				{
					return (Gas)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x000B7A9C File Offset: 0x000B5C9C
		public static bool IsInPrisonCell(this IntVec3 c, Map map)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_Passable);
			if (roomOrAdjacent != null)
			{
				return roomOrAdjacent.isPrisonCell;
			}
			Log.Error("Checking prison cell status of " + c + " which is not in or adjacent to a room.", false);
			return false;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x000B7AD8 File Offset: 0x000B5CD8
		public static bool UsesOutdoorTemperature(this IntVec3 c, Map map)
		{
			Room room = c.GetRoom(map, RegionType.Set_All);
			if (room != null)
			{
				return room.UsesOutdoorTemperature;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice != null)
			{
				IntVec3[] array = GenAdj.CellsAdjacent8Way(edifice).ToArray<IntVec3>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].InBounds(map))
					{
						room = array[i].GetRoom(map, RegionType.Set_All);
						if (room != null && room.UsesOutdoorTemperature)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}
	}
}
