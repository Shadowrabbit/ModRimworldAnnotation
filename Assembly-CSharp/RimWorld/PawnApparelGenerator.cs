using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F5 RID: 5109
	public static class PawnApparelGenerator
	{
		// Token: 0x06006E76 RID: 28278 RVA: 0x0004ACFD File Offset: 0x00048EFD
		static PawnApparelGenerator()
		{
			PawnApparelGenerator.Reset();
		}

		// Token: 0x06006E77 RID: 28279 RVA: 0x0021DB0C File Offset: 0x0021BD0C
		public static void Reset()
		{
			PawnApparelGenerator.allApparelPairs = ThingStuffPair.AllWith((ThingDef td) => td.IsApparel);
			PawnApparelGenerator.freeWarmParkaMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Parka, ThingDefOf.Cloth) * 1.3f));
			PawnApparelGenerator.freeWarmHatMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Tuque, ThingDefOf.Cloth) * 1.3f));
		}

		// Token: 0x06006E78 RID: 28280 RVA: 0x0021DB90 File Offset: 0x0021BD90
		public static void GenerateStartingApparelFor(Pawn pawn, PawnGenerationRequest request)
		{
			if (!pawn.RaceProps.ToolUser || !pawn.RaceProps.IsFlesh)
			{
				return;
			}
			pawn.apparel.DestroyAll(DestroyMode.Vanish);
			float randomInRange = pawn.kindDef.apparelMoney.RandomInRange;
			float mapTemperature;
			NeededWarmth neededWarmth = PawnApparelGenerator.ApparelWarmthNeededNow(pawn, request, out mapTemperature);
			bool allowHeadgear = Rand.Value < pawn.kindDef.apparelAllowHeadgearChance;
			PawnApparelGenerator.debugSb = null;
			if (DebugViewSettings.logApparelGeneration)
			{
				PawnApparelGenerator.debugSb = new StringBuilder();
				PawnApparelGenerator.debugSb.AppendLine("Generating apparel for " + pawn);
				PawnApparelGenerator.debugSb.AppendLine("Money: " + randomInRange.ToString("F0"));
				PawnApparelGenerator.debugSb.AppendLine("Needed warmth: " + neededWarmth);
				PawnApparelGenerator.debugSb.AppendLine("Headgear allowed: " + allowHeadgear.ToString());
			}
			int @int = Rand.Int;
			PawnApparelGenerator.tmpApparelCandidates.Clear();
			for (int i = 0; i < PawnApparelGenerator.allApparelPairs.Count; i++)
			{
				ThingStuffPair thingStuffPair = PawnApparelGenerator.allApparelPairs[i];
				if (PawnApparelGenerator.CanUsePair(thingStuffPair, pawn, randomInRange, allowHeadgear, @int))
				{
					PawnApparelGenerator.tmpApparelCandidates.Add(thingStuffPair);
				}
			}
			if (randomInRange < 0.001f)
			{
				PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, PawnApparelGenerator.tmpApparelCandidates);
			}
			else
			{
				int num = 0;
				for (;;)
				{
					PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, PawnApparelGenerator.tmpApparelCandidates);
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.Append(num.ToString().PadRight(5) + "Trying: " + PawnApparelGenerator.workingSet.ToString());
					}
					if (num >= 10 || Rand.Value >= 0.85f || randomInRange >= 9999999f)
					{
						goto IL_234;
					}
					float num2 = Rand.Range(0.45f, 0.8f);
					float totalPrice = PawnApparelGenerator.workingSet.TotalPrice;
					if (totalPrice >= randomInRange * num2)
					{
						goto IL_234;
					}
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.AppendLine(string.Concat(new string[]
						{
							" -- Failed: Spent $",
							totalPrice.ToString("F0"),
							", < ",
							(num2 * 100f).ToString("F0"),
							"% of money."
						}));
					}
					IL_37D:
					num++;
					continue;
					IL_234:
					if (num < 20 && Rand.Value < 0.97f && !PawnApparelGenerator.workingSet.Covers(BodyPartGroupDefOf.Torso))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Does not cover torso.");
							goto IL_37D;
						}
						goto IL_37D;
					}
					else if (num < 30 && Rand.Value < 0.8f && PawnApparelGenerator.workingSet.CoatButNoShirt())
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Coat but no shirt.");
							goto IL_37D;
						}
						goto IL_37D;
					}
					else
					{
						if (num < 50)
						{
							bool mustBeSafe = num < 17;
							if (!PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, mustBeSafe, mapTemperature))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Wrong warmth.");
									goto IL_37D;
								}
								goto IL_37D;
							}
						}
						if (num >= 80 || !PawnApparelGenerator.workingSet.IsNaked(pawn.gender))
						{
							break;
						}
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Naked.");
							goto IL_37D;
						}
						goto IL_37D;
					}
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.Append(string.Concat(new object[]
					{
						" -- Approved! Total price: $",
						PawnApparelGenerator.workingSet.TotalPrice.ToString("F0"),
						", TotalInsulationCold: ",
						PawnApparelGenerator.workingSet.TotalInsulationCold
					}));
				}
			}
			if ((!pawn.kindDef.apparelIgnoreSeasons || request.ForceAddFreeWarmLayerIfNeeded) && !PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, true, mapTemperature))
			{
				PawnApparelGenerator.workingSet.AddFreeWarmthAsNeeded(neededWarmth, mapTemperature);
			}
			if (DebugViewSettings.logApparelGeneration)
			{
				Log.Message(PawnApparelGenerator.debugSb.ToString(), false);
			}
			PawnApparelGenerator.workingSet.GiveToPawn(pawn);
			PawnApparelGenerator.workingSet.Reset(null, null);
			if (pawn.kindDef.apparelColor != Color.white)
			{
				List<Apparel> wornApparel = pawn.apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					wornApparel[j].SetColor(pawn.kindDef.apparelColor, false);
				}
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				foreach (SpecificApparelRequirement specificApparelRequirement in from x in specificApparelRequirements
				where x.Color != default(Color)
				select x)
				{
					List<Apparel> wornApparel2 = pawn.apparel.WornApparel;
					for (int k = 0; k < wornApparel2.Count; k++)
					{
						if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirement, wornApparel2[k].def))
						{
							wornApparel2[k].SetColor(specificApparelRequirement.Color, false);
						}
					}
				}
			}
			foreach (Apparel thing in pawn.apparel.WornApparel)
			{
				CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
				if (compBiocodable != null && Rand.Chance(request.BiocodeApparelChance))
				{
					compBiocodable.CodeFor(pawn);
				}
			}
		}

		// Token: 0x06006E79 RID: 28281 RVA: 0x0021E0FC File Offset: 0x0021C2FC
		private static void GenerateWorkingPossibleApparelSetFor(Pawn pawn, float money, List<ThingStuffPair> apparelCandidates)
		{
			PawnApparelGenerator.workingSet.Reset(pawn.RaceProps.body, pawn.def);
			float num = money;
			List<SpecificApparelRequirement> att = pawn.kindDef.specificApparelRequirements;
			if (att != null)
			{
				int l;
				int i;
				for (i = 0; i < att.Count; i = l + 1)
				{
					if (!att[i].RequiredTag.NullOrEmpty() || !att[i].AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>())
					{
						ThingStuffPair pair;
						if ((from pa in PawnApparelGenerator.allApparelPairs
						where PawnApparelGenerator.ApparelRequirementTagsMatch(att[i], pa.thing) && PawnApparelGenerator.ApparelRequirementHandlesThing(att[i], pa.thing) && PawnApparelGenerator.CanUseStuff(pawn, pa) && pa.thing.apparel.CorrectGenderForWearing(pawn.gender) && !PawnApparelGenerator.workingSet.PairOverlapsAnything(pa)
						select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair))
						{
							PawnApparelGenerator.workingSet.Add(pair);
							num -= pair.Price;
						}
					}
					l = i;
				}
			}
			List<ThingDef> reqApparel = pawn.kindDef.apparelRequired;
			if (reqApparel != null)
			{
				int l;
				int i;
				for (i = 0; i < reqApparel.Count; i = l + 1)
				{
					ThingStuffPair pair2;
					if ((from pa in PawnApparelGenerator.allApparelPairs
					where pa.thing == reqApparel[i] && PawnApparelGenerator.CanUseStuff(pawn, pa) && !PawnApparelGenerator.workingSet.PairOverlapsAnything(pa)
					select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair2))
					{
						PawnApparelGenerator.workingSet.Add(pair2);
						num -= pair2.Price;
					}
					l = i;
				}
			}
			PawnApparelGenerator.usableApparel.Clear();
			for (int j = 0; j < apparelCandidates.Count; j++)
			{
				if (!PawnApparelGenerator.workingSet.PairOverlapsAnything(apparelCandidates[j]))
				{
					PawnApparelGenerator.usableApparel.Add(apparelCandidates[j]);
				}
			}
			Func<ThingStuffPair, bool> <>9__4;
			while (Rand.Value >= 0.1f || money >= 9999999f)
			{
				IEnumerable<ThingStuffPair> source = PawnApparelGenerator.usableApparel;
				Func<ThingStuffPair, bool> predicate;
				if ((predicate = <>9__4) == null)
				{
					predicate = (<>9__4 = ((ThingStuffPair pa) => PawnApparelGenerator.CanUseStuff(pawn, pa)));
				}
				ThingStuffPair pair3;
				if (!source.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair3))
				{
					break;
				}
				PawnApparelGenerator.workingSet.Add(pair3);
				num -= pair3.Price;
				for (int k = PawnApparelGenerator.usableApparel.Count - 1; k >= 0; k--)
				{
					if (PawnApparelGenerator.usableApparel[k].Price > num || PawnApparelGenerator.workingSet.PairOverlapsAnything(PawnApparelGenerator.usableApparel[k]))
					{
						PawnApparelGenerator.usableApparel.RemoveAt(k);
					}
				}
			}
		}

		// Token: 0x06006E7A RID: 28282 RVA: 0x0021E41C File Offset: 0x0021C61C
		private static bool CanUseStuff(Pawn pawn, ThingStuffPair pair)
		{
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (!PawnApparelGenerator.ApparelRequirementCanUseStuff(specificApparelRequirements[i], pair))
					{
						return false;
					}
				}
			}
			return pair.stuff == null || pawn.Faction == null || pawn.Faction.def.CanUseStuffForApparel(pair.stuff);
		}

		// Token: 0x06006E7B RID: 28283 RVA: 0x0021E488 File Offset: 0x0021C688
		public static bool IsDerpApparel(ThingDef thing, ThingDef stuff)
		{
			if (stuff == null)
			{
				return false;
			}
			if (!thing.IsApparel)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < thing.stuffCategories.Count; i++)
			{
				if (thing.stuffCategories[i] != StuffCategoryDefOf.Woody && thing.stuffCategories[i] != StuffCategoryDefOf.Stony)
				{
					flag = true;
					break;
				}
			}
			return flag && (stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Woody) || stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Stony)) && (thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso) || thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs));
		}

		// Token: 0x06006E7C RID: 28284 RVA: 0x0021E54C File Offset: 0x0021C74C
		public static bool ApparelRequirementHandlesThing(SpecificApparelRequirement req, ThingDef thing)
		{
			return (req.BodyPartGroup == null || thing.apparel.bodyPartGroups.Contains(req.BodyPartGroup)) && (req.ApparelLayer == null || thing.apparel.layers.Contains(req.ApparelLayer));
		}

		// Token: 0x06006E7D RID: 28285 RVA: 0x0021E5A0 File Offset: 0x0021C7A0
		public static bool ApparelRequirementTagsMatch(SpecificApparelRequirement req, ThingDef thing)
		{
			return (!req.RequiredTag.NullOrEmpty() && thing.apparel.tags.Contains(req.RequiredTag)) || (!req.AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>() && (from x in req.AlternateTagChoices
			where thing.apparel.tags.Contains(x.tag) && Rand.Value < x.chance
			select x).Any<SpecificApparelRequirement.TagChance>());
		}

		// Token: 0x06006E7E RID: 28286 RVA: 0x0004AD32 File Offset: 0x00048F32
		private static bool ApparelRequirementCanUseStuff(SpecificApparelRequirement req, ThingStuffPair pair)
		{
			return req.Stuff == null || !PawnApparelGenerator.ApparelRequirementHandlesThing(req, pair.thing) || (pair.stuff != null && req.Stuff == pair.stuff);
		}

		// Token: 0x06006E7F RID: 28287 RVA: 0x0021E614 File Offset: 0x0021C814
		private static bool CanUsePair(ThingStuffPair pair, Pawn pawn, float moneyLeft, bool allowHeadgear, int fixedSeed)
		{
			if (pair.Price > moneyLeft)
			{
				return false;
			}
			if (!allowHeadgear && PawnApparelGenerator.IsHeadgear(pair.thing))
			{
				return false;
			}
			if (!pair.thing.apparel.CorrectGenderForWearing(pawn.gender))
			{
				return false;
			}
			if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
			{
				bool flag = false;
				for (int i = 0; i < pawn.kindDef.apparelTags.Count; i++)
				{
					for (int j = 0; j < pair.thing.apparel.tags.Count; j++)
					{
						if (pawn.kindDef.apparelTags[i] == pair.thing.apparel.tags[j])
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (!pawn.kindDef.apparelDisallowTags.NullOrEmpty<string>())
			{
				for (int k = 0; k < pawn.kindDef.apparelDisallowTags.Count; k++)
				{
					if (pair.thing.apparel.tags.Contains(pawn.kindDef.apparelDisallowTags[k]))
					{
						return false;
					}
				}
			}
			return pair.thing.generateAllowChance >= 1f || Rand.ChanceSeeded(pair.thing.generateAllowChance, fixedSeed ^ (int)pair.thing.shortHash ^ 64128343);
		}

		// Token: 0x06006E80 RID: 28288 RVA: 0x0004AD66 File Offset: 0x00048F66
		public static bool IsHeadgear(ThingDef td)
		{
			return td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead);
		}

		// Token: 0x06006E81 RID: 28289 RVA: 0x0021E774 File Offset: 0x0021C974
		private static NeededWarmth ApparelWarmthNeededNow(Pawn pawn, PawnGenerationRequest request, out float mapTemperature)
		{
			int tile = request.Tile;
			if (tile == -1)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					tile = anyPlayerHomeMap.Tile;
				}
			}
			if (tile == -1)
			{
				mapTemperature = 21f;
				return NeededWarmth.Any;
			}
			NeededWarmth neededWarmth = NeededWarmth.Any;
			Twelfth twelfth = GenLocalDate.Twelfth(tile);
			mapTemperature = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			for (int i = 0; i < 2; i++)
			{
				NeededWarmth neededWarmth2 = PawnApparelGenerator.CalculateNeededWarmth(pawn, tile, twelfth);
				if (neededWarmth2 != NeededWarmth.Any)
				{
					neededWarmth = neededWarmth2;
					break;
				}
				twelfth = twelfth.NextTwelfth();
			}
			if (!pawn.kindDef.apparelIgnoreSeasons)
			{
				return neededWarmth;
			}
			if (request.ForceAddFreeWarmLayerIfNeeded && neededWarmth == NeededWarmth.Warm)
			{
				return neededWarmth;
			}
			return NeededWarmth.Any;
		}

		// Token: 0x06006E82 RID: 28290 RVA: 0x0021E808 File Offset: 0x0021CA08
		public static NeededWarmth CalculateNeededWarmth(Pawn pawn, int tile, Twelfth twelfth)
		{
			float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			if (num < pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 4f)
			{
				return NeededWarmth.Warm;
			}
			if (num > pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) + 4f)
			{
				return NeededWarmth.Cool;
			}
			return NeededWarmth.Any;
		}

		// Token: 0x06006E83 RID: 28291 RVA: 0x0021E858 File Offset: 0x0021CA58
		[DebugOutput]
		private static void ApparelPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnApparelGenerator.allApparelPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[8];
			array[0] = new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName);
			array[1] = new TableDataGetter<ThingStuffPair>("stuff", delegate(ThingStuffPair p)
			{
				if (p.stuff == null)
				{
					return "";
				}
				return p.stuff.defName;
			});
			array[2] = new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString());
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => (p.Commonality * 100f).ToString("F4"));
			array[4] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F4"));
			array[5] = new TableDataGetter<ThingStuffPair>("insulationCold", delegate(ThingStuffPair p)
			{
				if (p.InsulationCold != 0f)
				{
					return p.InsulationCold.ToString();
				}
				return "";
			});
			array[6] = new TableDataGetter<ThingStuffPair>("headgear", delegate(ThingStuffPair p)
			{
				if (!PawnApparelGenerator.IsHeadgear(p.thing))
				{
					return "";
				}
				return "*";
			});
			array[7] = new TableDataGetter<ThingStuffPair>("derp", delegate(ThingStuffPair p)
			{
				if (!PawnApparelGenerator.IsDerpApparel(p.thing, p.stuff))
				{
					return "";
				}
				return "D";
			});
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		// Token: 0x06006E84 RID: 28292 RVA: 0x0004AD96 File Offset: 0x00048F96
		[DebugOutput]
		private static void ApparelPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnApparelGenerator.allApparelPairs);
		}

		// Token: 0x040048DC RID: 18652
		private static List<ThingStuffPair> allApparelPairs = new List<ThingStuffPair>();

		// Token: 0x040048DD RID: 18653
		private static float freeWarmParkaMaxPrice;

		// Token: 0x040048DE RID: 18654
		private static float freeWarmHatMaxPrice;

		// Token: 0x040048DF RID: 18655
		private static PawnApparelGenerator.PossibleApparelSet workingSet = new PawnApparelGenerator.PossibleApparelSet();

		// Token: 0x040048E0 RID: 18656
		private static StringBuilder debugSb = null;

		// Token: 0x040048E1 RID: 18657
		private const int PracticallyInfinity = 9999999;

		// Token: 0x040048E2 RID: 18658
		private static List<ThingStuffPair> tmpApparelCandidates = new List<ThingStuffPair>();

		// Token: 0x040048E3 RID: 18659
		private static List<ThingStuffPair> usableApparel = new List<ThingStuffPair>();

		// Token: 0x020013F6 RID: 5110
		private class PossibleApparelSet
		{
			// Token: 0x1700110A RID: 4362
			// (get) Token: 0x06006E85 RID: 28293 RVA: 0x0004ADA2 File Offset: 0x00048FA2
			public int Count
			{
				get
				{
					return this.aps.Count;
				}
			}

			// Token: 0x1700110B RID: 4363
			// (get) Token: 0x06006E86 RID: 28294 RVA: 0x0004ADAF File Offset: 0x00048FAF
			public float TotalPrice
			{
				get
				{
					return this.aps.Sum((ThingStuffPair pa) => pa.Price);
				}
			}

			// Token: 0x1700110C RID: 4364
			// (get) Token: 0x06006E87 RID: 28295 RVA: 0x0004ADDB File Offset: 0x00048FDB
			public float TotalInsulationCold
			{
				get
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold);
				}
			}

			// Token: 0x1700110D RID: 4365
			// (get) Token: 0x06006E88 RID: 28296 RVA: 0x0004AE07 File Offset: 0x00049007
			public List<ThingStuffPair> ApparelsForReading
			{
				get
				{
					return this.aps;
				}
			}

			// Token: 0x06006E89 RID: 28297 RVA: 0x0004AE0F File Offset: 0x0004900F
			public void Reset(BodyDef body, ThingDef raceDef)
			{
				this.aps.Clear();
				this.lgps.Clear();
				this.body = body;
				this.raceDef = raceDef;
			}

			// Token: 0x06006E8A RID: 28298 RVA: 0x0021E9FC File Offset: 0x0021CBFC
			public void Add(ThingStuffPair pair)
			{
				this.aps.Add(pair);
				for (int i = 0; i < pair.thing.apparel.layers.Count; i++)
				{
					ApparelLayerDef layer = pair.thing.apparel.layers[i];
					BodyPartGroupDef[] interferingBodyPartGroups = pair.thing.apparel.GetInterferingBodyPartGroups(this.body);
					for (int j = 0; j < interferingBodyPartGroups.Length; j++)
					{
						this.lgps.Add(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
					}
				}
			}

			// Token: 0x06006E8B RID: 28299 RVA: 0x0021EA88 File Offset: 0x0021CC88
			public bool PairOverlapsAnything(ThingStuffPair pair)
			{
				if (!this.lgps.Any<ApparelUtility.LayerGroupPair>())
				{
					return false;
				}
				for (int i = 0; i < pair.thing.apparel.layers.Count; i++)
				{
					ApparelLayerDef layer = pair.thing.apparel.layers[i];
					BodyPartGroupDef[] interferingBodyPartGroups = pair.thing.apparel.GetInterferingBodyPartGroups(this.body);
					for (int j = 0; j < interferingBodyPartGroups.Length; j++)
					{
						if (this.lgps.Contains(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j])))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06006E8C RID: 28300 RVA: 0x0021EB1C File Offset: 0x0021CD1C
			public bool CoatButNoShirt()
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (this.aps[i].thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						for (int j = 0; j < this.aps[i].thing.apparel.layers.Count; j++)
						{
							ApparelLayerDef apparelLayerDef = this.aps[i].thing.apparel.layers[j];
							if (apparelLayerDef == ApparelLayerDefOf.OnSkin)
							{
								flag2 = true;
							}
							if (apparelLayerDef == ApparelLayerDefOf.Shell || apparelLayerDef == ApparelLayerDefOf.Middle)
							{
								flag = true;
							}
						}
					}
				}
				return flag && !flag2;
			}

			// Token: 0x06006E8D RID: 28301 RVA: 0x0021EBE8 File Offset: 0x0021CDE8
			public bool Covers(BodyPartGroupDef bp)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (this.aps[i].thing.apparel.bodyPartGroups.Contains(bp))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06006E8E RID: 28302 RVA: 0x0021EC34 File Offset: 0x0021CE34
			public bool IsNaked(Gender gender)
			{
				switch (gender)
				{
				case Gender.None:
					return false;
				case Gender.Male:
					return !this.Covers(BodyPartGroupDefOf.Legs);
				case Gender.Female:
					return !this.Covers(BodyPartGroupDefOf.Legs) || !this.Covers(BodyPartGroupDefOf.Torso);
				default:
					return false;
				}
			}

			// Token: 0x06006E8F RID: 28303 RVA: 0x0021EC88 File Offset: 0x0021CE88
			public bool SatisfiesNeededWarmth(NeededWarmth warmth, bool mustBeSafe = false, float mapTemperature = 21f)
			{
				if (warmth == NeededWarmth.Any)
				{
					return true;
				}
				if (mustBeSafe && !GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(mapTemperature))
				{
					return false;
				}
				if (warmth == NeededWarmth.Cool)
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationHeat) >= -2f;
				}
				if (warmth == NeededWarmth.Warm)
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold) >= 52f;
				}
				throw new NotImplementedException();
			}

			// Token: 0x06006E90 RID: 28304 RVA: 0x0021ED34 File Offset: 0x0021CF34
			public void AddFreeWarmthAsNeeded(NeededWarmth warmth, float mapTemperature)
			{
				if (warmth == NeededWarmth.Any)
				{
					return;
				}
				if (warmth == NeededWarmth.Cool)
				{
					return;
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine();
					PawnApparelGenerator.debugSb.AppendLine("Trying to give free warm layer.");
				}
				for (int i = 0; i < 3; i++)
				{
					if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine("Checking to give free torso-cover at max price " + PawnApparelGenerator.freeWarmParkaMaxPrice);
						}
						Predicate<ThingStuffPair> parkaPairValidator = (ThingStuffPair pa) => pa.Price <= PawnApparelGenerator.freeWarmParkaMaxPrice && pa.InsulationCold > 0f && pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso) && pa.thing.apparel.canBeGeneratedToSatisfyWarmth && this.GetReplacedInsulationCold(pa) < pa.InsulationCold;
						int j = 0;
						Func<ThingStuffPair, bool> <>9__1;
						Func<ThingStuffPair, bool> <>9__3;
						while (j < 2)
						{
							ThingStuffPair candidate;
							if (j == 0)
							{
								IEnumerable<ThingStuffPair> allApparelPairs = PawnApparelGenerator.allApparelPairs;
								Func<ThingStuffPair, bool> predicate;
								if ((predicate = <>9__1) == null)
								{
									predicate = (<>9__1 = ((ThingStuffPair pa) => parkaPairValidator(pa) && pa.InsulationCold < 40f));
								}
								if (allApparelPairs.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out candidate))
								{
									goto IL_15D;
								}
							}
							else
							{
								IEnumerable<ThingStuffPair> allApparelPairs2 = PawnApparelGenerator.allApparelPairs;
								Func<ThingStuffPair, bool> predicate2;
								if ((predicate2 = <>9__3) == null)
								{
									predicate2 = (<>9__3 = ((ThingStuffPair pa) => parkaPairValidator(pa)));
								}
								if (allApparelPairs2.Where(predicate2).TryMaxBy((ThingStuffPair x) => x.InsulationCold - this.GetReplacedInsulationCold(x), out candidate))
								{
									goto IL_15D;
								}
							}
							j++;
							continue;
							IL_15D:
							if (DebugViewSettings.logApparelGeneration)
							{
								PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
								{
									"Giving free torso-cover: ",
									candidate,
									" insulation=",
									candidate.InsulationCold
								}));
								IEnumerable<ThingStuffPair> source = this.aps;
								Func<ThingStuffPair, bool> predicate3;
								Func<ThingStuffPair, bool> <>9__6;
								if ((predicate3 = <>9__6) == null)
								{
									predicate3 = (<>9__6 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, candidate.thing, this.body)));
								}
								foreach (ThingStuffPair thingStuffPair in source.Where(predicate3))
								{
									PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
									{
										"    -replaces ",
										thingStuffPair.ToString(),
										" InsulationCold=",
										thingStuffPair.InsulationCold
									}));
								}
							}
							this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, candidate.thing, this.body));
							this.aps.Add(candidate);
							break;
						}
					}
					if (GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(mapTemperature))
					{
						break;
					}
				}
				if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
				{
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.AppendLine("Checking to give free hat at max price " + PawnApparelGenerator.freeWarmHatMaxPrice);
					}
					Predicate<ThingStuffPair> hatPairValidator = (ThingStuffPair pa) => pa.Price <= PawnApparelGenerator.freeWarmHatMaxPrice && pa.InsulationCold >= 7f && (pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead)) && this.GetReplacedInsulationCold(pa) < pa.InsulationCold;
					ThingStuffPair hatPair;
					if ((from pa in PawnApparelGenerator.allApparelPairs
					where hatPairValidator(pa)
					select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out hatPair))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
							{
								"Giving free hat: ",
								hatPair,
								" insulation=",
								hatPair.InsulationCold
							}));
							IEnumerable<ThingStuffPair> source2 = this.aps;
							Func<ThingStuffPair, bool> predicate4;
							Func<ThingStuffPair, bool> <>9__11;
							if ((predicate4 = <>9__11) == null)
							{
								predicate4 = (<>9__11 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, hatPair.thing, this.body)));
							}
							foreach (ThingStuffPair thingStuffPair2 in source2.Where(predicate4))
							{
								PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
								{
									"    -replaces ",
									thingStuffPair2.ToString(),
									" InsulationCold=",
									thingStuffPair2.InsulationCold
								}));
							}
						}
						this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, hatPair.thing, this.body));
						this.aps.Add(hatPair);
					}
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine("New TotalInsulationCold: " + this.TotalInsulationCold);
				}
			}

			// Token: 0x06006E91 RID: 28305 RVA: 0x0021F1E4 File Offset: 0x0021D3E4
			public void GiveToPawn(Pawn pawn)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					Apparel apparel = (Apparel)ThingMaker.MakeThing(this.aps[i].thing, this.aps[i].stuff);
					PawnGenerator.PostProcessGeneratedGear(apparel, pawn);
					if (ApparelUtility.HasPartsToWear(pawn, apparel.def))
					{
						pawn.apparel.Wear(apparel, false, false);
					}
				}
				for (int j = 0; j < this.aps.Count; j++)
				{
					for (int k = 0; k < this.aps.Count; k++)
					{
						if (j != k && !ApparelUtility.CanWearTogether(this.aps[j].thing, this.aps[k].thing, pawn.RaceProps.body))
						{
							Log.Error(string.Concat(new object[]
							{
								pawn,
								" generated with apparel that cannot be worn together: ",
								this.aps[j],
								", ",
								this.aps[k]
							}), false);
							return;
						}
					}
				}
			}

			// Token: 0x06006E92 RID: 28306 RVA: 0x0021F318 File Offset: 0x0021D518
			private float GetReplacedInsulationCold(ThingStuffPair newAp)
			{
				float num = 0f;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(this.aps[i].thing, newAp.thing, this.body))
					{
						num += this.aps[i].InsulationCold;
					}
				}
				return num;
			}

			// Token: 0x06006E93 RID: 28307 RVA: 0x0021F380 File Offset: 0x0021D580
			public override string ToString()
			{
				string str = "[";
				for (int i = 0; i < this.aps.Count; i++)
				{
					str = str + this.aps[i].ToString() + ", ";
				}
				return str + "]";
			}

			// Token: 0x040048E4 RID: 18660
			private List<ThingStuffPair> aps = new List<ThingStuffPair>();

			// Token: 0x040048E5 RID: 18661
			private HashSet<ApparelUtility.LayerGroupPair> lgps = new HashSet<ApparelUtility.LayerGroupPair>();

			// Token: 0x040048E6 RID: 18662
			private BodyDef body;

			// Token: 0x040048E7 RID: 18663
			private ThingDef raceDef;

			// Token: 0x040048E8 RID: 18664
			private const float StartingMinTemperature = 12f;

			// Token: 0x040048E9 RID: 18665
			private const float TargetMinTemperature = -40f;

			// Token: 0x040048EA RID: 18666
			private const float StartingMaxTemperature = 32f;

			// Token: 0x040048EB RID: 18667
			private const float TargetMaxTemperature = 30f;
		}
	}
}
