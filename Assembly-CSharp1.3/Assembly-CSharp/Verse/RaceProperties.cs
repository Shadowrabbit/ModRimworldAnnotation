using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000AF RID: 175
	public class RaceProperties
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001BDC3 File Offset: 0x00019FC3
		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0001BDD1 File Offset: 0x00019FD1
		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001BDDF File Offset: 0x00019FDF
		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001BDF1 File Offset: 0x00019FF1
		public bool Insect
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Insectoid;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001BE00 File Offset: 0x0001A000
		public bool Dryad
		{
			get
			{
				return this.animalType == AnimalType.Dryad;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001BE0B File Offset: 0x0001A00B
		public bool EatsFood
		{
			get
			{
				return this.foodType > FoodTypeFlags.None;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001BE18 File Offset: 0x0001A018
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001BE78 File Offset: 0x0001A078
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001BECC File Offset: 0x0001A0CC
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001BF18 File Offset: 0x0001A118
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001BF2E File Offset: 0x0001A12E
		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001BF3D File Offset: 0x0001A13D
		public bool IsFlesh
		{
			get
			{
				return this.FleshType != FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001BF4F File Offset: 0x0001A14F
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0001BF6F File Offset: 0x0001A16F
		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdMigrationAllowed;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001BF81 File Offset: 0x0001A181
		public bool CanPassFences
		{
			get
			{
				return !this.FenceBlocked;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001BF8C File Offset: 0x0001A18C
		public bool FenceBlocked
		{
			get
			{
				return this.Roamer;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001BF94 File Offset: 0x0001A194
		public bool Roamer
		{
			get
			{
				return this.roamMtbDays != null;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001BFA4 File Offset: 0x0001A1A4
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

		// Token: 0x06000576 RID: 1398 RVA: 0x0001BFF9 File Offset: 0x0001A1F9
		public RulePackDef GetNameGenerator(Gender gender)
		{
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				return this.nameGeneratorFemale;
			}
			return this.nameGenerator;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001C014 File Offset: 0x0001A214
		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001C024 File Offset: 0x0001A224
		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && (this.willNeverEat == null || !this.willNeverEat.Contains(t)) && this.Eats(t.ingestible.foodType);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001C07D File Offset: 0x0001A27D
		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) > FoodTypeFlags.None;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001C094 File Offset: 0x0001A294
		public void ResolveReferencesSpecial()
		{
			if (this.specificMeatDef != null)
			{
				this.meatDef = this.specificMeatDef;
			}
			else if (this.useMeatFrom != null)
			{
				this.meatDef = this.useMeatFrom.race.meatDef;
			}
			if (this.useLeatherFrom != null)
			{
				this.leatherDef = this.useLeatherFrom.race.leatherDef;
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001C0F3 File Offset: 0x0001A2F3
		public IEnumerable<string> ConfigErrors(ThingDef thingDef)
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
			if (thingDef.IsCaravanRideable())
			{
				if (!this.lifeStageAges.Any((LifeStageAge s) => s.def.caravanRideable))
				{
					yield return "must contain at least one lifeStage with caravanRideable when CaravanRidingSpeedFactor is defined";
				}
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
			if (this.specificMeatDef != null && this.useMeatFrom != null)
			{
				yield return "specificMeatDef and useMeatFrom are both non-null. specificMeatDef will be chosen.";
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
			if (this.fleshType == FleshTypeDefOf.Normal && this.gestationPeriodDays < 0f)
			{
				yield return "normal flesh but gestationPeriodDays not configured.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001C10A File Offset: 0x0001A30A
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
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "HungerRate".Translate(), ((req.Thing as Pawn).needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false) * 60000f).ToString("0.##"), RaceProperties.NutritionEatenPerDayExplanation(req.Thing as Pawn, false, false, true), 1600, null, null, false);
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
			if (parentDef.race.Animal || this.FenceBlocked)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_BlockedByFences".Translate(), this.FenceBlocked ? "Yes".Translate() : "No".Translate(), "Stat_Race_BlockedByFences_Desc".Translate(), 2040, null, null, false);
			}
			if (parentDef.race.Animal)
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "PackAnimal".Translate(), this.packAnimal ? "Yes".Translate() : "No".Translate(), "PackAnimalExplanation".Translate(), 2202, null, null, false);
				yield return statDrawEntry2;
				Pawn pawn;
				if (req.HasThing && (pawn = (req.Thing as Pawn)) != null && pawn.gender != Gender.None)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Sex".Translate(), pawn.gender.GetLabel(false).CapitalizeFirst(), pawn.gender.GetLabel(false).CapitalizeFirst(), 2208, null, null, false);
				}
				if (parentDef.race.nuzzleMtbHours > 0f)
				{
					StatDrawEntry statDrawEntry3 = new StatDrawEntry(StatCategoryDefOf.PawnSocial, "NuzzleInterval".Translate(), Mathf.RoundToInt(parentDef.race.nuzzleMtbHours * 2500f).ToStringTicksToPeriod(true, false, true, true), "NuzzleIntervalExplanation".Translate(), 500, null, null, false);
					yield return statDrawEntry3;
				}
				if (parentDef.race.roamMtbDays != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_RoamInterval".Translate(), Mathf.RoundToInt(parentDef.race.roamMtbDays.Value * 60000f).ToStringTicksToPeriod(true, false, true, true), "Stat_Race_RoamInterval_Desc".Translate(), 2030, null, null, false);
				}
				foreach (StatDrawEntry statDrawEntry4 in AnimalProductionUtility.AnimalProductionStats(parentDef))
				{
					yield return statDrawEntry4;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001C128 File Offset: 0x0001A328
		public static string NutritionEatenPerDayExplanation(Pawn p, bool showDiet = false, bool showLegend = false, bool showCalculations = true)
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

		// Token: 0x0400030B RID: 779
		public Intelligence intelligence;

		// Token: 0x0400030C RID: 780
		private FleshTypeDef fleshType;

		// Token: 0x0400030D RID: 781
		private ThingDef bloodDef;

		// Token: 0x0400030E RID: 782
		public bool hasGenders = true;

		// Token: 0x0400030F RID: 783
		public bool needsRest = true;

		// Token: 0x04000310 RID: 784
		public ThinkTreeDef thinkTreeMain;

		// Token: 0x04000311 RID: 785
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x04000312 RID: 786
		public PawnNameCategory nameCategory;

		// Token: 0x04000313 RID: 787
		public FoodTypeFlags foodType;

		// Token: 0x04000314 RID: 788
		public BodyDef body;

		// Token: 0x04000315 RID: 789
		public Type deathActionWorkerClass;

		// Token: 0x04000316 RID: 790
		public List<AnimalBiomeRecord> wildBiomes;

		// Token: 0x04000317 RID: 791
		public SimpleCurve ageGenerationCurve;

		// Token: 0x04000318 RID: 792
		public bool makesFootprints;

		// Token: 0x04000319 RID: 793
		public int executionRange = 2;

		// Token: 0x0400031A RID: 794
		public float lifeExpectancy = 10f;

		// Token: 0x0400031B RID: 795
		public List<HediffGiverSetDef> hediffGiverSets;

		// Token: 0x0400031C RID: 796
		public float? roamMtbDays;

		// Token: 0x0400031D RID: 797
		public bool allowedOnCaravan = true;

		// Token: 0x0400031E RID: 798
		public bool canReleaseToWild = true;

		// Token: 0x0400031F RID: 799
		public bool playerCanChangeMaster = true;

		// Token: 0x04000320 RID: 800
		public bool showTrainables = true;

		// Token: 0x04000321 RID: 801
		public bool herdAnimal;

		// Token: 0x04000322 RID: 802
		public bool packAnimal;

		// Token: 0x04000323 RID: 803
		public bool predator;

		// Token: 0x04000324 RID: 804
		public float maxPreyBodySize = 99999f;

		// Token: 0x04000325 RID: 805
		public float wildness;

		// Token: 0x04000326 RID: 806
		public float petness;

		// Token: 0x04000327 RID: 807
		public float nuzzleMtbHours = -1f;

		// Token: 0x04000328 RID: 808
		public float manhunterOnDamageChance;

		// Token: 0x04000329 RID: 809
		public float manhunterOnTameFailChance;

		// Token: 0x0400032A RID: 810
		public bool canBePredatorPrey = true;

		// Token: 0x0400032B RID: 811
		public bool herdMigrationAllowed = true;

		// Token: 0x0400032C RID: 812
		public AnimalType animalType;

		// Token: 0x0400032D RID: 813
		public List<ThingDef> willNeverEat;

		// Token: 0x0400032E RID: 814
		public bool giveNonToolUserBeatFireVerb;

		// Token: 0x0400032F RID: 815
		public float gestationPeriodDays = -1f;

		// Token: 0x04000330 RID: 816
		public SimpleCurve litterSizeCurve;

		// Token: 0x04000331 RID: 817
		public float mateMtbHours = 12f;

		// Token: 0x04000332 RID: 818
		[NoTranslate]
		public List<string> untrainableTags;

		// Token: 0x04000333 RID: 819
		[NoTranslate]
		public List<string> trainableTags;

		// Token: 0x04000334 RID: 820
		public TrainabilityDef trainability;

		// Token: 0x04000335 RID: 821
		private RulePackDef nameGenerator;

		// Token: 0x04000336 RID: 822
		private RulePackDef nameGeneratorFemale;

		// Token: 0x04000337 RID: 823
		public float nameOnTameChance;

		// Token: 0x04000338 RID: 824
		public float baseBodySize = 1f;

		// Token: 0x04000339 RID: 825
		public float baseHealthScale = 1f;

		// Token: 0x0400033A RID: 826
		public float baseHungerRate = 1f;

		// Token: 0x0400033B RID: 827
		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		// Token: 0x0400033C RID: 828
		[MustTranslate]
		public string meatLabel;

		// Token: 0x0400033D RID: 829
		public Color meatColor = Color.white;

		// Token: 0x0400033E RID: 830
		public float meatMarketValue = 2f;

		// Token: 0x0400033F RID: 831
		public ThingDef specificMeatDef;

		// Token: 0x04000340 RID: 832
		public ThingDef useMeatFrom;

		// Token: 0x04000341 RID: 833
		public ThingDef useLeatherFrom;

		// Token: 0x04000342 RID: 834
		public ThingDef leatherDef;

		// Token: 0x04000343 RID: 835
		public ShadowData specialShadowData;

		// Token: 0x04000344 RID: 836
		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

		// Token: 0x04000345 RID: 837
		public SoundDef soundMeleeHitPawn;

		// Token: 0x04000346 RID: 838
		public SoundDef soundMeleeHitBuilding;

		// Token: 0x04000347 RID: 839
		public SoundDef soundMeleeMiss;

		// Token: 0x04000348 RID: 840
		public SoundDef soundMeleeDodge;

		// Token: 0x04000349 RID: 841
		[Unsaved(false)]
		private DeathActionWorker deathActionWorkerInt;

		// Token: 0x0400034A RID: 842
		[Unsaved(false)]
		public ThingDef meatDef;

		// Token: 0x0400034B RID: 843
		[Unsaved(false)]
		public ThingDef corpseDef;

		// Token: 0x0400034C RID: 844
		[Unsaved(false)]
		private PawnKindDef cachedAnyPawnKind;
	}
}
