using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001568 RID: 5480
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x060076DD RID: 30429 RVA: 0x0005033D File Offset: 0x0004E53D
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x060076DE RID: 30430 RVA: 0x0005035C File Offset: 0x0004E55C
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x060076DF RID: 30431 RVA: 0x00050378 File Offset: 0x0004E578
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.def.DurationTicks;
			}
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x060076E0 RID: 30432 RVA: 0x0005038E File Offset: 0x0004E58E
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x060076E1 RID: 30433 RVA: 0x000503AB File Offset: 0x0004E5AB
		public virtual float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			return this.opinionOffset * this.AgeFactor;
		}

		// Token: 0x060076E2 RID: 30434 RVA: 0x000503E1 File Offset: 0x0004E5E1
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060076E3 RID: 30435 RVA: 0x000503E9 File Offset: 0x0004E5E9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x060076E4 RID: 30436 RVA: 0x00050407 File Offset: 0x0004E607
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060076E5 RID: 30437 RVA: 0x00050420 File Offset: 0x0004E620
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x060076E6 RID: 30438 RVA: 0x00242C40 File Offset: 0x00240E40
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}

		// Token: 0x04004E66 RID: 20070
		public float opinionOffset;
	}
}
