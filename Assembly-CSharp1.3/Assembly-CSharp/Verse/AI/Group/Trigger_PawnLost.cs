using System;

namespace Verse.AI.Group
{
	// Token: 0x020006A4 RID: 1700
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x06002F54 RID: 12116 RVA: 0x00118AF0 File Offset: 0x00116CF0
		public Trigger_PawnLost(PawnLostCondition condition = PawnLostCondition.Undefined, Pawn pawn = null)
		{
			this.condition = condition;
			this.pawn = pawn;
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x00118B06 File Offset: 0x00116D06
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (this.condition == PawnLostCondition.Undefined || signal.condition == this.condition) && (this.pawn == null || this.pawn == signal.Pawn);
		}

		// Token: 0x04001CEF RID: 7407
		private Pawn pawn;

		// Token: 0x04001CF0 RID: 7408
		private PawnLostCondition condition;
	}
}
