using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9B RID: 3739
	public class Thought_Memory : Thought
	{
		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060057D6 RID: 22486 RVA: 0x001DDF59 File Offset: 0x001DC159
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x060057D7 RID: 22487 RVA: 0x001DDF6E File Offset: 0x001DC16E
		public override int DurationTicks
		{
			get
			{
				if (this.durationTicksOverride < 0)
				{
					return this.def.DurationTicks;
				}
				return this.durationTicksOverride;
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x060057D8 RID: 22488 RVA: 0x001DDF8B File Offset: 0x001DC18B
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x060057D9 RID: 22489 RVA: 0x001DDF93 File Offset: 0x001DC193
		public virtual bool ShouldDiscard
		{
			get
			{
				return this.age > this.DurationTicks;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x060057DA RID: 22490 RVA: 0x001DDFA4 File Offset: 0x001DC1A4
		public override string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null || this.cachedLabelCapForOtherPawn != this.otherPawn || this.cachedLabelCapForStageIndex != this.CurStageIndex)
				{
					if (this.otherPawn != null)
					{
						this.cachedLabelCap = base.CurStage.label.Formatted(this.otherPawn.LabelShort, this.otherPawn).CapitalizeFirst();
						if (this.def.Worker != null)
						{
							this.cachedLabelCap = this.def.Worker.PostProcessLabel(this.pawn, this.cachedLabelCap);
						}
					}
					else
					{
						this.cachedLabelCap = base.LabelCap;
					}
					this.cachedLabelCapForOtherPawn = this.otherPawn;
					this.cachedLabelCapForStageIndex = this.CurStageIndex;
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060057DB RID: 22491 RVA: 0x001DE07C File Offset: 0x001DC27C
		public override string LabelCapSocial
		{
			get
			{
				string text;
				if (base.CurStage.labelSocial != null)
				{
					text = base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"));
				}
				else
				{
					text = base.LabelCapSocial;
				}
				if (this.sourcePrecept != null)
				{
					text += " (" + "Ideo".Translate() + ")";
				}
				return text;
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x060057DC RID: 22492 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool Save
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x001DE108 File Offset: 0x001DC308
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060057DE RID: 22494 RVA: 0x001DE114 File Offset: 0x001DC314
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.moodOffset, "moodOffset", 0, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
			Scribe_Values.Look<int>(ref this.durationTicksOverride, "durationTicksOverride", -1, false);
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x001DE196 File Offset: 0x001DC396
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		// Token: 0x060057E0 RID: 22496 RVA: 0x001DE1AA File Offset: 0x001DC3AA
		public void Renew()
		{
			this.age = 0;
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x001DE1B4 File Offset: 0x001DC3B4
		public virtual bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			if (thoughts.memories.NumMemoriesInGroup(this) >= this.def.stackLimit)
			{
				Thought_Memory thought_Memory = thoughts.memories.OldestMemoryInGroup(this);
				if (thought_Memory != null)
				{
					showBubble = (thought_Memory.age > thought_Memory.DurationTicks / 2);
					thought_Memory.Renew();
					return true;
				}
			}
			showBubble = true;
			return false;
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x001DE220 File Offset: 0x001DC420
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		// Token: 0x060057E3 RID: 22499 RVA: 0x001DE268 File Offset: 0x001DC468
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = base.MoodOffset();
			num *= this.moodPowerFactor;
			num += (float)this.moodOffset;
			if (this.def.lerpMoodToZero)
			{
				num *= 1f - (float)this.age / (float)this.DurationTicks;
			}
			return num;
		}

		// Token: 0x060057E4 RID: 22500 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_NewThoughtInGroupAdded(Thought_Memory memory)
		{
		}

		// Token: 0x060057E5 RID: 22501 RVA: 0x001DE2D0 File Offset: 0x001DC4D0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.def.defName,
				", moodPowerFactor=",
				this.moodPowerFactor,
				", moodOffset=",
				this.moodOffset,
				", age=",
				this.age,
				")"
			});
		}

		// Token: 0x040033CC RID: 13260
		public float moodPowerFactor = 1f;

		// Token: 0x040033CD RID: 13261
		public int moodOffset;

		// Token: 0x040033CE RID: 13262
		public Pawn otherPawn;

		// Token: 0x040033CF RID: 13263
		public int age;

		// Token: 0x040033D0 RID: 13264
		private int forcedStage;

		// Token: 0x040033D1 RID: 13265
		public int durationTicksOverride = -1;

		// Token: 0x040033D2 RID: 13266
		private string cachedLabelCap;

		// Token: 0x040033D3 RID: 13267
		private Pawn cachedLabelCapForOtherPawn;

		// Token: 0x040033D4 RID: 13268
		private int cachedLabelCapForStageIndex = -1;
	}
}
