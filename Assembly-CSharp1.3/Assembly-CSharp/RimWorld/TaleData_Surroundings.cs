using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001026 RID: 4134
	public class TaleData_Surroundings : TaleData
	{
		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x0600619C RID: 24988 RVA: 0x00212A05 File Offset: 0x00210C05
		public bool Outdoors
		{
			get
			{
				return this.weather != null;
			}
		}

		// Token: 0x0600619D RID: 24989 RVA: 0x00212A10 File Offset: 0x00210C10
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.tile, "tile", 0, false);
			Scribe_Values.Look<float>(ref this.temperature, "temperature", 0f, false);
			Scribe_Values.Look<float>(ref this.snowDepth, "snowDepth", 0f, false);
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
			Scribe_Defs.Look<RoomRoleDef>(ref this.roomRole, "roomRole");
			Scribe_Values.Look<float>(ref this.roomImpressiveness, "roomImpressiveness", 0f, false);
			Scribe_Values.Look<float>(ref this.roomBeauty, "roomBeauty", 0f, false);
			Scribe_Values.Look<float>(ref this.roomCleanliness, "roomCleanliness", 0f, false);
		}

		// Token: 0x0600619E RID: 24990 RVA: 0x00212ABD File Offset: 0x00210CBD
		public override IEnumerable<Rule> GetRules(Dictionary<string, string> constants = null)
		{
			yield return new Rule_String("BIOME", Find.WorldGrid[this.tile].biome.label);
			if (this.roomRole != null && this.roomRole != RoomRoleDefOf.None)
			{
				yield return new Rule_String("ROOM_role", this.roomRole.label);
				yield return new Rule_String("ROOM_roleDefinite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.roomRole.label, false, false));
				yield return new Rule_String("ROOM_roleIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.roomRole.label, false, false));
				RoomStatScoreStage impressiveness = RoomStatDefOf.Impressiveness.GetScoreStage(this.roomImpressiveness);
				RoomStatScoreStage beauty = RoomStatDefOf.Beauty.GetScoreStage(this.roomBeauty);
				RoomStatScoreStage cleanliness = RoomStatDefOf.Cleanliness.GetScoreStage(this.roomCleanliness);
				yield return new Rule_String("ROOM_impressiveness", impressiveness.label);
				yield return new Rule_String("ROOM_impressivenessIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(impressiveness.label, false, false));
				yield return new Rule_String("ROOM_beauty", beauty.label);
				yield return new Rule_String("ROOM_beautyIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(beauty.label, false, false));
				yield return new Rule_String("ROOM_cleanliness", cleanliness.label);
				yield return new Rule_String("ROOM_cleanlinessIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(cleanliness.label, false, false));
				impressiveness = null;
				beauty = null;
				cleanliness = null;
			}
			yield break;
		}

		// Token: 0x0600619F RID: 24991 RVA: 0x00212AD0 File Offset: 0x00210CD0
		public static TaleData_Surroundings GenerateFrom(IntVec3 c, Map map)
		{
			TaleData_Surroundings taleData_Surroundings = new TaleData_Surroundings();
			taleData_Surroundings.tile = map.Tile;
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			if (roomOrAdjacent != null)
			{
				if (roomOrAdjacent.PsychologicallyOutdoors)
				{
					taleData_Surroundings.weather = map.weatherManager.CurWeatherPerceived;
				}
				taleData_Surroundings.roomRole = roomOrAdjacent.Role;
				taleData_Surroundings.roomImpressiveness = roomOrAdjacent.GetStat(RoomStatDefOf.Impressiveness);
				taleData_Surroundings.roomBeauty = roomOrAdjacent.GetStat(RoomStatDefOf.Beauty);
				taleData_Surroundings.roomCleanliness = roomOrAdjacent.GetStat(RoomStatDefOf.Cleanliness);
			}
			if (!GenTemperature.TryGetTemperatureForCell(c, map, out taleData_Surroundings.temperature))
			{
				taleData_Surroundings.temperature = 21f;
			}
			taleData_Surroundings.snowDepth = map.snowGrid.GetDepth(c);
			return taleData_Surroundings;
		}

		// Token: 0x060061A0 RID: 24992 RVA: 0x00212B81 File Offset: 0x00210D81
		public static TaleData_Surroundings GenerateRandom(Map map)
		{
			return TaleData_Surroundings.GenerateFrom(CellFinder.RandomCell(map), map);
		}

		// Token: 0x040037AC RID: 14252
		public int tile;

		// Token: 0x040037AD RID: 14253
		public float temperature;

		// Token: 0x040037AE RID: 14254
		public float snowDepth;

		// Token: 0x040037AF RID: 14255
		public WeatherDef weather;

		// Token: 0x040037B0 RID: 14256
		public RoomRoleDef roomRole;

		// Token: 0x040037B1 RID: 14257
		public float roomImpressiveness;

		// Token: 0x040037B2 RID: 14258
		public float roomBeauty;

		// Token: 0x040037B3 RID: 14259
		public float roomCleanliness;
	}
}
