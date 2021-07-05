using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001045 RID: 4165
	public class QuestPart_DelayRandom : QuestPart_Delay
	{
		// Token: 0x06005AD4 RID: 23252 RVA: 0x0003EF66 File Offset: 0x0003D166
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.delayTicks = this.delayTicksRange.RandomInRange;
			base.Enable(receivedArgs);
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x001D6AA0 File Offset: 0x001D4CA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.delayTicksRange, "delayTicksRange", default(IntRange), false);
		}

		// Token: 0x06005AD6 RID: 23254 RVA: 0x0003EF80 File Offset: 0x0003D180
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicksRange = new IntRange(833, 2500);
		}

		// Token: 0x04003D0C RID: 15628
		public IntRange delayTicksRange;
	}
}
