using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9F RID: 3743
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x060057F3 RID: 22515 RVA: 0x001DE53D File Offset: 0x001DC73D
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x060057F4 RID: 22516 RVA: 0x001DE55C File Offset: 0x001DC75C
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x060057F5 RID: 22517 RVA: 0x001DE578 File Offset: 0x001DC778
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.DurationTicks;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x060057F6 RID: 22518 RVA: 0x001DE589 File Offset: 0x001DC789
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x001DE5A6 File Offset: 0x001DC7A6
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

		// Token: 0x060057F8 RID: 22520 RVA: 0x001DE5DC File Offset: 0x001DC7DC
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x001DE5E4 File Offset: 0x001DC7E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x001DE602 File Offset: 0x001DC802
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x001DE61B File Offset: 0x001DC81B
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x060057FC RID: 22524 RVA: 0x001DE624 File Offset: 0x001DC824
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}

		// Token: 0x040033D9 RID: 13273
		public float opinionOffset;
	}
}
