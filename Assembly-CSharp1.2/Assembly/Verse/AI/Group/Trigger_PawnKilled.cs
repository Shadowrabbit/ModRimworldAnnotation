using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B08 RID: 2824
	public class Trigger_PawnKilled : Trigger
	{
		// Token: 0x0600422B RID: 16939 RVA: 0x0003152E File Offset: 0x0002F72E
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && signal.condition == PawnLostCondition.IncappedOrKilled && signal.Pawn.Dead;
		}
	}
}
