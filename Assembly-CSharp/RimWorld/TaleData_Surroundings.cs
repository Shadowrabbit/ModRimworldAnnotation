using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001636 RID: 5686
	public class TaleData_Surroundings : TaleData
	{
		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06007B9C RID: 31644 RVA: 0x000530F7 File Offset: 0x000512F7
		public bool Outdoors
		{
			get
			{
				return this.weather != null;
			}
		}

		// Token: 0x06007B9D RID: 31645 RVA: 0x002518B4 File Offset: 0x0024FAB4
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

		// Token: 0x06007B9E RID: 31646 RVA: 0x00053102 File Offset: 0x00051302
		public override IEnumerable<Rule> GetRules()
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

		// Token: 0x06007B9F RID: 31647 RVA: 0x00251964 File Offset: 0x0024FB64
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

		// Token: 0x06007BA0 RID: 31648 RVA: 0x00053112 File Offset: 0x00051312
		public static TaleData_Surroundings GenerateRandom(Map map)
		{
			return TaleData_Surroundings.GenerateFrom(CellFinder.RandomCell(map), map);
		}

		// Token: 0x040050FF RID: 20735
		public int tile;

		// Token: 0x04005100 RID: 20736
		public float temperature;

		// Token: 0x04005101 RID: 20737
		public float snowDepth;

		// Token: 0x04005102 RID: 20738
		public WeatherDef weather;

		// Token: 0x04005103 RID: 20739
		public RoomRoleDef roomRole;

		// Token: 0x04005104 RID: 20740
		public float roomImpressiveness;

		// Token: 0x04005105 RID: 20741
		public float roomBeauty;

		// Token: 0x04005106 RID: 20742
		public float roomCleanliness;
	}
}
