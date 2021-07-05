using System;

namespace Verse
{
	// Token: 0x0200029F RID: 671
	public class HediffComp_GiveNeurocharge : HediffComp
	{
		// Token: 0x06001285 RID: 4741 RVA: 0x0006AA74 File Offset: 0x00068C74
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.parent.pawn.health.lastReceivedNeuralSuperchargeTick = Find.TickManager.TicksGame;
		}
	}
}
