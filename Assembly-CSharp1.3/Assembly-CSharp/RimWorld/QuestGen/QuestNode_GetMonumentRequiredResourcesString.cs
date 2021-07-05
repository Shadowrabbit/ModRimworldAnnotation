using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001679 RID: 5753
	public class QuestNode_GetMonumentRequiredResourcesString : QuestNode
	{
		// Token: 0x060085EB RID: 34283 RVA: 0x0030014C File Offset: 0x002FE34C
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060085EC RID: 34284 RVA: 0x00300156 File Offset: 0x002FE356
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060085ED RID: 34285 RVA: 0x00300164 File Offset: 0x002FE364
		private void DoWork(Slate slate)
		{
			MonumentMarker value = this.monumentMarker.GetValue(slate);
			if (value == null)
			{
				if (!this.storeMarketValueAs.GetValue(slate).NullOrEmpty())
				{
					slate.Set<float>(this.storeMarketValueAs.GetValue(slate), 0f, false);
				}
				return;
			}
			Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
			List<Pair<List<StuffCategoryDef>, int>> list4 = new List<Pair<List<StuffCategoryDef>, int>>();
			List<List<Pair<ThingDef, int>>> list2 = new List<List<Pair<ThingDef, int>>>();
			int num = value.sketch.Entities.Where(delegate(SketchEntity x)
			{
				SketchTerrain sketchTerrain;
				return (sketchTerrain = (x as SketchTerrain)) != null && sketchTerrain.treatSimilarAsSame;
			}).Count<SketchEntity>();
			foreach (SketchEntity sketchEntity in value.sketch.Entities)
			{
				SketchBuildable sketchBuildable = sketchEntity as SketchBuildable;
				if (sketchBuildable != null)
				{
					if (sketchBuildable.Buildable.MadeFromStuff && sketchBuildable.Stuff == null)
					{
						int num2 = this.FindStuffsIndexFor(sketchBuildable.Buildable, list4);
						if (num2 < 0)
						{
							list4.Add(new Pair<List<StuffCategoryDef>, int>(sketchBuildable.Buildable.stuffCategories, sketchBuildable.Buildable.CostStuffCount));
						}
						else
						{
							list4[num2] = new Pair<List<StuffCategoryDef>, int>(list4[num2].First, list4[num2].Second + sketchBuildable.Buildable.CostStuffCount);
						}
						if (sketchBuildable.Buildable.CostList != null)
						{
							for (int i = 0; i < sketchBuildable.Buildable.CostList.Count; i++)
							{
								ThingDefCountClass thingDefCountClass = sketchBuildable.Buildable.CostList[i];
								int num3;
								if (!dictionary.TryGetValue(thingDefCountClass.thingDef, out num3))
								{
									num3 = 0;
								}
								dictionary[thingDefCountClass.thingDef] = num3 + thingDefCountClass.count;
							}
						}
					}
					else
					{
						SketchTerrain st;
						if ((st = (sketchBuildable as SketchTerrain)) != null && st.treatSimilarAsSame)
						{
							using (IEnumerator<TerrainDef> enumerator2 = (from x in DefDatabase<TerrainDef>.AllDefs
							where st.IsSameOrSimilar(x)
							select x).GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									TerrainDef terrainDef = enumerator2.Current;
									if (!terrainDef.CostList.NullOrEmpty<ThingDefCountClass>())
									{
										List<Pair<ThingDef, int>> list = new List<Pair<ThingDef, int>>();
										foreach (ThingDefCountClass thingDefCountClass2 in terrainDef.CostList)
										{
											list.Add(new Pair<ThingDef, int>(thingDefCountClass2.thingDef, thingDefCountClass2.count * num));
										}
										if (!list2.Any((List<Pair<ThingDef, int>> x) => x.SetsEqual(list)))
										{
											list2.Add(list);
										}
									}
								}
								continue;
							}
						}
						List<ThingDefCountClass> list3 = sketchBuildable.Buildable.CostListAdjusted(sketchBuildable.Stuff, true);
						for (int j = 0; j < list3.Count; j++)
						{
							int num4;
							if (!dictionary.TryGetValue(list3[j].thingDef, out num4))
							{
								num4 = 0;
							}
							dictionary[list3[j].thingDef] = num4 + list3[j].count;
						}
					}
				}
			}
			float num5 = 0f;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pair<List<StuffCategoryDef>, int> pair in list4)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("  - " + "AnyOf".Translate() + ": " + (from x in pair.First
				select x.label).ToCommaList(false, false) + " x" + pair.Second);
				num5 += this.GetCheapestStuffMarketValue(pair.First, pair.Second);
			}
			foreach (KeyValuePair<ThingDef, int> keyValuePair in dictionary)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("  - " + GenLabel.ThingLabel(keyValuePair.Key, null, keyValuePair.Value).CapitalizeFirst());
				num5 += keyValuePair.Key.BaseMarketValue * (float)keyValuePair.Value;
			}
			if (list2.Any<List<Pair<ThingDef, int>>>())
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("  - " + "AnyOf".Translate() + ":");
				foreach (List<Pair<ThingDef, int>> source in list2)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("    - " + (from x in source
					select x.First.label + " x" + x.Second).ToCommaList(false, false));
				}
				num5 += this.GetCheapestThingMarketValue(list2);
			}
			slate.Set<string>(this.storeAs.GetValue(slate), stringBuilder.ToString(), false);
			if (!this.storeMarketValueAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), num5, false);
			}
		}

		// Token: 0x060085EE RID: 34286 RVA: 0x003007C8 File Offset: 0x002FE9C8
		private int FindStuffsIndexFor(BuildableDef buildable, List<Pair<List<StuffCategoryDef>, int>> anyOf)
		{
			for (int i = 0; i < anyOf.Count; i++)
			{
				if (anyOf[i].First.SetsEqual(buildable.stuffCategories))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060085EF RID: 34287 RVA: 0x00300808 File Offset: 0x002FEA08
		private float GetCheapestStuffMarketValue(List<StuffCategoryDef> categories, int count)
		{
			if (!categories.Any<StuffCategoryDef>())
			{
				return 0f;
			}
			float num = float.MaxValue;
			for (int i = 0; i < categories.Count; i++)
			{
				foreach (ThingDef thingDef in GenStuff.AllowedStuffs(categories, TechLevel.Undefined))
				{
					int num2 = Mathf.Max(Mathf.RoundToInt((float)count / thingDef.VolumePerUnit), 1);
					float num3 = thingDef.BaseMarketValue * (float)num2;
					if (num3 < num)
					{
						num = num3;
					}
				}
			}
			return num;
		}

		// Token: 0x060085F0 RID: 34288 RVA: 0x003008A0 File Offset: 0x002FEAA0
		private float GetCheapestThingMarketValue(List<List<Pair<ThingDef, int>>> costs)
		{
			if (!costs.Any<List<Pair<ThingDef, int>>>())
			{
				return 0f;
			}
			float num = float.MaxValue;
			for (int i = 0; i < costs.Count; i++)
			{
				float num2 = 0f;
				for (int j = 0; j < costs[i].Count; j++)
				{
					num2 += costs[i][j].First.BaseMarketValue * (float)costs[i][j].Second;
				}
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x040053B4 RID: 21428
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053B5 RID: 21429
		[NoTranslate]
		public SlateRef<string> storeMarketValueAs;

		// Token: 0x040053B6 RID: 21430
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
