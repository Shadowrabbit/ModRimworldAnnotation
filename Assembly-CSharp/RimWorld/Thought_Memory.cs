using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001565 RID: 5477
	public class Thought_Memory : Thought
	{
		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x060076C7 RID: 30407 RVA: 0x00050263 File Offset: 0x0004E463
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x060076C8 RID: 30408 RVA: 0x00050278 File Offset: 0x0004E478
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x060076C9 RID: 30409 RVA: 0x00050280 File Offset: 0x0004E480
		public virtual bool ShouldDiscard
		{
			get
			{
				return this.age > this.def.DurationTicks;
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x060076CA RID: 30410 RVA: 0x0024284C File Offset: 0x00240A4C
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

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x060076CB RID: 30411 RVA: 0x00242924 File Offset: 0x00240B24
		public override string LabelCapSocial
		{
			get
			{
				if (base.CurStage.labelSocial != null)
				{
					return base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"));
				}
				return base.LabelCapSocial;
			}
		}

		// Token: 0x060076CC RID: 30412 RVA: 0x00050295 File Offset: 0x0004E495
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060076CD RID: 30413 RVA: 0x0024297C File Offset: 0x00240B7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x060076CE RID: 30414 RVA: 0x0005029E File Offset: 0x0004E49E
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		// Token: 0x060076CF RID: 30415 RVA: 0x000502B2 File Offset: 0x0004E4B2
		public void Renew()
		{
			this.age = 0;
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x002429DC File Offset: 0x00240BDC
		public virtual bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			if (thoughts.memories.NumMemoriesInGroup(this) >= this.def.stackLimit)
			{
				Thought_Memory thought_Memory = thoughts.memories.OldestMemoryInGroup(this);
				if (thought_Memory != null)
				{
					showBubble = (thought_Memory.age > thought_Memory.def.DurationTicks / 2);
					thought_Memory.Renew();
					return true;
				}
			}
			showBubble = true;
			return false;
		}

		// Token: 0x060076D1 RID: 30417 RVA: 0x00242A4C File Offset: 0x00240C4C
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		// Token: 0x060076D2 RID: 30418 RVA: 0x000502BB File Offset: 0x0004E4BB
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			return base.MoodOffset() * this.moodPowerFactor;
		}

		// Token: 0x060076D3 RID: 30419 RVA: 0x00242A94 File Offset: 0x00240C94
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.def.defName,
				", moodPowerFactor=",
				this.moodPowerFactor,
				", age=",
				this.age,
				")"
			});
		}

		// Token: 0x04004E5D RID: 20061
		public float moodPowerFactor = 1f;

		// Token: 0x04004E5E RID: 20062
		public Pawn otherPawn;

		// Token: 0x04004E5F RID: 20063
		public int age;

		// Token: 0x04004E60 RID: 20064
		private int forcedStage;

		// Token: 0x04004E61 RID: 20065
		private string cachedLabelCap;

		// Token: 0x04004E62 RID: 20066
		private Pawn cachedLabelCapForOtherPawn;

		// Token: 0x04004E63 RID: 20067
		private int cachedLabelCapForStageIndex = -1;
	}
}
