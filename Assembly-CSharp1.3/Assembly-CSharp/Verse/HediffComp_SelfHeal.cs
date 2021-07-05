using System;

namespace Verse
{
	// Token: 0x020002B4 RID: 692
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0006B9EB File Offset: 0x00069BEB
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0006B9F8 File Offset: 0x00069BF8
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0006BA0C File Offset: 0x00069C0C
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}

		// Token: 0x04000E2C RID: 3628
		public int ticksSinceHeal;
	}
}
