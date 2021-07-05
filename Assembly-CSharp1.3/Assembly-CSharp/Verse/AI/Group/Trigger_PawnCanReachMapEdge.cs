using System;

namespace Verse.AI.Group
{
	// Token: 0x020006A3 RID: 1699
	public class Trigger_PawnCanReachMapEdge : Trigger
	{
		// Token: 0x06002F52 RID: 12114 RVA: 0x00118A80 File Offset: 0x00116C80
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
