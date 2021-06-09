using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CB2 RID: 3250
	public class JobGiver_GetJoyInGatheringArea : JobGiver_GetJoy
	{
		// Token: 0x06004B74 RID: 19316 RVA: 0x001A5AB4 File Offset: 0x001A3CB4
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return null;
			}
			if (pawn.needs.joy == null)
			{
				return null;
			}
			if (pawn.needs.joy.CurLevelPercentage > 0.92f)
			{
				return null;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return def.Worker.TryGiveJobInGatheringArea(pawn, cell);
		}
	}
}
