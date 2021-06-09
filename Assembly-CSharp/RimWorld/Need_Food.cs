using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E8 RID: 5352
	public class Need_Food : Need
	{
		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x0600734D RID: 29517 RVA: 0x0004D972 File Offset: 0x0004BB72
		public bool Starving
		{
			get
			{
				return this.CurCategory == HungerCategory.Starving;
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x0600734E RID: 29518 RVA: 0x0004D97D File Offset: 0x0004BB7D
		public float PercentageThreshUrgentlyHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.4f;
			}
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x0600734F RID: 29519 RVA: 0x0004D995 File Offset: 0x0004BB95
		public float PercentageThreshHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.8f;
			}
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06007350 RID: 29520 RVA: 0x0004D9AD File Offset: 0x0004BBAD
		public float NutritionBetweenHungryAndFed
		{
			get
			{
				return (1f - this.PercentageThreshHungry) * this.MaxLevel;
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06007351 RID: 29521 RVA: 0x0004D9C2 File Offset: 0x0004BBC2
		public HungerCategory CurCategory
		{
			get
			{
				if (base.CurLevelPercentage <= 0f)
				{
					return HungerCategory.Starving;
				}
				if (base.CurLevelPercentage < this.PercentageThreshUrgentlyHungry)
				{
					return HungerCategory.UrgentlyHungry;
				}
				if (base.CurLevelPercentage < this.PercentageThreshHungry)
				{
					return HungerCategory.Hungry;
				}
				return HungerCategory.Fed;
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06007352 RID: 29522 RVA: 0x0004D9F4 File Offset: 0x0004BBF4
		public float FoodFallPerTick
		{
			get
			{
				return this.FoodFallPerTickAssumingCategory(this.CurCategory, false);
			}
		}

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x06007353 RID: 29523 RVA: 0x0004DA03 File Offset: 0x0004BC03
		public int TicksUntilHungryWhenFed
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false));
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x06007354 RID: 29524 RVA: 0x0004DA19 File Offset: 0x0004BC19
		public int TicksUntilHungryWhenFedIgnoringMalnutrition
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true));
			}
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x06007355 RID: 29525 RVA: 0x000236C9 File Offset: 0x000218C9
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x06007356 RID: 29526 RVA: 0x0004DA2F File Offset: 0x0004BC2F
		public override float MaxLevel
		{
			get
			{
				return this.pawn.BodySize * this.pawn.ageTracker.CurLifeStage.foodMaxFactor;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x06007357 RID: 29527 RVA: 0x0004DA52 File Offset: 0x0004BC52
		public float NutritionWanted
		{
			get
			{
				return this.MaxLevel - this.CurLevel;
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x06007358 RID: 29528 RVA: 0x00233510 File Offset: 0x00231710
		private float HungerRate
		{
			get
			{
				return this.pawn.ageTracker.CurLifeStage.hungerRateFactor * this.pawn.RaceProps.baseHungerRate * this.pawn.health.hediffSet.HungerRateFactor * ((this.pawn.story == null || this.pawn.story.traits == null) ? 1f : this.pawn.story.traits.HungerRateFactor) * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06007359 RID: 29529 RVA: 0x002335A8 File Offset: 0x002317A8
		private float HungerRateIgnoringMalnutrition
		{
			get
			{
				return this.pawn.ageTracker.CurLifeStage.hungerRateFactor * this.pawn.RaceProps.baseHungerRate * this.pawn.health.hediffSet.GetHungerRateFactor(HediffDefOf.Malnutrition) * ((this.pawn.story == null || this.pawn.story.traits == null) ? 1f : this.pawn.story.traits.HungerRateFactor) * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x0600735A RID: 29530 RVA: 0x0004DA61 File Offset: 0x0004BC61
		public int TicksStarving
		{
			get
			{
				return Mathf.Max(0, Find.TickManager.TicksGame - this.lastNonStarvingTick);
			}
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x0600735B RID: 29531 RVA: 0x0004DA7A File Offset: 0x0004BC7A
		private float MalnutritionSeverityPerInterval
		{
			get
			{
				return 0.0011333333f * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(this.pawn.thingIDNumber ^ 2551674));
			}
		}

		// Token: 0x0600735C RID: 29532 RVA: 0x0004DAA7 File Offset: 0x0004BCA7
		public Need_Food(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600735D RID: 29533 RVA: 0x0004DABB File Offset: 0x0004BCBB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastNonStarvingTick, "lastNonStarvingTick", -99999, false);
		}

		// Token: 0x0600735E RID: 29534 RVA: 0x00233644 File Offset: 0x00231844
		public float FoodFallPerTickAssumingCategory(HungerCategory cat, bool ignoreMalnutrition = false)
		{
			float num = ignoreMalnutrition ? this.HungerRateIgnoringMalnutrition : this.HungerRate;
			switch (cat)
			{
			case HungerCategory.Fed:
				return 2.6666667E-05f * num;
			case HungerCategory.Hungry:
				return 2.6666667E-05f * num * 0.5f;
			case HungerCategory.UrgentlyHungry:
				return 2.6666667E-05f * num * 0.25f;
			case HungerCategory.Starving:
				return 2.6666667E-05f * num * 0.15f;
			default:
				return 999f;
			}
		}

		// Token: 0x0600735F RID: 29535 RVA: 0x002336B4 File Offset: 0x002318B4
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.FoodFallPerTick * 150f;
			}
			if (!this.Starving)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
			if (!this.IsFrozen)
			{
				if (this.Starving)
				{
					HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, this.MalnutritionSeverityPerInterval);
					return;
				}
				HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, -this.MalnutritionSeverityPerInterval);
			}
		}

		// Token: 0x06007360 RID: 29536 RVA: 0x00233738 File Offset: 0x00231938
		public override void SetInitialLevel()
		{
			if (this.pawn.RaceProps.Humanlike)
			{
				base.CurLevelPercentage = 0.8f;
			}
			else
			{
				base.CurLevelPercentage = Rand.Range(0.5f, 0.9f);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06007361 RID: 29537 RVA: 0x00233794 File Offset: 0x00231994
		public override string GetTipString()
		{
			return string.Concat(new string[]
			{
				base.LabelCap,
				": ",
				base.CurLevelPercentage.ToStringPercent(),
				" (",
				this.CurLevel.ToString("0.##"),
				" / ",
				this.MaxLevel.ToString("0.##"),
				")\n",
				this.def.description
			});
		}

		// Token: 0x06007362 RID: 29538 RVA: 0x00233820 File Offset: 0x00231A20
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.PercentageThreshHungry);
			this.threshPercents.Add(this.PercentageThreshUrgentlyHungry);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}

		// Token: 0x04004C10 RID: 19472
		private int lastNonStarvingTick = -99999;

		// Token: 0x04004C11 RID: 19473
		public const float BaseFoodFallPerTick = 2.6666667E-05f;

		// Token: 0x04004C12 RID: 19474
		private const float BaseMalnutritionSeverityPerDay = 0.17f;

		// Token: 0x04004C13 RID: 19475
		private const float BaseMalnutritionSeverityPerInterval = 0.0011333333f;
	}
}
