using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFB RID: 4091
	public class ThoughtDef : Def
	{
		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x0600592C RID: 22828 RVA: 0x001D1A6C File Offset: 0x001CFC6C
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
				Log.Error("Cannot get good label for ThoughtDef " + this.defName, false);
				return this.defName;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x0600592D RID: 22829 RVA: 0x0003DE73 File Offset: 0x0003C073
		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000f);
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x0600592E RID: 22830 RVA: 0x001D1B0C File Offset: 0x001CFD0C
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

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x0600592F RID: 22831 RVA: 0x0003DE82 File Offset: 0x0003C082
		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06005930 RID: 22832 RVA: 0x0003DE8D File Offset: 0x0003C08D
		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06005931 RID: 22833 RVA: 0x0003DEA4 File Offset: 0x0003C0A4
		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != int.MinValue;
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06005932 RID: 22834 RVA: 0x0003DEB6 File Offset: 0x0003C0B6
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

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06005933 RID: 22835 RVA: 0x0003DEF6 File Offset: 0x0003C0F6
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

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06005934 RID: 22836 RVA: 0x0003DF2A File Offset: 0x0003C12A
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

		// Token: 0x06005935 RID: 22837 RVA: 0x0003DF5C File Offset: 0x0003C15C
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

		// Token: 0x06005936 RID: 22838 RVA: 0x0003DF6C File Offset: 0x0003C16C
		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
		}

		// Token: 0x04003BB2 RID: 15282
		public Type thoughtClass;

		// Token: 0x04003BB3 RID: 15283
		public Type workerClass;

		// Token: 0x04003BB4 RID: 15284
		public List<ThoughtStage> stages = new List<ThoughtStage>();

		// Token: 0x04003BB5 RID: 15285
		public int stackLimit = 1;

		// Token: 0x04003BB6 RID: 15286
		public float stackedEffectMultiplier = 0.75f;

		// Token: 0x04003BB7 RID: 15287
		public float durationDays;

		// Token: 0x04003BB8 RID: 15288
		public bool invert;

		// Token: 0x04003BB9 RID: 15289
		public bool validWhileDespawned;

		// Token: 0x04003BBA RID: 15290
		public ThoughtDef nextThought;

		// Token: 0x04003BBB RID: 15291
		public List<TraitDef> nullifyingTraits;

		// Token: 0x04003BBC RID: 15292
		public List<TaleDef> nullifyingOwnTales;

		// Token: 0x04003BBD RID: 15293
		public List<TraitDef> requiredTraits;

		// Token: 0x04003BBE RID: 15294
		public int requiredTraitsDegree = int.MinValue;

		// Token: 0x04003BBF RID: 15295
		public StatDef effectMultiplyingStat;

		// Token: 0x04003BC0 RID: 15296
		public HediffDef hediff;

		// Token: 0x04003BC1 RID: 15297
		public GameConditionDef gameCondition;

		// Token: 0x04003BC2 RID: 15298
		public bool nullifiedIfNotColonist;

		// Token: 0x04003BC3 RID: 15299
		public ThoughtDef thoughtToMake;

		// Token: 0x04003BC4 RID: 15300
		[NoTranslate]
		private string icon;

		// Token: 0x04003BC5 RID: 15301
		public bool showBubble;

		// Token: 0x04003BC6 RID: 15302
		public int stackLimitForSameOtherPawn = -1;

		// Token: 0x04003BC7 RID: 15303
		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		// Token: 0x04003BC8 RID: 15304
		public float maxCumulatedOpinionOffset = float.MaxValue;

		// Token: 0x04003BC9 RID: 15305
		public TaleDef taleDef;

		// Token: 0x04003BCA RID: 15306
		[Unsaved(false)]
		private ThoughtWorker workerInt;

		// Token: 0x04003BCB RID: 15307
		[Unsaved(false)]
		private BoolUnknown isMemoryCached = BoolUnknown.Unknown;

		// Token: 0x04003BCC RID: 15308
		private Texture2D iconInt;
	}
}
