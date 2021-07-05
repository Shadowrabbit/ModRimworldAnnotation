using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000309 RID: 777
	public static class GenTemperature
	{
		// Token: 0x060013CF RID: 5071 RVA: 0x000CBDA0 File Offset: 0x000C9FA0
		public static float AverageTemperatureAtTileForTwelfth(int tile, Twelfth twelfth)
		{
			int num = 30000;
			int num2 = 300000 * (int)twelfth;
			float num3 = 0f;
			for (int i = 0; i < 120; i++)
			{
				int absTick = num2 + num + Mathf.RoundToInt((float)i / 120f * 300000f);
				num3 += GenTemperature.GetTemperatureFromSeasonAtTile(absTick, tile);
			}
			return num3 / 120f;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x000CBDFC File Offset: 0x000C9FFC
		public static float MinTemperatureAtTile(int tile)
		{
			float num = float.MaxValue;
			for (int i = 0; i < 3600000; i += 27000)
			{
				num = Mathf.Min(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x000CBE34 File Offset: 0x000CA034
		public static float MaxTemperatureAtTile(int tile)
		{
			float num = float.MinValue;
			for (int i = 0; i < 3600000; i += 27000)
			{
				num = Mathf.Max(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x000143D4 File Offset: 0x000125D4
		public static FloatRange ComfortableTemperatureRange(this Pawn p)
		{
			return new FloatRange(p.GetStatValue(StatDefOf.ComfyTemperatureMin, true), p.GetStatValue(StatDefOf.ComfyTemperatureMax, true));
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x000CBE6C File Offset: 0x000CA06C
		public static FloatRange ComfortableTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = new FloatRange(raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null), raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null));
			if (apparel != null)
			{
				result.min -= apparel.Sum((ThingStuffPair x) => x.InsulationCold);
				result.max += apparel.Sum((ThingStuffPair x) => x.InsulationHeat);
			}
			return result;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x000CBEFC File Offset: 0x000CA0FC
		public static FloatRange SafeTemperatureRange(this Pawn p)
		{
			FloatRange result = p.ComfortableTemperatureRange();
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x000CBF34 File Offset: 0x000CA134
		public static FloatRange SafeTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = GenTemperature.ComfortableTemperatureRange(raceDef, apparel);
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x000CBF6C File Offset: 0x000CA16C
		public static float GetTemperatureForCell(IntVec3 c, Map map)
		{
			float result;
			GenTemperature.TryGetTemperatureForCell(c, map, out result);
			return result;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x000CBF84 File Offset: 0x000CA184
		public static bool TryGetTemperatureForCell(IntVec3 c, Map map, out float tempResult)
		{
			if (map == null)
			{
				Log.Error("Got temperature for null map.", false);
				tempResult = 21f;
				return true;
			}
			if (!c.InBounds(map))
			{
				tempResult = 21f;
				return false;
			}
			if (GenTemperature.TryGetDirectAirTemperatureForCell(c, map, out tempResult))
			{
				return true;
			}
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return GenTemperature.TryGetAirTemperatureAroundThing(list[i], out tempResult);
				}
			}
			return false;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x000CC008 File Offset: 0x000CA208
		public static bool TryGetDirectAirTemperatureForCell(IntVec3 c, Map map, out float temperature)
		{
			if (!c.InBounds(map))
			{
				temperature = 21f;
				return false;
			}
			RoomGroup roomGroup = c.GetRoomGroup(map);
			if (roomGroup == null)
			{
				temperature = 21f;
				return false;
			}
			temperature = roomGroup.Temperature;
			return true;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x000CC044 File Offset: 0x000CA244
		public static bool TryGetAirTemperatureAroundThing(Thing t, out float temperature)
		{
			float num = 0f;
			int num2 = 0;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(t);
			for (int i = 0; i < list.Count; i++)
			{
				float num3;
				if (list[i].InBounds(t.Map) && GenTemperature.TryGetDirectAirTemperatureForCell(list[i], t.Map, out num3))
				{
					num += num3;
					num2++;
				}
			}
			if (num2 > 0)
			{
				temperature = num / (float)num2;
				return true;
			}
			temperature = 21f;
			return false;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x000CC0C0 File Offset: 0x000CA2C0
		public static float OffsetFromSunCycle(int absTick, int tile)
		{
			float num = GenDate.DayPercent((long)absTick, Find.WorldGrid.LongLatOf(tile).x);
			return Mathf.Cos(6.2831855f * (num + 0.32f)) * 7f;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000CC100 File Offset: 0x000CA300
		public static float OffsetFromSeasonCycle(int absTick, int tile)
		{
			float num = (float)(absTick / 60000 % 60) / 60f;
			return Mathf.Cos(6.2831855f * (num - Season.Winter.GetMiddleTwelfth(0f).GetBeginningYearPct())) * -GenTemperature.SeasonalShiftAmplitudeAt(tile);
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x000143F3 File Offset: 0x000125F3
		public static float GetTemperatureFromSeasonAtTile(int absTick, int tile)
		{
			if (absTick == 0)
			{
				absTick = 1;
			}
			return Find.WorldGrid[tile].temperature + GenTemperature.OffsetFromSeasonCycle(absTick, tile);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x000CC144 File Offset: 0x000CA344
		public static float GetTemperatureAtTile(int tile)
		{
			Map map = Current.Game.FindMap(tile);
			if (map != null)
			{
				return map.mapTemperature.OutdoorTemp;
			}
			return GenTemperature.GetTemperatureFromSeasonAtTile(GenTicks.TicksAbs, tile);
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x000CC178 File Offset: 0x000CA378
		public static float SeasonalShiftAmplitudeAt(int tile)
		{
			if (Find.WorldGrid.LongLatOf(tile).y >= 0f)
			{
				return TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
			}
			return -TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x000CC1C8 File Offset: 0x000CA3C8
		public static List<Twelfth> TwelfthsInAverageTemperatureRange(int tile, float minTemp, float maxTemp)
		{
			List<Twelfth> twelfths = new List<Twelfth>();
			for (int i = 0; i < 12; i++)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					twelfths.Add((Twelfth)i);
				}
			}
			if (twelfths.Count <= 1 || twelfths.Count == 12)
			{
				return twelfths;
			}
			if (twelfths.Contains(Twelfth.Twelfth) && twelfths.Contains(Twelfth.First))
			{
				int num2 = (int)twelfths.First((Twelfth m) => !twelfths.Contains((Twelfth)(m - Twelfth.Second)));
				List<Twelfth> list = new List<Twelfth>();
				int num3 = num2;
				while (num3 < 12 && twelfths.Contains((Twelfth)num3))
				{
					list.Add((Twelfth)num3);
					num3++;
				}
				int num4 = 0;
				while (num4 < 12 && twelfths.Contains((Twelfth)num4))
				{
					list.Add((Twelfth)num4);
					num4++;
				}
			}
			return twelfths;
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000CC2C8 File Offset: 0x000CA4C8
		public static Twelfth EarliestTwelfthInAverageTemperatureRange(int tile, float minTemp, float maxTemp)
		{
			int i = 0;
			while (i < 12)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					if (i != 0)
					{
						return (Twelfth)i;
					}
					Twelfth twelfth = (Twelfth)i;
					for (int j = 0; j < 12; j++)
					{
						float num2 = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth.PreviousTwelfth());
						if (num2 < minTemp || num2 > maxTemp)
						{
							return twelfth;
						}
						twelfth = twelfth.PreviousTwelfth();
					}
					return (Twelfth)i;
				}
				else
				{
					i++;
				}
			}
			return Twelfth.Undefined;
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x000CC330 File Offset: 0x000CA530
		public static bool PushHeat(IntVec3 c, Map map, float energy)
		{
			if (map == null)
			{
				Log.Error("Added heat to null map.", false);
				return false;
			}
			RoomGroup roomGroup = c.GetRoomGroup(map);
			if (roomGroup != null)
			{
				return roomGroup.PushHeat(energy);
			}
			GenTemperature.neighRoomGroups.Clear();
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = c + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(map))
				{
					roomGroup = intVec.GetRoomGroup(map);
					if (roomGroup != null)
					{
						GenTemperature.neighRoomGroups.Add(roomGroup);
					}
				}
			}
			float energy2 = energy / (float)GenTemperature.neighRoomGroups.Count;
			for (int j = 0; j < GenTemperature.neighRoomGroups.Count; j++)
			{
				GenTemperature.neighRoomGroups[j].PushHeat(energy2);
			}
			bool result = GenTemperature.neighRoomGroups.Count > 0;
			GenTemperature.neighRoomGroups.Clear();
			return result;
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x000CC3F8 File Offset: 0x000CA5F8
		public static void PushHeat(Thing t, float energy)
		{
			if (t.GetRoomGroup() != null)
			{
				GenTemperature.PushHeat(t.Position, t.Map, energy);
				return;
			}
			IntVec3 c;
			if (GenAdj.TryFindRandomAdjacentCell8WayWithRoomGroup(t, out c))
			{
				GenTemperature.PushHeat(c, t.Map, energy);
			}
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000CC43C File Offset: 0x000CA63C
		public static float ControlTemperatureTempChange(IntVec3 cell, Map map, float energyLimit, float targetTemperature)
		{
			RoomGroup roomGroup = cell.GetRoomGroup(map);
			if (roomGroup == null || roomGroup.UsesOutdoorTemperature)
			{
				return 0f;
			}
			float b = energyLimit / (float)roomGroup.CellCount;
			float a = targetTemperature - roomGroup.Temperature;
			float num;
			if (energyLimit > 0f)
			{
				num = Mathf.Min(a, b);
				num = Mathf.Max(num, 0f);
			}
			else
			{
				num = Mathf.Max(a, b);
				num = Mathf.Min(num, 0f);
			}
			return num;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x000CC4B0 File Offset: 0x000CA6B0
		public static void EqualizeTemperaturesThroughBuilding(Building b, float rate, bool twoWay)
		{
			int num = 0;
			float num2 = 0f;
			if (twoWay)
			{
				for (int i = 0; i < 2; i++)
				{
					IntVec3 intVec = (i == 0) ? (b.Position + b.Rotation.FacingCell) : (b.Position - b.Rotation.FacingCell);
					if (intVec.InBounds(b.Map))
					{
						RoomGroup roomGroup = intVec.GetRoomGroup(b.Map);
						if (roomGroup != null)
						{
							num2 += roomGroup.Temperature;
							GenTemperature.beqRoomGroups[num] = roomGroup;
							num++;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 intVec2 = b.Position + GenAdj.CardinalDirections[j];
					if (intVec2.InBounds(b.Map))
					{
						RoomGroup roomGroup2 = intVec2.GetRoomGroup(b.Map);
						if (roomGroup2 != null)
						{
							num2 += roomGroup2.Temperature;
							GenTemperature.beqRoomGroups[num] = roomGroup2;
							num++;
						}
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			float num3 = num2 / (float)num;
			RoomGroup roomGroup3 = b.GetRoomGroup();
			if (roomGroup3 != null)
			{
				roomGroup3.Temperature = num3;
			}
			if (num == 1)
			{
				return;
			}
			float num4 = 1f;
			for (int k = 0; k < num; k++)
			{
				if (!GenTemperature.beqRoomGroups[k].UsesOutdoorTemperature)
				{
					float temperature = GenTemperature.beqRoomGroups[k].Temperature;
					float num5 = (num3 - temperature) * rate;
					float num6 = num5 / (float)GenTemperature.beqRoomGroups[k].CellCount;
					float num7 = GenTemperature.beqRoomGroups[k].Temperature + num6;
					if (num5 > 0f && num7 > num3)
					{
						num7 = num3;
					}
					else if (num5 < 0f && num7 < num3)
					{
						num7 = num3;
					}
					float num8 = Mathf.Abs((num7 - temperature) * (float)GenTemperature.beqRoomGroups[k].CellCount / num5);
					if (num8 < num4)
					{
						num4 = num8;
					}
				}
			}
			for (int l = 0; l < num; l++)
			{
				if (!GenTemperature.beqRoomGroups[l].UsesOutdoorTemperature)
				{
					float temperature2 = GenTemperature.beqRoomGroups[l].Temperature;
					float num9 = (num3 - temperature2) * rate * num4 / (float)GenTemperature.beqRoomGroups[l].CellCount;
					GenTemperature.beqRoomGroups[l].Temperature += num9;
				}
			}
			for (int m = 0; m < GenTemperature.beqRoomGroups.Length; m++)
			{
				GenTemperature.beqRoomGroups[m] = null;
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00014413 File Offset: 0x00012613
		public static float RotRateAtTemperature(float temperature)
		{
			if (temperature < 0f)
			{
				return 0f;
			}
			if (temperature >= 10f)
			{
				return 1f;
			}
			return (temperature - 0f) / 10f;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x000CC708 File Offset: 0x000CA908
		public static bool FactionOwnsPassableRoomInTemperatureRange(Faction faction, FloatRange tempRange, Map map)
		{
			if (faction == Faction.OfPlayer)
			{
				List<Room> allRooms = map.regionGrid.allRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					Room room = allRooms[i];
					if (room.RegionType.Passable() && !room.Fogged && tempRange.Includes(room.Temperature))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x000CC76C File Offset: 0x000CA96C
		public static string GetAverageTemperatureLabel(int tile)
		{
			return Find.WorldGrid[tile].temperature.ToStringTemperature("F1") + " " + string.Format("({0} {1} {2})", GenTemperature.MinTemperatureAtTile(tile).ToStringTemperature("F0"), "RangeTo".Translate(), GenTemperature.MaxTemperatureAtTile(tile).ToStringTemperature("F0"));
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0001443E File Offset: 0x0001263E
		public static float CelsiusTo(float temp, TemperatureDisplayMode oldMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Celsius:
				return temp;
			case TemperatureDisplayMode.Fahrenheit:
				return temp * 1.8f + 32f;
			case TemperatureDisplayMode.Kelvin:
				return temp + 273.15f;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x00014471 File Offset: 0x00012671
		public static float CelsiusToOffset(float temp, TemperatureDisplayMode oldMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Celsius:
				return temp;
			case TemperatureDisplayMode.Fahrenheit:
				return temp * 1.8f;
			case TemperatureDisplayMode.Kelvin:
				return temp;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00014498 File Offset: 0x00012698
		public static float ConvertTemperatureOffset(float temp, TemperatureDisplayMode oldMode, TemperatureDisplayMode newMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Fahrenheit:
				temp /= 1.8f;
				break;
			}
			switch (newMode)
			{
			case TemperatureDisplayMode.Fahrenheit:
				temp *= 1.8f;
				break;
			}
			return temp;
		}

		// Token: 0x04000FA2 RID: 4002
		public static readonly Color ColorSpotHot = new Color(1f, 0f, 0f, 0.6f);

		// Token: 0x04000FA3 RID: 4003
		public static readonly Color ColorSpotCold = new Color(0f, 0f, 1f, 0.6f);

		// Token: 0x04000FA4 RID: 4004
		public static readonly Color ColorRoomHot = new Color(1f, 0f, 0f, 0.3f);

		// Token: 0x04000FA5 RID: 4005
		public static readonly Color ColorRoomCold = new Color(0f, 0f, 1f, 0.3f);

		// Token: 0x04000FA6 RID: 4006
		private static List<RoomGroup> neighRoomGroups = new List<RoomGroup>();

		// Token: 0x04000FA7 RID: 4007
		private static RoomGroup[] beqRoomGroups = new RoomGroup[4];
	}
}
