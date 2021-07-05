using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B13 RID: 2835
	public class QuestPart_DelayRandom : QuestPart_Delay
	{
		// Token: 0x060042B4 RID: 17076 RVA: 0x001651CC File Offset: 0x001633CC
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.delayTicks = this.delayTicksRange.RandomInRange;
			base.Enable(receivedArgs);
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x001651E8 File Offset: 0x001633E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.delayTicksRange, "delayTicksRange", default(IntRange), false);
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x00165215 File Offset: 0x00163415
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicksRange = new IntRange(833, 2500);
		}

		// Token: 0x040028A0 RID: 10400
		public IntRange delayTicksRange;
	}
}
