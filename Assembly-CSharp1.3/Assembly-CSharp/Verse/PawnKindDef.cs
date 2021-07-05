using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E7 RID: 231
	public class PawnKindDef : Def
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001F306 File Offset: 0x0001D506
		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001F314 File Offset: 0x0001D514
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001F34E File Offset: 0x0001D54E
		public string GetLabelPlural(int count = -1)
		{
			if (!this.labelPlural.NullOrEmpty())
			{
				return this.labelPlural;
			}
			return Find.ActiveLanguageWorker.Pluralize(this.label, count);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001F375 File Offset: 0x0001D575
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.nameMakerFemale != null)
			{
				return this.nameMakerFemale;
			}
			if (this.nameMaker != null)
			{
				return this.nameMaker;
			}
			return null;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001F39C File Offset: 0x0001D59C
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001F3F0 File Offset: 0x0001D5F0
		public float GetAnimalPointsToHuntOrSlaughter()
		{
			return this.combatPower * 5f * (1f + this.RaceProps.manhunterOnDamageChance * 0.5f) * (1f + this.RaceProps.manhunterOnTameFailChance * 0.2f) * (1f + this.RaceProps.wildness) + this.race.BaseMarketValue;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001F457 File Offset: 0x0001D657
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.backstoryFilters != null && this.backstoryFiltersOverride != null)
			{
				yield return "both backstoryCategories and backstoryCategoriesOverride are defined";
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			if (this.baseRecruitDifficulty > 1.0001f)
			{
				yield return this.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
			}
			if (this.combatPower < 0f)
			{
				yield return this.defName + " has no combatPower.";
			}
			if (this.weaponMoney != FloatRange.Zero)
			{
				float num = 999999f;
				int num2;
				int i;
				for (i = 0; i < this.weaponTags.Count; i = num2 + 1)
				{
					IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
					select d;
					if (source.Any<ThingDef>())
					{
						num = Mathf.Min(num, source.Min((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d)));
					}
					num2 = i;
				}
				if (num < 999999f && num > this.weaponMoney.min)
				{
					yield return string.Concat(new object[]
					{
						"Cheapest weapon with one of my weaponTags costs ",
						num,
						" but weaponMoney min is ",
						this.weaponMoney.min,
						", so could end up weaponless."
					});
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return string.Concat(new object[]
				{
					"PawnKindDef defines ",
					this.lifeStages.Count,
					" lifeStages while race def defines ",
					this.RaceProps.lifeStageAges.Count
				});
			}
			if (this.apparelRequired != null)
			{
				int num2;
				for (int k = 0; k < this.apparelRequired.Count; k = num2 + 1)
				{
					for (int j = k + 1; j < this.apparelRequired.Count; j = num2 + 1)
					{
						if (!ApparelUtility.CanWearTogether(this.apparelRequired[k], this.apparelRequired[j], this.race.race.body))
						{
							yield return string.Concat(new object[]
							{
								"required apparel can't be worn together (",
								this.apparelRequired[k],
								", ",
								this.apparelRequired[j],
								")"
							});
						}
						num2 = j;
					}
					num2 = k;
				}
			}
			if (this.alternateGraphics != null)
			{
				using (List<AlternateGraphic>.Enumerator enumerator2 = this.alternateGraphics.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Weight < 0f)
						{
							yield return "alternate graphic has negative weight.";
						}
					}
				}
				List<AlternateGraphic>.Enumerator enumerator2 = default(List<AlternateGraphic>.Enumerator);
			}
			if (this.RaceProps.Humanlike && this.initialResistanceRange == null)
			{
				yield return "initial resistance range is undefined for humanlike pawn kind.";
			}
			if (this.RaceProps.Humanlike && this.initialWillRange == null)
			{
				yield return "initial will range is undefined for humanlike pawn kind.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001F467 File Offset: 0x0001D667
		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		// Token: 0x0400051C RID: 1308
		public ThingDef race;

		// Token: 0x0400051D RID: 1309
		public FactionDef defaultFactionType;

		// Token: 0x0400051E RID: 1310
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x0400051F RID: 1311
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFiltersOverride;

		// Token: 0x04000520 RID: 1312
		[NoTranslate]
		public List<string> backstoryCategories;

		// Token: 0x04000521 RID: 1313
		[MustTranslate]
		public string labelPlural;

		// Token: 0x04000522 RID: 1314
		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		// Token: 0x04000523 RID: 1315
		public List<AlternateGraphic> alternateGraphics;

		// Token: 0x04000524 RID: 1316
		public List<TraitRequirement> forcedTraits;

		// Token: 0x04000525 RID: 1317
		public List<TraitDef> disallowedTraits;

		// Token: 0x04000526 RID: 1318
		public float alternateGraphicChance;

		// Token: 0x04000527 RID: 1319
		[LoadAlias("hairTags")]
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x04000528 RID: 1320
		public HairDef forcedHair;

		// Token: 0x04000529 RID: 1321
		public Color? forcedHairColor;

		// Token: 0x0400052A RID: 1322
		public List<MissingPart> missingParts;

		// Token: 0x0400052B RID: 1323
		public RulePackDef nameMaker;

		// Token: 0x0400052C RID: 1324
		public RulePackDef nameMakerFemale;

		// Token: 0x0400052D RID: 1325
		public float backstoryCryptosleepCommonality;

		// Token: 0x0400052E RID: 1326
		public FloatRange? chronologicalAgeRange;

		// Token: 0x0400052F RID: 1327
		public int minGenerationAge;

		// Token: 0x04000530 RID: 1328
		public int maxGenerationAge = 999999;

		// Token: 0x04000531 RID: 1329
		public bool factionLeader;

		// Token: 0x04000532 RID: 1330
		public Gender? fixedGender;

		// Token: 0x04000533 RID: 1331
		public bool allowOldAgeInjuries = true;

		// Token: 0x04000534 RID: 1332
		public bool generateInitialNonFamilyRelations = true;

		// Token: 0x04000535 RID: 1333
		public bool destroyGearOnDrop;

		// Token: 0x04000536 RID: 1334
		public float defendPointRadius = -1f;

		// Token: 0x04000537 RID: 1335
		public bool factionHostileOnKill;

		// Token: 0x04000538 RID: 1336
		public bool factionHostileOnDeath;

		// Token: 0x04000539 RID: 1337
		public FloatRange? initialResistanceRange;

		// Token: 0x0400053A RID: 1338
		public FloatRange? initialWillRange;

		// Token: 0x0400053B RID: 1339
		public float royalTitleChance;

		// Token: 0x0400053C RID: 1340
		public RoyalTitleDef titleRequired;

		// Token: 0x0400053D RID: 1341
		public RoyalTitleDef minTitleRequired;

		// Token: 0x0400053E RID: 1342
		public List<RoyalTitleDef> titleSelectOne;

		// Token: 0x0400053F RID: 1343
		public bool allowRoyalRoomRequirements = true;

		// Token: 0x04000540 RID: 1344
		public bool allowRoyalApparelRequirements = true;

		// Token: 0x04000541 RID: 1345
		public bool isFighter = true;

		// Token: 0x04000542 RID: 1346
		public float combatPower = -1f;

		// Token: 0x04000543 RID: 1347
		public bool canArriveManhunter = true;

		// Token: 0x04000544 RID: 1348
		public bool canBeSapper;

		// Token: 0x04000545 RID: 1349
		public bool isGoodBreacher;

		// Token: 0x04000546 RID: 1350
		public float baseRecruitDifficulty = 0.5f;

		// Token: 0x04000547 RID: 1351
		public bool aiAvoidCover;

		// Token: 0x04000548 RID: 1352
		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		// Token: 0x04000549 RID: 1353
		public float acceptArrestChanceFactor = 1f;

		// Token: 0x0400054A RID: 1354
		public QualityCategory itemQuality = QualityCategory.Normal;

		// Token: 0x0400054B RID: 1355
		public QualityCategory? forceWeaponQuality;

		// Token: 0x0400054C RID: 1356
		public bool forceNormalGearQuality;

		// Token: 0x0400054D RID: 1357
		public FloatRange gearHealthRange = FloatRange.One;

		// Token: 0x0400054E RID: 1358
		public FloatRange weaponMoney = FloatRange.Zero;

		// Token: 0x0400054F RID: 1359
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x04000550 RID: 1360
		public ThingDef weaponStuffOverride;

		// Token: 0x04000551 RID: 1361
		public ThingStyleDef weaponStyleDef;

		// Token: 0x04000552 RID: 1362
		public FloatRange apparelMoney = FloatRange.Zero;

		// Token: 0x04000553 RID: 1363
		public List<ThingDef> apparelRequired;

		// Token: 0x04000554 RID: 1364
		[NoTranslate]
		public List<string> apparelTags;

		// Token: 0x04000555 RID: 1365
		[NoTranslate]
		public List<string> apparelDisallowTags;

		// Token: 0x04000556 RID: 1366
		public float apparelAllowHeadgearChance = 1f;

		// Token: 0x04000557 RID: 1367
		public bool apparelIgnoreSeasons;

		// Token: 0x04000558 RID: 1368
		public bool ignoreFactionApparelStuffRequirements;

		// Token: 0x04000559 RID: 1369
		public Color apparelColor = Color.white;

		// Token: 0x0400055A RID: 1370
		public Color? skinColorOverride;

		// Token: 0x0400055B RID: 1371
		public List<SpecificApparelRequirement> specificApparelRequirements;

		// Token: 0x0400055C RID: 1372
		public List<ThingDef> techHediffsRequired;

		// Token: 0x0400055D RID: 1373
		public FloatRange techHediffsMoney = FloatRange.Zero;

		// Token: 0x0400055E RID: 1374
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x0400055F RID: 1375
		[NoTranslate]
		public List<string> techHediffsDisallowTags;

		// Token: 0x04000560 RID: 1376
		public float techHediffsChance;

		// Token: 0x04000561 RID: 1377
		public int techHediffsMaxAmount = 1;

		// Token: 0x04000562 RID: 1378
		public float biocodeWeaponChance;

		// Token: 0x04000563 RID: 1379
		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		// Token: 0x04000564 RID: 1380
		public PawnInventoryOption inventoryOptions;

		// Token: 0x04000565 RID: 1381
		public float invNutrition;

		// Token: 0x04000566 RID: 1382
		public ThingDef invFoodDef;

		// Token: 0x04000567 RID: 1383
		public float chemicalAddictionChance;

		// Token: 0x04000568 RID: 1384
		public float combatEnhancingDrugsChance;

		// Token: 0x04000569 RID: 1385
		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		// Token: 0x0400056A RID: 1386
		public List<ChemicalDef> forcedAddictions = new List<ChemicalDef>();

		// Token: 0x0400056B RID: 1387
		public bool trader;

		// Token: 0x0400056C RID: 1388
		public List<SkillRange> skills;

		// Token: 0x0400056D RID: 1389
		public WorkTags requiredWorkTags;

		// Token: 0x0400056E RID: 1390
		public int extraSkillLevels;

		// Token: 0x0400056F RID: 1391
		public int minTotalSkillLevels;

		// Token: 0x04000570 RID: 1392
		public int minBestSkillLevel;

		// Token: 0x04000571 RID: 1393
		[MustTranslate]
		public string labelMale;

		// Token: 0x04000572 RID: 1394
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x04000573 RID: 1395
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000574 RID: 1396
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x04000575 RID: 1397
		public IntRange wildGroupSize = IntRange.one;

		// Token: 0x04000576 RID: 1398
		public float ecoSystemWeight = 1f;

		// Token: 0x04000577 RID: 1399
		private const int MaxWeaponMoney = 999999;
	}
}
