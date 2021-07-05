using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA4 RID: 3748
	public class Thought_DelayedRitual : Thought_Situational
	{
		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06005816 RID: 22550 RVA: 0x001DEC32 File Offset: 0x001DCE32
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.obligation.precept.Named("RITUAL"));
			}
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06005817 RID: 22551 RVA: 0x001DEC5E File Offset: 0x001DCE5E
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.obligation.precept.Named("RITUAL"));
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06005818 RID: 22552 RVA: 0x001DEC8A File Offset: 0x001DCE8A
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.obligation.precept.activeObligations.Contains(this.obligation);
			}
		}

		// Token: 0x06005819 RID: 22553 RVA: 0x001DECB1 File Offset: 0x001DCEB1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<RitualObligation>(ref this.obligation, "obligation", false);
		}

		// Token: 0x0600581A RID: 22554 RVA: 0x001DECCC File Offset: 0x001DCECC
		protected override ThoughtState CurrentStateInternal()
		{
			if (this.pawn.IsSlave)
			{
				return false;
			}
			if (this.obligation.precept.OnGracePeriod)
			{
				return false;
			}
			if (this.obligation.precept.activeObligations.Contains(this.obligation))
			{
				return ThoughtState.ActiveAtStage(this.obligation.DelayStage);
			}
			return false;
		}

		// Token: 0x040033DE RID: 13278
		public RitualObligation obligation;
	}
}
