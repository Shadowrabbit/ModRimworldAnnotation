using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200092E RID: 2350
	public class ThinkNode_HarmedRecently : ThinkNode_Conditional
	{
		// Token: 0x06003C93 RID: 15507 RVA: 0x0014FC73 File Offset: 0x0014DE73
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.lastHarmTick > 0 && Find.TickManager.TicksGame < pawn.mindState.lastHarmTick + this.thresholdTicks;
		}

		// Token: 0x040020B2 RID: 8370
		public int thresholdTicks = 2500;
	}
}
