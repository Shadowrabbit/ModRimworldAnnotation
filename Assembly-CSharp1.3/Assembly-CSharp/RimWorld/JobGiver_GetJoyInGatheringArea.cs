using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200079C RID: 1948
	public class JobGiver_GetJoyInGatheringArea : JobGiver_GetJoy
	{
		// Token: 0x06003537 RID: 13623 RVA: 0x0012D244 File Offset: 0x0012B444
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

		// Token: 0x06003538 RID: 13624 RVA: 0x0012D2AB File Offset: 0x0012B4AB
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}
}
