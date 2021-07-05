using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA2 RID: 3746
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x0600580B RID: 22539 RVA: 0x001DE9A8 File Offset: 0x001DCBA8
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x0600580C RID: 22540 RVA: 0x001DE9C4 File Offset: 0x001DCBC4
		public override string LabelCap
		{
			get
			{
				if (!this.reason.NullOrEmpty())
				{
					TaggedString taggedString = base.CurStage.label.Formatted(this.reason.Named("REASON"), this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN")).CapitalizeFirst();
					if (this.def.Worker != null)
					{
						taggedString = this.def.Worker.PostProcessLabel(this.pawn, taggedString);
					}
					return taggedString;
				}
				if (this.def.Worker == null)
				{
					return base.CurStage.LabelCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN")).CapitalizeFirst();
				}
				return base.LabelCap;
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x0600580D RID: 22541 RVA: 0x001DEAB0 File Offset: 0x001DCCB0
		public override string LabelCapSocial
		{
			get
			{
				if (base.CurStage.labelSocial != null)
				{
					return base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"), this.reason.Named("REASON"));
				}
				return base.LabelCapSocial;
			}
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x001DEB16 File Offset: 0x001DCD16
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x001DEB20 File Offset: 0x001DCD20
		public virtual float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = base.CurStage.baseOpinionOffset;
			if (this.def.effectMultiplyingStat != null)
			{
				num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true) * this.otherPawn.GetStatValue(this.def.effectMultiplyingStat, true);
			}
			return num;
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x001DEB94 File Offset: 0x001DCD94
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		// Token: 0x06005811 RID: 22545 RVA: 0x001DEBC6 File Offset: 0x001DCDC6
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x040033DD RID: 13277
		public Pawn otherPawn;
	}
}
