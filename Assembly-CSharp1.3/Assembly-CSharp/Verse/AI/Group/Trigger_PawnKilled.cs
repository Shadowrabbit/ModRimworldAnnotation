using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020006A6 RID: 1702
	public class Trigger_PawnKilled : Trigger
	{
		// Token: 0x06002F58 RID: 12120 RVA: 0x001187D3 File Offset: 0x001169D3
		public Trigger_PawnKilled()
		{
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x00118B8C File Offset: 0x00116D8C
		public Trigger_PawnKilled(List<Pawn> exceptions)
		{
			this.exceptions = exceptions;
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x00118B9C File Offset: 0x00116D9C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.IncappedOrKilled && signal.Pawn.Dead) && (this.exceptions.NullOrEmpty<Pawn>() || !this.exceptions.Contains(signal.Pawn));
		}

		// Token: 0x04001CF2 RID: 7410
		private List<Pawn> exceptions;
	}
}
