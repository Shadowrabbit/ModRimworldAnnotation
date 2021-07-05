using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000288 RID: 648
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x000699AA File Offset: 0x00067BAA
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x000699B7 File Offset: 0x00067BB7
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0 || (this.Props.requiredMentalState != null && base.Pawn.MentalStateDef != this.Props.requiredMentalState);
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x000699F6 File Offset: 0x00067BF6
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showRemainingTime)
				{
					return base.CompLabelInBracketsExtra;
				}
				return this.ticksToDisappear.ToStringTicksToPeriod(true, true, true, true);
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00069A1B File Offset: 0x00067C1B
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksToDisappear = this.Props.disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00069A39 File Offset: 0x00067C39
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear--;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00069A4C File Offset: 0x00067C4C
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00069A84 File Offset: 0x00067C84
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00069A98 File Offset: 0x00067C98
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear;
		}

		// Token: 0x04000DDD RID: 3549
		public int ticksToDisappear;
	}
}
