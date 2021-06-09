using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B05 RID: 2821
	public class Trigger_PawnCanReachMapEdge : Trigger
	{
		// Token: 0x06004225 RID: 16933 RVA: 0x00189134 File Offset: 0x00187334
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 193 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}
