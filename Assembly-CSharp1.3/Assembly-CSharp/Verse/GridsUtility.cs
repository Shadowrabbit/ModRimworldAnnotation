using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020001B3 RID: 435
	public static class GridsUtility
	{
		// Token: 0x06000C3E RID: 3134 RVA: 0x00041CEE File Offset: 0x0003FEEE
		public static float GetTemperature(this IntVec3 loc, Map map)
		{
			return GenTemperature.GetTemperatureForCell(loc, map);
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00041CF7 File Offset: 0x0003FEF7
		public static Region GetRegion(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00041D01 File Offset: 0x0003FF01
		public static District GetDistrict(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.DistrictAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00041D0B File Offset: 0x0003FF0B
		public static Room GetRoom(this IntVec3 loc, Map map)
		{
			return RegionAndRoomQuery.RoomAt(loc, map, RegionType.Set_All);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00041D16 File Offset: 0x0003FF16
		public static Room GetRoomOrAdjacent(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAtOrAdjacent(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00041D20 File Offset: 0x0003FF20
		public static List<Thing> GetThingList(this IntVec3 c, Map map)
		{
			return map.thingGrid.ThingsListAt(c);
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00041D2E File Offset: 0x0003FF2E
		public static float GetSnowDepth(this IntVec3 c, Map map)
		{
			return map.snowGrid.GetDepth(c);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00041D3C File Offset: 0x0003FF3C
		public static bool Fogged(this Thing t)
		{
			return t.Map.fogGrid.IsFogged(t.Position);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00041D54 File Offset: 0x0003FF54
		public static bool Fogged(this IntVec3 c, Map map)
		{
			return map.fogGrid.IsFogged(c);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00041D62 File Offset: 0x0003FF62
		public static RoofDef GetRoof(this IntVec3 c, Map map)
		{
			return map.roofGrid.RoofAt(c);
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00041D70 File Offset: 0x0003FF70
		public static bool Roofed(this IntVec3 c, Map map)
		{
			return map.roofGrid.Roofed(c);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00041D80 File Offset: 0x0003FF80
		public static bool Filled(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00041DA8 File Offset: 0x0003FFA8
		public static TerrainDef GetTerrain(this IntVec3 c, Map map)
		{
			return map.terrainGrid.TerrainAt(c);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00041DB6 File Offset: 0x0003FFB6
		public static Zone GetZone(this IntVec3 c, Map map)
		{
			return map.zoneManager.ZoneAt(c);
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00041DC4 File Offset: 0x0003FFC4
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

		// Token: 0x06000C4D RID: 3149 RVA: 0x00041E14 File Offset: 0x00040014
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

		// Token: 0x06000C4E RID: 3150 RVA: 0x00041E6C File Offset: 0x0004006C
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

		// Token: 0x06000C4F RID: 3151 RVA: 0x00041EAC File Offset: 0x000400AC
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

		// Token: 0x06000C50 RID: 3152 RVA: 0x00041EF4 File Offset: 0x000400F4
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

		// Token: 0x06000C51 RID: 3153 RVA: 0x00041F40 File Offset: 0x00040140
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

		// Token: 0x06000C52 RID: 3154 RVA: 0x00041F88 File Offset: 0x00040188
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

		// Token: 0x06000C53 RID: 3155 RVA: 0x00041FD0 File Offset: 0x000401D0
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

		// Token: 0x06000C54 RID: 3156 RVA: 0x00042010 File Offset: 0x00040210
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

		// Token: 0x06000C55 RID: 3157 RVA: 0x0004204C File Offset: 0x0004024C
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

		// Token: 0x06000C56 RID: 3158 RVA: 0x00042088 File Offset: 0x00040288
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

		// Token: 0x06000C57 RID: 3159 RVA: 0x000420C4 File Offset: 0x000402C4
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

		// Token: 0x06000C58 RID: 3160 RVA: 0x00042100 File Offset: 0x00040300
		public static IPlantToGrowSettable GetPlantToGrowSettable(this IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetEdifice(map) as IPlantToGrowSettable;
			if (plantToGrowSettable == null)
			{
				plantToGrowSettable = (c.GetZone(map) as IPlantToGrowSettable);
			}
			return plantToGrowSettable;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0004212C File Offset: 0x0004032C
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

		// Token: 0x06000C5A RID: 3162 RVA: 0x00042178 File Offset: 0x00040378
		public static Building_Door GetDoor(this IntVec3 c, Map map)
		{
			Building_Door result;
			if ((result = (c.GetEdifice(map) as Building_Door)) != null)
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00042198 File Offset: 0x00040398
		public static Building GetFence(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def.IsFence)
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000421C0 File Offset: 0x000403C0
		public static Building GetEdifice(this IntVec3 c, Map map)
		{
			return map.edificeGrid[c];
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x000421CE File Offset: 0x000403CE
		public static Thing GetCover(this IntVec3 c, Map map)
		{
			return map.coverGrid[c];
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x000421DC File Offset: 0x000403DC
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

		// Token: 0x06000C5F RID: 3167 RVA: 0x00042224 File Offset: 0x00040424
		public static bool IsInPrisonCell(this IntVec3 c, Map map)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_Passable);
			if (roomOrAdjacent != null)
			{
				return roomOrAdjacent.IsPrisonCell;
			}
			Log.Error("Checking prison cell status of " + c + " which is not in or adjacent to a room.");
			return false;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00042260 File Offset: 0x00040460
		public static bool UsesOutdoorTemperature(this IntVec3 c, Map map)
		{
			Room room = c.GetRoom(map);
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
						room = array[i].GetRoom(map);
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
