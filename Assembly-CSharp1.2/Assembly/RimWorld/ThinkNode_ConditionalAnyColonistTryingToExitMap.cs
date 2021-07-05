using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E6F RID: 3695
	public class ThinkNode_ConditionalAnyColonistTryingToExitMap : ThinkNode_Conditional
	{
		// Token: 0x06005316 RID: 21270 RVA: 0x001BFDB0 File Offset: 0x001BDFB0
		protected override bool Satisfied(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return false;
			}
			foreach (Pawn pawn2 in mapHeld.mapPawns.FreeColonistsSpawned)
			{
				Job curJob = pawn2.CurJob;
				if (curJob != null && curJob.exitMapOnArrival)
				{
					return true;
				}
			}
			return false;
		}
	}
}
