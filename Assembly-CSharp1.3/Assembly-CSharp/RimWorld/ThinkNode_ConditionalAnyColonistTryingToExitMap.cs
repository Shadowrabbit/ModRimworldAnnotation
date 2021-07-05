using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000926 RID: 2342
	public class ThinkNode_ConditionalAnyColonistTryingToExitMap : ThinkNode_Conditional
	{
		// Token: 0x06003C82 RID: 15490 RVA: 0x0014F8E8 File Offset: 0x0014DAE8
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
