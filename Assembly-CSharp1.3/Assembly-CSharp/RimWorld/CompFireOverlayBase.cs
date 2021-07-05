using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001130 RID: 4400
	public abstract class CompFireOverlayBase : ThingComp
	{
		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x060069C3 RID: 27075 RVA: 0x0023A37D File Offset: 0x0023857D
		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x060069C4 RID: 27076 RVA: 0x0023A38C File Offset: 0x0023858C
		public float FireSize
		{
			get
			{
				if (this.startedGrowingAtTick < 0)
				{
					return this.Props.fireSize;
				}
				return Mathf.Lerp(this.Props.fireSize, this.Props.finalFireSize, (float)(GenTicks.TicksAbs - this.startedGrowingAtTick) / this.Props.fireGrowthDurationTicks);
			}
		}

		// Token: 0x060069C5 RID: 27077 RVA: 0x0023A3E2 File Offset: 0x002385E2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startedGrowingAtTick, "startedGrowingAtTick", -1, false);
		}

		// Token: 0x04003B0E RID: 15118
		protected int startedGrowingAtTick = -1;
	}
}
