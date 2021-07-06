using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E1 RID: 5345
	public class Need_Chemical_Any : Need
	{
		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x0600733A RID: 29498 RVA: 0x0004D877 File Offset: 0x0004BA77
		private Trait TraitDrugDesire
		{
			get
			{
				return this.pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x0600733B RID: 29499 RVA: 0x0004D893 File Offset: 0x0004BA93
		private SimpleCurve FallCurve
		{
			get
			{
				if (this.TraitDrugDesire.Degree == 2)
				{
					return Need_Chemical_Any.FascinationDegreeFallCurve;
				}
				return Need_Chemical_Any.InterestDegreeFallCurve;
			}
		}

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x0600733C RID: 29500 RVA: 0x00233074 File Offset: 0x00231274
		private float FallPerNeedIntervalTick
		{
			get
			{
				Trait traitDrugDesire = this.TraitDrugDesire;
				float num = 1f;
				if (traitDrugDesire.Degree == 2)
				{
					num = 1.25f;
				}
				num *= this.FallCurve.Evaluate(this.CurLevel);
				return this.def.fallPerDay * num / 60000f * 150f;
			}
		}

		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x0600733D RID: 29501 RVA: 0x0004D8AE File Offset: 0x0004BAAE
		private Need_Chemical_Any.LevelThresholds CurrentLevelThresholds
		{
			get
			{
				if (this.TraitDrugDesire.Degree == 2)
				{
					return Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood;
				}
				return Need_Chemical_Any.InterestDegreeLevelThresholdsForMood;
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x0600733E RID: 29502 RVA: 0x002330C8 File Offset: 0x002312C8
		public Need_Chemical_Any.MoodBuff MoodBuffForCurrentLevel
		{
			get
			{
				if (this.Disabled)
				{
					return Need_Chemical_Any.MoodBuff.Neutral;
				}
				Need_Chemical_Any.LevelThresholds currentLevelThresholds = this.CurrentLevelThresholds;
				float curLevel = this.CurLevel;
				if (curLevel <= currentLevelThresholds.extremelyNegative)
				{
					return Need_Chemical_Any.MoodBuff.ExtremelyNegative;
				}
				if (curLevel <= currentLevelThresholds.veryNegative)
				{
					return Need_Chemical_Any.MoodBuff.VeryNegative;
				}
				if (curLevel <= currentLevelThresholds.negative)
				{
					return Need_Chemical_Any.MoodBuff.Negative;
				}
				if (curLevel <= currentLevelThresholds.positive)
				{
					return Need_Chemical_Any.MoodBuff.Neutral;
				}
				if (curLevel <= currentLevelThresholds.veryPositive)
				{
					return Need_Chemical_Any.MoodBuff.Positive;
				}
				return Need_Chemical_Any.MoodBuff.VeryPositive;
			}
		}

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x0600733F RID: 29503 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x06007340 RID: 29504 RVA: 0x0004D8C9 File Offset: 0x0004BAC9
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x06007341 RID: 29505 RVA: 0x0004D8D4 File Offset: 0x0004BAD4
		private bool Disabled
		{
			get
			{
				return this.TraitDrugDesire == null || this.TraitDrugDesire.Degree < 1;
			}
		}

		// Token: 0x06007342 RID: 29506 RVA: 0x00233128 File Offset: 0x00231328
		public void Notify_IngestedDrug(Thing drug)
		{
			if (this.Disabled)
			{
				return;
			}
			DrugCategory drugCategory = drug.def.ingestible.drugCategory;
			if (drugCategory == DrugCategory.Social)
			{
				this.CurLevel += 0.2f;
				return;
			}
			if (drugCategory != DrugCategory.Hard)
			{
				return;
			}
			this.CurLevel += 0.3f;
		}

		// Token: 0x06007343 RID: 29507 RVA: 0x0004D8EE File Offset: 0x0004BAEE
		public Need_Chemical_Any(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06007344 RID: 29508 RVA: 0x0004D8F7 File Offset: 0x0004BAF7
		public override void SetInitialLevel()
		{
			this.CurLevel = 0.5f;
		}

		// Token: 0x06007345 RID: 29509 RVA: 0x00233180 File Offset: 0x00231380
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			Trait traitDrugDesire = this.TraitDrugDesire;
			if (traitDrugDesire != null && this.lastThresholdUpdateTraitRef != traitDrugDesire)
			{
				this.lastThresholdUpdateTraitRef = traitDrugDesire;
				this.threshPercents = new List<float>();
				Need_Chemical_Any.LevelThresholds currentLevelThresholds = this.CurrentLevelThresholds;
				this.threshPercents.Add(currentLevelThresholds.extremelyNegative);
				this.threshPercents.Add(currentLevelThresholds.veryNegative);
				this.threshPercents.Add(currentLevelThresholds.negative);
				this.threshPercents.Add(currentLevelThresholds.positive);
				this.threshPercents.Add(currentLevelThresholds.veryPositive);
			}
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}

		// Token: 0x06007346 RID: 29510 RVA: 0x0004D904 File Offset: 0x0004BB04
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.SetInitialLevel();
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			this.CurLevel -= this.FallPerNeedIntervalTick;
		}

		// Token: 0x04004BE6 RID: 19430
		public const int InterestTraitDegree = 1;

		// Token: 0x04004BE7 RID: 19431
		public const int FascinationTraitDegree = 2;

		// Token: 0x04004BE8 RID: 19432
		private const float FallPerTickFactorForChemicalFascination = 1.25f;

		// Token: 0x04004BE9 RID: 19433
		public const float GainForHardDrugIngestion = 0.3f;

		// Token: 0x04004BEA RID: 19434
		public const float GainForSocialDrugIngestion = 0.2f;

		// Token: 0x04004BEB RID: 19435
		private static readonly SimpleCurve InterestDegreeFallCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.3f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative, 0.6f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative + 0.001f, 1f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.positive, 1f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x04004BEC RID: 19436
		private static readonly SimpleCurve FascinationDegreeFallCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.4f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative, 0.7f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative + 0.001f, 1f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.positive, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.15f),
				true
			}
		};

		// Token: 0x04004BED RID: 19437
		private static readonly Need_Chemical_Any.LevelThresholds FascinationDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.1f,
			veryNegative = 0.25f,
			negative = 0.4f,
			positive = 0.7f,
			veryPositive = 0.85f
		};

		// Token: 0x04004BEE RID: 19438
		private static readonly Need_Chemical_Any.LevelThresholds InterestDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.01f,
			veryNegative = 0.15f,
			negative = 0.3f,
			positive = 0.6f,
			veryPositive = 0.75f
		};

		// Token: 0x04004BEF RID: 19439
		private Trait lastThresholdUpdateTraitRef;

		// Token: 0x020014E2 RID: 5346
		public enum MoodBuff
		{
			// Token: 0x04004BF1 RID: 19441
			ExtremelyNegative,
			// Token: 0x04004BF2 RID: 19442
			VeryNegative,
			// Token: 0x04004BF3 RID: 19443
			Negative,
			// Token: 0x04004BF4 RID: 19444
			Neutral,
			// Token: 0x04004BF5 RID: 19445
			Positive,
			// Token: 0x04004BF6 RID: 19446
			VeryPositive
		}

		// Token: 0x020014E3 RID: 5347
		public struct LevelThresholds
		{
			// Token: 0x04004BF7 RID: 19447
			public float extremelyNegative;

			// Token: 0x04004BF8 RID: 19448
			public float veryNegative;

			// Token: 0x04004BF9 RID: 19449
			public float negative;

			// Token: 0x04004BFA RID: 19450
			public float positive;

			// Token: 0x04004BFB RID: 19451
			public float veryPositive;
		}
	}
}
