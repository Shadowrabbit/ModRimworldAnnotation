using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B25 RID: 2853
	public class QuestPart_Filter_AcceptedAfterTicks : QuestPart_Filter
	{
		// Token: 0x06004301 RID: 17153 RVA: 0x001663D8 File Offset: 0x001645D8
		protected override bool Pass(SignalArgs args)
		{
			return this.quest.TicksSinceAccepted > this.timeTicks;
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x001663ED File Offset: 0x001645ED
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeTicks, "timeTicks", 0, false);
		}

		// Token: 0x040028C8 RID: 10440
		public int timeTicks;
	}
}
