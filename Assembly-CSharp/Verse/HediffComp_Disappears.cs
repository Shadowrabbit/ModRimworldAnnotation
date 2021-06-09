using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003CA RID: 970
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x00016E02 File Offset: 0x00015002
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x0600180D RID: 6157 RVA: 0x00016E0F File Offset: 0x0001500F
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x00016E27 File Offset: 0x00015027
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

		// Token: 0x0600180F RID: 6159 RVA: 0x00016E4C File Offset: 0x0001504C
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksToDisappear = this.Props.disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x00016E6A File Offset: 0x0001506A
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear--;
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x000DE374 File Offset: 0x000DC574
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x00016E7A File Offset: 0x0001507A
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00016E8E File Offset: 0x0001508E
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear;
		}

		// Token: 0x04001247 RID: 4679
		public int ticksToDisappear;
	}
}
