using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B04 RID: 2820
	public class Trigger_PawnCannotReachMapEdge : Trigger
	{
		// Token: 0x06004223 RID: 16931 RVA: 0x001890C4 File Offset: 0x001872C4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
