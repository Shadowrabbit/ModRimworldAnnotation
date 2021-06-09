using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B06 RID: 2822
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x06004227 RID: 16935 RVA: 0x00031492 File Offset: 0x0002F692
		public Trigger_PawnLost(PawnLostCondition condition = PawnLostCondition.Undefined, Pawn pawn = null)
		{
			this.condition = condition;
			this.pawn = pawn;
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x000314A8 File Offset: 0x0002F6A8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (this.condition == PawnLostCondition.Undefined || signal.condition == this.condition) && (this.pawn == null || this.pawn == signal.Pawn);
		}

		// Token: 0x04002D65 RID: 11621
		private Pawn pawn;

		// Token: 0x04002D66 RID: 11622
		private PawnLostCondition condition;
	}
}
