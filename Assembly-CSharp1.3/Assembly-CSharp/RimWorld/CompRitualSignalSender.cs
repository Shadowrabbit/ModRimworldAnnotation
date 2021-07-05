using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001187 RID: 4487
	public class CompRitualSignalSender : ThingComp
	{
		// Token: 0x06006BF2 RID: 27634 RVA: 0x00243526 File Offset: 0x00241726
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(30))
			{
				bool flag = this.ritualTarget;
				this.ritualTarget = this.parent.IsRitualTarget();
				if (flag != this.ritualTarget)
				{
					this.parent.BroadcastCompSignal("RitualTargetChanged");
				}
			}
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x00243566 File Offset: 0x00241766
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.ritualTarget, "ritualTarget", false, false);
		}

		// Token: 0x04003C0D RID: 15373
		public bool ritualTarget;

		// Token: 0x04003C0E RID: 15374
		private const int CheckInterval = 30;

		// Token: 0x04003C0F RID: 15375
		public const string RitualTargetChangedSignal = "RitualTargetChanged";
	}
}
