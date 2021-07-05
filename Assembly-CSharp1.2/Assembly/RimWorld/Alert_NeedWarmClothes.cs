using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194A RID: 6474
	public class Alert_NeedWarmClothes : Alert
	{
		// Token: 0x06008F6A RID: 36714 RVA: 0x00060139 File Offset: 0x0005E339
		public Alert_NeedWarmClothes()
		{
			this.defaultLabel = "NeedWarmClothes".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F6B RID: 36715 RVA: 0x00294738 File Offset: 0x00292938
		private int NeededWarmClothesCount(Map map)
		{
			int num = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonists)
			{
				if (pawn.Spawned || pawn.BrieflyDespawned())
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008F6C RID: 36716 RVA: 0x002947A0 File Offset: 0x002929A0
		private int ColonistsWithWarmClothesCount(Map map)
		{
			float num = this.LowestTemperatureComing(map);
			int num2 = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonists)
			{
				if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.GetStatValue(StatDefOf.ComfyTemperatureMin, true) <= num)
				{
					num2++;
				}
			}
			return num2;
		}

		// Token: 0x06008F6D RID: 36717 RVA: 0x00294820 File Offset: 0x00292A20
		private void GetColonistsWithoutWarmClothes(Map map, List<Pawn> outResult)
		{
			outResult.Clear();
			float num = this.LowestTemperatureComing(map);
			foreach (Pawn pawn in map.mapPawns.FreeColonists)
			{
				if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.GetStatValue(StatDefOf.ComfyTemperatureMin, true) > num)
				{
					outResult.Add(pawn);
				}
			}
		}

		// Token: 0x06008F6E RID: 36718 RVA: 0x002948A8 File Offset: 0x00292AA8
		private int FreeWarmClothesSetsCount(Map map)
		{
			Alert_NeedWarmClothes.jackets.Clear();
			Alert_NeedWarmClothes.shirts.Clear();
			Alert_NeedWarmClothes.pants.Clear();
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsInAnyStorage() && list[i].GetStatValue(StatDefOf.Insulation_Cold, true) > 0f)
				{
					if (list[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						if (list[i].def.apparel.layers.Contains(ApparelLayerDefOf.OnSkin))
						{
							Alert_NeedWarmClothes.shirts.Add(list[i]);
						}
						else
						{
							Alert_NeedWarmClothes.jackets.Add(list[i]);
						}
					}
					if (list[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs))
					{
						Alert_NeedWarmClothes.pants.Add(list[i]);
					}
				}
			}
			Alert_NeedWarmClothes.jackets.Sort(Alert_NeedWarmClothes.SortByInsulationCold);
			Alert_NeedWarmClothes.shirts.Sort(Alert_NeedWarmClothes.SortByInsulationCold);
			Alert_NeedWarmClothes.pants.Sort(Alert_NeedWarmClothes.SortByInsulationCold);
			float num = ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - this.LowestTemperatureComing(map);
			if (num <= 0f)
			{
				return GenMath.Max(Alert_NeedWarmClothes.jackets.Count, Alert_NeedWarmClothes.shirts.Count, Alert_NeedWarmClothes.pants.Count);
			}
			int num2 = 0;
			while (Alert_NeedWarmClothes.jackets.Any<Thing>() || Alert_NeedWarmClothes.shirts.Any<Thing>() || Alert_NeedWarmClothes.pants.Any<Thing>())
			{
				float num3 = 0f;
				if (Alert_NeedWarmClothes.jackets.Any<Thing>())
				{
					Thing thing = Alert_NeedWarmClothes.jackets[Alert_NeedWarmClothes.jackets.Count - 1];
					Alert_NeedWarmClothes.jackets.RemoveLast<Thing>();
					num3 += thing.GetStatValue(StatDefOf.Insulation_Cold, true);
				}
				if (num3 < num && Alert_NeedWarmClothes.shirts.Any<Thing>())
				{
					Thing thing2 = Alert_NeedWarmClothes.shirts[Alert_NeedWarmClothes.shirts.Count - 1];
					Alert_NeedWarmClothes.shirts.RemoveLast<Thing>();
					num3 += thing2.GetStatValue(StatDefOf.Insulation_Cold, true);
				}
				if (num3 < num && Alert_NeedWarmClothes.pants.Any<Thing>())
				{
					for (int j = 0; j < Alert_NeedWarmClothes.pants.Count; j++)
					{
						float statValue = Alert_NeedWarmClothes.pants[j].GetStatValue(StatDefOf.Insulation_Cold, true);
						if (statValue + num3 >= num)
						{
							num3 += statValue;
							Alert_NeedWarmClothes.pants.RemoveAt(j);
							break;
						}
					}
				}
				if (num3 < num)
				{
					break;
				}
				num2++;
			}
			Alert_NeedWarmClothes.jackets.Clear();
			Alert_NeedWarmClothes.shirts.Clear();
			Alert_NeedWarmClothes.pants.Clear();
			return num2;
		}

		// Token: 0x06008F6F RID: 36719 RVA: 0x00060168 File Offset: 0x0005E368
		private int MissingWarmClothesCount(Map map)
		{
			if (this.LowestTemperatureComing(map) >= ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
			{
				return 0;
			}
			return Mathf.Max(this.NeededWarmClothesCount(map) - this.ColonistsWithWarmClothesCount(map) - this.FreeWarmClothesSetsCount(map), 0);
		}

		// Token: 0x06008F70 RID: 36720 RVA: 0x00294B7C File Offset: 0x00292D7C
		private float LowestTemperatureComing(Map map)
		{
			Twelfth twelfth = GenLocalDate.Twelfth(map);
			float a = this.GetTemperature(twelfth, map);
			for (int i = 0; i < 3; i++)
			{
				twelfth = twelfth.NextTwelfth();
				a = Mathf.Min(a, this.GetTemperature(twelfth, map));
			}
			return Mathf.Min(a, map.mapTemperature.OutdoorTemp);
		}

		// Token: 0x06008F71 RID: 36721 RVA: 0x00294BCC File Offset: 0x00292DCC
		public override TaggedString GetExplanation()
		{
			Map map = this.MapWithMissingWarmClothes();
			if (map == null)
			{
				return "";
			}
			int num = this.MissingWarmClothesCount(map);
			if (num == this.NeededWarmClothesCount(map))
			{
				return "NeedWarmClothesDesc1All".Translate() + "\n\n" + "NeedWarmClothesDesc2".Translate(this.LowestTemperatureComing(map).ToStringTemperature("F0"));
			}
			return "NeedWarmClothesDesc1".Translate(num) + "\n\n" + "NeedWarmClothesDesc2".Translate(this.LowestTemperatureComing(map).ToStringTemperature("F0"));
		}

		// Token: 0x06008F72 RID: 36722 RVA: 0x00294C7C File Offset: 0x00292E7C
		public override AlertReport GetReport()
		{
			Map map = this.MapWithMissingWarmClothes();
			if (map == null)
			{
				return false;
			}
			this.colonistsWithoutWarmClothes.Clear();
			this.GetColonistsWithoutWarmClothes(map, this.colonistsWithoutWarmClothes);
			return AlertReport.CulpritsAre(this.colonistsWithoutWarmClothes);
		}

		// Token: 0x06008F73 RID: 36723 RVA: 0x00294CC0 File Offset: 0x00292EC0
		private Map MapWithMissingWarmClothes()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && this.LowestTemperatureComing(map) < 5f && this.MissingWarmClothesCount(map) > 0)
				{
					return map;
				}
			}
			return null;
		}

		// Token: 0x06008F74 RID: 36724 RVA: 0x000601A2 File Offset: 0x0005E3A2
		private float GetTemperature(Twelfth twelfth, Map map)
		{
			return GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth);
		}

		// Token: 0x04005B62 RID: 23394
		private static List<Thing> jackets = new List<Thing>();

		// Token: 0x04005B63 RID: 23395
		private static List<Thing> shirts = new List<Thing>();

		// Token: 0x04005B64 RID: 23396
		private static List<Thing> pants = new List<Thing>();

		// Token: 0x04005B65 RID: 23397
		private const float MedicinePerColonistThreshold = 2f;

		// Token: 0x04005B66 RID: 23398
		private const int CheckNextTwelfthsCount = 3;

		// Token: 0x04005B67 RID: 23399
		private const float CanShowAlertOnlyIfTempBelow = 5f;

		// Token: 0x04005B68 RID: 23400
		private static Comparison<Thing> SortByInsulationCold = (Thing a, Thing b) => a.GetStatValue(StatDefOf.Insulation_Cold, true).CompareTo(b.GetStatValue(StatDefOf.Insulation_Cold, true));

		// Token: 0x04005B69 RID: 23401
		private List<Pawn> colonistsWithoutWarmClothes = new List<Pawn>();
	}
}
