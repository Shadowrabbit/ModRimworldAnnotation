using System;

namespace Verse
{
	// Token: 0x0200027F RID: 639
	public class HediffComp_ChanceToRemove : HediffComp
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x00069508 File Offset: 0x00067708
		public HediffCompProperties_ChanceToRemove Props
		{
			get
			{
				return (HediffCompProperties_ChanceToRemove)this.props;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x00069515 File Offset: 0x00067715
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.removeNextInterval && this.currentInterval <= 0);
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00069538 File Offset: 0x00067738
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

		// Token: 0x06001228 RID: 4648 RVA: 0x000695A8 File Offset: 0x000677A8
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<bool>(ref this.removeNextInterval, "removeNextInterval", false, false);
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x000695CE File Offset: 0x000677CE
		public override string CompDebugString()
		{
			return string.Format("currentInterval: {0}\nremove: {1}", this.currentInterval, this.removeNextInterval);
		}

		// Token: 0x04000DCD RID: 3533
		public int currentInterval;

		// Token: 0x04000DCE RID: 3534
		public bool removeNextInterval;
	}
}
