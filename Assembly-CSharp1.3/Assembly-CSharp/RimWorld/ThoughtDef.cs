using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ADB RID: 2779
	public class ThoughtDef : Def
	{
		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x0015F81C File Offset: 0x0015DA1C
		public string Label
		{
			get
			{
				if (!this.label.NullOrEmpty())
				{
					return this.label;
				}
				if (!this.stages.NullOrEmpty<ThoughtStage>())
				{
					if (!this.stages[0].label.NullOrEmpty())
					{
						return this.stages[0].label;
					}
					if (!this.stages[0].labelSocial.NullOrEmpty())
					{
						return this.stages[0].labelSocial;
					}
				}
				Log.Error("Cannot get good label for ThoughtDef " + this.defName);
				return this.defName;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06004174 RID: 16756 RVA: 0x0015F8B9 File Offset: 0x0015DAB9
		public override TaggedString LabelCap
		{
			get
			{
				if (this.Label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.Label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06004175 RID: 16757 RVA: 0x0015F8F8 File Offset: 0x0015DAF8
		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000f);
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06004176 RID: 16758 RVA: 0x0015F908 File Offset: 0x0015DB08
		public bool IsMemory
		{
			get
			{
				if (this.isMemoryCached == BoolUnknown.Unknown)
				{
					this.isMemoryCached = ((this.durationDays > 0f || typeof(Thought_Memory).IsAssignableFrom(this.thoughtClass)) ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.isMemoryCached == BoolUnknown.True;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06004177 RID: 16759 RVA: 0x0015F955 File Offset: 0x0015DB55
		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06004178 RID: 16760 RVA: 0x0015F960 File Offset: 0x0015DB60
		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06004179 RID: 16761 RVA: 0x0015F977 File Offset: 0x0015DB77
		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != int.MinValue;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x0600417A RID: 16762 RVA: 0x0015F989 File Offset: 0x0015DB89
		public ThoughtWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (ThoughtWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x0600417B RID: 16763 RVA: 0x0015F9C9 File Offset: 0x0015DBC9
		public Type ThoughtClass
		{
			get
			{
				if (this.thoughtClass != null)
				{
					return this.thoughtClass;
				}
				if (this.IsMemory)
				{
					return typeof(Thought_Memory);
				}
				return typeof(Thought_Situational);
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x0600417C RID: 16764 RVA: 0x0015F9FD File Offset: 0x0015DBFD
		public Texture2D Icon
		{
			get
			{
				if (this.iconInt == null)
				{
					if (this.icon == null)
					{
						return null;
					}
					this.iconInt = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconInt;
			}
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x0015FA2F File Offset: 0x0015DC2F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.stages.NullOrEmpty<ThoughtStage>())
			{
				yield return "no stages";
			}
			if (this.workerClass != null && this.nextThought != null)
			{
				yield return "has a nextThought but also has a workerClass. nextThought only works for memories";
			}
			if (this.IsMemory && this.workerClass != null)
			{
				yield return "has a workerClass but is a memory. workerClass only works for situational thoughts, not memories";
			}
			if (!this.IsMemory && this.workerClass == null && this.IsSituational)
			{
				yield return "is a situational thought but has no workerClass. Situational thoughts require workerClasses to analyze the situation";
			}
			int num;
			for (int i = 0; i < this.stages.Count; i = num + 1)
			{
				if (this.stages[i] != null)
				{
					foreach (string text2 in this.stages[i].ConfigErrors())
					{
						yield return text2;
					}
					enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x0015FA3F File Offset: 0x0015DC3F
		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
		}

		// Token: 0x04002765 RID: 10085
		public Type thoughtClass;

		// Token: 0x04002766 RID: 10086
		public Type workerClass;

		// Token: 0x04002767 RID: 10087
		public List<ThoughtStage> stages = new List<ThoughtStage>();

		// Token: 0x04002768 RID: 10088
		public int stackLimit = 1;

		// Token: 0x04002769 RID: 10089
		public float stackedEffectMultiplier = 0.75f;

		// Token: 0x0400276A RID: 10090
		public float durationDays;

		// Token: 0x0400276B RID: 10091
		public bool invert;

		// Token: 0x0400276C RID: 10092
		public bool validWhileDespawned;

		// Token: 0x0400276D RID: 10093
		public ThoughtDef nextThought;

		// Token: 0x0400276E RID: 10094
		public ThoughtDef producesMemoryThought;

		// Token: 0x0400276F RID: 10095
		public List<TraitDef> nullifyingTraits;

		// Token: 0x04002770 RID: 10096
		public List<TraitDef> neverNullifyIfAnyTrait;

		// Token: 0x04002771 RID: 10097
		public List<TraitRequirement> nullifyingTraitDegrees;

		// Token: 0x04002772 RID: 10098
		public List<TaleDef> nullifyingOwnTales;

		// Token: 0x04002773 RID: 10099
		public List<PreceptDef> nullifyingPrecepts;

		// Token: 0x04002774 RID: 10100
		public List<TraitDef> requiredTraits;

		// Token: 0x04002775 RID: 10101
		public int requiredTraitsDegree = int.MinValue;

		// Token: 0x04002776 RID: 10102
		public StatDef effectMultiplyingStat;

		// Token: 0x04002777 RID: 10103
		public HediffDef hediff;

		// Token: 0x04002778 RID: 10104
		public GameConditionDef gameCondition;

		// Token: 0x04002779 RID: 10105
		public bool nullifiedIfNotColonist;

		// Token: 0x0400277A RID: 10106
		public ThoughtDef thoughtToMake;

		// Token: 0x0400277B RID: 10107
		[NoTranslate]
		private string icon;

		// Token: 0x0400277C RID: 10108
		public bool showBubble;

		// Token: 0x0400277D RID: 10109
		public ExpectationDef minExpectationForNegativeThought;

		// Token: 0x0400277E RID: 10110
		public bool lerpMoodToZero;

		// Token: 0x0400277F RID: 10111
		public int stackLimitForSameOtherPawn = -1;

		// Token: 0x04002780 RID: 10112
		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		// Token: 0x04002781 RID: 10113
		public float maxCumulatedOpinionOffset = float.MaxValue;

		// Token: 0x04002782 RID: 10114
		public TaleDef taleDef;

		// Token: 0x04002783 RID: 10115
		public Gender gender;

		// Token: 0x04002784 RID: 10116
		[Unsaved(false)]
		private ThoughtWorker workerInt;

		// Token: 0x04002785 RID: 10117
		[Unsaved(false)]
		private BoolUnknown isMemoryCached = BoolUnknown.Unknown;

		// Token: 0x04002786 RID: 10118
		private Texture2D iconInt;
	}
}
