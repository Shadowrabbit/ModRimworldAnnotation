using System;

namespace Verse.AI.Group
{
	// Token: 0x020006A2 RID: 1698
	public class Trigger_PawnCannotReachMapEdge : Trigger
	{
		// Token: 0x06002F50 RID: 12112 RVA: 0x00118A10 File Offset: 0x00116C10
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
