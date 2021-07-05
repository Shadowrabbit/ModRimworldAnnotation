using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156B RID: 5483
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x060076F3 RID: 30451 RVA: 0x000504B9 File Offset: 0x0004E6B9
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x060076F4 RID: 30452 RVA: 0x00242E68 File Offset: 0x00241068
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

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x060076F5 RID: 30453 RVA: 0x00242F54 File Offset: 0x00241154
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

		// Token: 0x060076F6 RID: 30454 RVA: 0x000504D5 File Offset: 0x0004E6D5
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060076F7 RID: 30455 RVA: 0x00242FBC File Offset: 0x002411BC
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

		// Token: 0x060076F8 RID: 30456 RVA: 0x00243030 File Offset: 0x00241230
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		// Token: 0x060076F9 RID: 30457 RVA: 0x000504DD File Offset: 0x0004E6DD
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x04004E6A RID: 20074
		public Pawn otherPawn;
	}
}
