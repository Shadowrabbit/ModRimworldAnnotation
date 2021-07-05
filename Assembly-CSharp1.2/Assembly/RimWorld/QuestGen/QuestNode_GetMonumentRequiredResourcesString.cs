using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F2C RID: 7980
	public class QuestNode_GetMonumentRequiredResourcesString : QuestNode
	{
		// Token: 0x0600AA95 RID: 43669 RVA: 0x0006FBDD File Offset: 0x0006DDDD
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600AA96 RID: 43670 RVA: 0x0006FBE7 File Offset: 0x0006DDE7
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AA97 RID: 43671 RVA: 0x0031BEF0 File Offset: 0x0031A0F0
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
							list4.Add(new Pair<List<StuffCategoryDef>, int>(sketchBuildable.Buildable.stuffCategories, sketchBuildable.Buildable.costStuffCount));
						}
						else
						{
							list4[num2] = new Pair<List<StuffCategoryDef>, int>(list4[num2].First, list4[num2].Second + sketchBuildable.Buildable.costStuffCount);
						}
						if (sketchBuildable.Buildable.costList != null)
						{
							for (int i = 0; i < sketchBuildable.Buildable.costList.Count; i++)
							{
								ThingDefCountClass thingDefCountClass = sketchBuildable.Buildable.costList[i];
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
									if (!terrainDef.costList.NullOrEmpty<ThingDefCountClass>())
									{
										List<Pair<ThingDef, int>> list = new List<Pair<ThingDef, int>>();
										foreach (ThingDefCountClass thingDefCountClass2 in terrainDef.costList)
										{
											list.Add(new Pair<ThingDef, int>(thingDefCountClass2.thingDef, thingDefCountClass2.count * num));
										}
										if (!list2.Any((List<Pair<ThingDef, int>> x) => x.ListsEqualIgnoreOrder(list)))
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
				select x.label).ToCommaList(false) + " x" + pair.Second);
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
					select x.First.label + " x" + x.Second).ToCommaList(false));
				}
				num5 += this.GetCheapestThingMarketValue(list2);
			}
			slate.Set<string>(this.storeAs.GetValue(slate), stringBuilder.ToString(), false);
			if (!this.storeMarketValueAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), num5, false);
			}
		}

		// Token: 0x0600AA98 RID: 43672 RVA: 0x0031C554 File Offset: 0x0031A754
		private int FindStuffsIndexFor(BuildableDef buildable, List<Pair<List<StuffCategoryDef>, int>> anyOf)
		{
			for (int i = 0; i < anyOf.Count; i++)
			{
				if (anyOf[i].First.ListsEqualIgnoreOrder(buildable.stuffCategories))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600AA99 RID: 43673 RVA: 0x0031C594 File Offset: 0x0031A794
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

		// Token: 0x0600AA9A RID: 43674 RVA: 0x0031C62C File Offset: 0x0031A82C
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

		// Token: 0x040073EB RID: 29675
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040073EC RID: 29676
		[NoTranslate]
		public SlateRef<string> storeMarketValueAs;

		// Token: 0x040073ED RID: 29677
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
