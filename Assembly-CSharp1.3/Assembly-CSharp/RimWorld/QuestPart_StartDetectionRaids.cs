using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4E RID: 2894
	public class QuestPart_StartDetectionRaids : QuestPart
	{
		// Token: 0x060043BE RID: 17342 RVA: 0x00168ADC File Offset: 0x00166CDC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TimedDetectionRaids component = this.worldObject.GetComponent<TimedDetectionRaids>();
				int ticks = 240000;
				if (this.delayRangeHours != null)
				{
					ticks = (int)(this.delayRangeHours.Value.RandomInRange * 2500f);
					component.delayRangeHours = this.delayRangeHours.Value;
				}
				if (this.firstRaidDelayTicks != null)
				{
					ticks = this.firstRaidDelayTicks.Value;
				}
				component.StartDetectionCountdown(ticks, -1);
				component.SetNotifiedSilently();
			}
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x00168B78 File Offset: 0x00166D78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
			Scribe_Values.Look<FloatRange?>(ref this.delayRangeHours, "delayRangeHours", null, false);
			Scribe_Values.Look<int?>(ref this.firstRaidDelayTicks, "firstRaidDelayTicks", null, false);
		}

		// Token: 0x0400291E RID: 10526
		public string inSignal;

		// Token: 0x0400291F RID: 10527
		public FloatRange? delayRangeHours;

		// Token: 0x04002920 RID: 10528
		public WorldObject worldObject;

		// Token: 0x04002921 RID: 10529
		public int? firstRaidDelayTicks;
	}
}
