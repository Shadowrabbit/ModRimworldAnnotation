using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E49 RID: 3657
	public class Need_Food : Need
	{
		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x060054AD RID: 21677 RVA: 0x001CB162 File Offset: 0x001C9362
		public bool Starving
		{
			get
			{
				return this.CurCategory == HungerCategory.Starving;
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x060054AE RID: 21678 RVA: 0x001CB16D File Offset: 0x001C936D
		public float PercentageThreshUrgentlyHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.4f;
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x060054AF RID: 21679 RVA: 0x001CB185 File Offset: 0x001C9385
		public float PercentageThreshHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.8f;
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x060054B0 RID: 21680 RVA: 0x001CB19D File Offset: 0x001C939D
		public float NutritionBetweenHungryAndFed
		{
			get
			{
				return (1f - this.PercentageThreshHungry) * this.MaxLevel;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x060054B1 RID: 21681 RVA: 0x001CB1B2 File Offset: 0x001C93B2
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

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x060054B2 RID: 21682 RVA: 0x001CB1E4 File Offset: 0x001C93E4
		public float FoodFallPerTick
		{
			get
			{
				return this.FoodFallPerTickAssumingCategory(this.CurCategory, false);
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x060054B3 RID: 21683 RVA: 0x001CB1F3 File Offset: 0x001C93F3
		public int TicksUntilHungryWhenFed
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false));
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x060054B4 RID: 21684 RVA: 0x001CB209 File Offset: 0x001C9409
		public int TicksUntilHungryWhenFedIgnoringMalnutrition
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true));
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x060054B5 RID: 21685 RVA: 0x000B955A File Offset: 0x000B775A
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x060054B6 RID: 21686 RVA: 0x001CB21F File Offset: 0x001C941F
		public override float MaxLevel
		{
			get
			{
				return this.pawn.BodySize * this.pawn.ageTracker.CurLifeStage.foodMaxFactor;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x060054B7 RID: 21687 RVA: 0x001CB242 File Offset: 0x001C9442
		public float NutritionWanted
		{
			get
			{
				return this.MaxLevel - this.CurLevel;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x060054B8 RID: 21688 RVA: 0x001CB254 File Offset: 0x001C9454
		private float HungerRate
		{
			get
			{
				return Need_Food.BaseHungerRateFactor(this.pawn.ageTracker.CurLifeStage, this.pawn.def) * this.pawn.health.hediffSet.HungerRateFactor * ((this.pawn.story == null || this.pawn.story.traits == null) ? 1f : this.pawn.story.traits.HungerRateFactor) * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x060054B9 RID: 21689 RVA: 0x001CB2E8 File Offset: 0x001C94E8
		private float HungerRateIgnoringMalnutrition
		{
			get
			{
				return this.pawn.ageTracker.CurLifeStage.hungerRateFactor * this.pawn.RaceProps.baseHungerRate * this.pawn.health.hediffSet.GetHungerRateFactor(HediffDefOf.Malnutrition) * ((this.pawn.story == null || this.pawn.story.traits == null) ? 1f : this.pawn.story.traits.HungerRateFactor) * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x060054BA RID: 21690 RVA: 0x001CB384 File Offset: 0x001C9584
		public int TicksStarving
		{
			get
			{
				return Mathf.Max(0, Find.TickManager.TicksGame - this.lastNonStarvingTick);
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x060054BB RID: 21691 RVA: 0x001CB39D File Offset: 0x001C959D
		private float MalnutritionSeverityPerInterval
		{
			get
			{
				return 0.0011333333f * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(this.pawn.thingIDNumber ^ 2551674));
			}
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x001CB3CA File Offset: 0x001C95CA
		public Need_Food(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x001CB3DE File Offset: 0x001C95DE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastNonStarvingTick, "lastNonStarvingTick", -99999, false);
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x001CB3FC File Offset: 0x001C95FC
		public float FoodFallPerTickAssumingCategory(HungerCategory cat, bool ignoreMalnutrition = false)
		{
			float hungerRate = ignoreMalnutrition ? this.HungerRateIgnoringMalnutrition : this.HungerRate;
			return Need_Food.BaseFoodFallPerTickAssumingCategory(cat, hungerRate);
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x001CB424 File Offset: 0x001C9624
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

		// Token: 0x060054C0 RID: 21696 RVA: 0x001CB4A8 File Offset: 0x001C96A8
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

		// Token: 0x060054C1 RID: 21697 RVA: 0x001CB504 File Offset: 0x001C9704
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

		// Token: 0x060054C2 RID: 21698 RVA: 0x001CB590 File Offset: 0x001C9790
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.PercentageThreshHungry);
			this.threshPercents.Add(this.PercentageThreshUrgentlyHungry);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip);
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x001CB5EC File Offset: 0x001C97EC
		public static float BaseFoodFallPerTickAssumingCategory(HungerCategory cat, float hungerRate)
		{
			switch (cat)
			{
			case HungerCategory.Fed:
				return 2.6666667E-05f * hungerRate;
			case HungerCategory.Hungry:
				return 2.6666667E-05f * hungerRate * 0.5f;
			case HungerCategory.UrgentlyHungry:
				return 2.6666667E-05f * hungerRate * 0.25f;
			case HungerCategory.Starving:
				return 0f;
			default:
				return 999f;
			}
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x001CB640 File Offset: 0x001C9840
		public static float BaseHungerRateFactor(LifeStageDef lifeStage, ThingDef pawnDef)
		{
			return lifeStage.hungerRateFactor * pawnDef.race.baseHungerRate;
		}

		// Token: 0x04003202 RID: 12802
		private int lastNonStarvingTick = -99999;

		// Token: 0x04003203 RID: 12803
		public const float BaseFoodFallPerTick = 2.6666667E-05f;

		// Token: 0x04003204 RID: 12804
		public const float FallPerTickFactor_Hungry = 0.5f;

		// Token: 0x04003205 RID: 12805
		public const float FallPerTickFactor_UrgentlyHungry = 0.25f;

		// Token: 0x04003206 RID: 12806
		private const float BaseMalnutritionSeverityPerDay = 0.17f;

		// Token: 0x04003207 RID: 12807
		private const float BaseMalnutritionSeverityPerInterval = 0.0011333333f;
	}
}
