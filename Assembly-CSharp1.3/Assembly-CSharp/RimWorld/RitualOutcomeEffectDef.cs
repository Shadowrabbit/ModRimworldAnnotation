using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6B RID: 3947
	public class RitualOutcomeEffectDef : Def
	{
		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x06005D92 RID: 23954 RVA: 0x0020145C File Offset: 0x001FF65C
		public string Description
		{
			get
			{
				if (this.minMoodCached == 3.4028235E+38f && !this.outcomeChances.NullOrEmpty<OutcomeChance>())
				{
					foreach (OutcomeChance outcomeChance in this.outcomeChances)
					{
						if (outcomeChance.memory != null)
						{
							float baseMoodEffect = outcomeChance.memory.stages[0].baseMoodEffect;
							if (baseMoodEffect > this.maxMoodCached)
							{
								this.maxMoodCached = baseMoodEffect;
							}
							if (baseMoodEffect < this.minMoodCached)
							{
								this.minMoodCached = baseMoodEffect;
							}
							this.moodDurationCached = outcomeChance.memory.durationDays;
						}
					}
				}
				return this.description.Formatted(this.minMoodCached.Named("MINMOOD"), this.maxMoodCached.ToStringWithSign("0.##").Named("MAXMOOD"), this.moodDurationCached.Named("MOODDAYS"));
			}
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x06005D93 RID: 23955 RVA: 0x0020156C File Offset: 0x001FF76C
		public OutcomeChance BestOutcome
		{
			get
			{
				if (this.bestOutcomeCached == null)
				{
					this.bestOutcomeCached = this.outcomeChances.MaxBy((OutcomeChance o) => o.positivityIndex);
				}
				return this.bestOutcomeCached;
			}
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x06005D94 RID: 23956 RVA: 0x002015AC File Offset: 0x001FF7AC
		public OutcomeChance WorstOutcome
		{
			get
			{
				if (this.worstOutcomeCached == null)
				{
					this.worstOutcomeCached = this.outcomeChances.MinBy((OutcomeChance o) => o.positivityIndex);
				}
				return this.worstOutcomeCached;
			}
		}

		// Token: 0x06005D95 RID: 23957 RVA: 0x002015EC File Offset: 0x001FF7EC
		public RitualOutcomeEffectWorker GetInstance()
		{
			return (RitualOutcomeEffectWorker)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x06005D96 RID: 23958 RVA: 0x00201608 File Offset: 0x001FF808
		public string OutcomeMoodBreakdown(OutcomeChance outcome)
		{
			if (outcome.memory != null && outcome.memory.stages[0].baseMoodEffect != 0f)
			{
				return "RitualOutcomeExtraDesc_Mood".Translate(outcome.memory.stages[0].baseMoodEffect.ToStringWithSign("0.##"), outcome.memory.durationDays);
			}
			return "";
		}

		// Token: 0x06005D97 RID: 23959 RVA: 0x00201684 File Offset: 0x001FF884
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			float num = 0f;
			foreach (OutcomeChance outcomeChance in this.outcomeChances)
			{
				num += outcomeChance.chance;
			}
			if (this.outcomeChances.Any<OutcomeChance>() && Mathf.Abs(num - 1f) > 0.0001f)
			{
				yield return "Sum of outcome chances doesn't add up to 1.0. total=" + num;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400360F RID: 13839
		public Type workerClass;

		// Token: 0x04003610 RID: 13840
		public float startingQuality;

		// Token: 0x04003611 RID: 13841
		public float minQuality;

		// Token: 0x04003612 RID: 13842
		public float maxQuality = 1f;

		// Token: 0x04003613 RID: 13843
		public ThoughtDef memoryDef;

		// Token: 0x04003614 RID: 13844
		public List<OutcomeChance> outcomeChances = new List<OutcomeChance>();

		// Token: 0x04003615 RID: 13845
		public SimpleCurve honorFromQuality;

		// Token: 0x04003616 RID: 13846
		[MustTranslate]
		public List<string> extraPredictedOutcomeDescriptions;

		// Token: 0x04003617 RID: 13847
		[MustTranslate]
		public List<string> extraInfoLines;

		// Token: 0x04003618 RID: 13848
		public EffecterDef effecter;

		// Token: 0x04003619 RID: 13849
		public FleckDef fleckDef;

		// Token: 0x0400361A RID: 13850
		public int flecksPerCell;

		// Token: 0x0400361B RID: 13851
		public FloatRange fleckRotationRange = new FloatRange(0f, 360f);

		// Token: 0x0400361C RID: 13852
		public FloatRange fleckScaleRange = FloatRange.One;

		// Token: 0x0400361D RID: 13853
		public FloatRange fleckVelocityAngle = FloatRange.Zero;

		// Token: 0x0400361E RID: 13854
		public FloatRange fleckVelocitySpeed = FloatRange.Zero;

		// Token: 0x0400361F RID: 13855
		public ThingDef filthDefToSpawn;

		// Token: 0x04003620 RID: 13856
		public IntRange filthCountToSpawn = IntRange.zero;

		// Token: 0x04003621 RID: 13857
		public List<RitualOutcomeComp> comps;

		// Token: 0x04003622 RID: 13858
		public bool warnOnLowQuality = true;

		// Token: 0x04003623 RID: 13859
		public bool allowAttachableOutcome = true;

		// Token: 0x04003624 RID: 13860
		private float minMoodCached = float.MaxValue;

		// Token: 0x04003625 RID: 13861
		private float maxMoodCached = float.MinValue;

		// Token: 0x04003626 RID: 13862
		private float moodDurationCached = -1f;

		// Token: 0x04003627 RID: 13863
		private OutcomeChance bestOutcomeCached;

		// Token: 0x04003628 RID: 13864
		private OutcomeChance worstOutcomeCached;
	}
}
