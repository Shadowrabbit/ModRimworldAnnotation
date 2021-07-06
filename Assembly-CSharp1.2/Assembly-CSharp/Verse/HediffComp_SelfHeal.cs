using System;

namespace Verse
{
	// Token: 0x020003EF RID: 1007
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600188E RID: 6286 RVA: 0x000174B3 File Offset: 0x000156B3
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x000174C0 File Offset: 0x000156C0
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x000174D4 File Offset: 0x000156D4
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}

		// Token: 0x04001293 RID: 4755
		public int ticksSinceHeal;
	}
}
