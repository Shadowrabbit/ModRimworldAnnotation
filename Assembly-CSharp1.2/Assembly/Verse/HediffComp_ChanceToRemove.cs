using System;

namespace Verse
{
	// Token: 0x020003BF RID: 959
	public class HediffComp_ChanceToRemove : HediffComp
	{
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x00016BAF File Offset: 0x00014DAF
		public HediffCompProperties_ChanceToRemove Props
		{
			get
			{
				return (HediffCompProperties_ChanceToRemove)this.props;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060017E1 RID: 6113 RVA: 0x00016BBC File Offset: 0x00014DBC
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.removeNextInterval && this.currentInterval <= 0);
			}
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000DDC6C File Offset: 0x000DBE6C
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.CompShouldRemove)
			{
				return;
			}
			if (this.currentInterval > 0)
			{
				this.currentInterval--;
				return;
			}
			if (Rand.Chance(this.Props.chance))
			{
				this.removeNextInterval = true;
				this.currentInterval = Rand.Range(0, this.Props.intervalTicks);
				return;
			}
			this.currentInterval = this.Props.intervalTicks;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00016BDE File Offset: 0x00014DDE
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<bool>(ref this.removeNextInterval, "removeNextInterval", false, false);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00016C04 File Offset: 0x00014E04
		public override string CompDebugString()
		{
			return string.Format("currentInterval: {0}\nremove: {1}", this.currentInterval, this.removeNextInterval);
		}

		// Token: 0x0400122A RID: 4650
		public int currentInterval;

		// Token: 0x0400122B RID: 4651
		public bool removeNextInterval;
	}
}
