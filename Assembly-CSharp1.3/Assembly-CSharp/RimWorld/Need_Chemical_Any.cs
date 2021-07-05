using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E44 RID: 3652
	public class Need_Chemical_Any : Need
	{
		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x0600549A RID: 21658 RVA: 0x001CABCD File Offset: 0x001C8DCD
		private Trait TraitDrugDesire
		{
			get
			{
				return this.pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x0600549B RID: 21659 RVA: 0x001CABE9 File Offset: 0x001C8DE9
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

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x0600549C RID: 21660 RVA: 0x001CAC04 File Offset: 0x001C8E04
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

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x0600549D RID: 21661 RVA: 0x001CAC58 File Offset: 0x001C8E58
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

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x0600549E RID: 21662 RVA: 0x001CAC74 File Offset: 0x001C8E74
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

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x0600549F RID: 21663 RVA: 0x0001276E File Offset: 0x0001096E
		public override int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x060054A0 RID: 21664 RVA: 0x001CACD1 File Offset: 0x001C8ED1
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x060054A1 RID: 21665 RVA: 0x001CACDC File Offset: 0x001C8EDC
		private bool Disabled
		{
			get
			{
				return this.TraitDrugDesire == null || this.TraitDrugDesire.Degree < 1;
			}
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x001CACF8 File Offset: 0x001C8EF8
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

		// Token: 0x060054A3 RID: 21667 RVA: 0x001CAD4D File Offset: 0x001C8F4D
		public Need_Chemical_Any(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x001CAD56 File Offset: 0x001C8F56
		public override void SetInitialLevel()
		{
			this.CurLevel = 0.5f;
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x001CAD64 File Offset: 0x001C8F64
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null)
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
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip);
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x001CAE01 File Offset: 0x001C9001
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

		// Token: 0x040031E4 RID: 12772
		public const int InterestTraitDegree = 1;

		// Token: 0x040031E5 RID: 12773
		public const int FascinationTraitDegree = 2;

		// Token: 0x040031E6 RID: 12774
		private const float FallPerTickFactorForChemicalFascination = 1.25f;

		// Token: 0x040031E7 RID: 12775
		public const float GainForHardDrugIngestion = 0.3f;

		// Token: 0x040031E8 RID: 12776
		public const float GainForSocialDrugIngestion = 0.2f;

		// Token: 0x040031E9 RID: 12777
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

		// Token: 0x040031EA RID: 12778
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

		// Token: 0x040031EB RID: 12779
		private static readonly Need_Chemical_Any.LevelThresholds FascinationDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.1f,
			veryNegative = 0.25f,
			negative = 0.4f,
			positive = 0.7f,
			veryPositive = 0.85f
		};

		// Token: 0x040031EC RID: 12780
		private static readonly Need_Chemical_Any.LevelThresholds InterestDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.01f,
			veryNegative = 0.15f,
			negative = 0.3f,
			positive = 0.6f,
			veryPositive = 0.75f
		};

		// Token: 0x040031ED RID: 12781
		private Trait lastThresholdUpdateTraitRef;

		// Token: 0x020022AF RID: 8879
		public enum MoodBuff
		{
			// Token: 0x0400844A RID: 33866
			ExtremelyNegative,
			// Token: 0x0400844B RID: 33867
			VeryNegative,
			// Token: 0x0400844C RID: 33868
			Negative,
			// Token: 0x0400844D RID: 33869
			Neutral,
			// Token: 0x0400844E RID: 33870
			Positive,
			// Token: 0x0400844F RID: 33871
			VeryPositive
		}

		// Token: 0x020022B0 RID: 8880
		public struct LevelThresholds
		{
			// Token: 0x04008450 RID: 33872
			public float extremelyNegative;

			// Token: 0x04008451 RID: 33873
			public float veryNegative;

			// Token: 0x04008452 RID: 33874
			public float negative;

			// Token: 0x04008453 RID: 33875
			public float positive;

			// Token: 0x04008454 RID: 33876
			public float veryPositive;
		}
	}
}
