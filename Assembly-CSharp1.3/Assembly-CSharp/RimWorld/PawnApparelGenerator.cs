using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB6 RID: 3510
	public static class PawnApparelGenerator
	{
		// Token: 0x06005139 RID: 20793 RVA: 0x001B4110 File Offset: 0x001B2310
		static PawnApparelGenerator()
		{
			PawnApparelGenerator.Reset();
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x001B4148 File Offset: 0x001B2348
		public static void Reset()
		{
			PawnApparelGenerator.allApparelPairs = ThingStuffPair.AllWith((ThingDef td) => td.IsApparel);
			PawnApparelGenerator.freeWarmParkaMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Parka, ThingDefOf.Cloth) * 1.3f));
			PawnApparelGenerator.freeWarmHatMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Tuque, ThingDefOf.Cloth) * 1.3f));
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x001B41CC File Offset: 0x001B23CC
		public static void GenerateStartingApparelFor(Pawn pawn, PawnGenerationRequest request)
		{
			if (!pawn.RaceProps.ToolUser || !pawn.RaceProps.IsFlesh)
			{
				return;
			}
			pawn.apparel.DestroyAll(DestroyMode.Vanish);
			Pawn_OutfitTracker outfits = pawn.outfits;
			if (outfits != null)
			{
				OutfitForcedHandler forcedHandler = outfits.forcedHandler;
				if (forcedHandler != null)
				{
					forcedHandler.Reset();
				}
			}
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
						goto IL_250;
					}
					float num2 = Rand.Range(0.45f, 0.8f);
					float totalPrice = PawnApparelGenerator.workingSet.TotalPrice;
					if (totalPrice >= randomInRange * num2)
					{
						goto IL_250;
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
					IL_399:
					num++;
					continue;
					IL_250:
					if (num < 20 && Rand.Value < 0.97f && !PawnApparelGenerator.workingSet.Covers(BodyPartGroupDefOf.Torso))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Does not cover torso.");
							goto IL_399;
						}
						goto IL_399;
					}
					else if (num < 30 && Rand.Value < 0.8f && PawnApparelGenerator.workingSet.CoatButNoShirt())
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Coat but no shirt.");
							goto IL_399;
						}
						goto IL_399;
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
									goto IL_399;
								}
								goto IL_399;
							}
						}
						if (num >= 80 || !PawnApparelGenerator.workingSet.IsNaked(pawn.gender))
						{
							break;
						}
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Naked.");
							goto IL_399;
						}
						goto IL_399;
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
				Log.Message(PawnApparelGenerator.debugSb.ToString());
			}
			PawnApparelGenerator.workingSet.GiveToPawn(pawn);
			PawnApparelGenerator.workingSet.Reset(null, null);
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				PawnApparelGenerator.PostProcessApparel(apparel, pawn);
				CompBiocodable compBiocodable = apparel.TryGetComp<CompBiocodable>();
				if (compBiocodable != null && !compBiocodable.Biocoded && Rand.Chance(request.BiocodeApparelChance))
				{
					compBiocodable.CodeFor(pawn);
				}
			}
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x001B4650 File Offset: 0x001B2850
		public static void PostProcessApparel(Apparel apparel, Pawn pawn)
		{
			if (pawn.kindDef.apparelColor != Color.white)
			{
				apparel.SetColor(pawn.kindDef.apparelColor, false);
			}
			Ideo ideo = pawn.Ideo;
			apparel.SetStyleDef((ideo != null) ? ideo.GetStyleFor(apparel.def) : null);
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirements[i], apparel.def))
					{
						Color color = specificApparelRequirements[i].GetColor();
						if (color != default(Color))
						{
							apparel.SetColor(color, false);
						}
						if (specificApparelRequirements[i].StyleDef != null)
						{
							apparel.SetStyleDef(specificApparelRequirements[i].StyleDef);
						}
						if (specificApparelRequirements[i].Locked)
						{
							pawn.apparel.Lock(apparel);
						}
						if (specificApparelRequirements[i].Biocode)
						{
							CompBiocodable compBiocodable = apparel.TryGetComp<CompBiocodable>();
							if (compBiocodable != null)
							{
								compBiocodable.CodeFor(pawn);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x001B4768 File Offset: 0x001B2968
		public static Apparel GenerateApparelOfDefFor(Pawn pawn, ThingDef apparelDef)
		{
			ThingStuffPair thingStuffPair;
			if (!(from pa in PawnApparelGenerator.allApparelPairs
			where pa.thing == apparelDef && PawnApparelGenerator.CanUseStuff(pawn, pa)
			select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out thingStuffPair))
			{
				if (!(from pa in PawnApparelGenerator.allApparelPairs
				where pa.thing == apparelDef
				select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out thingStuffPair))
				{
					return null;
				}
			}
			return (Apparel)ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x001B4820 File Offset: 0x001B2A20
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
			while ((pawn.Ideo == null || !pawn.Ideo.IdeoPrefersNudityForGender(pawn.gender) || (pawn.Faction != null && pawn.Faction.IsPlayer)) && (Rand.Value >= 0.1f || money >= 9999999f))
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

		// Token: 0x0600513F RID: 20799 RVA: 0x001B4B90 File Offset: 0x001B2D90
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
			return pair.stuff == null || pawn.Faction == null || pawn.kindDef.ignoreFactionApparelStuffRequirements || pawn.Faction.def.CanUseStuffForApparel(pair.stuff);
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x001B4C08 File Offset: 0x001B2E08
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

		// Token: 0x06005141 RID: 20801 RVA: 0x001B4CCC File Offset: 0x001B2ECC
		public static bool ApparelRequirementHandlesThing(SpecificApparelRequirement req, ThingDef thing)
		{
			return (req.BodyPartGroup == null || thing.apparel.bodyPartGroups.Contains(req.BodyPartGroup)) && (req.ApparelLayer == null || thing.apparel.layers.Contains(req.ApparelLayer)) && (req.ApparelDef == null || thing == req.ApparelDef);
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x001B4D34 File Offset: 0x001B2F34
		public static bool ApparelRequirementTagsMatch(SpecificApparelRequirement req, ThingDef thing)
		{
			return (!req.RequiredTag.NullOrEmpty() && thing.apparel.tags.Contains(req.RequiredTag)) || (!req.AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>() && (from x in req.AlternateTagChoices
			where thing.apparel.tags.Contains(x.tag) && Rand.Value < x.chance
			select x).Any<SpecificApparelRequirement.TagChance>());
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x001B4DA5 File Offset: 0x001B2FA5
		private static bool ApparelRequirementCanUseStuff(SpecificApparelRequirement req, ThingStuffPair pair)
		{
			return req.Stuff == null || !PawnApparelGenerator.ApparelRequirementHandlesThing(req, pair.thing) || (pair.stuff != null && req.Stuff == pair.stuff);
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x001B4DDC File Offset: 0x001B2FDC
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

		// Token: 0x06005145 RID: 20805 RVA: 0x001B4F3C File Offset: 0x001B313C
		public static bool IsHeadgear(ThingDef td)
		{
			return td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead);
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x001B4F6C File Offset: 0x001B316C
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

		// Token: 0x06005147 RID: 20807 RVA: 0x001B5000 File Offset: 0x001B3200
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

		// Token: 0x06005148 RID: 20808 RVA: 0x001B5050 File Offset: 0x001B3250
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

		// Token: 0x06005149 RID: 20809 RVA: 0x001B51F1 File Offset: 0x001B33F1
		[DebugOutput]
		private static void ApparelPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnApparelGenerator.allApparelPairs);
		}

		// Token: 0x0400301A RID: 12314
		private static List<ThingStuffPair> allApparelPairs = new List<ThingStuffPair>();

		// Token: 0x0400301B RID: 12315
		private static float freeWarmParkaMaxPrice;

		// Token: 0x0400301C RID: 12316
		private static float freeWarmHatMaxPrice;

		// Token: 0x0400301D RID: 12317
		private static PawnApparelGenerator.PossibleApparelSet workingSet = new PawnApparelGenerator.PossibleApparelSet();

		// Token: 0x0400301E RID: 12318
		private static StringBuilder debugSb = null;

		// Token: 0x0400301F RID: 12319
		private const int PracticallyInfinity = 9999999;

		// Token: 0x04003020 RID: 12320
		private static List<ThingStuffPair> tmpApparelCandidates = new List<ThingStuffPair>();

		// Token: 0x04003021 RID: 12321
		private static List<ThingStuffPair> usableApparel = new List<ThingStuffPair>();

		// Token: 0x0200223B RID: 8763
		private class PossibleApparelSet
		{
			// Token: 0x17001DD7 RID: 7639
			// (get) Token: 0x0600C20C RID: 49676 RVA: 0x003D4DD0 File Offset: 0x003D2FD0
			public int Count
			{
				get
				{
					return this.aps.Count;
				}
			}

			// Token: 0x17001DD8 RID: 7640
			// (get) Token: 0x0600C20D RID: 49677 RVA: 0x003D4DDD File Offset: 0x003D2FDD
			public float TotalPrice
			{
				get
				{
					return this.aps.Sum((ThingStuffPair pa) => pa.Price);
				}
			}

			// Token: 0x17001DD9 RID: 7641
			// (get) Token: 0x0600C20E RID: 49678 RVA: 0x003D4E09 File Offset: 0x003D3009
			public float TotalInsulationCold
			{
				get
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold);
				}
			}

			// Token: 0x17001DDA RID: 7642
			// (get) Token: 0x0600C20F RID: 49679 RVA: 0x003D4E35 File Offset: 0x003D3035
			public List<ThingStuffPair> ApparelsForReading
			{
				get
				{
					return this.aps;
				}
			}

			// Token: 0x0600C210 RID: 49680 RVA: 0x003D4E3D File Offset: 0x003D303D
			public void Reset(BodyDef body, ThingDef raceDef)
			{
				this.aps.Clear();
				this.lgps.Clear();
				this.body = body;
				this.raceDef = raceDef;
			}

			// Token: 0x0600C211 RID: 49681 RVA: 0x003D4E64 File Offset: 0x003D3064
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

			// Token: 0x0600C212 RID: 49682 RVA: 0x003D4EF0 File Offset: 0x003D30F0
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

			// Token: 0x0600C213 RID: 49683 RVA: 0x003D4F84 File Offset: 0x003D3184
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

			// Token: 0x0600C214 RID: 49684 RVA: 0x003D5050 File Offset: 0x003D3250
			public bool Covers(BodyPartGroupDef bp)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					if ((bp != BodyPartGroupDefOf.Legs || !this.aps[i].thing.apparel.legsNakedUnlessCoveredBySomethingElse) && this.aps[i].thing.apparel.bodyPartGroups.Contains(bp))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600C215 RID: 49685 RVA: 0x003D50C0 File Offset: 0x003D32C0
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

			// Token: 0x0600C216 RID: 49686 RVA: 0x003D5114 File Offset: 0x003D3314
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

			// Token: 0x0600C217 RID: 49687 RVA: 0x003D51C0 File Offset: 0x003D33C0
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

			// Token: 0x0600C218 RID: 49688 RVA: 0x003D5670 File Offset: 0x003D3870
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
							}));
							return;
						}
					}
				}
			}

			// Token: 0x0600C219 RID: 49689 RVA: 0x003D57A4 File Offset: 0x003D39A4
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

			// Token: 0x0600C21A RID: 49690 RVA: 0x003D580C File Offset: 0x003D3A0C
			public override string ToString()
			{
				string str = "[";
				for (int i = 0; i < this.aps.Count; i++)
				{
					str = str + this.aps[i].ToString() + ", ";
				}
				return str + "]";
			}

			// Token: 0x0400829A RID: 33434
			private List<ThingStuffPair> aps = new List<ThingStuffPair>();

			// Token: 0x0400829B RID: 33435
			private HashSet<ApparelUtility.LayerGroupPair> lgps = new HashSet<ApparelUtility.LayerGroupPair>();

			// Token: 0x0400829C RID: 33436
			private BodyDef body;

			// Token: 0x0400829D RID: 33437
			private ThingDef raceDef;

			// Token: 0x0400829E RID: 33438
			private const float StartingMinTemperature = 12f;

			// Token: 0x0400829F RID: 33439
			private const float TargetMinTemperature = -40f;

			// Token: 0x040082A0 RID: 33440
			private const float StartingMaxTemperature = 32f;

			// Token: 0x040082A1 RID: 33441
			private const float TargetMaxTemperature = 30f;
		}
	}
}
