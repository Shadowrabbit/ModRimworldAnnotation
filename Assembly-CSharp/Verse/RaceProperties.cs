using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000115 RID: 277
	public class RaceProperties
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0000C095 File Offset: 0x0000A295
		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x0000C0A3 File Offset: 0x0000A2A3
		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0000C0B1 File Offset: 0x0000A2B1
		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x0000C0C3 File Offset: 0x0000A2C3
		public bool EatsFood
		{
			get
			{
				return this.foodType > FoodTypeFlags.None;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00092568 File Offset: 0x00090768
		public float FoodLevelPercentageWantEat
		{
			get
			{
				switch (this.ResolvedDietCategory)
				{
				case DietCategory.NeverEats:
					return 0.3f;
				case DietCategory.Herbivorous:
					return 0.45f;
				case DietCategory.Dendrovorous:
					return 0.45f;
				case DietCategory.Ovivorous:
					return 0.4f;
				case DietCategory.Omnivorous:
					return 0.3f;
				case DietCategory.Carnivorous:
					return 0.3f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x000925C8 File Offset: 0x000907C8
		public DietCategory ResolvedDietCategory
		{
			get
			{
				if (!this.EatsFood)
				{
					return DietCategory.NeverEats;
				}
				if (this.Eats(FoodTypeFlags.Tree))
				{
					return DietCategory.Dendrovorous;
				}
				if (this.Eats(FoodTypeFlags.Meat))
				{
					if (this.Eats(FoodTypeFlags.VegetableOrFruit) || this.Eats(FoodTypeFlags.Plant))
					{
						return DietCategory.Omnivorous;
					}
					return DietCategory.Carnivorous;
				}
				else
				{
					if (this.Eats(FoodTypeFlags.AnimalProduct))
					{
						return DietCategory.Ovivorous;
					}
					return DietCategory.Herbivorous;
				}
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0009261C File Offset: 0x0009081C
		public DeathActionWorker DeathActionWorker
		{
			get
			{
				if (this.deathActionWorkerInt == null)
				{
					if (this.deathActionWorkerClass != null)
					{
						this.deathActionWorkerInt = (DeathActionWorker)Activator.CreateInstance(this.deathActionWorkerClass);
					}
					else
					{
						this.deathActionWorkerInt = new DeathActionWorker_Simple();
					}
				}
				return this.deathActionWorkerInt;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0000C0CE File Offset: 0x0000A2CE
		public FleshTypeDef FleshType
		{
			get
			{
				if (this.fleshType != null)
				{
					return this.fleshType;
				}
				return FleshTypeDefOf.Normal;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0000C0E4 File Offset: 0x0000A2E4
		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x0000C0F3 File Offset: 0x0000A2F3
		public bool IsFlesh
		{
			get
			{
				return this.FleshType != FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0000C105 File Offset: 0x0000A305
		public ThingDef BloodDef
		{
			get
			{
				if (this.bloodDef != null)
				{
					return this.bloodDef;
				}
				if (this.IsFlesh)
				{
					return ThingDefOf.Filth_Blood;
				}
				return null;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0000C125 File Offset: 0x0000A325
		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdMigrationAllowed;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00092668 File Offset: 0x00090868
		public PawnKindDef AnyPawnKind
		{
			get
			{
				if (this.cachedAnyPawnKind == null)
				{
					List<PawnKindDef> allDefsListForReading = DefDatabase<PawnKindDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].race.race == this)
						{
							this.cachedAnyPawnKind = allDefsListForReading[i];
							break;
						}
					}
				}
				return this.cachedAnyPawnKind;
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0000C137 File Offset: 0x0000A337
		public RulePackDef GetNameGenerator(Gender gender)
		{
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				return this.nameGeneratorFemale;
			}
			return this.nameGenerator;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0000C152 File Offset: 0x0000A352
		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x000926C0 File Offset: 0x000908C0
		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && (this.willNeverEat == null || !this.willNeverEat.Contains(t)) && this.Eats(t.ingestible.foodType);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0000C160 File Offset: 0x0000A360
		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) > FoodTypeFlags.None;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0000C177 File Offset: 0x0000A377
		public void ResolveReferencesSpecial()
		{
			if (this.useMeatFrom != null)
			{
				this.meatDef = this.useMeatFrom.race.meatDef;
			}
			if (this.useLeatherFrom != null)
			{
				this.leatherDef = this.useLeatherFrom.race.leatherDef;
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0000C1B5 File Offset: 0x0000A3B5
		public IEnumerable<string> ConfigErrors()
		{
			if (this.soundMeleeHitPawn == null)
			{
				yield return "soundMeleeHitPawn is null";
			}
			if (this.soundMeleeHitBuilding == null)
			{
				yield return "soundMeleeHitBuilding is null";
			}
			if (this.soundMeleeMiss == null)
			{
				yield return "soundMeleeMiss is null";
			}
			if (this.predator && !this.Eats(FoodTypeFlags.Meat))
			{
				yield return "predator but doesn't eat meat";
			}
			int num;
			for (int i = 0; i < this.lifeStageAges.Count; i = num + 1)
			{
				for (int j = 0; j < i; j = num + 1)
				{
					if (this.lifeStageAges[j].minAge > this.lifeStageAges[i].minAge)
					{
						yield return "lifeStages minAges are not in ascending order";
					}
					num = j;
				}
				num = i;
			}
			if (this.litterSizeCurve != null)
			{
				foreach (string text in this.litterSizeCurve.ConfigErrors("litterSizeCurve"))
				{
					yield return text;
				}
				IEnumerator<string> enumerator = null;
			}
			if (this.nameOnTameChance > 0f && this.nameGenerator == null)
			{
				yield return "can be named, but has no nameGenerator";
			}
			if (this.Animal && this.wildness < 0f)
			{
				yield return "is animal but wildness is not defined";
			}
			if (this.useMeatFrom != null && this.useMeatFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use meat from non-pawn " + this.useMeatFrom;
			}
			if (this.useMeatFrom != null && this.useMeatFrom.race.useMeatFrom != null)
			{
				yield return string.Concat(new object[]
				{
					"tries to use meat from ",
					this.useMeatFrom,
					" which uses meat from ",
					this.useMeatFrom.race.useMeatFrom
				});
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use leather from non-pawn " + this.useLeatherFrom;
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.race.useLeatherFrom != null)
			{
				yield return string.Concat(new object[]
				{
					"tries to use leather from ",
					this.useLeatherFrom,
					" which uses leather from ",
					this.useLeatherFrom.race.useLeatherFrom
				});
			}
			if (this.Animal && this.trainability == null)
			{
				yield return "animal has trainability = null";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0000C1C5 File Offset: 0x0000A3C5
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Race".Translate(), parentDef.LabelCap, parentDef.description, 2100, null, null, false);
			if (!parentDef.race.IsMechanoid)
			{
				string text = this.foodType.ToHumanString().CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Diet".Translate(), text, "Stat_Race_Diet_Desc".Translate(text), 1500, null, null, false);
			}
			if (req.HasThing && req.Thing is Pawn && (req.Thing as Pawn).needs != null && (req.Thing as Pawn).needs.food != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "HungerRate".Translate(), ((req.Thing as Pawn).needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false) * 60000f).ToString("0.##"), RaceProperties.NutritionEatenPerDayExplanation_NewTemp(req.Thing as Pawn, false, false, true), 1600, null, null, false);
			}
			if (parentDef.race.leatherDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "LeatherType".Translate(), parentDef.race.leatherDef.LabelCap, "Stat_Race_LeatherType_Desc".Translate(), 3550, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(parentDef.race.leatherDef, -1)
				}, false);
			}
			if (parentDef.race.Animal || this.wildness > 0f)
			{
				StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Wildness".Translate(), this.wildness.ToStringPercent(), TrainableUtility.GetWildnessExplanation(parentDef), 2050, null, null, false);
				yield return statDrawEntry;
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "HarmedRevengeChance".Translate(), PawnUtility.GetManhunterOnDamageChance(parentDef.race).ToStringPercent(), "HarmedRevengeChanceExplanation".Translate(), 510, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "TameFailedRevengeChance".Translate(), parentDef.race.manhunterOnTameFailChance.ToStringPercent(), "Stat_Race_Animal_TameFailedRevengeChance_Desc".Translate(), 511, null, null, false);
			}
			if (this.intelligence < Intelligence.Humanlike && this.trainability != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Trainability".Translate(), this.trainability.LabelCap, "Stat_Race_Trainability_Desc".Translate(), 2500, null, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_LifeExpectancy".Translate(), this.lifeExpectancy.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Absolute), "Stat_Race_LifeExpectancy_Desc".Translate(), 2000, null, null, false);
			if (this.intelligence < Intelligence.Humanlike && !parentDef.race.IsMechanoid)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "AnimalFilthRate".Translate(), (PawnUtility.AnimalFilthChancePerCell(parentDef, parentDef.race.baseBodySize) * 1000f).ToString("F2"), "AnimalFilthRateExplanation".Translate(1000.ToString()), 2203, null, null, false);
			}
			if (parentDef.race.Animal)
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "PackAnimal".Translate(), this.packAnimal ? "Yes".Translate() : "No".Translate(), "PackAnimalExplanation".Translate(), 2202, null, null, false);
				yield return statDrawEntry2;
				if (parentDef.race.nuzzleMtbHours > 0f)
				{
					StatDrawEntry statDrawEntry3 = new StatDrawEntry(StatCategoryDefOf.PawnSocial, "NuzzleInterval".Translate(), Mathf.RoundToInt(parentDef.race.nuzzleMtbHours * 2500f).ToStringTicksToPeriod(true, false, true, true), "NuzzleIntervalExplanation".Translate(), 500, null, null, false);
					yield return statDrawEntry3;
				}
			}
			yield break;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000C1E3 File Offset: 0x0000A3E3
		[Obsolete("Will be replaced with NutritionEatenPerDayExplanation_NewTemp soon.")]
		public static string NutritionEatenPerDayExplanation(Pawn p)
		{
			return RaceProperties.NutritionEatenPerDayExplanation_NewTemp(p, true, true, false);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0009271C File Offset: 0x0009091C
		public static string NutritionEatenPerDayExplanation_NewTemp(Pawn p, bool showDiet = false, bool showLegend = false, bool showCalculations = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("NutritionEatenPerDayTip".Translate(ThingDefOf.MealSimple.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##")));
			stringBuilder.AppendLine();
			if (showDiet)
			{
				stringBuilder.AppendLine("CanEat".Translate() + ": " + p.RaceProps.foodType.ToHumanString());
				stringBuilder.AppendLine();
			}
			if (showLegend)
			{
				stringBuilder.AppendLine("Legend".Translate() + ":");
				stringBuilder.AppendLine("NoDietCategoryLetter".Translate() + " - " + DietCategory.Omnivorous.ToStringHuman());
				DietCategory[] array = (DietCategory[])Enum.GetValues(typeof(DietCategory));
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != DietCategory.NeverEats && array[i] != DietCategory.Omnivorous)
					{
						stringBuilder.AppendLine(array[i].ToStringHumanShort() + " - " + array[i].ToStringHuman());
					}
				}
				stringBuilder.AppendLine();
			}
			if (showCalculations)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + (p.ageTracker.CurLifeStage.hungerRateFactor * p.RaceProps.baseHungerRate * 2.6666667E-05f * 60000f).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute));
				if (p.health.hediffSet.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate() + ": " + p.health.hediffSet.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Hediff hediff in p.health.hediffSet.hediffs)
					{
						if (hediff.CurStage != null && hediff.CurStage.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + hediff.LabelCap + ": " + hediff.CurStage.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
					foreach (Hediff hediff2 in p.health.hediffSet.hediffs)
					{
						if (hediff2.CurStage != null && hediff2.CurStage.hungerRateFactorOffset != 0f)
						{
							stringBuilder.AppendLine("    " + hediff2.LabelCap + ": +" + hediff2.CurStage.hungerRateFactorOffset.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Factor));
						}
					}
				}
				if (p.story != null && p.story.traits != null && p.story.traits.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate() + ": " + p.story.traits.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Trait trait in p.story.traits.allTraits)
					{
						if (trait.CurrentData.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + trait.LabelCap + ": " + trait.CurrentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
				}
				if (p.GetStatValue(StatDefOf.HungerRateMultiplier, true) != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(StatDefOf.HungerRateMultiplier.LabelCap + ": " + p.GetStatValue(StatDefOf.HungerRateMultiplier, true).ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Factor));
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + (p.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false) * 60000f).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x040004ED RID: 1261
		public Intelligence intelligence;

		// Token: 0x040004EE RID: 1262
		private FleshTypeDef fleshType;

		// Token: 0x040004EF RID: 1263
		private ThingDef bloodDef;

		// Token: 0x040004F0 RID: 1264
		public bool hasGenders = true;

		// Token: 0x040004F1 RID: 1265
		public bool needsRest = true;

		// Token: 0x040004F2 RID: 1266
		public ThinkTreeDef thinkTreeMain;

		// Token: 0x040004F3 RID: 1267
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x040004F4 RID: 1268
		public PawnNameCategory nameCategory;

		// Token: 0x040004F5 RID: 1269
		public FoodTypeFlags foodType;

		// Token: 0x040004F6 RID: 1270
		public BodyDef body;

		// Token: 0x040004F7 RID: 1271
		public Type deathActionWorkerClass;

		// Token: 0x040004F8 RID: 1272
		public List<AnimalBiomeRecord> wildBiomes;

		// Token: 0x040004F9 RID: 1273
		public SimpleCurve ageGenerationCurve;

		// Token: 0x040004FA RID: 1274
		public bool makesFootprints;

		// Token: 0x040004FB RID: 1275
		public int executionRange = 2;

		// Token: 0x040004FC RID: 1276
		public float lifeExpectancy = 10f;

		// Token: 0x040004FD RID: 1277
		public List<HediffGiverSetDef> hediffGiverSets;

		// Token: 0x040004FE RID: 1278
		public bool herdAnimal;

		// Token: 0x040004FF RID: 1279
		public bool packAnimal;

		// Token: 0x04000500 RID: 1280
		public bool predator;

		// Token: 0x04000501 RID: 1281
		public float maxPreyBodySize = 99999f;

		// Token: 0x04000502 RID: 1282
		public float wildness;

		// Token: 0x04000503 RID: 1283
		public float petness;

		// Token: 0x04000504 RID: 1284
		public float nuzzleMtbHours = -1f;

		// Token: 0x04000505 RID: 1285
		public float manhunterOnDamageChance;

		// Token: 0x04000506 RID: 1286
		public float manhunterOnTameFailChance;

		// Token: 0x04000507 RID: 1287
		public bool canBePredatorPrey = true;

		// Token: 0x04000508 RID: 1288
		public bool herdMigrationAllowed = true;

		// Token: 0x04000509 RID: 1289
		public List<ThingDef> willNeverEat;

		// Token: 0x0400050A RID: 1290
		public float gestationPeriodDays = 10f;

		// Token: 0x0400050B RID: 1291
		public SimpleCurve litterSizeCurve;

		// Token: 0x0400050C RID: 1292
		public float mateMtbHours = 12f;

		// Token: 0x0400050D RID: 1293
		[NoTranslate]
		public List<string> untrainableTags;

		// Token: 0x0400050E RID: 1294
		[NoTranslate]
		public List<string> trainableTags;

		// Token: 0x0400050F RID: 1295
		public TrainabilityDef trainability;

		// Token: 0x04000510 RID: 1296
		private RulePackDef nameGenerator;

		// Token: 0x04000511 RID: 1297
		private RulePackDef nameGeneratorFemale;

		// Token: 0x04000512 RID: 1298
		public float nameOnTameChance;

		// Token: 0x04000513 RID: 1299
		public float nameOnNuzzleChance;

		// Token: 0x04000514 RID: 1300
		public float baseBodySize = 1f;

		// Token: 0x04000515 RID: 1301
		public float baseHealthScale = 1f;

		// Token: 0x04000516 RID: 1302
		public float baseHungerRate = 1f;

		// Token: 0x04000517 RID: 1303
		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		// Token: 0x04000518 RID: 1304
		[MustTranslate]
		public string meatLabel;

		// Token: 0x04000519 RID: 1305
		public Color meatColor = Color.white;

		// Token: 0x0400051A RID: 1306
		public float meatMarketValue = 2f;

		// Token: 0x0400051B RID: 1307
		public ThingDef useMeatFrom;

		// Token: 0x0400051C RID: 1308
		public ThingDef useLeatherFrom;

		// Token: 0x0400051D RID: 1309
		public ThingDef leatherDef;

		// Token: 0x0400051E RID: 1310
		public ShadowData specialShadowData;

		// Token: 0x0400051F RID: 1311
		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

		// Token: 0x04000520 RID: 1312
		public SoundDef soundMeleeHitPawn;

		// Token: 0x04000521 RID: 1313
		public SoundDef soundMeleeHitBuilding;

		// Token: 0x04000522 RID: 1314
		public SoundDef soundMeleeMiss;

		// Token: 0x04000523 RID: 1315
		public SoundDef soundMeleeDodge;

		// Token: 0x04000524 RID: 1316
		[Unsaved(false)]
		private DeathActionWorker deathActionWorkerInt;

		// Token: 0x04000525 RID: 1317
		[Unsaved(false)]
		public ThingDef meatDef;

		// Token: 0x04000526 RID: 1318
		[Unsaved(false)]
		public ThingDef corpseDef;

		// Token: 0x04000527 RID: 1319
		[Unsaved(false)]
		private PawnKindDef cachedAnyPawnKind;
	}
}
